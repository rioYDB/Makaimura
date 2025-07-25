using UnityEngine;
using System.Collections;

public class cannon : MonoBehaviour
{
    public GameObject BulletPrefab;       // ���˂��鑄�̃v���n�u��Inspector����ݒ�
    public float fireRate = 2.0f;        // ���𔭎˂���Ԋu (�b)
    public int fireDirection = 1;        // ������ԕ��� (1:�E, -1:��)

    void Start()
    {
        StartCoroutine(EnemyBulletRoutine()); // ���˃��[�`�����J�n
    }

    IEnumerator EnemyBulletRoutine()
    {
        while (true) // �������[�v�Œ���I�ɔ���
        {
            yield return new WaitForSeconds(fireRate); // ���ˊԊu��҂�

            // ���𐶐�����ʒu�́A���̔��ˌ��̏ꏊ
            // ���̉�]�͂Ȃ� (Quaternion.identity)
            GameObject newSpear = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            // �����������̃X�N���v�g�ɕ�����`����
            Enemybullet spearScript = newSpear.GetComponent<Enemybullet>();
            if (spearScript != null)
            {
                spearScript.SetDirection(fireDirection);
            }
        }
    }
}
