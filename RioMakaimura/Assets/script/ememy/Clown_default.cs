using UnityEngine;

public class Clown_default : MonoBehaviour
{
    public Transform player;                   // プレイヤーの位置を取得するため
    public GameObject projectilePrefab;        // 弾のPrefab（プレハブ）
    public Transform shootPoint;               // 弾を発射する位置

    public float moveSpeed = 2f;               // 左右に動く速さ
    public float moveRange = 3f;               // 左右の移動範囲（中心からどれだけ離れるか）
    public float fireInterval = 2f;            // 弾を撃つ間隔（秒）
    public float projectileSpeed = 5f;         // 弾のスピード
    public float stopBeforeShootTime = 0.5f; // 撃つ前に止まる時間

    private Vector2 startPos;                  // 敵が初期にいた位置（移動の中心）
    private float direction = 1f;              // 今の移動方向（1：右、-1：左）
    private float timer;                       // 発射タイマー

    private bool isShooting = false;
    private float stopTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 初期位置を記録
        startPos = transform.position;

        // 最初の発射までのカウントダウン（すぐ撃たないように）
        timer = fireInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // 発射準備中（停止中）の処理
        if (isShooting)
        {
            // 待機時間のカウントダウン
            stopTimer -= Time.deltaTime;

            // カウント終了で弾を撃つ
            if (stopTimer <= 0f)
            {
                Shoot();                   // 弾を発射
                isShooting = false;       // 発射完了（再び移動開始）
                timer = fireInterval;     // 次の発射までの時間をリセット
            }

            return; // 停止中は移動もしない
        }

        // 移動処理
        PatrolMove();

        // 弾の発射タイマー更新
        timer -= Time.deltaTime;

        // 一定時間ごとに止まって撃つ準備に入る
        if (timer <= 0f)
        {
            isShooting = true;                      // 停止モードに入る
            stopTimer = stopBeforeShootTime;        // 停止時間を設定
        }
    }

    void PatrolMove()
    {
        // 今の方向にスピードをかけて横方向に移動する
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // 初期位置からどれだけ離れたか確認し、範囲を超えたら方向反転
        if (Mathf.Abs(transform.position.x - startPos.x) > moveRange)
        {
            direction *= -1f; // 向きを逆にする
            Flip();           // 見た目も反転させる（画像が左右向きに変わる）
        }
    }

    void Flip()
    {
        // 左右のスケールを反転して見た目を反転させる
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    void Shoot()
    {
        // プレイヤーがいなければ撃たない
        if (player == null) return;

        // プレイヤー方向を取得（弾は横にだけ飛ばす）
        Vector2 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        // 弾の発射位置 = ピエロの位置 + プレイヤー方向に少し進めた位置
        float offset = 0.5f; // プレイヤー方向にどれだけ前に出すか（調整可）
        Vector2 shootPos = (Vector2)transform.position + dir * offset;

        // 弾を生成
        GameObject projectile = Instantiate(projectilePrefab, shootPos, Quaternion.identity);

        // 弾のRigidbody2Dを取得して速度を与える
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * projectileSpeed;

        //// プレイヤーのColliderと弾のColliderを無視するように設定
        //Collider2D playerCol = player.GetComponent<Collider2D>();
        //Collider2D projCol = projectile.GetComponent<Collider2D>();
        //Physics2D.IgnoreCollision(projCol, playerCol);
    }
}
