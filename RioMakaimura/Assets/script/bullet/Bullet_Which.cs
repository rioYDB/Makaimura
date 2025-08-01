using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  �z�[�~���O�̉�]���x
    private Transform EnemyTarget;      //�z�[�~���O�Ώۂ̃G�l�~�[
    private Vector3 currentMoveDirection;
    public LayerMask Ground;            //�n�ʂ𔻒肷�郌�C���[


    protected override void Start()
    {
        //base.Start(); //�e�N���X��Start()���J�n

        //�q�N���X��Start()���e����������
        FindEnemy(); //��ԋ߂��G�l�~�[��T���֐��̌Ăяo��

        // �^�[�Q�b�g�����������珉���̈ړ��������Z�b�g
        if (EnemyTarget != null)
        {
            // �e�N���X�� movespeed ���g���ď����̈ړ�����������
            currentMoveDirection = (EnemyTarget.position - transform.position).normalized * movespeed;
        }
        else
        {
            // �^�[�Q�b�g��������Ȃ��ꍇ�́A��U�e���~�߂邩�A����̕����֐i�܂��邩
            // ��~�����āAUpdate�ōĒT�������݂�
            currentMoveDirection = Vector3.zero;
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();//�e�N���X��Update()���J�n

        Horming(); //�z�[�~���O�����̌Ăяo��

        // �v�Z���ꂽ�ړ������Ŏ��ۂɒe���ړ�������
        transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);
        // Space.World ���w�肷�邱�ƂŁA�e�̃��[�J�����W�ł͂Ȃ����[���h���W�ňړ����܂��B

        // �e�̌����ڂ̌�����i�s�����iX���j�ɍ��킹��
        // currentMoveDirection�̕����ɒe��X���i�E�����j���������܂��B
        if (currentMoveDirection != Vector3.zero) // ���ʂȉ�]��h������
        {
            transform.right = currentMoveDirection;
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            Debug.Log("�z�[�~���O�e���n�ʂɁI");
            Destroy(gameObject); // �n�ʂɃg���K�[������z�[�~���O�e������
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {

            //BulletMoves(collision.gameObject);

            // �G��Enemy_HP�X�N���v�g�����邩�m�F
            enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1); // �_���[�W�ʂ�1�Ƃ���i�K�v�ɉ����ĕς���j
            }

        }
    }



    //protected override void BulletMoves(GameObject Enemy)
    //{
    //    Debug.Log("�����ŃA�^�b�N�I�I�I");
        
    //    Destroy(gameObject);
    //}




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

            // �ΏۂƂȂ�G���L���ȃQ�[���I�u�W�F�N�g�ł��邩�`�F�b�N
            if (enemeis == null) continue; // �����G�����ɔj�󂳂�Ă�����X�L�b�v

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
        // �����z�[�~���O����^�[�Q�b�g���������ĂȂ��A�܂��̓^�[�Q�b�g�����ł��Ă�����
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            // ���݂̈ړ�������ێ������܂܁A�V�����^�[�Q�b�g��T��
            // ��������ύX��
            // �^�[�Q�b�g��������Ȃ��Ă��A���݂̈ړ������͕ύX���Ȃ�
            Vector3 lastMoveDirection = currentMoveDirection; // �Ō�ɒǂ��Ă���������ێ�
            FindEnemy(); // �V�����^�[�Q�b�g��T��

            if (EnemyTarget == null) // �V�����^�[�Q�b�g��������Ȃ���΁A�Ō�̕����֐i�ݑ�����
            {
                currentMoveDirection = lastMoveDirection; // �ێ����Ă����������ێ�
                return;
            }
        }

        // �^�[�Q�b�g�̕������v�Z���A�e�N���X�� movespeed ���g���Ĉړ������ɐݒ肷��
        currentMoveDirection = (EnemyTarget.position - transform.position).normalized * movespeed;
    }
}



