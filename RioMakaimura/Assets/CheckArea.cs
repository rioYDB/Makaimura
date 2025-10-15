using UnityEngine;

public class CheckArea : MonoBehaviour
{
    // ★ Inspectorで設定する項目
    public Downdoor targetDoor; // 閉じる扉のコントローラーをここに設定
    public bool triggerOnce = true;          // 一度発動したら再発動しないようにするか

    private bool hasTriggered = false;       // 既に発動したかどうかのフラグ

    // このエリアに何か入ってきたときの処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 既に発動済みで、一度きりのトリガーなら何もしない
        if (triggerOnce && hasTriggered)
        {
            return;
        }

        // プレイヤーが触れたことを確認
        if (other.CompareTag("Player"))
        {
            if (targetDoor != null)
            {
                targetDoor.StartFalling(); // 扉を閉じる命令
                hasTriggered = true;    // 発動フラグを立てる

                // トリガーエリアのコライダーを無効化（オプション：一度きりの発動を確実にする）
                if (triggerOnce)
                {
                    GetComponent<Collider2D>().enabled = false;
                }
            }
            else
            {
                Debug.LogWarning("Target Doorが設定されていません。Trigger ZoneのInspectorを確認してください。");
            }
        }
    }
}
