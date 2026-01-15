using UnityEngine;
using System.Collections;

public class enemy_HP : MonoBehaviour
{
    public int maxHP;       // 最大HP（インスペクターで調整）
    private int currentHP;

    //色変化
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    
    //ダメージエフェクト
    public GameObject D_Effect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer を取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("敵がダメージを受けた。残りHP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

        StartCoroutine(DamageEffect());

        StartCoroutine(FlashRed()); // 赤くフラッシュ
    }


    protected virtual void Die()
    {
        Debug.Log("敵が倒れた！");
        Destroy(gameObject);
    }

   

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;

            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration); // 少し待つ
            spriteRenderer.color = originalColor;
        }
    }

    //ダメージエフェクト(コインのエフェクトを流用)
    private IEnumerator DamageEffect()
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
        if (D_Effect != null)
        {
            GameObject effect = Instantiate(D_Effect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f); // 0.5秒で消す
        }

        // 少し待ってから消える
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }
}
