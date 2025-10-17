using UnityEngine;

public class Downdoor : MonoBehaviour
{
    // �� Inspector�Őݒ肷�鍀��
    public float fallSpeed = 5.0f;          // ���������鑬�x
    public float fallDistance = 10.0f;      // ���������鋗��
    public bool isFallen = false;           // �������������ǂ����̃t���O

    // �� ���̋����ɕK�v�ȃR���|�[�l���g
    private Collider2D doorCollider;
    private Vector3 initialPosition;         // �����ʒu
    private Vector3 targetPosition;          // �ŏI�I�ȖڕW�ʒu

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        initialPosition = transform.position;

        // �����ڕW�ʒu��Start���Ɍv�Z���Ă���
        targetPosition = initialPosition + Vector3.down * fallDistance;

        // ������Ԃł̓R���C�_�[��L���ɂ��Ă��� (���Ƃ��ċ@�\�����邽��)
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }

        // �⑫�F����Rigidbody2D���t���Ă�����A������Kinematic�ɂ��ďd�͂𖳌��ɂ��Ă�������
        // Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // �����������ԂɂȂ�����A���t���[�����s
        if (isFallen)
        {
            FallDown();
        }
    }

    // �g���K�[�]�[������Ăяo����A�������J�n������J���\�b�h
    public void StartFalling()
    {
        if (!isFallen) // �܂������Ă��Ȃ���Ύ��s
        {
            Debug.Log("�g���b�v�����I���������܂����B");
            isFallen = true;
        }
    }

    // ���ۂ̗��������iY�������̈ړ��j
    private void FallDown()
    {
        // �ڕW�ʒu�ֈړ� (transform.position�𒼐ڑ���)
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            fallSpeed * Time.deltaTime
        );

        // �����؂����� (�ڕW�ʒu�ɓ��B������) �@�\���~
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // �����؂�����̏���
            // �R���C�_�[�𖳌������邱�ƂŁA���S�ɏ������悤�Ɍ�����
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }

            // �X�N���v�g��Update���~ (CPU���׌y��)
            enabled = false;
        }
    }
}
