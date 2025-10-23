using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("�N���������A�C�e��Prefab�i���ԂɑΉ��j")]
    public GameObject[] itemPrefabs;

    [Header("�X�|�[���ʒu�i�e�A�C�e���ɑΉ��j")]
    public Transform[] spawnPoints;

    [Header("�N���Ԋu�i�b�j")]
    public float spawnInterval = 3f;

    void Start()
    {
        // �A�C�e�����ƃX�|�[���ʒu������v���Ă��邩�m�F
        if (itemPrefabs.Length != spawnPoints.Length)
        {
            Debug.LogWarning("itemPrefabs��spawnPoints�̐�����v���Ă��܂���");
        }

        // ���Ԋu�ŗN�����鏈�����J��Ԃ�
        InvokeRepeating(nameof(SpawnAllItems), 0f, spawnInterval);
    }

    void SpawnAllItems()
    {
        // �e�X�|�[���ʒu�ɑΉ�����A�C�e���𐶐�
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < itemPrefabs.Length)
            {
                Instantiate(itemPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            }
        }
    }
}
