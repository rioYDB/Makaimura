using UnityEngine;

public class Bullet_Human : bullet
{

    public float BounceMoveSpeed = 5.0f;  //�ړ����x
    public float BounceHeight = 2.0f;     //�o�E���h���鍂��
    public float BounceDuration = 1.0f;   //�o�E���h����܂ł̎���

    private Vector3 StartPos;             //�e�����˂��ꂽ�ʒu
    private float StartTime;              //�e�̐������ꂽ����
    private float XDirection;             //�e��X����


    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�ʏ��ԂŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }



    protected override void Start()
    {
        base.Start();                   //�e�N���X��Start()���Ăяo��
        StartPos = transform.position;  //���˂��ꂽ�ʒu��c������
        StartTime = Time.time;          //�e�̐������ꂽ���Ԃ��L������


    }



    protected override void Update()
    {
        //base .Update(); ���ړ��𐧌䂵��������e�N���X��Update()�͌Ăяo���Ȃ�

        float ElapsedTime = Time.time - StartTime;                  //�e�����˂���Ă��牽�b��������
        float NomalizedCycleTime = ElapsedTime / BounceDuration;    //�e�����܂�Ă��牽��o�E���h������

        float XOffset = direction * BounceMoveSpeed�@* ElapsedTime; //������(X��)�ɂǂꂭ�炢�����񂾂��H

        float YOffset = Mathf.Sin(NomalizedCycleTime * Mathf.PI) * BounceHeight; //�T�C���J�[�u���g���A�o�E���h�̓��������

        //Y���W���}�C�i�X�ɂȂ�Ȃ��悤��
        if (YOffset < 0)
        {
            YOffset = 0;
        }


        transform.position = StartPos + new Vector3(XOffset, YOffset);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        if(collision.gameObject.CompareTag("Enemy"))
        {
            BulletMoves(collision.gameObject);
        }
    }
}
