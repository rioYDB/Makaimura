using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  ホーミングの回転速度
    private Transform EnemyTarget;      //ホーミング対象のエネミー
    private Vector3 currentMoveDirection;
    public LayerMask Ground;            //地面を判定するレイヤー


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
            // ターゲットが見つからない場合は、一旦弾を止めるか、特定の方向へ進ませるか
            // 停止させて、Updateで再探索を試みる
            currentMoveDirection = Vector3.zero;
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();//親クラスのUpdate()を開始

        Horming(); //ホーミング処理の呼び出し

        // 計算された移動方向で実際に弾を移動させる
        transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);
        // Space.World を指定することで、弾のローカル座標ではなくワールド座標で移動します。

        // 弾の見た目の向きを進行方向（X軸）に合わせる
        // currentMoveDirectionの方向に弾のX軸（右方向）を向かせます。
        if (currentMoveDirection != Vector3.zero) // 無駄な回転を防ぐため
        {
            transform.right = currentMoveDirection;
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

        Transform ClosestEmemy =null;

        Vector3 CurrentPosition=transform.position;                             //現在の位置


        foreach (GameObject enemeis in enemy)                                   //最も近いエネミーを見つける処理
        {

            // 対象となる敵が有効なゲームオブジェクトであるかチェック
            if (enemeis == null) continue; // もし敵が既に破壊されていたらスキップ

            Vector3 DirectionEnemy = enemeis.transform.position - CurrentPosition;      //現在の弾の位置からエネミーの距離を引いたベクトル

            float DistanceEnemy = DirectionEnemy.sqrMagnitude;                          //距離の二乗を計算


            //今まで近かったエネミーの距離よりDistanceEnemyの距離が短かったら
            if (DistanceEnemy < ClosestDistanceEnemy)                                   
            {
                ClosestDistanceEnemy=DistanceEnemy;

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
        // もしホーミングするターゲットが見つかってない、またはターゲットが消滅していたら
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            // 現在の移動方向を保持したまま、新しいターゲットを探す
            // ★ここを変更★
            // ターゲットが見つからなくても、現在の移動方向は変更しない
            Vector3 lastMoveDirection = currentMoveDirection; // 最後に追っていた方向を保持
            FindEnemy(); // 新しいターゲットを探す

            if (EnemyTarget == null) // 新しいターゲットも見つからなければ、最後の方向へ進み続ける
            {
                currentMoveDirection = lastMoveDirection; // 保持していた方向を維持
                return;
            }
        }

        // ターゲットの方向を計算し、親クラスの movespeed を使って移動方向に設定する
        currentMoveDirection = (EnemyTarget.position - transform.position).normalized * movespeed;
    }
}



