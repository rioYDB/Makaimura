using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    // �� Inspector�Őݒ肷�鍀��
    public Sprite openSprite;           // �����J������Ԃ̃X�v���C�g
    public Sprite closedSprite;         // ����������Ԃ̃X�v���C�g�i�����l�j

    [Tooltip("�J���Ă��邩�ǂ���")]
    public bool isOpen = false;

    // �� ���̋����ɕK�v�ȃR���|�[�l���g
    private SpriteRenderer sr;
    private Collider2D doorCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();

        // ������Ԃ�ݒ� (�ʏ�͕��Ă�����)
        SetDoorState(isOpen);
    }

    // �G�̎��S�X�N���v�g����Ăяo�������J���\�b�h
    public void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("�{�X���|����܂����B�����J���܂��I");
            isOpen = true;
            SetDoorState(isOpen);
        }
    }

    // ���̌����ڂƃR���C�_�[���X�V����v���C�x�[�g���\�b�h
    private void SetDoorState(bool open)
    {
        if (open)
        {
            // �����J����
            sr.sprite = openSprite;
            if (doorCollider != null)
            {
                doorCollider.enabled = false; // �v���C���[���ʂ��
            }
        }
        else
        {
            // �������
            sr.sprite = closedSprite;
            if (doorCollider != null)
            {
                doorCollider.enabled = true; // �v���C���[���ʂ�Ȃ�
            }
        }
    }

    // �v���C���[���G�ꂽ�Ƃ��̏����i�I�v�V�����j
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            // ��: ���̃V�[���֑J��
            // UnityEngine.SceneManagement.SceneManager.LoadScene("NextLevel");
        }
    }
}
