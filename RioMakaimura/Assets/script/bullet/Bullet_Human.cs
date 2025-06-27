using UnityEngine;

public class Bullet_Human : bullet
{

    public float BounceMoveSpeed = 0.0f;  //横方向の移動速度
    public float Bounceforce = 0.0f;     //バウンドする力
    public float Initialup = 0.0f;          //打ち出した時の上方向への力

    public LayerMask Ground;              //地面判定

    private Rigidbody2D rb;               //Rigitbody2Dを格納する変数


    


    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常攻撃でアタック");
        Destroy(Enemy);
        Destroy(gameObject);
    }



    protected override void Start()
    {
        base.Start();                       //親クラスのStart()を呼び出す

        rb = GetComponent<Rigidbody2D>();   //Rigidbody2D


        //初期速度を設定する
        rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, Initialup);

    }



    protected override void Update()
    {
        //Y軸の動きはRigidbody2D
        rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, rb.linearVelocity.y);


        //弾の見た目の向きは、X軸方向で固定しとく
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //地面レイヤーに設定されたオブジェクトに当たったかどうかをチェックする
        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            //上方向への力を加えてバウンドさせる
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Bounceforce);
            Debug.Log("地面に触れたよ👍笑");
        }
        
        if(collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }

}
