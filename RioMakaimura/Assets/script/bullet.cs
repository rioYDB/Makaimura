using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;     //槍の移動速度
	private float time;         //槍の消滅時間
	private float direction;    // 発射方向

	// Start is called before the first frame update
	void Start()
	{
		//プレイヤーから渡された方向に基づいて弾を発射
		direction = transform.localScale.x;  // プレイヤーの向きに合わせる
	}

	// Update is called once per frame
	void Update()
	{


		//プレイヤーの向きに合わせて弾のの移動
		transform.Translate(movespeed * direction , 0.0f, 0.0f);


	}

	//画面外に出たらこのオブジェクトを破壊
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	//敵に触れたらこのオブジェクトを破壊
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Enemyに触れたら破壊する
		if (collision.gameObject.tag == "Enemy")
		{
			Debug.Log("敵との衝突を検出");
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

}
