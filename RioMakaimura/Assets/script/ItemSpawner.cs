using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("�N���������A�C�e��Prefab�i���ԂɑΉ��j")]
    public GameObject[] itemPrefabs;

    [Header("�X�|�[���ʒu�i�e�A�C�e���ɑΉ��j")]
    public Transform[] spawnPoints;

    [Header("�N���܂ł̑ҋ@���ԁi�b�j")]
    public float respawnDelay = 2f;

    private GameObject[] currentItems;     // ���ݑ��݂���A�C�e��
    private bool[] isRespawning;           // ���X�|�[�������ǂ���

    void Start()
    {
        currentItems = new GameObject[spawnPoints.Length];
        isRespawning = new bool[spawnPoints.Length];
        SpawnAllItems();
    }

    void Update()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // �A�C�e���������Ă��āA�܂����X�|�[���������łȂ��Ȃ�
            if (currentItems[i] == null && !isRespawning[i])
            {
                StartCoroutine(RespawnItem(i));
            }
        }
    }

    System.Collections.IEnumerator RespawnItem(int index)
    {
        isRespawning[index] = true; // ���X�|�[�����ɐݒ�
        yield return new WaitForSeconds(respawnDelay);

        if (index < itemPrefabs.Length && currentItems[index] == null)
        {
            currentItems[index] = Instantiate(itemPrefabs[index], spawnPoints[index].position, Quaternion.identity);
        }

        isRespawning[index] = false; // ���X�|�[������
    }

    void SpawnAllItems()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < itemPrefabs.Length)
            {
                currentItems[i] = Instantiate(itemPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            }
        }
    }
}