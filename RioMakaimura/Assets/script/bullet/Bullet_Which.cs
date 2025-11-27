using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   // ホーミングの回転速度
    public float InitialFlyTime = 0.2f; // ★追加★ 初速でまっすぐ飛ぶ時間
    private float initialFlyTimer = 0f; // 初速タイマー

    private Transform EnemyTarget;      // ホーミング対象のエネミー
    private Vector3 currentMoveDirection;
    public LayerMask Ground;            // 地面を判定するレイヤー

    //---------------------------------------------------------------------------------------------------------------------------

    //敵が画面内にいない状態はゆらゆら動く弾
    public float waveAmplitude = 5.0f; //ゆらゆらの幅
    public float waveSpeed = 0.5f;     //ゆらゆらの速度
    private float PositionY;           //弾のY座標を保持する

    protected override void Start()
    {
        // 親クラスのStart()を呼び出す (movespeed, direction の設定のため)
        base.Start();

        // ターゲットを探すのは Start() で一度だけ
        FindEnemy();

        // ★修正★ InitialFlyTime中は、現在の回転（transform.right）を移動方向とする
        // transform.right は、player_control.csで設定されたZ軸の回転方向を指します。
        currentMoveDirection = transform.right * movespeed;

        // ターゲットがいない場合のゆらゆら処理のためにY座標を保存
        PositionY = transform.position.y;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // ★追加★ 初速時間タイマーを更新
        if (initialFlyTimer < InitialFlyTime)
        {
            initialFlyTimer += Time.deltaTime;

            // 初速中は、生成時の角度（currentMoveDirection）を使って移動する
            transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);
            return; // 初速中はホーミング処理をスキップ
        }

        // 初速時間が終わったらホーミング処理を開始
        Horming();

        // ホーミング中でない場合に「ゆらゆら」処理を適用 (ターゲットがいない、または消滅した場合)
        if (EnemyTarget == null)
        {
            // X軸方向は currentMoveDirection (直進速度) を利用して進む
            float newX = transform.position.x + currentMoveDirection.x * Time.deltaTime;

            // Y軸方向はサイン波を使ってゆらゆら動く
            float waveY = Mathf.Sin(Time.time * waveSpeed) * waveAmplitude;
            float newY = PositionY + waveY;

            // XとYを組み合わせて移動させる 
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
        else // ホーミング中（ターゲットが存在する）
        {
            // 計算された移動方向で実際に弾を移動させる
            transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);

            // 弾の見た目の向きを進行方向（currentMoveDirection）に合わせる
            if (currentMoveDirection != Vector3.zero)
            {
                // transform.right を使うことで、Z軸の回転を移動方向と一致させる
                transform.right = currentMoveDirection;
            }
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            Debug.Log("ホーミング弾が地面に！");
            Destroy(gameObject); // 地面にトリガーしたらホーミング弾を消す
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {

            // 敵にEnemy_HPスクリプトがあるか確認
            enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1); // ダメージ量を1とする（必要に応じて変える）
            }

        }
    }



    //関数名：FindEnemy()
    //用途：一番近いエネミーを探す処理
    private void FindEnemy()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");        //シーン内のEnemyのタグをすべて取得する

        float ClosestDistanceEnemy = Mathf.Infinity;                            //最も近いEnemyを探すための初期設定

        Transform ClosestEmemy = null;

        Vector3 CurrentPosition = transform.position;                             //現在の位置


        foreach (GameObject enemeis in enemy)                                   //最も近いエネミーを見つける処理
        {

            // 対象となる敵が有効なゲームオブジェクトであるかチェック
            if (enemeis == null) continue; // もし敵が既に破壊されていたらスキップ

            Vector3 DirectionEnemy = enemeis.transform.position - CurrentPosition;      //現在の弾の位置からエネミーの距離を引いたベクトル

            float DistanceEnemy = DirectionEnemy.sqrMagnitude;                          //距離の二乗を計算


            //今まで近かったエネミーの距離よりDistanceEnemyの距離が短かったら
            if (DistanceEnemy < ClosestDistanceEnemy)
            {
                ClosestDistanceEnemy = DistanceEnemy;

                ClosestEmemy = enemeis.transform;  //最も近いエネミーを更新する
            }
        }

        EnemyTarget = ClosestEmemy;            //一番近いエネミーをターゲットにする
    }


    //関数名：Horming()
    //用途：ホーミング処理
    private void Horming()
    {
        // ターゲットが見つからない、またはターゲットが消滅していたら
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            EnemyTarget = null;
            // X軸方向だけは前の移動速度を維持し、Y軸はゆらゆら処理のためにリセット
            currentMoveDirection = new Vector3(currentMoveDirection.x, 0, 0);

            return;
        }

        // ターゲットが存在する場合のホーミング処理

        // ターゲットの方向を計算
        Vector3 targetDirection = (EnemyTarget.position - transform.position).normalized;

        // ホーミング速度で現在の移動方向をターゲット方向に近づける
        currentMoveDirection = Vector3.RotateTowards(
            currentMoveDirection.normalized,
            targetDirection,
            HormingSpeed * Time.deltaTime,
            0.0f
        ).normalized * movespeed;
    }
}