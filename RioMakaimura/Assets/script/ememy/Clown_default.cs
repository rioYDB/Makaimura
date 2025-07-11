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

    private Vector2 startPos;                  // 敵が初期にいた位置（移動の中心）
    private float direction = 1f;              // 今の移動方向（1：右、-1：左）
    private float timer;                       // 発射タイマー

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
        // 左右移動処理
        PatrolMove();

        // 発射タイマーを減らす
        timer -= Time.deltaTime;

        // タイマーが0以下なら弾を発射
        if (timer <= 0f)
        {
            Shoot();           // 弾を撃つ
            timer = fireInterval; // タイマーをリセット
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
            //Flip();           // 見た目も反転させる（画像が左右向きに変わる）
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

        // プレイヤー方向のベクトルを計算
        Vector2 dir = (player.position - shootPoint.position).normalized;

        // 弾は地面を転がす想定なので、上下成分はカット（水平のみ）
        dir.y = 0f;

        // 弾を発射位置に生成（プレハブからインスタンスを作る）
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // 弾のRigidbody2Dを取得して速度を与える
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * projectileSpeed;

        // プレイヤーのColliderと弾のColliderを無視するように設定
        Collider2D playerCol = player.GetComponent<Collider2D>();
        Collider2D projCol = projectile.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(projCol, playerCol);
    }
}
