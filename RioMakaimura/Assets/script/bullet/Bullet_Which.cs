using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  ホーミングの回転速度
    private Transform EnemyTarget;      //ホーミング対象のエネミー
    private Vector3 currentMoveDirection;
    public LayerMask Ground;            //地面を判定するレイヤー

    //---------------------------------------------------------------------------------------------------------------------------

    //敵が画面内にいない状態はゆらゆら動く弾
    public float waveAmplitude = 5.0f; //ゆらゆらの幅
    public float waveSpeed = 0.5f;     //ゆらゆらの速度
    private float PositionY;           //弾のY座標を保持する

    protected override void Start()
    {
        //base.Start(); //親クラスのStart()を開始

        //子クラスのStart()内容も処理する
        FindEnemy(); //一番近いエネミーを探す関数の呼び出し

        // ターゲットが見つかったら初期の移動方向をセット
        if (EnemyTarget != null)
        {
            // 親クラスの movespeed を使って初期の移動方向を決定
            currentMoveDirection = (EnemyTarget.position - transform.position).normalized * movespeed;
        }
        else
        {
            //親クラスのdirectionを使い前進する
            float initialDirectionX = transform.localScale.x > 0 ? 1f : -1f; // プレイヤーの向きを利用

            // 前進方向をセット（Y軸はゼロ）
            currentMoveDirection = new Vector3(initialDirectionX * movespeed, 0, 0);
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();//親クラスのUpdate()を開始

        Horming(); //ホーミング処理の呼び出し

        // ホーミング中でない場合に「ゆらゆら」処理を適用
        if (EnemyTarget == null)
        {
            // X軸方向は currentMoveDirection (前進速度) を利用して進む
            float newX = transform.position.x + currentMoveDirection.x * Time.deltaTime;

            // Y軸方向はサイン波を使ってゆらゆら動く
            float waveY = Mathf.Sin(Time.time * waveSpeed) * waveAmplitude;
            float newY = PositionY + waveY;

            // XとYを組み合わせて移動させる (Zは無視)
            transform.position = new Vector3(newX, newY, transform.position.z);

            // 移動に合わせて弾の向きを調整するロジックは、ホーミング中ではないので一旦保留
            // ゆらゆら移動時に見た目を追従させたい場合は、更に計算が必要です。
        }
        else // ホーミング中（ターゲットが存在する）
        {
            // 計算された移動方向で実際に弾を移動させる
            transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);

            // 弾の見た目の向きを進行方向（X軸）に合わせる
            if (currentMoveDirection != Vector3.zero)
            {
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

            //BulletMoves(collision.gameObject);

            // 敵にEnemy_HPスクリプトがあるか確認
            enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1); // ダメージ量を1とする（必要に応じて変える）
            }

        }
    }



    //protected override void BulletMoves(GameObject Enemy)
    //{
    //    Debug.Log("魔女でアタック！！！");

    //    Destroy(gameObject);
    //}




    //関数名：FindEnemy()
    //用途：一番近いエネミーを探す処理
    //引数：なし
    //戻り値：なし
    private void FindEnemy()
    {



        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");        //シーン内のEnemyのタグをすべて取得する

        float ClosestDistanceEnemy = Mathf.Infinity;                            //最も近いEnemyを探すための初期設定。Mathf.Infinityはまだ見つかっていない状態

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
    //引数：なし
    //戻り値：なし
    private void Horming()
    {
        // もしホーミングするターゲットが見つからない、またはターゲットが消滅していたら
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            // ターゲットをリセット
            EnemyTarget = null;

            // ★修正★ ターゲットが見つからなくても、現在のX軸移動方向（前進）は維持する
            float lastDirectionX = currentMoveDirection.x;
            FindEnemy(); // 新しいターゲットを探す

            if (EnemyTarget == null) // 新しいターゲットも見つからなければ
            {
                // X軸方向だけは前の移動速度を維持し、Y軸はゆらゆら処理のためにリセット
                currentMoveDirection = new Vector3(lastDirectionX, 0, 0);
                return; // ホーミング処理をここで終了
            }
        }

        // ターゲットが存在する場合のホーミング処理

        // ターゲットの方向を計算
        Vector3 targetDirection = (EnemyTarget.position - transform.position).normalized;

        // ホーミング速度で現在の移動方向をターゲット方向に近づける
        currentMoveDirection = Vector3.RotateTowards(
            currentMoveDirection.normalized,
            targetDirection,
            HormingSpeed * Time.deltaTime, // ホーミングスピードをTime.deltaTimeで割ることで、フレームレートに依存しないようにします
            0.0f
        ).normalized * movespeed;
    }
}



