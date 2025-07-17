using UnityEngine;

public class Bullet_Human : bullet
{
    // ★★★ Inspectorで設定する変数 ★★★
    public float BounceMoveSpeed = 5.0f;  // 横方向の初期速度（初速のみに使う）
    public float BounceForceY = 8.0f;     // 地面からのバウンドする上方向の力
    public float InitialUpForce = 5.0f;   // 最初に弾を撃ち出した時の上方向の力

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

        // ★★★修正：初期速度を設定する。Updateで速度を固定しない！★★★
        // X方向にはInitialUpForceの代わりにBounceMoveSpeedを直接加える
        rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, InitialUpForce);

        // 時間経過で自動的に消えるように設定
        Destroy(gameObject, TotalLifetime);
    }

    protected override void Update()
    {
        // ★★★修正：Update()では速度を直接固定しない！★★★
        // 物理的な跳ね返りを優先するため、rb.velocity.x を毎フレーム上書きするのをやめる。
        // rb.velocity = new Vector2(direction * BounceMoveSpeed, rb.linearVelocity.y); // この行を削除

        // 弾の見た目の向きは、X軸方向で固定 (Physics Material 2DのBouncinessで回転しないように)
        // または、移動方向に合わせて回転させる
        if (rb.linearVelocity.x != 0) // 弾が動いている時だけ向きを変える
        {
            // 移動方向 (rb.velocity.x) に合わせてlocalScale.x を反転
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(rb.linearVelocity.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面レイヤーに設定されたオブジェクトに当たったかどうかをチェックする
        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            currentBounces++; // バウンド回数をカウントアップ
            Debug.Log("地面/壁に触れたよ👍笑 - バウンド回数: " + currentBounces + " (Max: " + MaxBounces + ")");

            // 最大バウンド回数に達したら弾を消す
            if (currentBounces >= MaxBounces)
            {
                Destroy(gameObject);
                return; // これ以上バウンドさせない
            }

            // ★★★修正：壁・地面からの跳ね返りロジックを強化する！★★★
            // 当たった面の法線ベクトルを取得
            Vector2 surfaceNormal = collision.contacts[0].normal;

            // 入射ベクトル（衝突前の速度）を法線ベクトルで反射させる
            Vector2 reflectedVelocity = Vector2.Reflect(rb.linearVelocity, surfaceNormal);

            // 跳ね返り速度の適用
            // 跳ね返り後の速度の大きさを維持しつつ、反射方向に向かせる
            // 例えば、元の速度の大きさにBounceForceYで上向きの力を加えるなど
            float currentSpeedMagnitude = rb.linearVelocity.magnitude;

            // X方向の反射とY方向のバウンド力を組み合わせる
            // 法線が垂直に近い（壁）ならXを反転させ、Yは維持
            // 法線が水平に近い（地面）ならXは維持し、YをBounceForceYに
            Vector2 finalBounceVelocity;

            if (Mathf.Abs(surfaceNormal.x) > Mathf.Abs(surfaceNormal.y)) // 壁に近い（水平方向の法線が強い）
            {
                // 壁に当たった場合、X方向は反射させ、Y方向は現在の速度を維持
                finalBounceVelocity = new Vector2(reflectedVelocity.x, rb.linearVelocity.y);
                // ただし、X速度の大きさは少なくともInitialMoveSpeed相当にする
                finalBounceVelocity.x = Mathf.Sign(finalBounceVelocity.x) * Mathf.Max(Mathf.Abs(finalBounceVelocity.x), BounceMoveSpeed);
            }
            else // 地面に近い（垂直方向の法線が強い）
            {
                // 地面に当たった場合、Y方向をBounceForceYに設定し、X方向は現在の速度を維持
                finalBounceVelocity = new Vector2(rb.linearVelocity.x, BounceForceY);
                // Y速度が既に十分な場合は上書きしない
                if (finalBounceVelocity.y < BounceForceY) finalBounceVelocity.y = BounceForceY;
            }

            rb.linearVelocity = finalBounceVelocity; // 最終的な跳ね返り速度を適用

            // Debug.Log("反射！ 反射ベクトル: " + reflectedVelocity + ", 法線: " + surfaceNormal + ", 最終速度: " + rb.linearVelocity);
        }

        // 敵に当たった時の処理
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }

    // ★★★修正：OnTriggerEnter2Dをオーバーライドして、親のDestroyを呼ばないようにする★★★
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // base.OnTriggerEnter2D(collision); // 親のDestroy(gameObject)が呼ばれるのを防ぐ

        // 敵に当たった時だけBulletMovesを呼ぶ
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }
}