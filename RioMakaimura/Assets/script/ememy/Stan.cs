using UnityEngine;

public class Stan : MonoBehaviour
{
    [Header("スタン効果設定")]
    public float duration = 0.5f;           // 衝撃波の生存時間（見た目が続く時間）
    public float stunTime = 2.0f;           // 敵やプレイヤーがスタンする時間

    // Startで実行される衝突判定は、OnTriggerEnter2Dに任せます。

    void Start()
    {
        // 衝撃波はすぐに消えるため、生成と同時にタイマーを開始
        Destroy(gameObject, duration);
    }

    // 衝撃波のコライダーに触れたオブジェクトへの処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 敵味方問わずスタンさせるため、ターゲットをチェック

        // プレイヤーの検出
        if (other.CompareTag("Player"))
        {
            // プレイヤーのコントローラを取得し、スタンメソッドを呼び出す
            player_control playerController = other.GetComponent<player_control>();
            if (playerController != null)
            {
                // playerController.Stun(stunTime); // プレイヤー制御スクリプトにStunメソッドが必要
                Debug.Log("プレイヤーがスタンしました！");
            }
        }
        // 敵の検出 (ケルベロス以外の他の敵も含む)
        else if (other.CompareTag("Enemy"))
        {
            // 敵のコントローラを取得し、スタンメソッドを呼び出す
            // フランケン自身はスタンさせないように注意が必要です (Franken_Controllerは無視)
            if (other.GetComponent<Frankenstein>() == null)
            {
                // EnemyController enemyController = other.GetComponent<EnemyController>();
                // if (enemyController != null) enemyController.Stun(stunTime); 
                Debug.Log(other.gameObject.name + "がスタンしました！");
            }
        }
    }
}
