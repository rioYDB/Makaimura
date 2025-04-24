using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//変数宣言
	public float moveSpeed;         //移動速度
	public float jumpPower;         //ジャンプ力
	public LayerMask Ground;        //地面を判別するオブジェクトレイヤー
	public GameObject bulletPrefab;	//槍のプレハブ



	Rigidbody2D rb;                 //Rigidbody2Dの格納

	void Start()
    {
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update()
	{
		//左キーが押されたら
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			//プレイヤーを左に移動させる
			transform.Translate(-moveSpeed, 0.0f, 0.0f);
		}
		//右キーが押されたら
		if (Input.GetKey(KeyCode.RightArrow))
		{
			//プレイヤーを右に移動させる
			transform.Translate(moveSpeed, 0.0f, 0.0f);
		}

		//ジャンプ処理
		if (IsGrounded() == true)
		{
			Jump();
		}

		//攻撃処理
		if (Input.GetKeyDown(KeyCode.Z))
		{
			GameObject bullet =Instantiate(bulletPrefab);
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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector2.up * jumpPower);
			//++jumpcount;
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
		return ret;
	}
}

