using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  �z�[�~���O�̉�]���x
    private Transform EnemyTarget;      //�z�[�~���O�Ώۂ̃G�l�~�[


    protected override void Start()
    {
        base.Start(); //�e�N���X��Start()���J�n

        //�q�N���X��Start()���e����������
        FindEnemy(); //��ԋ߂��G�l�~�[��T���֐��̌Ăяo��


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();//�e�N���X��Update()���J�n

        

        Horming(); //�z�[�~���O�����̌Ăяo��
    }



    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�����ŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }




    //�֐����FFindEnemy()
    //�p�r�F��ԋ߂��G�l�~�[��T������
    //�����F�Ȃ�
    //�߂�l�F�Ȃ�
    private void FindEnemy()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");        //�V�[������Enemy�̃^�O�����ׂĎ擾����

        float ClosestDistanceEnemy = Mathf.Infinity;                            //�ł��߂�Enemy��T�����߂̏����ݒ�BMathf.Infinity�͂܂��������Ă��Ȃ����

        Transform ClosestEmemy =null;

        Vector3 CurrentPosition=transform.position;                             //���݂̈ʒu


        foreach (GameObject enemeis in enemy)                                   //�ł��߂��G�l�~�[�������鏈��
        {

            Vector3 DirectionEnemy = enemeis.transform.position - CurrentPosition;      //���݂̒e�̈ʒu����G�l�~�[�̋������������x�N�g��

            float DistanceEnemy = DirectionEnemy.sqrMagnitude;                          //�����̓����v�Z


            //���܂ŋ߂������G�l�~�[�̋������DistanceEnemy�̋������Z��������
            if (DistanceEnemy < ClosestDistanceEnemy)                                   
            {
                ClosestDistanceEnemy=DistanceEnemy;

                ClosestEmemy = enemeis.transform;  //�ł��߂��G�l�~�[���X�V����
            }

           

        }

        EnemyTarget = ClosestEmemy;            //��ԋ߂��G�l�~�[���^�[�Q�b�g�ɂ���

    }


    //�֐����FHorming()
    //�p�r�F�z�[�~���O����
    //�����F�Ȃ�
    //�߂�l�F�Ȃ�
    private void Horming()
    {
        //�����z�[�~���O����^�[�Q�b�g���������ĂȂ��Ƃ�
        if (EnemyTarget == null)
        {
            FindEnemy();
            return;
        }

        Vector3 Direction =(EnemyTarget.position - transform.position).normalized;          //�^�[�Q�b�g�����̃x�N�g�����v�Z

        float RotateStep =HormingSpeed*Time.deltaTime;                                      //

        Vector3 NewDirection = Vector3.RotateTowards(transform.position,Direction, RotateStep,0.0f);

        transform.right = NewDirection;
    }

}

