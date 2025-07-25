using UnityEngine;

public class Bullet_Okami : bullet
{
    public float lifetime = 0.2f; // 爪攻撃の持続時間（短い）

    protected override void Start()
    {
        base.Start(); //親クラスのStart()を開始

        //子クラスのStart()内容も処理する

        

    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("狼男でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "tendril")
        {
            Destroy(collision.gameObject);
        }
    }


    protected override void Update()
    {
        //base.Update();//親クラスのUpdate()を開始


        Destroy(gameObject, lifetime); // 自動で削除

    }
}
