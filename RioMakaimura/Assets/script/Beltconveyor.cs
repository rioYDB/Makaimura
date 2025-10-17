using UnityEngine;

public class Beltconveyor : MonoBehaviour
{
    // ★ Inspectorで設定する項目
    public float speed = 3.0f;              // コンベアがオブジェクトを流す速度
    public bool moveRight = true;           // 右に流すか (true: 右, false: 左)

    private float direction;                // 内部で使用する流れる方向 (-1 または 1)

    void Start()
    {
        // speed と moveRight に基づいて方向を決定
        direction = moveRight ? 1f : -1f;
    }

    // 衝突中のオブジェクトが Trigger を通過している間、継続的に実行される
    private void OnTriggerStay2D(Collider2D other)
    {
        // プレイヤーのタグであることを確認
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // プレイヤーのRigidbody2DのX軸の速度をコンベアの速度に設定する
                // Y軸の速度（ジャンプや落下）は維持する

                // プレイヤーのY軸速度（ジャンプや落下）は維持する
                float currentYVelocity = rb.linearVelocity.y;

                rb.linearVelocity =new Vector2(speed * direction, currentYVelocity);
            }
        }
        // 他にも押したいオブジェクトがあれば、そのタグもチェックする
    }
}
