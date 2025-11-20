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

    // ★追加: 衝突フラグと参照
    private bool isPlayerTouching = false; // プレイヤーが今フレームで触れたか
    private Rigidbody2D touchingPlayerRb = null; // 衝突中のプレイヤーのRb
    private Collider2D touchingPlayerCollider = null; // 衝突中のプレイヤーのCollider

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        wallCollider = GetComponent<Collider2D>();

        // 壁はIs Trigger OFF（ソリッド）であることを前提
        if (wallCollider != null)
        {
            wallCollider.enabled = true; // 確実にON
        }
    }

    // 【重要】OnCollisionStay2Dに変更し、壁に触れている間常にチェックする

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag)&&isPlayerTouching==false)
        {
            player_control player = collision.gameObject.GetComponent<player_control>();

            // プレイヤーが制御スクリプトを持っているかチェック
            if (player == null) return;

            // === プレイヤーの速度と方向を判定 ===
            Vector2 playerVelocity = player.Rb.linearVelocity;

            // プレイヤーが壁に向かって押しているか（通過しようとしているか）
            // プレイヤーの移動方向と壁の法線（衝突点）をチェックすることで、壁を押しているかを厳密に判定できる
            // ただし、単純化のためここではX軸の入力のみチェック
            float playerInput = Input.GetAxisRaw("Horizontal");

            // 衝突点から壁の法線（壁の向き）を取得
            Vector2 normal = collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector2.zero;


            // --- ★コアロジックの実行★ ---
            if (player.CurrentAttackType == AttackType.Which && Mathf.Abs(playerInput) > 0.1f)
            {
                // 魔女が壁を押している場合

                // 1. 衝突無視を有効化
                if (!isCurrentlyFaded)
                {
                    // 衝突無視を有効にすることで、魔女は壁の中にめり込むことができ、通過可能になる
                    Physics2D.IgnoreCollision(wallCollider, collision.collider, true);

                    isPlayerTouching = true;
                    
                    // 2. 透過アニメーション開始
                    StartCoroutine(SetWallState(true));
                }
            }
            else // 非魔女、または魔女だが壁を押していない場合
            {
                // 1. 衝突無視を解除（非魔女はブロックされ続ける）
                if (isCurrentlyFaded)
                {
                    Physics2D.IgnoreCollision(wallCollider, collision.collider, false);

                    // 2. 透過アニメーションを逆再生
                    StartCoroutine(SetWallState(false));
                }
                // 3. 非魔女の場合、このブロック以外では処理を行わないため、壁はソリッドのままブロックする

                
            }
        }

        isPlayerTouching = false;
    }

  
        // 壁の状態を切り替えるコルーチン (透過処理のみ)
        IEnumerator SetWallState(bool fadeOut)
    {
        // 状態変更をチェックし、コルーチンが二重に実行されるのを防ぐ
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