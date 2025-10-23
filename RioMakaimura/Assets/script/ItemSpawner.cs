using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("湧かせたいアイテムPrefab（順番に対応）")]
    public GameObject[] itemPrefabs;

    [Header("スポーン位置（各アイテムに対応）")]
    public Transform[] spawnPoints;

    [Header("湧く間隔（秒）")]
    public float spawnInterval = 3f;

    void Start()
    {
        // アイテム数とスポーン位置数が一致しているか確認
        if (itemPrefabs.Length != spawnPoints.Length)
        {
            Debug.LogWarning("itemPrefabsとspawnPointsの数が一致していません");
        }

        // 一定間隔で湧かせる処理を繰り返す
        InvokeRepeating(nameof(SpawnAllItems), 0f, spawnInterval);
    }

    void SpawnAllItems()
    {
        // 各スポーン位置に対応するアイテムを生成
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < itemPrefabs.Length)
            {
                Instantiate(itemPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            }
        }
    }
}
