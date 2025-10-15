using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CoinScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 他のスクリプトから簡単にアクセスできるようにするための「シングルトン」設定
    public static CoinScoreManager instance;

    // スコアを表示するTextMeshPro UI
    public TMP_Text scoreText;

    // 現在のスコア
    private int score = 0;

    // ゲーム開始時に1回だけ呼ばれる
    private void Awake()
    {
        // このスクリプトを唯一のインスタンスとして登録
        instance = this;
    }

    void Start()
    {
        scoreText.text = "Coins: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // スコアを加算する関数
    public void AddScore(int value)
    {
        // スコアにvalue分を足す
        score += value;
        
        // Textにスコアを表示（UIを更新）
        scoreText.text = "Coins: " + score.ToString();

        // スコアUIをピョンっと動かす演出
        StartCoroutine(ScoreTextEffect());

        //10枚集めたらリセット
        if (score % 10 == 0)
        {
            // ここでスコアをリセット
            ResetScore();

            // 必要なら「ボーナス演出」や「音」もここで追加できる
            Debug.Log("10枚集めた！スコアをリセットしました。");
        }
    }

    private IEnumerator ScoreTextEffect()
    {
        float duration = 0.1f;
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // 少し待ってから元に戻す
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }

    // スコアをリセットしたいときに使える関数（任意）
    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Coins: " + score.ToString();
    }

    // 現在のスコアを取得したいときに使う（任意）
    public int GetScore()
    {
        return score;
    }
  
}
