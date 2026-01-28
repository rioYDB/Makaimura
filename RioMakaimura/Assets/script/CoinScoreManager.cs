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

    // プレイヤーへの参照を追加
    private player_control playerHealth;

    // コルーチンを1つだけ動かすための変数を追加
    private Coroutine scaleCoroutine;
    private Vector3 originalScale;

    // ゲーム開始時に1回だけ呼ばれる
    private void Awake()
    {
        // このスクリプトを唯一のインスタンスとして登録
        instance = this;
    }

    void Start()
    {
        // プレイヤーを検索して取得
        playerHealth = FindAnyObjectByType<player_control>();

        scoreText.text = "Coins: " + score.ToString();

        originalScale = scoreText.transform.localScale; // ← 元の大きさを記録
    }

    // Update is called once per frame
    void Update()
    {

    }

    // スコアを加算する関数
    public void AddScore(int value)
    {
        score += value;

        // ★修正：scoreTextがセットされている時だけ更新する（エラー防止）
        if (scoreText != null)
        {
            scoreText.text = "Coins: " + score.ToString();

            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScoreTextEffect());
        }

        if (score % 10 == 0)
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(1);
            }
            ResetScore();
        }
    }

    private IEnumerator ScoreTextEffect()
    {
        float duration = 0.1f;
        Vector3 targetScale = originalScale * 1.2f;
        // スケールをリセットしてから拡大
        scoreText.transform.localScale = originalScale;

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

        scoreText.transform.localScale = originalScale; // 念のため完全に戻す
        scaleCoroutine = null; // コルーチン完了
    }

    // スコアをリセットしたいときに使える関数（任意）
    public void ResetScore()
    {
        score = 0;
        if (scoreText != null)
        {
            scoreText.text = "Coins: " + score.ToString();
        }
    }

    // 現在のスコアを取得したいときに使う（任意）
    public int GetScore()
    {
        return score;
    }
  
}
