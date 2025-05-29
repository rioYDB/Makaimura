using UnityEngine;

public class Bullet_Human : bullet
{
    protected override void Start()
    {
        base.Start(); //親クラスのStart()を開始

        //子クラスのStart()内容も処理する


    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常状態でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }


    protected override void Update()
    {
        base.Update();//親クラスのUpdate()を開始
    }
}
