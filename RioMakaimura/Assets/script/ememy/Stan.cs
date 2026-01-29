using UnityEngine;

public class Stan : MonoBehaviour
{
    public float lifeTime = 0.5f;
    public float stunTime = 2.0f;

    private Collider2D myCollider;
    private ContactFilter2D contactFilter;
    private Collider2D[] results = new Collider2D[5];

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        // プレイヤーを検知するためのフィルター設定
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(LayerMask.GetMask("Player")); // Playerレイヤーのみを対象にする

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 物理設定（チェック）を無視して、重なっているコライダーを取得
        int count = myCollider.Overlap(contactFilter, results);

        for (int i = 0; i < count; i++)
        {
            if (results[i].CompareTag("Player"))
            {
                player_control player = results[i].GetComponent<player_control>();
                if (player != null)
                {
                    player.playerHP(1);
                    player.Stun(stunTime);

                    // 1回当たったらこの衝撃波の判定を消す（連続ダメージ防止）
                    myCollider.enabled = false;
                    break;
                }
            }
        }
    }
}