using UnityEngine;

public class Bullet_Human : bullet
{



    protected override void Start()
    {
       

        


    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�ʏ��ԂŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }


    protected override void Update()
    {
        
    }
}
