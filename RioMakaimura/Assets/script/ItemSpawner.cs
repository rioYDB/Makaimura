using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("湧かせたいアイテムPrefab（順番に対応）")]
    public GameObject[] itemPrefabs;

    [Header("スポーン位置（各アイテムに対応）")]
    public Transform[] spawnPoints;

    [Header("湧くまでの待機時間（秒）")]
    public float respawnDelay = 2f;

    private GameObject[] currentItems;     // 現在存在するアイテム
    private bool[] isRespawning;           // リスポーン中かどうか

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
            // アイテムが消えていて、まだリスポーン準備中でないなら
            if (currentItems[i] == null && !isRespawning[i])
            {
                StartCoroutine(RespawnItem(i));
            }
        }
    }

    System.Collections.IEnumerator RespawnItem(int index)
    {
        isRespawning[index] = true; // リスポーン中に設定
        yield return new WaitForSeconds(respawnDelay);

        if (index < itemPrefabs.Length && currentItems[index] == null)
        {
            currentItems[index] = Instantiate(itemPrefabs[index], spawnPoints[index].position, Quaternion.identity);
        }

        isRespawning[index] = false; // リスポーン完了
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