using UnityEngine;

public class BossBodyDamege : MonoBehaviour
{
    [Header("ダメージ設定")]
    public int damageAmount = 1;
    public float damageInterval = 1.0f; // 連続ダメージの間隔
    private float nextDamageTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        // プレイヤーに触れている間
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            player_control player = other.GetComponent<player_control>();
            if (player != null)
            {
                player.playerHP(damageAmount);
                nextDamageTime = Time.time + damageInterval;
                Debug.Log("フランケン本体に接触：ダメージ！");
            }
        }
    }
}
