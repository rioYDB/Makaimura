using UnityEngine;

public class enemy_control : MonoBehaviour
{
	public float z_moveSpeed;       // 敵の移動速度
	private Transform player;      // プレイヤーの座標取得
	private Vector2 moveDirection; // 移動方向を記録する

	Rigidbody2D rb;                 //Rigidbody2Dの格納

	void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();

		GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			player = playerObj.transform;
		}

		// 最初に向かう方向を計算して固定
		if (player != null)
		{
			moveDirection = (player.position - transform.position).normalized;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (player == null) return;

		// 移動方向を保持し続ける
		transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

		// 方向転換をしないようにするため、回転処理は無効化
		transform.rotation = Quaternion.identity; // 常に回転をリセット（固定）
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// 自分が敵レイヤーに属している場合
		if (collision.gameObject.CompareTag("Enemy"))
		{
			// 衝突を無視する
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}

		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Haka")
		{
			Debug.Log("aya");

			// 反応しない（何もしない）
			return;
		}
	}

	void OnBecameInvisible()
	{
		// 画面外に出たら自動的に敵を破壊する
		Destroy(gameObject);
	}
}
