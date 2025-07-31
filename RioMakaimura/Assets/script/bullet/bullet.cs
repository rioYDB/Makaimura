using UnityEngine;

public abstract class bullet : MonoBehaviour　　　//抽象クラス(継承前提のクラス)
{
    public float movespeed;     //攻撃アイテムの移動速度
    protected float direction;    // 発射方向

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //プレイヤーから渡された方向に基づいて弾を発射
        direction = transform.localScale.x;  // プレイヤーの向きに合わせる
    }

    // Update is called once per frame
    protected virtual void Update()
    {


        //プレイヤーの向きに合わせて弾のの移動
        transform.Translate(movespeed * direction, 0.0f, 0.0f);


    }


    //敵に触れたらこのオブジェクトを破壊

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemyに触れたら破壊する
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("敵との衝突を検出");
            BulletMoves(collision.gameObject); //敵を消す
            Destroy(gameObject);//弾を消す
        }
    }


    //画面外に出たらこのオブジェクトを破壊
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


    protected abstract void BulletMoves(GameObject Enemy);   //抽象メソッド(子クラスに具体的な処理を書かせる)

}
