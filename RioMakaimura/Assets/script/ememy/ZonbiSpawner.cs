using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ZonbiSpawner : MonoBehaviour
{
    private player_control player_co;

   /* public LayerMask Ground;  */                                                      //地面を判別するオブジェクトレイヤー

    public Transform player;             // プレイヤーのTransform
    public GameObject enemyPrefab;       // 敵のプレハブ
    public float spawnDistance;     // プレイヤーから左右どれだけ離れた場所に出現させるか
    public float spawnInterval;     // スポーン間隔（秒）
    public float spawnWidthVariation = 2f; // 横方向のランダムなばらつき（プレイヤーの位置から±範囲）


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

        if (player_co.IsGrounded())
        {
            // 三体ずつ出現
            for (int i = 0; i < 3; i++)
            {
                // 左右どちらかをランダムで選ぶ
                int side = Random.value < 0.5f ? -1 : 1;

                // 出現位置を計算（左右どちらか）
                float spawnX = player.position.x + spawnDistance * side;

                // 横方向にランダムな幅を追加
                spawnX += Random.Range(-spawnWidthVariation, spawnWidthVariation); // 横のばらつきを追加

                // 最終的なスポーン位置
                Vector2 spawnPos = new Vector2(spawnX, player.position.y - 0.5f);


                // 敵を生成
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }

        }

    }

}
