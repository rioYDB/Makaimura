using UnityEngine;

public class Bullet_Which : bullet
{

    protected override void Start()
    {
        base.Start(); //�e�N���X��Start()���J�n

        //�q�N���X��Start()���e����������


    }



    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�����ŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
