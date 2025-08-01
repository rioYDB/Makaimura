using UnityEngine;

public class enemy_HP : MonoBehaviour
{
    public int maxHP;       // 最大HP（インスペクターで調整）
    private int currentHP;

    //色変化
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    

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
}
