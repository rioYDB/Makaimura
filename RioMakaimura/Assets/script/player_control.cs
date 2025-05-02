using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//変数宣言
	public float moveSpeed;												//移動速度
	public float jumpPower;												//ジャンプ力
	public LayerMask Ground;											//地面を判別するオブジェクトレイヤー
	public GameObject bulletPrefab;										//槍のプレハブ


	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //立ってる時のサイズ
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);				//しゃがんだ時のサイズ

	private bool IsSquat = false;                                       //しゃがみ判定
	private bool IsJumping;												//空中にいるか判定
	private float Moveinput;											//移動入力
	private Vector2 Movedirection = Vector2.zero;						// 移動方向を記憶しておく

	private Rigidbody2D rb;												//Rigidbody2Dの格納
	private BoxCollider2D bc;											//BoxCollider2Dの格納庫
	void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{

		//移動処理
		if (/*IsGrounded() == true && */IsSquat == false /*&& IsJumping == false*/)
		{

			Move();
		}

		//ジャンプ処理
		if (IsGrounded() == true && IsSquat == false)
		{
			Jump();
		}

		//Zキーが押されたら
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//攻撃処理
			GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

			bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1); // プレイヤーの向きに合わせて反転
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//EnemyとEnemyBulletに当たったらプレイヤーを破壊する
		if ((collision.gameObject.tag=="Enemy"|| collision.gameObject.tag == "EnemyBullet"))
		{
			Destroy(gameObject);

			//Sceneをリセットする
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// トリガーが発生した時の処理
	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	// 接触したオブジェクトのtag名がEnemyの場合は
	//	if (collision.gameObject.tag == "Enemy")
	//	{

	//		//// 最新スタイルの呼び出し
	//		//GameManager.Instance.RespawnPlayer();

	//		// Playerオブジェクトを消去する
	//		Destroy(gameObject);

	//		// 現在のシーンをリロード（最初からやり直し）
	//		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	//	}


	//}



	//関数名：Jump()
	//用途：ジャンプ処理
	//引数：なし
	//戻り値：なし
	void Jump()
	{
		//ジャンプ処理
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector2.up * jumpPower);

			//ジャンプ状態にする
			IsJumping = true;

		}


		//ジャンプ前の移動を記憶させる
		if (IsJumping == true)
		{

			rb.linearVelocity = new Vector2(Movedirection.x * moveSpeed, rb.linearVelocity.y);
		}

	}


	//関数名：Move()
	//用途：移動処理
	//引数：なし
	//戻り値：なし
	void Move()
	{

		Moveinput = Input.GetAxisRaw("Horizontal");


		//横移動方向を記憶させる



		//プレイヤーを移動させる
		transform.Translate(Moveinput * moveSpeed, 0.0f, 0.0f);
		if (Moveinput != 0)
		{
			Movedirection = new Vector2(Moveinput, 0f);
		}


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

