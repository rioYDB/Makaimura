using UnityEngine;

public class Bullet_Human : bullet
{

    public float bounceMoveSpeed = 5.0f;  //�ړ����x
    public float bounceHeight = 2.0f;     //�o�E���h���鍂��
    public float bounceDuration = 1.0f;   //�o�E���h����܂ł̎���

    private Vector3 StartPos;             //�e�����˂��ꂽ�ʒu
    private float StartTime;              //�e�̐������ꂽ����
    private float XDirection;             //�e��X����
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
