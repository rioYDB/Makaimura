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
		// プレイヤーのRigidbodyを取得（シーンに1人想定）
		GameObject playerObj = GameObject.FindWithTag("Player");
		Rigidbody2D playerRb = playerObj?.GetComponent<Rigidbody2D>();

		// 1. ゲームの進行を止め、プレイヤーを物理演算から切り離す
		Time.timeScale = 0f;
		if (playerRb != null)
		{
			// 物理演算の影響を受けないようにする（これで落ちなくなります）
			playerRb.bodyType = RigidbodyType2D.Kinematic;
			playerRb.linearVelocity = Vector2.zero; // 念のため速度もゼロに
		}

		// 2. テキストと暗いパネルを表示
		stageTitleText.text = stageName;
		stageTitleText.gameObject.SetActive(true);
		if (stageStartPanel != null) stageStartPanel.gameObject.SetActive(true);

		// 3. 3秒待つ
		yield return new WaitForSecondsRealtime(3.0f);

		// 4. テキストとパネルを非表示にする
		stageTitleText.gameObject.SetActive(false);
		if (stageStartPanel != null) stageStartPanel.gameObject.SetActive(false);

		// 5. プレイヤーを物理演算に戻し、ゲームを再開する
		if (playerRb != null)
		{
			playerRb.bodyType = RigidbodyType2D.Dynamic; // 動的な状態に戻す
		}
		Time.timeScale = 1f;
	}
}
