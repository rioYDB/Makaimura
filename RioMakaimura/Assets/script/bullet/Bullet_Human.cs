using UnityEngine;

public class Bullet_Human : bullet
{

    public float BounceMoveSpeed = 5.0f;  //�ړ����x
    public float Bounceforce = 2.0f;     //�o�E���h���鍂��
    public float Initialup = 1.0f;          //�o�E���h����܂ł̎���

    public LayerMask Ground;              //�n�ʔ��肷��ϐ�

    private Rigidbody2D rb;               //Rigitbody2D


    //private Vector3 StartPos;             //�e�����˂��ꂽ�ʒu
    //private float StartTime;              //�e�̐������ꂽ����
    //private float XDirection;             //�e��X����


    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�ʏ��ԂŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }



    protected override void Start()
    {
        base.Start();                       //�e�N���X��Start()���Ăяo��

        //rb = GetComponent<Rigidbody2D>();   //Rigidbody2D���擾����


        //�������x��ݒ�
        //rb.linearVelocity = new Vector2 (direction * BounceMoveSpeed,Initialup);



        ////StartPos = transform.position;  //���˂��ꂽ�ʒu��c������
        ////StartTime = Time.time;          //�e�̐������ꂽ���Ԃ��L������


    }



    protected override void Update()
    {
        //Y���̓�����Rigidbody2D�ɔC����
        //rb.linearVelocity = new Vector2(direction * BounceMoveSpeed, rb.linearVelocity.y);




        base.Update(); //���ړ��𐧌䂵��������e�N���X��Update()�͌Ăяo���Ȃ�




    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //base.OnTriggerEnter2D(collision);

    //    if(collision.gameObject.CompareTag("Enemy"))
    //    {
    //        BulletMoves(collision.gameObject);
    //    }
    //}
}
