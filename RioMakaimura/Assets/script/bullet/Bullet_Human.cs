using UnityEngine;

public class Bullet_Human : bullet
{



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
