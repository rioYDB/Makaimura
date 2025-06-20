using UnityEngine;

public class Bullet_Human : bullet
{

    public float bounceMoveSpeed = 5.0f;  //移動速度
    public float bounceHeight = 2.0f;     //バウンドする高さ
    public float bounceDuration = 1.0f;   //バウンドするまでの時間

    private Vector3 StartPos;             //弾が発射された位置
    private float StartTime;              //弾の生成された時間
    private float XDirection;             //弾のX方向
    protected override void Start()
    {
       

        


    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("通常状態でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }


    protected override void Update()
    {
        
    }
}
