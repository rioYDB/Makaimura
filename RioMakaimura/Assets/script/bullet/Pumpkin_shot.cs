using UnityEngine;


//かぼちゃ弾(壁に触れたら跳ね返るのではなく破壊される)
public class Pumpkin_shot : bullet
{
    public float Movespeed;               //かぼちゃの移動速度
    public float Upforce;                 //最初に上方向に掛ける力
    public float BounceForceY = 8.0f;     // 地面からのバウンドする上方向の力

    public float Bullet_Lifetime;         //かぼちゃの生存時間
    public int MaxBounces = 3;            // バウンドする最大回数 (この回数バウンドしたら消える)
    private int currentBounces = 0;       // 現在のバウンド回数


    public LayerMask Ground;              // 地面判定用レイヤー
    public LayerMask Wall;                //壁判定のレイヤー

    private Rigidbody2D rb;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Debug.LogError("Rigidbody2Dがついてないよ");
        }


        //プレイヤーとの衝突をしないようにする
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
            }
        }

        rb.linearVelocity = new Vector2(direction * Movespeed , Upforce);

        Destroy(gameObject,Bullet_Lifetime);


    }



    protected override void Update()
    {
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

            //ここを追加して、何に当たってるか見る
            Debug.Log("カボチャが衝突した相手: " + collision.gameObject.name + " (Layer: " + LayerMask.LayerToName(collision.gameObject.layer) + ")");

            currentBounces++; // バウンド回数をカウントアップ
            Debug.Log("地面/壁に触れたよ👍笑 - バウンド回数: " + currentBounces + " (Max: " + MaxBounces + ")");

            // 最大バウンド回数に達したら弾を消す
            if (currentBounces >= MaxBounces)
            {
                Destroy(gameObject);
                return; // これ以上バウンドさせない
            }


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
                finalBounceVelocity.x = Mathf.Sign(finalBounceVelocity.x) * Mathf.Max(Mathf.Abs(finalBounceVelocity.x), Movespeed);
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


        //WallLに設定されたオブジェクトに当たったかどうかをチェックする
        if (((1 << collision.gameObject.layer) & Wall) != 0)
        {
            // 当たったのが壁レイヤーの場合のみ破壊
            Debug.Log("壁に衝突！弾を破壊します。");
            Destroy(gameObject);
            return;
        }



        // 敵に当たった時の処理
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 敵にEnemy_HPスクリプトがあるか確認
            enemy_HP enemyHP = collision.gameObject.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1); // ダメージ量を1とする（必要に応じて変える）
            }

            Destroy(gameObject); // 弾を削除
        }
    }


}
