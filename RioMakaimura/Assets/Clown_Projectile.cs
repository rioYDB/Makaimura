using UnityEngine;

public class Clown_Projectile : MonoBehaviour
{
    public float lifetime = 5f; // 弾が自動で消えるまでの時間


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 一定時間後に自動で消える（メモリ節約）
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 自分がplayerに属している場合
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CircleCollider2D>());
        }

        //// プレイヤーに当たったときの処理（仮）
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // 例：ダメージ処理など
        //    // collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
        //}

        //// 何かに当たったら弾を消す
        //Destroy(gameObject);
    }
}
