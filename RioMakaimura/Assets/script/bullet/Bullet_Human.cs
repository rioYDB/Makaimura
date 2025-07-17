using UnityEngine;

public class Bullet_Human : bullet
{
    
    public float BounceMoveSpeed = 5.0f;  // 横方向の移動速度 (デフォルト値を5.0fに推奨)
    public float BounceForceY = 8.0f;     // バウンドする上方向の力 (デフォルト値を8.0fに推奨)
    public float InitialUpForce = 5.0f;   // 最初に弾を撃ち出した時の上方向の力 (デフォルト値を5.0fに推奨)


    public LayerMask Ground;              // 地面・壁判定用レイヤー (Inspectorで設定)

    public float TotalLifetime = 3.0f;    // 弾の総生存時間 (この時間経過で消える)
    public int MaxBounces = 3;            // バウンドする最大回数 (この回数バウンドしたら消える)

    private Rigidbody2D rb;               // Rigidbody2Dを格納する変数
    private int currentBounces = 0;       // 現在のバウンド回数

    private Collider2D playerCollider; // プレイヤーのCollider2Dへの参照

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常攻撃でアタック");
        Destroy(Enemy);
        Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start(); // 親クラスのStart()を呼び出す (direction の設定のため)

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得
        if (rb == null)
        {
            Debug.LogError("Bullet_HumanにはRigidbody2Dが必要！", this);
            return;
        }

        // プレイヤーのCollider2Dを取得し、衝突を無視するように設定
        GameObject player = GameObject.FindWithTag("Player"); // "Player"タグのオブジェクトを探す
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                // 弾自身のColliderとプレイヤーのColliderの衝突を無視する
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
            }
            else
            {
                Debug.LogWarning("プレイヤーオブジェクトにCollider2Dが見つかりません。", player);
            }
        }
        else
        {
            Debug.LogWarning("'Player'タグのオブジェクトが見つかりません。");
        }


        // 初期速度を設定する
        rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, InitialUpForce); // rb.velocityを使用

        // 時間経過で自動的に消えるように設定
        Destroy(gameObject, TotalLifetime);
    }

    protected override void Update()
    {
        // 水平速度は毎フレーム維持する (物理演算による減衰がある場合も考慮)
        rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, rb.linearVelocity.y);

        // 弾の見た目の向きは、X軸方向で固定
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面レイヤーに設定されたオブジェクトに当たったかどうかをチェックする
        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            currentBounces++; // バウンド回数をカウントアップ
            Debug.Log("地面/壁に触れたよ👍笑 - バウンド回数: " + currentBounces);

            // 最大バウンド回数に達したら弾を消す
            if (currentBounces >= MaxBounces)
            {
                Destroy(gameObject);
                return; // これ以上バウンドさせない
            }

            // バウンドする力を加える（跳ね返り処理）
            // 当たった面の法線ベクトルを取得
            Vector2 surfaceNormal = collision.contacts[0].normal;

            // 跳ね返り速度の適用 (減衰なし)
            // Y方向の上向きバウンド力を優先しつつ、X軸の反射も考慮する
            // 法線が垂直に近い（壁）ならXを反転させ、Yは維持
            // 法線が水平に近い（地面）ならXは維持し、YをBounceForceYに
            if (Mathf.Abs(surfaceNormal.x) > Mathf.Abs(surfaceNormal.y)) // 壁に近い（水平方向の法線が強い）
            {
                // 壁に当たった場合、X方向を反転させ、Y方向は現在の速度を維持
                // rb.velocity.x はUpdateで強制的にBounceMoveSpeedに設定し続けるため、
                // ここでは Y だけ調整する (減衰なし)
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y); // X方向のみ反転 (減衰なし)
            }
            else // 地面に近い（垂直方向の法線が強い）
            {
                // 地面に当たった場合、Y方向をバウンド力に設定し、X方向は維持 (減衰なし)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, BounceForceY); // Y方向の速度をバウンド力に設定
            }

            // Debug.Log("反射！ 法線: " + surfaceNormal);
        }

        // 敵に当たった時の処理
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }
}