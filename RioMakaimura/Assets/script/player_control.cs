using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class player_control : MonoBehaviour
{
	//変数宣言
	public float moveSpeed;                                                         //移動速度
	public float jumpPower;                                                         //ジャンプ力
	public LayerMask Ground;                                                        //地面を判別するオブジェクトレイヤー
	public GameObject bulletPrefab;                                             //槍のプレハブ
	public float AttackRate;                                                            //攻撃感覚
	public float CoolDown = 2.0f;                                                 //攻撃のクールダウン
	public float KnockbackForce;                                                    //ノックバック
	public float invincibleTime;                                                        //無敵時間
	public int maxBulletsOnScreen = 3;                                          //画面内に出るプレイヤー攻撃の最大の数

	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //立ってる時のサイズ
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);             //しゃがんだ時のサイズ

	private int HP = 2;                                                                 //HP
	private int AttackCount;                                                            //攻撃をカウントする変数
	private bool IsSquat = false;                                                   //しゃがみ判定
	private bool IsJumping;                                                         //空中にいるか判定
	private bool IsAttacking = true;                                                //攻撃できるか判定
	private bool IsInvincible = false;                                              //無敵状態か判定
	private float Moveinput;                                                          //移動入力
	private float LastAttackTime;                                                   //最後に攻撃した時間
	private float InvincibleTimer;                                                  //無敵時間タイマー
	private Vector2 Movedirection = Vector2.zero;                           // 移動方向を記憶しておく

	private Rigidbody2D rb;                                                         //Rigidbody2Dの格納
	private BoxCollider2D bc;                                                       //BoxCollider2Dの格納庫
	private SpriteRenderer SpriteRenderer;                                          //SpriteRendererを扱うための格納庫

    void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        LastAttackTime = -AttackRate;                                           // 最初の発射が即時できるように設定
		SpriteRenderer.color = Color.green;										//プレイヤーの色を緑色にする
	}

	// Update is called once per frame
	void Update()
	{

		//移動処理
		if (/*IsGrounded() == true && */ IsSquat == false /*&& IsJumping == false*/)
		{
			Move();
		}

		//ジャンプ処理
		if (IsGrounded() == true && IsSquat == false)
		{
			Jump();
		}

		//Zキーが押されたら
		if (Input.GetKeyDown(KeyCode.Z)||Input.GetButtonDown("Fire1"))
		{
				Attack();
		}


		//無敵タイマー
		if (IsInvincible == true)
		{
			InvincibleTimer -= Time.deltaTime;
			if (InvincibleTimer <= 0)
			{
				IsInvincible = false;
			}
		}




		//死亡条件
		if (HP == 0)
		{
			//プレイヤーを破壊
			Destroy(gameObject);

			Debug.Log("死ぬwwwwwwwwww");

			//Sceneをリセットする
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}



	}


    //ゴールオブジェクトに触れたらゴールシーンに切り替わる
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag =="Goal" )
		{
			SceneManager.LoadScene("Goal");
		}
    }



    private void OnCollisionEnter2D(Collision2D collision)
	{
		//EnemyとEnemyBulletに当たったらプレイヤーを破壊する
		if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"))
		{

			HP -= 1;
			Debug.Log("痛い");

			PlayerColor();

			// ノックバック処理
			Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
			GetComponent<Rigidbody2D>().AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);

			// 無敵状態を開始
			StartInvincibility();
		}

		if( collision.gameObject.tag == "Activearea")
		{
			HP = 0;
		}
	}



	//関数名：StartInvincibility()
	//用途：無敵時間処理
	//引数：なし
	//戻り値：なし
	void StartInvincibility()
	{
		IsInvincible = true;
		InvincibleTimer = invincibleTime;

		// 無敵時、敵との衝突を無視する
		StartCoroutine(IgnoreEnemyCollisionDuringInvincibility());

		// プレイヤーを点滅させるためのコルーチンを開始
		StartCoroutine(InvincibilityFlash());
	}



	//関数名：InvincibilityFlash()
	//用途：点滅処理
	//引数：なし
	//戻り値：なし
	// プレイヤーを点滅させるコルーチン
	IEnumerator InvincibilityFlash()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		while (IsInvincible)
		{
			sr.enabled = !sr.enabled; // 点滅（スプライトの表示・非表示切り替え）
			yield return new WaitForSeconds(0.1f); // 点滅の間隔
		}
		sr.enabled = true; // 最後にスプライトを表示
	}


	// 無敵時間中、プレイヤーと敵の衝突を無視する
	IEnumerator IgnoreEnemyCollisionDuringInvincibility()
	{
		// 無敵時間が終了するまで繰り返し
		while (IsInvincible)
		{
			// "Enemy" タグのついたオブジェクトをすべて取得
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			// 敵との衝突を無視
			foreach (GameObject enemy in enemies)
			{
				Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), true);
			}

			// 無敵時間中は衝突を無視し続ける
			yield return null;
		}

		// 無敵時間が終了したら、衝突を再び有効にする
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in allEnemies)
		{
			Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), false);
		}
	}

    //関数名：PlayerColor()
    //用途：プレイヤーのHPによる色の変更
    //引数：なし
    //戻り値：なし
    void PlayerColor()
	{
		switch(HP)
		{
			case 3:
			
			//プレイヤーのHPが2の時
			case 2:

                //プレイヤーの色を緑色にする
                SpriteRenderer.color = Color.green; 
				break;

            //プレイヤーのHPが2の時
            case 1:

                //プレイヤーの色を黄色にする
                SpriteRenderer.color = Color.yellow;
				break;

            //プレイヤーのHPが2の時
            case 0:

                //プレイヤーの色を赤色にする
                SpriteRenderer.color = Color.red;
				break;
		}
	}


	//関数名：Jump()
	//用途：ジャンプ処理
	//引数：なし
	//戻り値：なし
	void Jump()
	{
		//ジャンプ処理
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
		{
			rb.AddForce(Vector2.up * jumpPower);
			//ジャンプ状態にする
			IsJumping = true;

			//ジャンプ中は移動速度を制限する
			Moveinput *= 0.7f;
		}
	}


	//関数名：Move()
	//用途：移動処理
	//引数：なし
	//戻り値：なし
	void Move()
	{

		//地上にいないときは入力を受け付けない
		if (IsGrounded() == true)
		{
			Moveinput = Input.GetAxisRaw("Horizontal");

			//プレイヤーの向きを変更
			if (Moveinput != 0)
			{
				Movedirection = new Vector2(Moveinput, 0f);

				// 向きを反転させる処理
				Vector3 scale = transform.localScale;
				scale.x = Mathf.Abs(scale.x) * Mathf.Sign(Moveinput); // 左ならマイナス、右ならプラス
				transform.localScale = scale;
			}
		}


		//プレイヤーを移動させる
		transform.Translate(Moveinput * moveSpeed, 0.0f, 0.0f);

		if (Moveinput != 0)
		{
			Movedirection = new Vector2(Moveinput, 0f);
		}


	}

	


	//関数名：Attack()
	//用途：攻撃処理
	//引数：なし
	//戻り値：なし
	private void Attack()
	{
		//槍オブジェクトをすべて取得するために配列を作成
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Spear");

		if (bullets.Length >= maxBulletsOnScreen)
		{
			// 画面の最大数に達しているので発射しない

			return;
		}

		// 攻撃処理
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

		// プレイヤーの向きに合わせて反転
		bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);
	}




	//関数名：IsGrounded()
	//用途：接地判定処理
	//引数：なし
	//戻り値：接地している場合はtrue、していない場合はfalse
	bool IsGrounded()
	{
		bool ret = false;
		//下方向にrayを飛ばして、指定したレイヤーのオブジェクトと接触しているかどうか判別する
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, Ground);
		//ヒットしていない場合はnullが返される
		if (hit.collider != null)
		{
			ret = true;
		}
		if (ret == true)
		{
			IsJumping = false;
		}

		return ret;
	}
}
