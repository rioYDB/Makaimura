using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; // Imageを扱うために追加

public class StartManager : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private TMP_Text stageTitleText;
    [SerializeField] private Image stageStartPanel; // ★追加：暗くするパネル
    [SerializeField] private string stageName = "Stage 1";

    void Start()
    {
        StartCoroutine(StageStartSequence());
    }

    IEnumerator StageStartSequence()
    {
        // 1. ゲームの進行を止める
        Time.timeScale = 0f;

        // 2. テキストと暗いパネルを表示
        stageTitleText.text = stageName;
        stageTitleText.gameObject.SetActive(true);
        if (stageStartPanel != null) stageStartPanel.gameObject.SetActive(true); // ★表示

        // 3. 3秒待つ
        yield return new WaitForSecondsRealtime(3.0f);

        // 4. テキストとパネルを非表示にする
        stageTitleText.gameObject.SetActive(false);
        if (stageStartPanel != null) stageStartPanel.gameObject.SetActive(false); // ★非表示

        // 5. ゲームの進行を再開する
        Time.timeScale = 1f;
    }
}
