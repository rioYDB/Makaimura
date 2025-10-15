using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{

    public int value = 1; // コインの価値（スコア用）
    private bool isCollected = false; // 連続で取られないようにするため
    public GameObject sparkleEffect; // ← パーティクルプレハブを登録する用
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが触れたら
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // スコアを加算する（スコア管理クラスがあれば）
            CoinScoreManager.instance.AddScore(value);

            // コインの拡大演出を実行
            StartCoroutine(CollectEffect());

            // 取得音（任意）
            // AudioSource.PlayClipAtPoint(coinSound, transform.position);


        }
    }

    private IEnumerator CollectEffect()
    {
        float duration = 0.05f; // 拡大にかける時間
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        //パーティクルを出す！
        if (sparkleEffect != null)
        {
            GameObject effect = Instantiate(sparkleEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f); // 0.5秒で消す
        }

        // 少し待ってから消える
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }
}
