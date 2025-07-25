using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    public float moveSpeed = 10.0f; // 弾が飛ぶ速度
    public float lifetime = 5.0f;   // 画面外に出るか、一定時間で消える時間

    private int directionX; // 弾が飛ぶX方向 (1:右, -1:左)

    // 発射時にLauncherから方向を受け取るためのメソッド
    public void SetDirection(int dir)
    {
        directionX = dir;
        // 弾の見た目を方向に合わせて反転させる
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * directionX, transform.localScale.y, transform.localScale.z);
    }

    void Start()
    {
        Destroy(gameObject, lifetime); // 一定時間後に自身を消す
    }

    void Update()
    {
        // 弾を移動させる
        transform.Translate(Vector3.right * directionX * moveSpeed * Time.deltaTime, Space.World);
    }

    // プレイヤーにダメージを与える
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("槍に触れてダメージ！");
            other.GetComponent<player_control>().playerHP(1); // player_controlにTakeDamageメソッドがあれば
            Destroy(gameObject); // プレイヤーに当たったら槍は消える
        }
    }

    // 画面外に出たら自動的に消える (もしlifetimeより早く画面外に出る場合)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
