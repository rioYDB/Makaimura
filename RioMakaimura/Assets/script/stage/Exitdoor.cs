using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    // ★ Inspectorで設定する項目
    public Sprite openSprite;           // 扉が開いた状態のスプライト
    public Sprite closedSprite;         // 扉が閉じた状態のスプライト（初期値）

    [Tooltip("開いているかどうか")]
    public bool isOpen = false;

    // ★ 扉の挙動に必要なコンポーネント
    private SpriteRenderer sr;
    private Collider2D doorCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();

        // 初期状態を設定 (通常は閉じている状態)
        SetDoorState(isOpen);
    }

    // 敵の死亡スクリプトから呼び出される公開メソッド
    public void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("ボスが倒されました。扉が開きます！");
            isOpen = true;
            SetDoorState(isOpen);
        }
    }

    // 扉の見た目とコライダーを更新するプライベートメソッド
    private void SetDoorState(bool open)
    {
        if (open)
        {
            // 扉を開ける
            sr.sprite = openSprite;
            if (doorCollider != null)
            {
                doorCollider.enabled = false; // プレイヤーが通れる
            }
        }
        else
        {
            // 扉を閉じる
            sr.sprite = closedSprite;
            if (doorCollider != null)
            {
                doorCollider.enabled = true; // プレイヤーが通れない
            }
        }
    }

    // プレイヤーが触れたときの処理（オプション）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            // 例: 次のシーンへ遷移
            // UnityEngine.SceneManagement.SceneManager.LoadScene("NextLevel");
        }
    }
}
