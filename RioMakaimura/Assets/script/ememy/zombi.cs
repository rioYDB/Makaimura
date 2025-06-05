using UnityEngine;

public class enemy_control : MonoBehaviour
{
	public float z_moveSpeed;       // 敵の移動速度
    public float riseDuration = 1f;      // 地面から上がる時間
    public float riseHeight = 0.5f;      // 上昇する高さ（地面からの距離）

    private Transform player;      // プレイヤーの座標取得
	private Vector2 moveDirection; // 移動方向を記録する

	Rigidbody2D rb;                 //Rigidbody2Dの格納
    private BoxCollider2D boxCollider;   // Colliderの参照

    private float riseElapsed = 0f;
    private bool isRising = true;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector3 originalPosition;

    void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

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

        // コライダーの元のサイズとオフセットを保存
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;

        // 初期状態はコライダー小さめ＆少し下にずらす（地面に埋まってる状態）
        boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
        boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - originalColliderSize.y / 4f);

        // ポジションも少し下げる
        originalPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, originalPosition.y - riseHeight, originalPosition.z);


        // 3秒後に自分を破壊する
        Destroy(gameObject, 3f);
	}

	// Update is called once per frame
	void Update()
	{
        if (isRising)
        {
            // 上昇処理
            riseElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(riseElapsed / riseDuration);

            // コライダーサイズとオフセットを補間して戻す
            boxCollider.size = new Vector2(originalColliderSize.x, Mathf.Lerp(originalColliderSize.y / 2f, originalColliderSize.y, t));
            boxCollider.offset = new Vector2(originalColliderOffset.x, Mathf.Lerp(originalColliderOffset.y - originalColliderSize.y / 4f, originalColliderOffset.y, t));

            // ポジションも上昇させる
            float newY = Mathf.Lerp(originalPosition.y - riseHeight, originalPosition.y, t);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (t >= 1f)
            {
                isRising = false;
                // 上昇完了したので移動開始
            }
        }
        else
        {
            if (player == null) return;

            // 上昇完了後はプレイヤーに向かって移動する
            transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;
        }

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

		// 自分がplayerに属している場合
		if (collision.gameObject.CompareTag("Player"))
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
