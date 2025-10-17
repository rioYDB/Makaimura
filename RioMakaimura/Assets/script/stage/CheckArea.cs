using UnityEngine;

public class CheckArea : MonoBehaviour
{
    // �� Inspector�Őݒ肷�鍀��
    public Downdoor targetDoor; // ������̃R���g���[���[�������ɐݒ�
    public bool triggerOnce = true;          // ��x����������Ĕ������Ȃ��悤�ɂ��邩

    private bool hasTriggered = false;       // ���ɔ����������ǂ����̃t���O

    // ���̃G���A�ɉ��������Ă����Ƃ��̏���
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���ɔ����ς݂ŁA��x����̃g���K�[�Ȃ牽�����Ȃ�
        if (triggerOnce && hasTriggered)
        {
            return;
        }

        // �v���C���[���G�ꂽ���Ƃ��m�F
        if (other.CompareTag("Player"))
        {
            if (targetDoor != null)
            {
                targetDoor.StartFalling(); // ������閽��
                hasTriggered = true;    // �����t���O�𗧂Ă�

                // �g���K�[�G���A�̃R���C�_�[�𖳌����i�I�v�V�����F��x����̔������m���ɂ���j
                if (triggerOnce)
                {
                    GetComponent<Collider2D>().enabled = false;
                }
            }
            else
            {
                Debug.LogWarning("Target Door���ݒ肳��Ă��܂���BTrigger Zone��Inspector���m�F���Ă��������B");
            }
        }
    }
}
