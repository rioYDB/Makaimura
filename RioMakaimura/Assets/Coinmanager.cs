using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coinmanager : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private TMP_Text coinText; // 枚数を表示するTextMeshProテキスト

    void Update()
    {
        // CoinScoreManagerのインスタンスが存在するか確認
        if (CoinScoreManager.instance != null && coinText != null)
        {
            // 現在のスコアを取得して「× 5」の形式で表示
            // GetScore() メソッドが CoinScoreManager.cs にあることを確認してください
            coinText.text = "× " + CoinScoreManager.instance.GetScore().ToString();
        }
    }
}