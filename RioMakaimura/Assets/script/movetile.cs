using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class movetile : MonoBehaviour
{

    [SerializeField]
    private Vector3 leftEndPoint; // ���[�̍��W

    [SerializeField]
    private Vector3 rightEndPoint; // �E�[�̍��W



    [SerializeField]
    private float moveSpeed = 2f;    // ���̈ړ����x

    private float journeyLength; // �ړ�����S�̂̋���
    private float startTime;     // �ړ��J�n����

    void Start()
    {
        // ���݂̏��̈ʒu���A�ݒ肵���J�n�n�_�̋߂��Ɉړ������Ă���
        // �G�f�B�^��Őݒ肵��leftEndPoint��rightEndPoint���K�p�����悤�ɂ���
        transform.position = leftEndPoint;

        // ���[����E�[�܂ł̋������v�Z
        journeyLength = Vector3.Distance(leftEndPoint, rightEndPoint);
        startTime = Time.time; // �ړ��J�n���Ԃ��L�^
    }

    void Update()
    {
        // �����̎��Ԃ��v�Z (PingPong��0�`1�͈̔͂Ő��K�����ꂽ�l���擾)
        float pingPongTime = Mathf.PingPong((Time.time - startTime) * moveSpeed / journeyLength, 1f);

        // ���[�ƉE�[�̊Ԃ���`��� (Lerp) �ňړ�
        transform.position = Vector3.Lerp(leftEndPoint, rightEndPoint, pingPongTime);
    }

    // �v���C���[��������Ƃ��Ɉꏏ�ɓ����悤�ɂ���ꍇ (�I�v�V����)

    private void OnCollisionEnter2D(Collision2D collision) // ������ Collision2D �ɕύX
    {
        // "Player"�^�O�̃I�u�W�F�N�g���Փ˂����ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�����̎q�I�u�W�F�N�g�ɂ���
            collision.collider.transform.SetParent(transform);
            Debug.Log("�v���C���[���������ɏ�����I"); // �f�o�b�O���O�ǉ�
        }
    }


    private void OnCollisionExit2D(Collision2D collision) // ������ Collision2D �ɕύX
    {
        // "Player"�^�O�̃I�u�W�F�N�g���Փ˂��痣�ꂽ�ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�̎q�I�u�W�F�N�g����
            // SetParent(null) �̑O�ɁA�v���C���[��Transform���L�����`�F�b�N
            if (collision.collider.transform.parent == transform) // �e�����̏��ł��邱�Ƃ��m�F
            {
                collision.collider.transform.SetParent(null);
                Debug.Log("�v���C���[������������~�肽�I"); // �f�o�b�O���O�ǉ�
            }
        }
    }

    // �G�f�B�^��ňړ��͈͂��������邽�߂̃M�Y��
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // �G�f�B�^��Őݒ肵��leftEndPoint��rightEndPoint�����f�����悤��
        if (Application.isPlaying) // �v���C���Ȃ猻�݂̈ʒu���g��
        {
            Gizmos.DrawLine(leftEndPoint, rightEndPoint);
            Gizmos.DrawSphere(leftEndPoint, 0.2f);
            Gizmos.DrawSphere(rightEndPoint, 0.2f);
        }
        else // �G�f�B�^��Ȃ�Inspector�Őݒ肳�ꂽ�l���g��
        {
            // Inspector�Őݒ肳�ꂽ�l��null�łȂ����Ƃ��m�F (Unity 2020.1�ȍ~�ŕK�v�ɂȂ邱�Ƃ�����)
            if (leftEndPoint != Vector3.zero || rightEndPoint != Vector3.zero) // �K���ȃ`�F�b�N
            {
                Gizmos.DrawLine(leftEndPoint, rightEndPoint);
                Gizmos.DrawSphere(leftEndPoint, 0.2f);
                Gizmos.DrawSphere(rightEndPoint, 0.2f);
            }
        }
    }
}
