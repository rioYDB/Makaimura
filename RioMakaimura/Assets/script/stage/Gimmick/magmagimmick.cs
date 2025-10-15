using UnityEngine;
using System.Collections;
public class magmagimmick : MonoBehaviour
{
    public float initialDelay = 1.0f;    // ゲーム開始後、最初に噴き出すまでの時間
    public float flyingMagmaSpeed = 5.0f; // マグマが上に飛ぶ速度 (変数名修正: flyingMagma -> flyingMagmaSpeed)
    public float fallGravityScale = 1.0f; // 噴き出し後に落下する際の重力スケール (デフォルト値を1.0fに推奨)
    public float activeDuration = 1.0f;  // マグマが噴き出している（上昇中・落下中）の総時間
    public float coolDownDuration = 2.0f; // 引っ込んでから次に噴き出すまでのクールダウン時間
    public float hidePositionOffset = -10.0f; //マグマが完全に隠れるY座標オフセット (地面の下の十分に低い位置)
    

    private Animator animator; // 噴き出しアニメーション用 (オプション)
    private Collider2D hazardCollider; // ダメージ判定用コライダー
    private SpriteRenderer spriteRenderer; // 見た目表示用
    private Rigidbody2D rb; // Rigidbody2Dへの参照

    private Vector3 start噴き出しPosition; // マグマが地面から噴き出しを「開始する」Y座標の基準点
    private bool isHiding = false; // 現在隠れている最中か
    private float currentAlpha = 1.0f; // 現在の透明度

    void Start()
    {
        animator = GetComponent<Animator>();
        hazardCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("MagmaGimmickにはRigidbody2Dが必要です！", this);
            this.enabled = false; // スクリプトを無効化
            return;
        }

        // 噴き出し開始位置を、このオブジェクトが持つ初期位置とする
        start噴き出しPosition = transform.position;

        // 最初は完全に不透明にしておく
        if (spriteRenderer != null)
        {
            currentAlpha = 1.0f;
            Color tempColor = spriteRenderer.color;
            tempColor.a = currentAlpha;
            spriteRenderer.color = tempColor;
        }

        // Rigidbody2DをDynamicに設定し、常に物理演算の影響を受けるようにする
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f; // 最初は重力0
        rb.linearVelocity = Vector2.zero; // 速度も0

        // 最初は非表示に見えるようにする（地面の下に配置する）
        // transform.position = start噴き出しPosition + new Vector3(0, hidePositionOffset, 0); // 初期位置を地面の下に設定することもできる
        // または、初期位置はInspectorで設定し、Yをリセットしてスタートする
        StartCoroutine(GeyserCycle()); // 噴き出しサイクルを開始
    }

    IEnumerator GeyserCycle()
    {
        // まずは初期の隠れた位置に移動 (Inspectorで設定した開始Y座標より下のhidePositionOffset)
        // 最初の噴き出しの前に完全に隠れるようにする
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // 移動を止める
            rb.gravityScale = 0f; // 重力も無効
        }
        transform.position = start噴き出しPosition + new Vector3(0, hidePositionOffset, 0);

        // 最初だけ待機
        yield return new WaitForSeconds(initialDelay);

        while (true) // 無限ループでサイクルを繰り返す
        {
            // 噴き出す準備 (地面の基準点に移動し、透明度を戻す)
            // ワープせずにスムーズに現れるようにするなら、隠れた状態から上昇する
            // ここでは瞬時に地面の基準点に移動させておく
            transform.position = start噴き出しPosition; // 地面から噴き出す基準位置にリセット
            rb.linearVelocity = Vector2.zero; // 速度もリセット
            rb.gravityScale = 0f; // 上昇開始時は重力0

            

            // コライダーを有効にする (見た目が出ている間は有効)
            if (hazardCollider != null) hazardCollider.enabled = true;

            // アニメーターのトリガーを起動
            if (animator != null) animator.SetTrigger("Erupt");

            // 上に噴き出す速度を与える
            rb.linearVelocity = new Vector2(0, flyingMagmaSpeed); // 真上に飛ぶ速度を与える

            // activeDurationの間、噴き出し続ける (上昇・落下)
            yield return new WaitForSeconds(activeDuration);

            // activeDuration後、落下用重力を適用
            rb.gravityScale = fallGravityScale; // 落下用重力を適用

          

            // コライダーを無効にする (完全に隠れたら無効)
            if (hazardCollider != null) hazardCollider.enabled = false;

            // アニメーターの"Retract"トリガーを起動 (あれば)
            if (animator != null) animator.SetTrigger("Retract");

            // マグマが完全に隠れるY座標まで物理的に落下する
            // rb.gravityScaleがfallGravityScaleなので、そのまま落ちていく
            // transform.position.y が hidePositionOffset より下になるまで待機
            yield return new WaitUntil(() => transform.position.y <= start噴き出しPosition.y + hidePositionOffset);

            // 完全に隠れたら速度をリセットして次のサイクルへ
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f; // 次の噴き出しまで重力0

            yield return new WaitForSeconds(coolDownDuration); // クールダウン時間
        }
    }

    //アルファ値を変更するコルーチン
    

    // プレイヤーにダメージを与える (プレイヤーのタグが"Player"であることを前提)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ここにプレイヤーにダメージを与える処理を書く
            Debug.Log("マグマに触れてダメージ！");
            other.GetComponent<player_control>().playerHP(1); // player_controlにTakeDamageメソッドがあれば
        }
    }

   
}
