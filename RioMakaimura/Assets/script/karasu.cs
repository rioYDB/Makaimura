using UnityEngine;

public class karasu : MonoBehaviour
{
	public float e_moveSpeed;       // 敵の移動速度

	Rigidbody2D rb;                 //Rigidbody2Dの格納
	Renderer Ren;

	void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();

	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(-e_moveSpeed, 0.0f, 0.0f);      //横に移動
		transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 0.3f), transform.position.z);      //ふわふわ飛んでるように見せる
	}
	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}
}
