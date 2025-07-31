using UnityEngine;
using System.Collections;



public enum CerberusState
{
    Idle,       // �ҋ@
    Move,       // �ړ�
    Attack1,    // �U���p�^�[��1
    Attack2,    // �U���p�^�[��2
    Attack3,    // �U���p�^�[��3
    Hurt,       // ��e
    Dead        // ���S
}


public class Cerberus_Controller : MonoBehaviour
{
    public CerberusState currentState; // ���݂̃{�X�̏��
    public Transform playerTransform; // �v���C���[��Transform (�^�[�Q�b�g)

    private float attackCoolDownTimer = 0f;
    public float attackCoolDownTime = 3f; // �U���Ԃ̃N�[���_�E������

    public GameObject fireBreathPrefab; // ���̃v���n�u
    public Transform head1SpawnPoint; // ��1�̉������ʒu
    public Transform head2SpawnPoint; // ��2�̉������ʒu
    public Transform head3SpawnPoint; // ��3�̉������ʒu


    //�ړ�����
    public float moveSpeed = 2f; // �ړ����x
    public float moveRange = 3f; // ���E�ɓ����͈� (�����ʒu���獶�E�ɂ��̋���)
    private Vector2 initialPosition; // �P���x���X�̏����ʒu
    private int moveDirection = 1; // 1:�E, -1:��

    void Start()
    {
        currentState = CerberusState.Move; // �ŏ��͈ړ���Ԃ���n�߂�
        initialPosition = transform.position; // �����ʒu���L�^
    }

    void Update()
    {
        // �N�[���_�E���^�C�}�[���X�V
        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        // ���݂̏�Ԃɉ����ď����𕪊�
        switch (currentState)
        {
            case CerberusState.Idle:
                HandleIdleState();
                break;
            case CerberusState.Move:
                HandleMoveState();
                break;
            case CerberusState.Attack1:
                // �U�����͑��̏������u���b�N���A�U���I����҂�
                break;
            case CerberusState.Attack2:
                break;
            case CerberusState.Attack3:
                break;
            case CerberusState.Hurt:
                HandleHurtState();
                break;
            case CerberusState.Dead:
                HandleDeadState();
                break;
        }
    }

    // --- �e��Ԃ��Ƃ̏��� ---



    void HandleIdleState()
    {
        // �N�[���_�E�����I������玟�̍U����I��
        if (attackCoolDownTimer <= 0)
        {
            ChooseNextAttack();
        }
        else
        {
            // �U���N�[���_�E�����͈ړ���ԂɈڍs
            currentState = CerberusState.Move;
        }
    }

    void HandleMoveState()
    {
        // ���E�ړ��̖ڕW�ʒu���v�Z
        float targetX = initialPosition.x + moveRange * moveDirection;
        Vector2 targetPosition = new Vector2(targetX, transform.position.y);

        // ���݈ʒu����ڕW�ʒu�ֈړ�
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // �ڕW�ʒu�ɓ��B����������𔽓]
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f) // �قړ��B������
        {
            moveDirection *= -1; // �����𔽓]
            // �L�����N�^�[�̌����𔽓]������ꍇ�͂�����
            // transform.localScale = new Vector3(moveDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // �N�[���_�E�����I�������U���Ɉڍs
        if (attackCoolDownTimer <= 0)
        {
            ChooseNextAttack();
        }
    }
    void HandleHurtState()
    {
        // ��e�A�j���[�V�����̍Đ��A�m�b�N�o�b�N�Ȃ�
        // �A�j���[�V�����I�����Idle�ɖ߂�
    }

    void HandleDeadState()
    {
        // ���S�A�j���[�V�����A�Q�[���I�[�o�[�����Ȃ�
    }

    // --- �U���̑I�����W�b�N ---

    void ChooseNextAttack()
    {
        // �����_���A�܂���HP�ɉ������U���I�����W�b�N
        int randomAttack = Random.Range(0, 3); // 0, 1, 2�̂����ꂩ

        switch (randomAttack)
        {
            case 0:
                StartAttack1();
                break;
            case 1:
                StartAttack2();
                break;
            case 2:
                StartAttack3();
                break;
        }
    }

    // --- �e�U���p�^�[���̊J�n���\�b�h ---

    void StartAttack1()
    {
        currentState = CerberusState.Attack1;
        Debug.Log("�P���x���X: �U��1���J�n�I (�O�ʕ����ւ̉Ή��u���X)");
        StartCoroutine(Attack1Coroutine()); // �R���[�`���ōU���������J�n
    }

    void StartAttack2()
    {
        currentState = CerberusState.Attack2;
        Debug.Log("�P���x���X: �U��2���J�n�I (�n���ɏ����ăv���C���[�̉������яo��)");
        StartCoroutine(Attack2Coroutine());
    }

    void StartAttack3()
    {
        currentState = CerberusState.Attack3;
        Debug.Log("�P���x���X: �U��3���J�n�I (�ːi)");
        StartCoroutine(Attack3Coroutine());
    }

    // --- �e�U���p�^�[���̋�̓I�Ȏ��� (�R���[�`��) ---
    // �����Ɋe�U���̃��W�b�N���L�q���܂��B

    IEnumerator Attack1Coroutine()
    {
        // ��: �A�j���[�V�����Đ��i����f���A�j���[�V�����j
        // animator.SetTrigger("Attack1");

        yield return new WaitForSeconds(0.5f); // �A�j���[�V�����J�n����u���X���o��܂ł̑ҋ@

        // ���̐����Ɣ���
        // �e����Transform����ɁA�p�x�𒲐����ĉ��𐶐��E���˂���
        // ��: �v���C���[�̕���������������A�Œ��3�����i�O���A���΂߁A�E�΂߁j�ɂ�����
        // �����ł͉��ɌŒ��3�����Ƃ��܂�

        // ��1���甭�� (��: ����)
        if (fireBreathPrefab != null && head1SpawnPoint != null)
        {
            GameObject fire1 = Instantiate(fireBreathPrefab, head1SpawnPoint.position, Quaternion.identity);
            // fire1 �ɕt���Ă���X�N���v�g�ňړ����W�b�N�𐧌䂷�邩�ARigidbody2D�ŗ͂�������
            // fire1.GetComponent<FireBreathScript>().Initialize(Vector2.right); // ��
            // fire1.GetComponent<Rigidbody2D>().AddForce(Vector2.right * fireSpeed); // ��
        }

        // ��2���甭�� (��: ���΂�)
        if (fireBreathPrefab != null && head2SpawnPoint != null)
        {
            GameObject fire2 = Instantiate(fireBreathPrefab, head2SpawnPoint.position, Quaternion.Euler(0, 0, 45)); // 45�x��]
            // fire2.GetComponent<FireBreathScript>().Initialize(Quaternion.Euler(0, 0, 45) * Vector2.right); // ��
        }

        // ��3���甭�� (��: �E�΂�)
        if (fireBreathPrefab != null && head3SpawnPoint != null)
        {
            GameObject fire3 = Instantiate(fireBreathPrefab, head3SpawnPoint.position, Quaternion.Euler(0, 0, -45)); // -45�x��]
            // fire3.GetComponent<FireBreathScript>().Initialize(Quaternion.Euler(0, 0, -45) * Vector2.right); // ��
        }

        // ���̃G�t�F�N�g���������ԑҋ@
        yield return new WaitForSeconds(2.0f); // �����������鎞��

        Debug.Log("�U��1�I��");
        EndAttack();
    }

    IEnumerator Attack2Coroutine()
    {
        Debug.Log("�P���x���X: �U��2���J�n�I (�n���ɏ����ăv���C���[�̉������яo��)");

        // 1. �n���ɏ�����A�j���[�V����/����
        // animator.SetTrigger("Disappear");
        // GetComponent<SpriteRenderer>().enabled = false; // �X�v���C�g���\���ɂ����
        // Rigidbody2D������ꍇ�A���̊ԕ������Z�𖳌��ɂ���Ɨǂ�
        // GetComponent<Collider2D>().enabled = false; // �R���C�_�[�������ɂ���
        yield return new WaitForSeconds(1.0f); // ������A�j���[�V�����̎���

        // 2. �v���C���[�̌��݂̈ʒu���L���i�ŏI�I�ȖڕW�n�_�j
        Vector2 playerCurrentPos = playerTransform.position;

        // 3. �n������o�����邽�߂̊J�n�ʒu���v�Z
        // �v���C���[�̏������i����[���j�Ƀ��[�v����n�_��ݒ�
        // ��������яo���́u�J�n�_�v�ɂȂ�܂�
        float deepUndergroundYOffset = -5.0f; // ��: ���[�����邽�߂�Y�I�t�Z�b�g
        Vector2 startUndergroundPos = new Vector2(playerCurrentPos.x + Random.Range(-0.5f, 0.5f), playerCurrentPos.y + deepUndergroundYOffset);
        transform.position = startUndergroundPos; // �P���x���X��n���̊J�n�ʒu�Ƀ��[�v������

        // 4. �Ăяo������A�j���[�V����/����
        // GetComponent<SpriteRenderer>().enabled = true; // �X�v���C�g���ĕ\�������
        // animator.SetTrigger("Appear"); // ��яo���A�j���[�V�������Đ�

        // 5. �n������ŏI�I�ȏo���ʒu�܂ňړ�����
        float emergeDuration = 0.3f; // �n��ɏo��܂ł̎���
        Vector2 emergeTargetPos = new Vector2(playerCurrentPos.x, playerCurrentPos.y); // �ŏI�I�ɏo������n��ł̈ʒu
        float timer = 0f;

        while (timer < emergeDuration)
        {
            transform.position = Vector2.Lerp(startUndergroundPos, emergeTargetPos, timer / emergeDuration);
            timer += Time.deltaTime;
            yield return null; // 1�t���[���ҋ@
        }
        transform.position = emergeTargetPos; // �m���ɖڕW�ʒu�ɓ��B������

        // 6. �U������̔��� (��яo�����u�ԂɃ_���[�W��^����)
        // GetComponent<Collider2D>().enabled = true; // �R���C�_�[���ėL����
        // CallAttackHitbox(); // �U�������L���ɂ��郁�\�b�h
        // yield return new WaitForSeconds(0.2f); // �U�����肪�������鎞��
        // DisableAttackHitbox(); // �U������𖳌��ɂ��郁�\�b�h


        Debug.Log("�U��2�I��");
        EndAttack();
    }
    IEnumerator Attack3Coroutine()
    {
        Debug.Log("�P���x���X: �U��3���J�n�I (�ːi)");

        // ��: �ːi�J�n�A�j���[�V����
        // animator.SetTrigger("Dash");

        // �v���C���[�Ɍ���
        if (playerTransform.position.x < transform.position.x)
        {
            // �v���C���[�����ɂ���ꍇ�A��������
            // transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // �v���C���[���E�ɂ���ꍇ�A�E������
            // transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        //�v���C���[�ւ̓ːi
        float dashDuration = 0.5f;
        Vector2 startPos = transform.position;
        Vector2 targetPos = playerTransform.position; // �v���C���[�̌��݈ʒu�֓ːi

        // �ːi�����𐧌��������ꍇ�ATargetPos�𒲐����邱�Ƃ��ł��܂�
         float maxDashDistance = 5f;
         Vector2 direction = (targetPos - startPos).normalized;
         targetPos = startPos + direction * maxDashDistance; // �ő�5m�܂œːi

        float timer = 0f;
        while (timer < dashDuration)
        {
            // �ːi���͍U�������L���ɂ���
            // CallAttackHitbox();

            transform.position = Vector2.Lerp(startPos, targetPos, timer / dashDuration);
            timer += Time.deltaTime;
            yield return null; // 1�t���[���ҋ@
        }
        transform.position = targetPos; // �m���ɓ��B

        // �ːi�I����ɍU������𖳌��ɂ���
        // DisableAttackHitbox();

        // �ːi��̍d�����ԂȂ�
        // yield return new WaitForSeconds(0.5f); // ��

        Debug.Log("�U��3�I��");
        EndAttack();
    }

    // --- �U���I������ ---

    void EndAttack()
    {
        currentState = CerberusState.Idle; // �U���I����A�ҋ@��Ԃɖ߂�
        attackCoolDownTimer = attackCoolDownTime; // �N�[���_�E���J�n
    }

    // HP�Ǘ����e���W�b�N�͕ʓr����
    public void TakeDamage(int damage)
    {
        // HP��������
        // currentState = BossState.Hurt; // ��e��Ԃֈڍs���郍�W�b�N��ǉ�
    }
}
