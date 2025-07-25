using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public LayerMask groundLayer; // Inspector�Œn�ʂ�LayerMask��ݒ�

    // �ڒn����̌��ʂ��O���Ɍ��J����v���p�e�B
    public bool IsGrounded { get; private set; }

    void Update() // ���ꂪ������Update()�̒�`
    {
        // ���C���F�v���C���[�{�̂�Collider2D��BoxCollider2D�Ƃ��Ď擾����
        // ���v���C���[��Collider��CapsuleCollider2D�Ȃ�ACapsuleCollider2D�ɏC�����Ă�������
        //   player_control.cs�ł�BoxCollider2D�Ƃ��Ē�`����Ă���̂ŁA�����ł�BoxCollider2D���g�p���܂��B
        BoxCollider2D playerBoxCollider = transform.parent.GetComponent<BoxCollider2D>();
        if (playerBoxCollider == null)
        {
            Debug.LogError("PlayerGroundCheck: �v���C���[��BoxCollider2D��������܂���I", this);
            return;
        }

        // BoxCollider2D�̃T�C�Y�ƃI�t�Z�b�g���擾 (playerBoxCollider���g�p)
        Vector2 colliderSize = playerBoxCollider.size;
        Vector2 colliderOffset = playerBoxCollider.offset;

        // Raycast�̊J�n�ʒu��Collider�̒�ӂ̒������班����ɐݒ� (�q�I�u�W�F�N�g��Transform����ɒ���)
        Vector2 rayOrigin = (Vector2)transform.position + colliderOffset + Vector2.down * (colliderSize.y / 2f - 0.05f); // ������
        float rayLength = 0.1f; // �Z�����Č댟�m�����炷

        // �f�o�b�O�p��Ray��\��
        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

        // ��������ray���΂��āA�w�肵�����C���[�̃I�u�W�F�N�g�ƐڐG���Ă��邩�ǂ������ʂ���
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);

        IsGrounded = (hit.collider != null); // �q�b�g���Ă����true
    }

    // �I�v�V�����FInspector��Raycast�̎n�_�����o�I�Ɋm�F���邽�߂̃M�Y��
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f); // GroundChecker�̈ʒu������
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * 0.1f); // Raycast�̕���������
    }
}
