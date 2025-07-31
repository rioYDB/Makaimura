using UnityEngine;

public class Cerberus_bullet : MonoBehaviour
{
    public float moveSpeed = 5f; // 炎の移動速度
    public float lifeTime = 3f; // 炎が消えるまでの時間
    public int damageAmount = 2; // 炎が与えるダメージ量

    private Vector2 moveDirection; // 炎の進行方向

    // ケルベロスから呼ばれて、炎の進行方向を設定する
    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized; // 方向を正規化して渡す
        // 生成時にすぐに移動を開始する
    }

    void Start()
    {
        // 一定時間後に自身を消滅させる
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 進行方向に移動
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    // プレイヤーとの衝突判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーのタグが"Player"であることを確認
        {
            // プレイヤーにダメージを与える処理を呼び出す
            // 例: other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            Debug.Log("プレイヤーに炎が当たった！");

            // 炎は一度当たったら消えるようにする
            Destroy(gameObject);
        }
        
    }
}
