using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ZonbiSpawner : MonoBehaviour
{
    private player_control player_co;

    public Transform player;             // �v���C���[��Transform
    public GameObject enemyPrefab;       // �G�̃v���n�u
    public float spawnDistance;     // �v���C���[���獶�E�ǂꂾ�����ꂽ�ꏊ�ɏo�������邩
    public float spawnInterval;     // �X�|�[���Ԋu�i�b�j
    public float spawnWidthVariation = 2f; // �������̃����_���Ȃ΂���i�v���C���[�̈ʒu����}�͈́j

    List<GameObject> spawnedEnemies = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �v���C���[�̏��擾
        player_co = GameObject.Find("player").GetComponent<player_control>();

        // �ŏ��̏o����2�b��ɂ��āA���̌��spawnInterval�b���ƂɌJ��Ԃ��o��
        InvokeRepeating("SpawnEnemyFromSide", 2f, spawnInterval);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemyFromSide()
    {
        if (player == null) return;

        
        // �O�̂��o��
        for (int i = 0; i < 3; i++)
        {
            // ���E�ǂ��炩�������_���őI��
            int side = Random.value < 0.5f ? -1 : 1;

            // �o���ʒu���v�Z�i���E�ǂ��炩�j
            float spawnX = player.position.x + spawnDistance * side;

            // �������Ƀ����_���ȕ���ǉ�
            spawnX += Random.Range(-spawnWidthVariation, spawnWidthVariation); // ���̂΂����ǉ�

            // Ray���ォ�牺�Ɍ����Ēn�ʂ�T��
            Vector2 rayOrigin = new Vector2(spawnX, player.position.y + 5f);
            // Ray�����p
            Vector2 rayDirection = Vector2.down;
            float rayDistance = 10f;

            // Debug�p��Ray������
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 1f); // 1�b�����Ԃ�����\��

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10f, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                // �ŏI�I�ȃX�|�[���ʒu
                float enemyHeightOffset = 0.6f;  // �]���r�̑�������
                Vector2 spawnPos = new Vector2(spawnX, hit.point.y + enemyHeightOffset);

                // �G�𐶐�
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                newEnemy.GetComponent<enemy_control>().mOnDestly += (GameObject g) =>
                {
                    spawnedEnemies.Remove(g);
                };
                CapsuleCollider2D ignoreCollider = newEnemy.GetComponent<CapsuleCollider2D>();

                foreach (GameObject enemy in spawnedEnemies)
                {
                    if (enemy != null)
                    {
                        CapsuleCollider2D collider = enemy.GetComponent<CapsuleCollider2D>();
                        // �Փ˂𖳎�����
                        Physics2D.IgnoreCollision(collider, ignoreCollider);
                    }
                }
                spawnedEnemies.Add(newEnemy);
            }
            else
            {
                Debug.LogWarning("�n�ʂ�������Ȃ��������߁A�]���r���X�|�[�����܂���ł����B�ʒu: " + spawnX);
            }

            //// �ŏI�I�ȃX�|�[���ʒu
            //Vector2 spawnPos = new Vector2(spawnX, player.position.y - 0.5f);


            //// �G�𐶐�
            //Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

}
