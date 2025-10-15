using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class switchcontroller : MonoBehaviour
{
    
    public doorcontroller targetDoor; // �J����������DoorController�X�N���v�g�̎Q�Ƃ��h���b�O���h���b�v

    public string playerTag = "Player"; // �v���C���[�̃^�O
    public string playerAttackTag = "Spear"; // �v���C���[�̍U���̃^�O (��: ���Ȃ�"Spear")

    public Color pressedColor = Color.gray; // �X�C�b�`�������ꂽ���̐F (�f�t�H���g�͊D�F)
    private Color defaultColor; // �X�C�b�`�̏����F��ۑ�

    private SpriteRenderer spriteRenderer; // �X�C�b�`��SpriteRenderer

    private bool isActivated = false; // �X�C�b�`�����ɋN�����Ă��邩

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color; // �����F��ۑ�
        }

        if (targetDoor == null)
        {
            Debug.LogWarning("SwitchTrigger: �J�������ݒ肳��Ă��܂���I", this);
        }
    }

    // �v���C���[��U�����X�C�b�`�ɐG�ꂽ���Ƃ����o����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �X�C�b�`�����ɋN�����Ă���ꍇ�͉������Ȃ�
        if (isActivated)
        {
            return;
        }

        // �ڐG�����̂��v���C���[�A�܂��̓v���C���[�̍U�����`�F�b�N
        bool isPlayer = other.CompareTag(playerTag);
        bool isPlayerAttack = other.CompareTag(playerAttackTag);

        if (isPlayer || isPlayerAttack)
        {
            Debug.Log("�X�C�b�`�������ꂽ�I");
            ActivateSwitch();
        }
    }

    // �X�C�b�`���N�����郁�\�b�h
    void ActivateSwitch()
    {
        isActivated = true; // �X�C�b�`���N���ς݂ɐݒ�

        //�X�C�b�`�̌����ڂ�F�ɕύX���� (�I�v�V����)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = pressedColor; // �����ꂽ���̐F�ɕύX
        }

        // �����J������
        if (targetDoor != null)
        {
            targetDoor.OpenDoor(); // DoorController��OpenDoor���\�b�h���Ăяo��
        }
        else
        {
            Debug.LogWarning("SwitchTrigger: �J�����iDoorController�j��������܂���B", this);
        }

        // �X�C�b�`����x�G��邾���ŗǂ��̂ŁA���̃X�C�b�`���g�̓R���C�_�[�𖳌��� (�I�v�V����)
        // GetComponent<Collider2D>().enabled = false;
        // this.enabled = false; // �X�N���v�g�S�̂𖳌���
    }

    // �X�C�b�`�̏�Ԃ����Z�b�g�������ꍇ�i�K�v�ł���Βǉ��j
    public void ResetSwitch()
    {
        isActivated = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor; // �F��������Ԃɖ߂�
        }
        // GetComponent<Collider2D>().enabled = true; // �R���C�_�[��L����
    }
}

