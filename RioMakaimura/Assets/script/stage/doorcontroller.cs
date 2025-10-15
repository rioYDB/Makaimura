using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class doorcontroller : MonoBehaviour
{
    public float openSpeed = 1.0f; // �����J�����x
    public float openDistanceY = 5.0f; //��ɓ�������
    public AudioClip openSound; // �J���� (�I�v�V����)

    private Vector3 initialPosition;
    private Vector3 targetOpenPosition; //�J�������̖ڕW�ʒu
    private bool isOpening = false;
    private AudioSource audioSource; // ����炷����

    void Start()
    {
        initialPosition = transform.position;
        //�J�������̖ڕW�ʒu���v�Z (���݂̈ʒu����Y������openDistanceY������Ɉړ�)
        targetOpenPosition = initialPosition + new Vector3(0, openDistanceY, 0);
        audioSource = GetComponent<AudioSource>(); // AudioSource�R���|�[�l���g���擾
    }

    void Update()
    {
        if (isOpening)
        {
            // �������X�ɊJ���i�ڕW�ʒu�܂ňړ�������j
            transform.position = Vector3.MoveTowards(transform.position, targetOpenPosition, openSpeed * Time.deltaTime);

            // ���S�ɊJ������isOpening��false�ɂ���
            if (Vector3.Distance(transform.position, targetOpenPosition) < 0.01f)
            {
                isOpening = false;
                // Collider�𖳌������Ēʂ��悤�ɂ���
                Collider2D doorCollider = GetComponent<Collider2D>();
                if (doorCollider != null)
                {
                    doorCollider.enabled = false; // �������S�ɊJ������Փ˔���𖳌��ɂ���
                }
            }
        }
    }

    // �X�C�b�`����Ăяo����郁�\�b�h
    public void OpenDoor()
    {
        Debug.Log("�����J���I");
        isOpening = true;

        // ����炷 (�I�v�V����)
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
}
