using System;
using UnityEngine;

public class enemy_control : MonoBehaviour
{
    public float z_moveSpeed;       // 敵の移動速度
    public float riseDuration;      // 地面から上がる時間
    public float riseHeight;      // 上昇する高さ（地面からの距離）

    private Transform player;      // プレイヤーの座標取得
    private Vector2 moveDirection; // 移動方向を記録する

    Rigidbody2D rb;                 //Rigidbody2Dの格納
    private CapsuleCollider2D capsuleCollider;   // ColliderをCapsuleに変更

    public LayerMask Ground; // ← インスペクターから地面レイヤーを指定
    private bool isGrounded = false;


    private float riseElapsed = 0f;
    private bool isRising = true;
    private bool isSinking = false;
    private float sinkElapsed = 0f;
    public float sinkDuration = 1f;  // 沈む時間
    private bool sinkRequested = false; // 地面についたら沈むよう予約

    private float originalHeight;
    private float originalWidth;
    private Vector2 originalOffset;
    private Vector3 originalPosition;

    private Vector3 sinkStartPosition; // 現在位置を沈下開始地点として記録

    public delegate void OnDesloyDelegate(GameObject g);
    public OnDesloyDelegate mOnDestly = new  OnDesloyDelegate((GameObject g) =>{ });

    void Start()
	{
		//アタッチされているComponentを取得
		rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();


        GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			player = playerObj.transform;
		}

		// 最初に向かう方向を計算して固定
		if (player != null)
		{
			moveDirection = (player.position - transform.position).normalized;

            moveDirection.y = 0f;            // Y成分をゼロにする
            moveDirection = moveDirection.normalized; // 正規化する
        }

        // 元の高さ・幅・オフセットを保存
        originalHeight = capsuleCollider.size.y; // sizeがある場合
        originalWidth = capsuleCollider.size.x;  // sizeがある場合

        // もし size プロパティがない環境なら capsuleCollider.height, capsuleCollider.widthを使う
        // capsuleCollider.height = originalHeight; capsuleCollider.width = originalWidth; 



        // 初期状態はコライダーを小さく＆少し下にずらす（地面に埋まった状態）
        capsuleCollider.size = new Vector2(originalWidth, originalHeight / 2f);
        capsuleCollider.offset = new Vector2(originalOffset.x, originalOffset.y - originalHeight / 4f);

        // ポジションも少し下げる
        originalPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, originalPosition.y - riseHeight, originalPosition.z);

        //最初は上昇中なのでキネマティックで制御
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 5秒後に自分を破壊する
        Invoke("StartSinking", 5f);
    }

	// Update is called once per frame
	void Update()
	{
        Vector2 colliderSize = capsuleCollider.size * transform.lossyScale;

        // 接地チェック（ゾンビの足元から真下へ Ray を飛ばす）
        //Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - capsuleCollider.size.y / 2f + capsuleCollider.offset.y);
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - colliderSize.y / 2f);
        isGrounded = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, Ground);

        if (isRising)
        {
            // 上昇処理
            riseElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(riseElapsed / riseDuration);

            // コライダーサイズとオフセットを補間して戻す
            capsuleCollider.size = new Vector2(originalWidth, Mathf.Lerp(originalHeight / 2f, originalHeight, t));
            capsuleCollider.offset = new Vector2(originalOffset.x, Mathf.Lerp(originalOffset.y - originalHeight / 4f, originalOffset.y, t));

            // ポジションも上昇させる
            float newY = Mathf.Lerp(originalPosition.y - riseHeight, originalPosition.y, t);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (t >= 1f)
            {
                //上昇完了したのでDynamicに変更
                rb.bodyType = RigidbodyType2D.Dynamic;

                isRising = false;
                // 上昇完了したので移動開始
            }
        }
        else if (isSinking)
        {
            // 下昇処理
            sinkElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(sinkElapsed / sinkDuration);

            // コライダー縮小と位置下降
            capsuleCollider.size = new Vector2(originalWidth, Mathf.Lerp(originalHeight, originalHeight / 2f, t));
            capsuleCollider.offset = new Vector2(originalOffset.x, Mathf.Lerp(originalOffset.y, originalOffset.y - originalHeight / 4f, t));


            float newY = Mathf.Lerp(sinkStartPosition.y, sinkStartPosition.y - riseHeight, t);
            transform.position = new Vector3(sinkStartPosition.x, newY, sinkStartPosition.z);


            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (player == null) return;

            // 上昇完了後はプレイヤーに向かって移動する
            transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

            // 地面に接地していて沈下予約があれば沈む
            if (sinkRequested && isGrounded && !isSinking)
            {
                StartSinking();
                sinkRequested = false; // 一度開始したらフラグを戻す
            }
        }

        // 方向転換をしないようにするため、回転処理は無効化
        transform.rotation = Quaternion.identity; // 常に回転をリセット（固定）
	}

    private void OnDestroy()
    {
        //破棄直前のデリゲートを呼び出す
        mOnDestly(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
        // 自分が敵レイヤーに属している場合
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //	// 衝突を無視する
        //	Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
        //}

        // 自分がplayerに属している場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // 衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
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
        if (!isSinking && !isRising)
        {
            StartSinking();
        }
    }

    void StartSinking()
    {
        Debug.Log("StartSinking called - grounded? " + isGrounded);
        if (isSinking) return;

        if (isGrounded)
        {
            isSinking = true;
            sinkElapsed = 0f;
            sinkStartPosition = transform.position; // 現在位置を基準に沈下
            Debug.Log("Sink started!");
        }
        else
        {
            sinkRequested = true; // 今は空中なので、地面についたら沈ませる
            Debug.Log("Sink requested but not grounded yet.");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (capsuleCollider == null) return;

        // 接地チェック（ゾンビの足元から真下へ Ray を飛ばす）
        Vector2 colliderSize = capsuleCollider.size * transform.lossyScale;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - colliderSize.y / 2f);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * 0.1f);
    }


}
