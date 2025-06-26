using UnityEngine;

public class Bullet_Human : bullet
{

    public float BounceMoveSpeed = 5.0f;  //移動速度
    public float Bounceforce = 2.0f;     //バウンドする高さ
    public float Initialup = 1.0f;          //バウンドするまでの時間

    public LayerMask Ground;              //地面判定する変数

    private Rigidbody2D rb;               //Rigitbody2D


    //private Vector3 StartPos;             //弾が発射された位置
    //private float StartTime;              //弾の生成された時間
    //private float XDirection;             //弾のX方向


    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常状態でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }



    protected override void Start()
    {
        base.Start();                       //親クラスのStart()を呼び出す

        //rb = GetComponent<Rigidbody2D>();   //Rigidbody2Dを取得する


        //初期速度を設定
        //rb.linearVelocity = new Vector2 (direction * BounceMoveSpeed,Initialup);



        ////StartPos = transform.position;  //発射された位置を把握する
        ////StartTime = Time.time;          //弾の生成された時間を記憶する


    }



    protected override void Update()
    {
        //Y軸の動きはRigidbody2Dに任せる
        //rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, rb.linearVelocity.y);




        base.Update(); //←移動を制御したいから親クラスのUpdate()は呼び出さない




    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //base.OnTriggerEnter2D(collision);

    //    if(collision.gameObject.CompareTag("Enemy"))
    //    {
    //        BulletMoves(collision.gameObject);
    //    }
    //}
}
