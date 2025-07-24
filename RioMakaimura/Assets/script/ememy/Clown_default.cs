using UnityEngine;

public class Clown_default : MonoBehaviour
{
    public Transform player;                   // プレイヤーの位置を取得するため
    public GameObject projectilePrefab;        // 横方向の弾
    public GameObject axePrefab;            // 斧（放物線用）
    public Transform shootPoint;               // 弾を発射する位置

    public float moveSpeed = 2f;               // 左右に動く速さ
    public float moveRange = 3f;               // 左右の移動範囲（中心からどれだけ離れるか）
    public float fireInterval = 2f;            // 弾を撃つ間隔（秒）
    public float projectileSpeed = 5f;         // 弾のスピード
    public float axeSpeedX = 4f;            // 斧の横速度
    public float axeSpeedY = 6f;            // 斧の縦初速
    public float stopBeforeShootTime = 0.5f; // 撃つ前に止まる時間

    private Vector2 startPos;                  // 敵が初期にいた位置（移動の中心）
    private float direction = 1f;              // 今の移動方向（1：右、-1：左）
    private float timer;                       // 発射タイマー

    private bool isShooting = false;
    private float stopTimer = 0f;

    // 攻撃タイプ列挙（通常 or 斧）
    enum AttackType 
    {
        Bullet,
      　Axe 
    }
    private AttackType currentAttack;

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
                PerformAttack();         // ランダムな攻撃を実行
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
            // 攻撃準備（止まってランダム選択）
            isShooting = true;                      // 停止モードに入る
            stopTimer = stopBeforeShootTime;        // 停止時間を設定

            // 攻撃タイプをランダムに決定
            currentAttack = (Random.value < 0.5f) ? AttackType.Bullet : AttackType.Axe;

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
            
        }
    }

    // 実際に攻撃を実行
    void PerformAttack()
    {
        switch (currentAttack)
        {
            case AttackType.Bullet:
                ShootBullet();
                break;
            case AttackType.Axe:
                ThrowAxe();
                break;
        }
    }



    void ShootBullet()
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

       
    }

    // 斧を放物線で投げる
    void ThrowAxe()
    {
        if (player == null) return;

        // プレイヤーへの方向ベクトル（左右方向）
        Vector2 dir = (player.position - transform.position).normalized;

        // 発射位置を少し前にずらす
        float offset = 0.5f;
        Vector2 shootPos = (Vector2)transform.position + dir * offset;

        GameObject axe = Instantiate(axePrefab, shootPos, Quaternion.identity);

        // Rigidbody に放物線の速度を与える（横：dir.x * speedX、縦：speedY）
        Rigidbody2D rb = axe.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(dir.x * axeSpeedX, axeSpeedY);

        // 斧を回転させる（角速度を設定）
        float spinSpeed = 360f; // 角速度（°/秒） 正で時計回り、負で反時計回り
        rb.angularVelocity = -spinSpeed; // 好みで反転
    }
}
