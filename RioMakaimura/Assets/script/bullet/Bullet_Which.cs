using UnityEngine;

public class Bullet_Which : bullet
{

    public float HormingSpeed = 2.0f;   //  �z�[�~���O�̉�]���x
    private Transform EnemyTarget;      //�z�[�~���O�Ώۂ̃G�l�~�[
    private Vector3 currentMoveDirection;
    public LayerMask Ground;            //�n�ʂ𔻒肷�郌�C���[

    //---------------------------------------------------------------------------------------------------------------------------

    //�G����ʓ��ɂ��Ȃ���Ԃ͂���瓮���e
    public float waveAmplitude = 5.0f; //�����̕�
    public float waveSpeed = 0.5f;     //�����̑��x
    private float PositionY;           //�e��Y���W��ێ�����

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
            //�e�N���X��direction���g���O�i����
            float initialDirectionX = transform.localScale.x > 0 ? 1f : -1f; // �v���C���[�̌����𗘗p

            // �O�i�������Z�b�g�iY���̓[���j
            currentMoveDirection = new Vector3(initialDirectionX * movespeed, 0, 0);
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();//�e�N���X��Update()���J�n

        Horming(); //�z�[�~���O�����̌Ăяo��

        // �z�[�~���O���łȂ��ꍇ�Ɂu�����v������K�p
        if (EnemyTarget == null)
        {
            // X�������� currentMoveDirection (�O�i���x) �𗘗p���Đi��
            float newX = transform.position.x + currentMoveDirection.x * Time.deltaTime;

            // Y�������̓T�C���g���g���Ă���瓮��
            float waveY = Mathf.Sin(Time.time * waveSpeed) * waveAmplitude;
            float newY = PositionY + waveY;

            // X��Y��g�ݍ��킹�Ĉړ������� (Z�͖���)
            transform.position = new Vector3(newX, newY, transform.position.z);

            // �ړ��ɍ��킹�Ēe�̌����𒲐����郍�W�b�N�́A�z�[�~���O���ł͂Ȃ��̂ň�U�ۗ�
            // �����ړ����Ɍ����ڂ�Ǐ]���������ꍇ�́A�X�Ɍv�Z���K�v�ł��B
        }
        else // �z�[�~���O���i�^�[�Q�b�g�����݂���j
        {
            // �v�Z���ꂽ�ړ������Ŏ��ۂɒe���ړ�������
            transform.Translate(currentMoveDirection * Time.deltaTime, Space.World);

            // �e�̌����ڂ̌�����i�s�����iX���j�ɍ��킹��
            if (currentMoveDirection != Vector3.zero)
            {
                transform.right = currentMoveDirection;
            }
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

        Transform ClosestEmemy = null;

        Vector3 CurrentPosition = transform.position;                             //���݂̈ʒu


        foreach (GameObject enemeis in enemy)                                   //�ł��߂��G�l�~�[�������鏈��
        {

            // �ΏۂƂȂ�G���L���ȃQ�[���I�u�W�F�N�g�ł��邩�`�F�b�N
            if (enemeis == null) continue; // �����G�����ɔj�󂳂�Ă�����X�L�b�v

            Vector3 DirectionEnemy = enemeis.transform.position - CurrentPosition;      //���݂̒e�̈ʒu����G�l�~�[�̋������������x�N�g��

            float DistanceEnemy = DirectionEnemy.sqrMagnitude;                          //�����̓����v�Z


            //���܂ŋ߂������G�l�~�[�̋������DistanceEnemy�̋������Z��������
            if (DistanceEnemy < ClosestDistanceEnemy)
            {
                ClosestDistanceEnemy = DistanceEnemy;

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
        // �����z�[�~���O����^�[�Q�b�g��������Ȃ��A�܂��̓^�[�Q�b�g�����ł��Ă�����
        if (EnemyTarget == null || (EnemyTarget != null && EnemyTarget.gameObject == null))
        {
            // �^�[�Q�b�g�����Z�b�g
            EnemyTarget = null;

            // ���C���� �^�[�Q�b�g��������Ȃ��Ă��A���݂�X���ړ������i�O�i�j�͈ێ�����
            float lastDirectionX = currentMoveDirection.x;
            FindEnemy(); // �V�����^�[�Q�b�g��T��

            if (EnemyTarget == null) // �V�����^�[�Q�b�g��������Ȃ����
            {
                // X�����������͑O�̈ړ����x���ێ����AY���͂���珈���̂��߂Ƀ��Z�b�g
                currentMoveDirection = new Vector3(lastDirectionX, 0, 0);
                return; // �z�[�~���O�����������ŏI��
            }
        }

        // �^�[�Q�b�g�����݂���ꍇ�̃z�[�~���O����

        // �^�[�Q�b�g�̕������v�Z
        Vector3 targetDirection = (EnemyTarget.position - transform.position).normalized;

        // �z�[�~���O���x�Ō��݂̈ړ��������^�[�Q�b�g�����ɋ߂Â���
        currentMoveDirection = Vector3.RotateTowards(
            currentMoveDirection.normalized,
            targetDirection,
            HormingSpeed * Time.deltaTime, // �z�[�~���O�X�s�[�h��Time.deltaTime�Ŋ��邱�ƂŁA�t���[�����[�g�Ɉˑ����Ȃ��悤�ɂ��܂�
            0.0f
        ).normalized * movespeed;
    }
}



