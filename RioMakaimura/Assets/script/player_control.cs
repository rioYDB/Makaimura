using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//変数宣言
	public float moveSpeed;                                     //移動速度
	public float jumpPower;                                     //ジャンプ力
	public LayerMask Ground;                                    //地面を判別するオブジェクトレイヤー
	public GameObject bulletPrefab;                             //槍のプレハブ

	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //立ってる時のサイズ
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);         //しゃがんだ時のサイズ

	private bool IsSquat = false;                                           //しゃがみ判定
	private bool IsJumping;                                     //空中にいるか判定
	private float Moveinput;                                    //移動入力
	private Vector2 Movedirection = Vector2.zero;             // 移動方向を記憶しておく

	private Rigidbody2D rb;                                     //Rigidbody2Dの格納
	private BoxCollider2D bc;                                   //BoxCollider2Dの格納庫
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
		if (IsGrounded() == true && IsSquat == false && IsJumping == false)
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
			Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		}


	}

	// トリガーが発生した時の処理
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 接触したオブジェクトのtag名がEnemyの場合は
		if (collision.gameObject.tag == "Enemy")
		{
			// Playerオブジェクトを消去する
			Destroy(gameObject);
		}
	}



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

		/*
		
		//下矢印キーが押されたら
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			//プレイヤーをしゃがませる
			Squat(true);
		}
		

		//下矢印キーを離したら
		if(Input.GetKeyUp(KeyCode.DownArrow))
		{
			Debug.Log("通った");
			//プレイヤーのしゃがみを辞めさせる
			Squat(false);
			
		}
		*/
	}

	/*
	private void Squat(bool squat)
	{
		IsSquat=squat;

		//しゃがみ時のサイズ変更
		transform.localScale = squat ? SquatSize : StandSize;

		//コライダーのサイズも変更する。
		if (squat)
		{
			bc.size = new Vector2(bc.size.x, SquatSize.y); //しゃがんだ時のサイズに変更
			Debug.Log("しゃがみ中");
		}
		
		else
		{
			bc.size = new Vector2(bc.size.x,StandSize.y); //元のサイズに戻す
		}
	}
	*/

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

