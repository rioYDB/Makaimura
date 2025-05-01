using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;     //槍の移動速度
	private float time;         //槍の消滅時間

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//槍の移動
		transform.Translate(movespeed, 0.0f, 0.0f);


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
