using UnityEngine;

public class Stan : MonoBehaviour
{
    [Header("�X�^�����ʐݒ�")]
    public float duration = 0.5f;           // �Ռ��g�̐������ԁi�����ڂ��������ԁj
    public float stunTime = 2.0f;           // �G��v���C���[���X�^�����鎞��

    // Start�Ŏ��s�����Փ˔���́AOnTriggerEnter2D�ɔC���܂��B

    void Start()
    {
        // �Ռ��g�͂����ɏ����邽�߁A�����Ɠ����Ƀ^�C�}�[���J�n
        Destroy(gameObject, duration);
    }

    // �Ռ��g�̃R���C�_�[�ɐG�ꂽ�I�u�W�F�N�g�ւ̏���
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �G������킸�X�^�������邽�߁A�^�[�Q�b�g���`�F�b�N

        // �v���C���[�̌��o
        if (other.CompareTag("Player"))
        {
            // �v���C���[�̃R���g���[�����擾���A�X�^�����\�b�h���Ăяo��
            player_control playerController = other.GetComponent<player_control>();
            if (playerController != null)
            {
                // playerController.Stun(stunTime); // �v���C���[����X�N���v�g��Stun���\�b�h���K�v
                Debug.Log("�v���C���[���X�^�����܂����I");
            }
        }
        // �G�̌��o (�P���x���X�ȊO�̑��̓G���܂�)
        else if (other.CompareTag("Enemy"))
        {
            // �G�̃R���g���[�����擾���A�X�^�����\�b�h���Ăяo��
            // �t�����P�����g�̓X�^�������Ȃ��悤�ɒ��ӂ��K�v�ł� (Franken_Controller�͖���)
            if (other.GetComponent<Frankenstein>() == null)
            {
                // EnemyController enemyController = other.GetComponent<EnemyController>();
                // if (enemyController != null) enemyController.Stun(stunTime); 
                Debug.Log(other.gameObject.name + "���X�^�����܂����I");
            }
        }
    }
}
