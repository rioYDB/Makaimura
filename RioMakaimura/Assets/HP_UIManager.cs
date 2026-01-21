using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HP_UIManager : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private player_control player; // シーン上のPlayerをドラッグ&ドロップ
    [SerializeField] private Image playerIconImage;  // UIのImage
    [SerializeField] private TMP_Text lifeText;      // UIのText

    [Header("表示用アイコン設定")]
    [SerializeField] private Sprite humanIcon;
    [SerializeField] private Sprite okamiIcon;
    [SerializeField] private Sprite witchIcon;
    [SerializeField] private Sprite vampireIcon;

    void Update()
    {
        if (player == null) return;

        // 1. プレイヤーの状態（AttackType）に合わせてアイコンを切り替える
        UpdateIcon(player.CurrentAttackType);

        // 2. プレイヤーのHP（残機）を取得してテキストを更新する
        // player_control.cs 内の HP 変数にアクセス（※後述の修正が必要）
        UpdateLifeText();
    }

    private void UpdateIcon(AttackType type)
    {
        switch (type)
        {
            case AttackType.Human:
                playerIconImage.sprite = humanIcon;
                break;
            case AttackType.Okami:
                playerIconImage.sprite = okamiIcon;
                break;
            case AttackType.Which:
                playerIconImage.sprite = witchIcon;
                break;
            case AttackType.Vampire:
                playerIconImage.sprite = vampireIcon;
                break;
        }
    }

    private void UpdateLifeText()
    {
        // プレイヤーの現在のHPを表示（例: × 2）
        // HP変数を取得するためにplayer_control側の修正が必要です
        lifeText.text = "× " + player.GetCurrentHP();
    }
}
