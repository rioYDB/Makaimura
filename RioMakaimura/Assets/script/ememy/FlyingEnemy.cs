using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float detectionRangeX = 10.0f; // プレイヤーを横方向に検知する範囲
    public float detectionRangeY = 5.0f;  // プレイヤーを縦方向に検知する範囲 (敵より上でも下でも)
    public float initialJumpForce = 8.0f; // プレイヤー発見時に上に跳ね上がる力 (推奨値: 8.0～15.0)
    public float diveForceX = 5.0f;       // ★修正：目標地点へ飛び込む横方向への「力」（追尾ではない）
    public float fallGravityScale = 5.0f; // 飛び込み開始後の重力スケール (推奨値: 3.0～8.0)
    public float destroyAfterHitDelay = 0.5f; // 地面着地後やプレイヤー接触後に消えるまでの時間

    private Rigidbody2D rb;
    private Transform player;
    private bool playerDetected = false;
    private bool hasBegunDiving = false; // ★修正：落下（飛び込み）を開始したかどうかのフラグ
    private Vector3 targetDivePosition;   // プレイヤー検知時の目標地点（X,Y座標）

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("FlyingEnemy: Rigidbody2D component is missing.", this);
            enabled = false;
            return;
        }

        rb.gravityScale = 0f; // 最初は落下しないように重力を0に
        rb.linearVelocity = Vector2.zero; // 最初は速度も0

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("FlyingEnemy: 'Player'タグのオブジェクトが見つかりません！", this);
            this.enabled = false;
        }
    }

    void Update()
    {
        if (player == null) return; // プレイヤーがいなければ何もしない

        if (!playerDetected) // プレイヤーをまだ発見していない状態
        {
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
            float distanceY = Mathf.Abs(player.position.y - transform.position.y);

            if (distanceX <= detectionRangeX && distanceY <= detectionRangeY)
            {
                playerDetected = true;
                hasBegunDiving = false; // 飛び込みフラグをリセット
                targetDivePosition = player.position; // プレイヤーを検知した位置を目標地点に設定

                // プレイヤー発見で、まず上に跳ね上げる！
                rb.gravityScale = 0f; // 跳ね上がり中は重力0
                rb.linearVelocity = new Vector2(0, initialJumpForce); // 上向きの初速を与える（X速度は0）
                Debug.Log("プレイヤー発見！跳ね上がり開始！目標地点: " + targetDivePosition);
            }
        }
        else // プレイヤー発見済み、行動中
        {
            // 最高点に達したか、Y速度が0以下になったら落下開始
            if (!hasBegunDiving && rb.linearVelocity.y <= 0)
            {
                rb.gravityScale = fallGravityScale; // 落下用重力を適用
                hasBegunDiving = true; // 飛び込み開始フラグを立てる
                Debug.Log("最高点到達、飛び込み開始！");

                // ★修正：飛び込み開始時に、目標地点へ向かうX方向への力を一度だけ加える
                float directionToTargetX = Mathf.Sign(targetDivePosition.x - transform.position.x);
                // rb.velocity = new Vector2(directionToTargetX * diveForceX, rb.velocity.y); // これは速度設定
                rb.AddForce(new Vector2(directionToTargetX * diveForceX, 0), ForceMode2D.Impulse); // ★Impulseで横方向に力を加える
            }
            // else (まだinitialJumpForceで上昇中の場合) は、X速度は0のまま
        }
    }

    // プレイヤーや地面との衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーに当たった場合
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("追尾敵がプレイヤーに当たった！");
            // ここにプレイヤーにダメージを与える処理を書く
            // 例: collision.gameObject.GetComponent<player_control>().TakeDamage(1);
            Destroy(gameObject); // 敵は消滅
        }
        // 地面に当たった場合
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) // Groundレイヤーに設定されていることを前提
        {
            Debug.Log("追尾敵が地面に着地！");
            // 地面に着地したら、一定時間後に消える
            rb.gravityScale = 0f; // 地面に着いたら重力を無効に
            rb.linearVelocity = Vector2.zero; // 速度をリセット
            // 見た目のアニメーションやエフェクトなどを再生
            Destroy(gameObject, destroyAfterHitDelay); // 少し経ってから消える
        }
    }

    // オプション：プレイヤーのColliderがIs Triggerの場合のダメージ判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerDetected) // プレイヤーに触れてダメージ（検出後）
        {
            Debug.Log("追尾敵がプレイヤーに触れた！(トリガー)");
            // ここにプレイヤーにダメージを与える処理を書く
            // Destroy(gameObject);
        }
    }
}