using UnityEngine;
using System.Collections.Generic;

public class Mummy_Spawn : MonoBehaviour
{

    public GameObject enemyPrefab;        // �~�C���̃v���n�u
    public Transform player;              // �v���C���[�i�����̔��f�Ɏg�p�j

    public float spawnInterval = 3f;      // �X�|�[���Ԋu�i�b�j

    public Vector2 spawnAreaMin;          // �X�|�[���͈͂̍������W�i�C���X�y�N�^�[�Őݒ�j
    public Vector2 spawnAreaMax;          // �X�|�[���͈͂̉E����W�i�C���X�y�N�^�[�Őݒ�j

    public LayerMask groundLayer;         // �n�ʂ̃��C���[�i"Ground" �Ȃǂ��w��j
    public float rayHeight = 5f;          // ��󂩂�Ray���������i�X�|�[��Y�̊�j
    public float rayDistance = 10f;       // Ray�̒����i�ǂ̂��炢���ɒ��ׂ邩�j
    public float enemyHeightOffset = 0.6f;// �n�ʂ��班����ɕ������ďo���i�߂荞�ݖh�~�j

    List<GameObject> spawnedEnemies = new List<GameObject>(); // �X�|�[�������]���r�̋L�^


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �w��Ԋu�ŃX�|�[���������Ăяo���i�ŏ���2�b��j
        InvokeRepeating("SpawnEnemyInArea", 2f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �X�|�[�������{��
    void SpawnEnemyInArea()
    {
        // ��x��3�̃X�|�[��
        for (int i = 0; i < 3; i++)
        {
            // �X�|�[���͈͓��Ń����_����X���W������
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);

            // ��󂩂�Ray�������߂�Y���W�i�͈͂̏�[��肳��ɏ�j
            float spawnY = spawnAreaMax.y + rayHeight;

            Vector2 rayOrigin = new Vector2(spawnX, spawnY);

            // Ray�������i�f�o�b�O�p�j
            Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1f);

            // �n�ʂ�T��Raycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundLayer);

            if (hit.collider != null)
            {
                // �n�ʂ̃q�b�g�ʒu�ɁA�I�t�Z�b�g�������ăX�|�[���ʒu������
                Vector2 spawnPos = new Vector2(spawnX, hit.point.y + enemyHeightOffset);

                // �]���r�𐶐�
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                // �v���C���[�̈ʒu�����Č��������E���]
                if (player != null)
                {
                    Vector3 scale = newEnemy.transform.localScale;
                    scale.x = (spawnPos.x < player.position.x) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                    newEnemy.transform.localScale = scale;
                }

                // �]���r���|���ꂽ�Ƃ��Ƀ��X�g����폜
                newEnemy.GetComponent<enemy_control>().mOnDestly += (GameObject g) =>
                {
                    spawnedEnemies.Remove(g);
                };

                // ���̃]���r�Ɠ����蔻��𖳌����i���蔲��������j
                CapsuleCollider2D ignoreCollider = newEnemy.GetComponent<CapsuleCollider2D>();
                foreach (GameObject enemy in spawnedEnemies)
                {
                    if (enemy != null)
                    {
                        CapsuleCollider2D col = enemy.GetComponent<CapsuleCollider2D>();
                        Physics2D.IgnoreCollision(col, ignoreCollider);
                    }
                }

                // ���X�g�ɒǉ�
                spawnedEnemies.Add(newEnemy);
            }
            else
            {
                // �n�ʂ�������Ȃ������ꍇ�̌x�����O
                Debug.LogWarning("�n�ʂ�������Ȃ��������߁A�]���r���X�|�[�����܂���ł����BX���W: " + spawnX);
            }
        }
    }

    // �X�|�[���͈͂�Scene�r���[�ŉ����i�I�𒆂̂݁j
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // ���S�ƃT�C�Y���v�Z���ĕ`��
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2f;
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 1);

        Gizmos.DrawWireCube(center, size);
    }

}
