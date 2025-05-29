using UnityEngine;

public class Bullet_Okami : bullet
{
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
