using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ZonbiSpawner : MonoBehaviour
{
    private player_control player_co;

    public Transform player;             // プレイヤーのTransform
    public GameObject enemyPrefab;       // 敵のプレハブ
    public float spawnDistance;     // プレイヤーから左右どれだけ離れた場所に出現させるか
    public float spawnInterval;     // スポーン間隔（秒）
    public float spawnWidthVariation = 2f; // 横方向のランダムなばらつき（プレイヤーの位置から±範囲）

    List<GameObject> spawnedEnemies = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーの情報取得
        player_co = GameObject.Find("player").GetComponent<player_control>();

        // 最初の出現を2秒後にして、その後はspawnInterval秒ごとに繰り返し出現
        InvokeRepeating("SpawnEnemyFromSide", 2f, spawnInterval);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemyFromSide()
    {
        if (player == null) return;

        
        // 三体ずつ出現
        for (int i = 0; i < 3; i++)
        {
            // 左右どちらかをランダムで選ぶ
            int side = Random.value < 0.5f ? -1 : 1;

            // 出現位置を計算（左右どちらか）
            float spawnX = player.position.x + spawnDistance * side;

            // 横方向にランダムな幅を追加
            spawnX += Random.Range(-spawnWidthVariation, spawnWidthVariation); // 横のばらつきを追加

            // Rayを上から下に撃って地面を探す
            Vector2 rayOrigin = new Vector2(spawnX, player.position.y + 5f);
            // Ray可視化用
            Vector2 rayDirection = Vector2.down;
            float rayDistance = 10f;

            // Debug用にRayを可視化
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 1f); // 1秒だけ赤い線を表示

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10f, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                // 最終的なスポーン位置
                float enemyHeightOffset = 0.6f;  // ゾンビの足元調整
                Vector2 spawnPos = new Vector2(spawnX, hit.point.y + enemyHeightOffset);

                // 敵を生成
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
                        // 衝突を無視する
                        Physics2D.IgnoreCollision(collider, ignoreCollider);
                    }
                }
                spawnedEnemies.Add(newEnemy);
            }
            else
            {
                Debug.LogWarning("地面が見つからなかったため、ゾンビをスポーンしませんでした。位置: " + spawnX);
            }

            //// 最終的なスポーン位置
            //Vector2 spawnPos = new Vector2(spawnX, player.position.y - 0.5f);


            //// 敵を生成
            //Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

}
