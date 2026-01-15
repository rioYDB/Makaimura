using UnityEngine;

public class Bullet_Which : bullet
{
    [SerializeField] public float HormingSpeed = 2.0f;   // ホーミングの回転速度
    public float InitialFlyTime = 0.2f;                 // 初速でまっすぐ飛ぶ時間
    private float initialFlyTimer = 0f;                 // 初速タイマー

    private Transform EnemyTarget;                      // ホーミング対象のエネミー
    private Vector3 currentMoveDirection;
    public LayerMask Ground;                            // 地面を判定するレイヤー

    [Header("Animation Curve Settings")]
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);
    // カーブの横軸（時間）を進める倍率
    [SerializeField] private float curveSpeedMultiplier = 1.0f;
    private float curveTimer = 0f;                      // 生成からの経過時間を記録する用

    protected override void Start()
    {
        // 親クラスのStart()を呼び出す
        base.Start();

        FindEnemy();

        // ★修正ポイント★
        // インスタンス化された時の「向き（角度）」をそのまま初期の移動方向に設定します。
        // これにより、player_control.cs で設定した 0度, 45度, 90度 が活かされます。
        currentMoveDirection = transform.right * movespeed;
    }

    protected override void Update()
    {
        // 1. 初速タイマーの更新
        if (initialFlyTimer < InitialFlyTime)
        {
            initialFlyTimer += Time.deltaTime;
            transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);
            return;
        }

        // 2. アニメーションカーブによる速度の計算
        curveTimer += Time.deltaTime * curveSpeedMultiplier;
        float curveMultiplier = speedCurve.Evaluate(curveTimer);
        float currentCurveSpeed = movespeed * curveMultiplier;

        // 3. ホーミング方向の計算（定義に合わせて引数なしで呼ぶ）
        Horming();

        // 4. 移動ベクトルの向きを維持しつつ、速度をカーブのものに更新
        if (currentMoveDirection != Vector3.zero)
        {
            currentMoveDirection = currentMoveDirection.normalized * currentCurveSpeed;
        }

        // 5. 移動処理
        transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);

        // 6. 弾の向きを進行方向に合わせる
        if (currentMoveDirection != Vector3.zero)
        {
            transform.right = currentMoveDirection;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1);
            }
        }
    }

    private void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistanceEnemy = Mathf.Infinity;
        Transform closestEnemy = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 directionToEnemy = enemy.transform.position - currentPosition;
            float distanceEnemy = directionToEnemy.sqrMagnitude;

            if (distanceEnemy < closestDistanceEnemy)
            {
                closestDistanceEnemy = distanceEnemy;
                closestEnemy = enemy.transform;
            }
        }
        EnemyTarget = closestEnemy;
    }

    // ★修正★ 引数なしの定義に統一
    private void Horming()
    {
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            EnemyTarget = null;
            // ターゲットがいない場合、現在の向きで直進を続ける（ゆらゆら処理は削除済み）
            return;
        }

        // ターゲットが存在する場合のホーミング処理
        Vector3 targetDirection = (EnemyTarget.position - transform.position).normalized;

        // 現在の向きをターゲット方向に徐々に回転させる
        currentMoveDirection = Vector3.RotateTowards(
            currentMoveDirection.normalized,
            targetDirection,
            HormingSpeed * Time.deltaTime,
            0.0f
        ).normalized;
        // ※速度の掛け合わせは Update 内で行うため、ここでは向き(normalized)のみ計算
    }
}