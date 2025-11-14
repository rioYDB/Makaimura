using UnityEngine;
using System.Collections;


public class FadeArea : MonoBehaviour
{
    // ★ Inspectorで設定する項目
    public float FadeDuration = 0.5f;
    public float TargetAlpha = 0.3f;
    public string PlayerTag = "Player";

    private SpriteRenderer sr;
    private Collider2D wallCollider;
    private bool isCurrentlyFaded = false;
    private int playerLayer; // プレイヤーのレイヤー番号

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        wallCollider = GetComponent<Collider2D>();
        playerLayer = LayerMask.NameToLayer(PlayerTag); // Playerタグからレイヤー番号を取得（通常はPlayerレイヤー）

        // 壁は初期状態でSOLID（物理的）で、不透過であることを前提
        if (wallCollider != null)
        {
            wallCollider.enabled = true; // 確実にON
        }
    }

    // プレイヤーが壁に衝突している間、継続的に実行
    // プレイヤーが壁に衝突した時、一度だけ実行
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            player_control player = collision.gameObject.GetComponent<player_control>();

            if (player != null && player.CurrentAttackType == AttackType.Which)
            {
                // 1. 衝突無視を有効化
                Physics2D.IgnoreCollision(wallCollider, collision.collider, true);

                // ★★★ 修正ポイント: プレイヤーを壁の中に少し押し込む ★★★
                // プレイヤーが壁の中にめり込むように、移動方向の反対側にプレイヤーを少し移動させる
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                float pushAmount = 0.05f; // 押し込む距離（調整可能）

                // 衝突の瞬間、プレイヤーを壁から引き離す方向に、ごくわずかに移動させる
                // または、衝突を無視した直後、プレイヤーを壁の反対側へ一瞬だけ押し出す。

                // シンプルな押し込み: プレイヤーを壁の厚み分だけ押す
                Vector2 playerRbPos = collision.rigidbody.position;
                Vector2 offset = pushDirection * pushAmount;
                collision.rigidbody.position += offset;

                // プレイヤーの速度を再適用
                collision.rigidbody.linearVelocity = player.Rb.linearVelocity; // プレイヤーの現在の速度を維持
                                                                               // ★★★ 修正ポイントここまで ★★★

                StartCoroutine(SetWallState(true));
            }
            // else: 非魔女状態なら無視。
        }
    }

    // プレイヤーが壁から離れた時、実行
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            // 衝突無視を解除し、壁を復帰させる
            Physics2D.IgnoreCollision(wallCollider, collision.collider, false);
            StartCoroutine(SetWallState(false));
        }
    }

    // 壁の状態を切り替えるコルーチン (透過処理のみ)
    IEnumerator SetWallState(bool fadeOut)
    {
        if (fadeOut == isCurrentlyFaded) yield break;
        isCurrentlyFaded = fadeOut;

        Color targetColor = sr.color;
        targetColor.a = fadeOut ? TargetAlpha : 1.0f;

        Color startColor = sr.color;
        float timer = 0f;

        while (timer < FadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / FadeDuration;
            sr.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }
        sr.color = targetColor;
    }
}
