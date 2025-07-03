using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public enum AttackType {Human,Okami,Which,Vampire };                                //攻撃種類を管理する

public class player_control : MonoBehaviour
{
	//変数宣言
	public float moveSpeed;                                                         //移動速度
	public float jumpPower;                                                         //ジャンプ力
    public LayerMask Ground;                                                        //地面を判別するオブジェクトレイヤー
    public LayerMask LadderLayer;                                                   //hasigoを判別するオブジェクトレイヤー

    //---------------------------------------------------------------------------------------------------------------
	//攻撃パターンと変身
                                                                                //攻撃判定の変数
    private AttackType currentAttack = AttackType.Human;                        //攻撃する種類がはじめは人間状態の攻撃にするため
    public void ChangeAttack(AttackType newType)
    {
        currentAttack = newType;
    }

    public AttackType attackType;

    public GameObject HumanWeapon;                                             //人状態で攻撃する（槍）のプレハブ
    public GameObject OkamiWeapon;                                             //狼男状態で攻撃する（爪）のプレハブ
    public GameObject WhichWeapon;                                             //魔女状態で攻撃する（魔法）のプレハブ
	public GameObject VampireWeapon;										   //ヴァンパイア状態で攻撃する(火柱)のプレハブ

    private GameObject spearToShoot;

   
	public float KnockbackForce;                                                    //ノックバック
	public float invincibleTime;                                                        //無敵時間
	public int maxBulletsOnScreen = 3;                                          //画面内に出るプレイヤー攻撃の最大の数

	public int FirePillarCnt = 0;												//火柱を何連させるか
	public float FirePillarDelay = 0.0f;										//火柱の出現間隔
	public float FirePillarSpread = 0.0f;										//火柱がどのくらい広がるか
	public float FirePillarOffset = 0.0f;										//プレイヤーから火柱を出現させる距離
	public int maxFirePillarOnScreen = 0;										//火柱の最大出現数

//-------------------------------------------------------------------------------------------------------------------
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
//-----------------------------------------------------------------------------------------------------------------------
//仮想チェンジ用
    public Sprite Okami;        //当たった時に画像を変えるため
    public Sprite Which;        //当たった時に画像を変えるため
	public Sprite Vampire;		//当たった時に画像を変えるため


    private bool HumanChange = true;		//人間状態の判定をする
	private bool OkamiChange = false;       //狼男状態の判定をする
    private bool WhichChange = false;		//魔女状態の判定をする


    private Image image;            //画像の管理
    bool text1enableKey = true;

    // 画像描画用のコンポーネント
    SpriteRenderer sr;

    //-----------------------------------------------------------------------------------------------------------------------
    //はしご登り関連の追加変数
    public float ladderClimbSpeed = 3.0f; // はしごを登る速度
    private bool isClimbingLadder = false; // はしごを登っている最中かどうか
    private bool canClimbLadder = false;   // はしごに触れていて、登れる状態にあるかどうか
    private float originalGravityScale;    // 元の重力値を保持
	//-----------------------------------------------------------------------------------------------------------------------
	//しゃがむ処理の変数
	private Vector2 OriginColliderSize;
	private Vector2 OriginClliderOffset;
	private Vector3 OriginLocalScale;



    void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
		SpriteRenderer.color = Color.green;                                     //プレイヤーの色を緑色にする

        // SpriteRendererコンポーネントを取得します
        image = GetComponent<Image>();
        // SpriteのSpriteRendererコンポーネントを取得
        sr = gameObject.GetComponent<SpriteRenderer>();

		//元の重力値を保持
        originalGravityScale = rb.gravityScale;

		//しゃがみ機能の初期設定
		OriginColliderSize =bc.size;
		OriginClliderOffset = bc.offset;

		OriginLocalScale =transform.localScale;

        BulletChange("Human");
    }

	// Update is called once per frame
	void Update()
	{

        // はしごを登る処理を最優先
        if (isClimbingLadder)
        {
            ClimbLadder(); // はしごを登る処理
            return; // はしごを登っている間は他の動作をスキップ
        }

        //移動処理
        if (/*IsGrounded() == true && */ IsSquat == false /*&& IsJumping == false*/)
		{
			Move();
		}

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LadderLayer);
        
		//ジャンプ処理
        if ((IsGrounded() == true || hit.collider != null ) && IsSquat == false)
		{
			Jump();
		}

		//Zキーが押されたら
		if (Input.GetKeyDown(KeyCode.Z)||Input.GetButtonDown("Fire1"))
		{
				Attack();
		}


        // はしごに触れていて、かつ上方向の入力がある場合
        if (canClimbLadder && (Input.GetAxisRaw("Vertical") > 0.3f ))
        {
            StartClimbingLadder();
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
            SceneManager.LoadScene("GameOver");
        }



	}


    //ゴールオブジェクトに触れたらゴールシーンに切り替わる
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag =="Goal" )
		{
			SceneManager.LoadScene("Goal");
		}

        //狼男アイテムに触れたら自分が狼男になる
        if (collision.gameObject.tag == "Okami")
        {

            sr.sprite = Okami;
			BulletChange("Okami");

            Debug.Log("狼男に変身");

            Destroy(collision.gameObject);
        }


        //魔女アイテムに触れたら自分が魔女になる
        if (collision.gameObject.tag == "Which")
        {

            sr.sprite = Which;
            BulletChange("Which");

            Debug.Log("魔女に変身");

            Destroy(collision.gameObject);
        }

        //ヴァンパイアアイテムに触れたら自分が魔女になる
        if (collision.gameObject.tag == "Vampire")
        {

            sr.sprite = Vampire;
            BulletChange("Vampire");

            Debug.Log("ヴァンパイアに変身");

            Destroy(collision.gameObject);
        }



        //はしごに触れたら登れるようになる
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimbLadder = true;
        }


    }

    //はしごから離れた処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimbLadder = false;
            // はしごのコライダーから完全に離れたら、はしご登り状態を強制終了する
            // これで、横に移動してはしごから外れた場合も対応できる
            if (isClimbingLadder)
            {
                StopClimbingLadder();
            }
        }
    }




    //接触判定（敵やアイテム）
    private void OnCollisionEnter2D(Collision2D collision)
	{
		//EnemyとEnemyBulletに当たったらプレイヤーを破壊する
		if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"))
		{
			if (!IsInvincible)
			{
				HP -= 1;
				Debug.Log("痛い");


				PlayerColor();

                //// ノックバック処理
                //Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
                //GetComponent<Rigidbody2D>().AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);
				// 無敵状態を開始
				StartInvincibility();
            }
            
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
		//足元が梯子かどうかチェック
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LadderLayer);
		bool onLadder = false;

		//真下に梯子がある
        if (hit.collider != null && !canClimbLadder)
        {
			onLadder = true;
            GetComponent<SetGravity>().IsEnable = false;
            rb.gravityScale = 0f; // 重力を無効にする
			rb.linearVelocityY = 0.0f;

			//下キーが押されている
			if(Input.GetAxisRaw("Vertical") < -0.3f)
			{
				//梯子モードに遷移
				StartClimbingLadder();
				return;

			}

		}
		else
		{
            GetComponent<SetGravity>().IsEnable = true;
			rb.gravityScale = originalGravityScale;
        }

        //地上にいないときは入力を受け付けない
        if (IsGrounded() == true || onLadder)
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
		////槍オブジェクトをすべて取得するために配列を作成
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Spear");


        // プレイヤーの向きを取得
        float playerDirection = transform.localScale.x > 0 ? 1f : -1f;

        //発射位置を制御
        float Offsetx = 1.0f;
        Vector3 SpwanPos = transform.position + new Vector3(transform.localScale.x > 0 ? Offsetx : -Offsetx, 0, 0);

		//火柱の位置を設定
        Vector3 spawnPosition = transform.position + new Vector3(playerDirection * FirePillarOffset, 0f, 0f);


        if (currentAttack == AttackType.Vampire)
		{
			//火柱の処理
			//数の制限
			GameObject[] Pillars =GameObject.FindGameObjectsWithTag(VampireWeapon.tag);
			if (Pillars.Length > maxFirePillarOnScreen)
			{
				Debug.Log("火柱出すぎてアチアチやでぇ...");
				return;
			}

			//火柱を出力
			StartCoroutine(SpawnFirePillarsRoutine(spawnPosition, playerDirection, FirePillarCnt, FirePillarDelay, FirePillarSpread,Ground));

		}

		else　//ヴァンパイア以外の状態の攻撃
		{

			if (bullets.Length >= maxBulletsOnScreen)
			{
				// 画面の最大数に達しているので発射しない

				return;
			}


            // 攻撃処理
            GameObject bullet = Instantiate(spearToShoot, SpwanPos, Quaternion.identity);

            // プレイヤーの向きに合わせて反転
            bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);

        }
        
		
        
    }

    //関数名：BulletChange()
    //用途：攻撃処理
    //引数：string
    //戻り値：なし
    private void BulletChange(string BulletName)
	{
        //発射する弾の種類を管理する（switchで）
        spearToShoot = HumanWeapon;

        switch (BulletName)
        {
            case "Okami":
                currentAttack = AttackType.Okami; // currentAttack もここで設定
                spearToShoot = OkamiWeapon;
                break;
            case "Which":
                currentAttack = AttackType.Which;
                spearToShoot = WhichWeapon;
                break;

            case "Vampire":
                currentAttack = AttackType.Vampire;
                spearToShoot = VampireWeapon;
                break;

			default:
                currentAttack = AttackType.Human;
                spearToShoot = HumanWeapon;
				break;

        }
    }


    //関数名：IsGrounded()
    //用途：接地判定処理
    //引数：なし
    //戻り値：接地している場合はtrue、していない場合はfalse
    public bool IsGrounded()
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

    //関数名：StartClimbingLadder()
    //用途：はしごを登り始める処理
    //引数：void
    //戻り値：なし
    void StartClimbingLadder()
    {
        isClimbingLadder = true;
        rb.gravityScale = 0f; // 重力を無効にする
        rb.linearVelocity = Vector2.zero; // 現在の移動をリセット

		//Gravityのスクリプトも止める
		GetComponent<SetGravity>().IsEnable = false;
    }

    //関数名：ClimbLadder()
    //用途：はしごを登る処理
    //引数：void
    //戻り値：なし
    void ClimbLadder()
    {
		if (!isClimbingLadder)
		{
			return;
		}

        // Y軸の入力（上矢印キーまたはLスティックの上下）を取得
        float verticalInput = Input.GetAxisRaw("Vertical");

		//地上にいる、かつ、下方向に入力したら、梯子を解除してよい
		//もしくはジャンプボタンを押したら梯子を解除
        if ((verticalInput < -0.1f && IsGrounded()) || Input.GetButtonDown("Jump"))
        {
            StopClimbingLadder();
			return;
        }

		Debug.Log("Input = " + verticalInput + " velocity = "+ rb.linearVelocity + " verticalInput * ladderClimbSpeed= " + verticalInput * ladderClimbSpeed);
        rb.linearVelocity = new Vector2(0f, verticalInput * ladderClimbSpeed);
    }


    //関数名：StopClimbingLadder()
    //用途：はしご登り終了処理
    //引数：void
    //戻り値：なし
    void StopClimbingLadder()
    {
        isClimbingLadder = false;
        rb.gravityScale = originalGravityScale; // 重力を元に戻す

        // 停止時の速度をリセット（滑り落ち防止）だが、
        // はしごから降りた直後にジャンプや移動に移行したい場合、
        // rb.velocity.x を維持する必要があるかもしれない。
        // rb.velocity = Vector2.zero; // ここをコメントアウトして、X軸速度を維持するか検討
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // X軸速度は維持し、Y軸速度のみ0にする

		// 地面にいる場合は、IsJumpingをfalseにリセット
		IsJumping = !IsGrounded();

		//Gravityのスクリプトは再開
		GetComponent<SetGravity>().IsEnable = true;
    }



    IEnumerator SpawnFirePillarsRoutine(Vector3 basePosition, float playerDirection, int count, float delay, float spread,LayerMask Ground)
    {
        for (int i = 0; i < count; i++)
        {
            float offsetX = (i - (count - 1) / 2.0f) * spread * playerDirection;
            // 火柱を生成するX座標は、プレイヤーの目の前を中心に広がるように計算する
            float targetX = basePosition.x + offsetX;

          
            float groundY = basePosition.y; // 一旦プレイヤーのY座標を仮の地面として設定

            // 弾を真下（Vector2.down）に飛ばして、地面（groundLayer）に当たるかをチェックする
            // ここでのRaycastの長さ（例: 10f）は、プレイヤーの高さから確実に地面に届くように長めにする
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(targetX, basePosition.y + 10f), Vector2.down, 20f,Ground); // ★Raycastの開始位置と長さを調整

            if (hit.collider != null)
            {
                // 地面が見つかったら、その地面の上端のY座標を取得する
                groundY = hit.point.y; // Raycastが当たった場所のY座標
            }
            else
            {
                // もし地面が見つからへんかったら（空中にRaycastが届かんとか）、デバッグログを出して、
                // 元のプレイヤーのY座標を使うとか、何らかのフォールバック処理を考える
                // 今回はDebug.Logしとくわ。
                Debug.LogWarning("火柱の足元に地面が見つかりませんでした！ 火柱はプレイヤーの高さから出ます。", this);
            }

            // 火柱を生成する最終的な位置やで！Y座標は地面の高さを使う
            Vector3 firePillarSpawnPos = new Vector3(targetX, groundY, basePosition.z);

            // 火柱を生成するで
            GameObject pillarInstance = Instantiate(VampireWeapon, firePillarSpawnPos, Quaternion.identity);

            // 火柱の見た目もプレイヤーの向きに合わせるで (この部分は変更なし)
            SpriteRenderer pillarSr = pillarInstance.GetComponent<SpriteRenderer>();
            if (pillarSr != null)
            {
                pillarSr.flipX = (playerDirection == -1);
            }
            else
            {
                Vector3 pillarScale = pillarInstance.transform.localScale;
                pillarScale.x = Mathf.Abs(pillarScale.x) * playerDirection;
                pillarInstance.transform.localScale = pillarScale;
            }

            // 次の火柱が出るまで少し待つ
            yield return new WaitForSeconds(delay);
        }
    }

    
    private GameObject CreateAttackInstance(GameObject prefab, Vector3 spawnPosition, float playerDirection)
    {
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

        SpriteRenderer instanceSr = instance.GetComponent<SpriteRenderer>();
        if (instanceSr != null)
        {
            instanceSr.flipX = (playerDirection == -1);
        }
        else
        {
            Vector3 instanceScale = instance.transform.localScale;
            instanceScale.x = Mathf.Abs(instanceScale.x) * playerDirection;
            instance.transform.localScale = instanceScale;
        }
        return instance;
    }



}
