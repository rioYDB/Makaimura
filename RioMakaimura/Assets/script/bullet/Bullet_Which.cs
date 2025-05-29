using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  ホーミングの回転速度
    private Transform EnemyTarget;      //ホーミング対象のエネミー


    protected override void Start()
    {
        base.Start(); //親クラスのStart()を開始

        //子クラスのStart()内容も処理する
        FindEnemy(); //一番近いエネミーを探す関数の呼び出し


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();//親クラスのUpdate()を開始

        

        Horming(); //ホーミング処理の呼び出し
    }



    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("魔女でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }




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
        //もしホーミングするターゲットが見つかってないとき
        if (EnemyTarget == null)
        {
            FindEnemy();
            return;
        }

        Vector3 Direction =(EnemyTarget.position - transform.position).normalized;          //ターゲット方向のベクトルを計算

        float RotateStep =HormingSpeed*Time.deltaTime;                                      //

        Vector3 NewDirection = Vector3.RotateTowards(transform.position,Direction, RotateStep,0.0f);

        transform.right = NewDirection;
    }

}

