using UnityEngine;

public class ZonbiSpawner : MonoBehaviour
{

    public Transform player;             // �v���C���[��Transform
    public GameObject enemyPrefab;       // �G�̃v���n�u
    public float spawnDistance;     // �v���C���[���獶�E�ǂꂾ�����ꂽ�ꏊ�ɏo�������邩
    public float spawnInterval;     // �X�|�[���Ԋu�i�b�j
    public float spawnWidthVariation = 2f; // �������̃����_���Ȃ΂���i�v���C���[�̈ʒu����}�͈́j


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

            // �ŏI�I�ȃX�|�[���ʒu
            Vector2 spawnPos = new Vector2(spawnX, -4.0f);


            // �G�𐶐�
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
       
    }
}
