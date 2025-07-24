using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0.5f;     // プレイヤーが乗ってから落下するまでの時間
    public float destroyDelay = 2.0f;  // 落下を開始してから完全に消えるまでの時間
    public float resetDelay = 3.0f;    // 落下後に元の位置に戻るまでの時間 (再利用する場合)
    public bool canRespawn = true;     // 落下後に元の位置に戻るか（再利用するか）

    private Vector3 initialPosition;   // 床の初期位置を記憶
    private Rigidbody2D rb;            // Rigidbody2Dへの参照

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position; // 初期位置を記録

        // 最初は落下しないように重力を無効にしておく
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Kinematicにして物理的な影響を受けないようにする
            // または rb.gravityScale = 0; としても良いが、Kinematicの方がより安定する
        }
    }

    // プレイヤーが乗ったことを検出する
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーが上に乗ったことを確認
        // プレイヤーのタグが "Player" であることを前提
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが床の上に乗っていることを確認 (オプション: 真上から乗ったか)
            // if (collision.contacts[0].normal.y > 0.5f) // 床の法線が上向き（プレイヤーが上から来た）
            // {
            StartCoroutine(FallAndRespawn());
            // }
        }
    }

    // 落下とリスポーンのコルーチン
    IEnumerator FallAndRespawn()
    {
        // プレイヤーが乗ってからfallDelay秒待つ
        yield return new WaitForSeconds(fallDelay);

        // 重力を有効にして落下させる
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Dynamicにして重力を有効にする
            rb.gravityScale = 1f; // ここで重力をかける (SetGravityを使わない場合)
        }

        // 落下開始からdestroyDelay秒後に床を消す（または無効にする）
        yield return new WaitForSeconds(destroyDelay);

        // 床を消すか、無効にする
        gameObject.SetActive(false); // オブジェクトを非表示・無効化

        // 再利用しない場合はここでコルーチンを終了
        if (!canRespawn)
        {
            Destroy(gameObject); // 完全削除
            yield break; // コルーチンを終了
        }

        // リスポーンする場合
        yield return new WaitForSeconds(resetDelay); // リセットまでの時間を待つ

        // 床を元の位置に戻し、物理をリセットして再表示
        transform.position = initialPosition;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;      // 速度をリセット
            rb.angularVelocity = 0f;         // 回転速度をリセット
            rb.bodyType = RigidbodyType2D.Kinematic; // 再びKinematicに戻す
            rb.gravityScale = 0f;            // 重力を0に戻す
        }
        gameObject.SetActive(true); // 再表示
    }



    // エディタ上で初期位置を表示 (オプション)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(initialPosition, 0.3f); // 初期位置に球を表示
    }
}