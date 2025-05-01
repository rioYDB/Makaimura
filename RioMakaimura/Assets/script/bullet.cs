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
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			return;
		}


		if (collision.gameObject.tag == "Enemy")
		{
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

}
