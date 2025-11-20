using UnityEngine;
using System.Collections.Generic;

public class Mummy_Spawn : MonoBehaviour
{

    public GameObject enemyPrefab;        // ミイラのプレハブ
    public Transform player;              // プレイヤー（向きの判断に使用）

    public float spawnInterval = 3f;      // スポーン間隔（秒）

    public Vector2 spawnAreaMin;          // スポーン範囲の左下座標（インスペクターで設定）
    public Vector2 spawnAreaMax;          // スポーン範囲の右上座標（インスペクターで設定）

    public LayerMask groundLayer;         // 地面のレイヤー（"Ground" などを指定）
    public float rayHeight = 5f;          // 上空からRayを撃つ高さ（スポーンYの基準）
    public float rayDistance = 10f;       // Rayの長さ（どのくらい下に調べるか）
    public float enemyHeightOffset = 0.6f;// 地面から少し上に浮かせて出す（めり込み防止）

    List<GameObject> spawnedEnemies = new List<GameObject>(); // スポーンしたゾンビの記録


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 指定間隔でスポーン処理を呼び出す（最初は2秒後）
        InvokeRepeating("SpawnEnemyInArea", 2f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // スポーン処理本体
    void SpawnEnemyInArea()
    {
        // 一度に3体スポーン
        for (int i = 0; i < 3; i++)
        {
            // スポーン範囲内でランダムなX座標を決定
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);

            // 上空からRayを撃つためのY座標（範囲の上端よりさらに上）
            float spawnY = spawnAreaMax.y + rayHeight;

            Vector2 rayOrigin = new Vector2(spawnX, spawnY);

            // Rayを可視化（デバッグ用）
            Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1f);

            // 地面を探すRaycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundLayer);

            if (hit.collider != null)
            {
                // 地面のヒット位置に、オフセットを加えてスポーン位置を決定
                Vector2 spawnPos = new Vector2(spawnX, hit.point.y + enemyHeightOffset);

                // ゾンビを生成
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                // プレイヤーの位置を見て向きを左右反転
                if (player != null)
                {
                    Vector3 scale = newEnemy.transform.localScale;
                    scale.x = (spawnPos.x < player.position.x) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                    newEnemy.transform.localScale = scale;
                }

                // ゾンビが倒されたときにリストから削除
                newEnemy.GetComponent<enemy_control>().mOnDestly += (GameObject g) =>
                {
                    spawnedEnemies.Remove(g);
                };

                // 他のゾンビと当たり判定を無効化（すり抜けさせる）
                CapsuleCollider2D ignoreCollider = newEnemy.GetComponent<CapsuleCollider2D>();
                foreach (GameObject enemy in spawnedEnemies)
                {
                    if (enemy != null)
                    {
                        CapsuleCollider2D col = enemy.GetComponent<CapsuleCollider2D>();
                        Physics2D.IgnoreCollision(col, ignoreCollider);
                    }
                }

                // リストに追加
                spawnedEnemies.Add(newEnemy);
            }
            else
            {
                // 地面が見つからなかった場合の警告ログ
                Debug.LogWarning("地面が見つからなかったため、ゾンビをスポーンしませんでした。X座標: " + spawnX);
            }
        }
    }

    // スポーン範囲をSceneビューで可視化（選択中のみ）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 中心とサイズを計算して描画
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2f;
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 1);

        Gizmos.DrawWireCube(center, size);
    }

}
