using UnityEngine;

public class Bullet_Human : bullet
{

    public float BounceMoveSpeed = 5.0f;  //移動速度
    public float BounceHeight = 2.0f;     //バウンドする高さ
    public float BounceDuration = 1.0f;   //バウンドするまでの時間

    private Vector3 StartPos;             //弾が発射された位置
    private float StartTime;              //弾の生成された時間
    private float XDirection;             //弾のX方向


    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常状態でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }



    protected override void Start()
    {
        base.Start();                   //親クラスのStart()を呼び出す
        StartPos = transform.position;  //発射された位置を把握する
        StartTime = Time.time;          //弾の生成された時間を記憶する


    }



    protected override void Update()
    {
        //base .Update(); ←移動を制御したいから親クラスのUpdate()は呼び出さない

        float ElapsedTime = Time.time - StartTime;                  //弾が発射されてから何秒立ったか
        float NomalizedCycleTime = ElapsedTime / BounceDuration;    //弾が生まれてから何回バウンドしたか

        float XOffset = direction * BounceMoveSpeed　* ElapsedTime; //横方向(X軸)にどれくらいすすんだか？

        float YOffset = Mathf.Sin(NomalizedCycleTime * Mathf.PI) * BounceHeight; //サインカーブを使い、バウンドの動きを作る

        //Y座標がマイナスにならないように
        if (YOffset < 0)
        {
            YOffset = 0;
        }


        transform.position = StartPos + new Vector3(XOffset, YOffset);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        if(collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }
}
