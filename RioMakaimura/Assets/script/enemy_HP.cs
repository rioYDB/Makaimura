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

	private bool isDead = false;


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

	//	public void TakeDamage(int damage)
	//{
	//    if (isDead) return; // ← ここが一番大事

	//    currentHP -= damage;
	//    Debug.Log("敵がダメージを受けた。残りHP: " + currentHP);

	//    StartCoroutine(FlashRed());

	//    if (currentHP <= 0)
	//    {
	//        Die();
	//    }
	//    else
	//    {
	//        StartCoroutine(DamageEffect());
	//    }
	//}

	public void TakeDamage(int damage)
	{
		if (isDead) return; // ← ここが一番大事

		currentHP -= damage;
		Debug.Log("敵がダメージを受けた。残りHP: " + currentHP);

		StartCoroutine(FlashRed());

		if (currentHP <= 0)
		{
			Die();
		}
		else
		{
			StartCoroutine(DamageEffect());
		}
	}




	//protected virtual void Die()
	//{
	//	Debug.Log("敵が倒れた！");
	//	StartCoroutine(DeathEffect());
	//}
	protected virtual void Die()
	{
		if (isDead) return;

		isDead = true; // ← 先に立てる！！
		Debug.Log("敵が倒れた！");
		StartCoroutine(DeathEffect());
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

	/// <summary>
	///ダメージエフェクト(コインのエフェクトを流用)
	/// </summary>
	/// <returns></returns>
	private IEnumerator DamageEffect()
	{
		float duration = 0.05f; // 拡大にかける時間
		Vector3 originalScale = transform.localScale;
		Vector3 targetScale = originalScale * 1f;

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
		yield return new WaitForSeconds(0.003f);

	}
	private IEnumerator DeathEffect()
	{
		// 全Colliderを無効化
		foreach (var col in GetComponentsInChildren<Collider2D>())
		{
			col.enabled = false;
		}

		// Rigidbodyも止める（任意）
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
			rb.simulated = false;
		}

		float duration = 0.05f;
		Vector3 originalScale = transform.localScale;
		Vector3 targetScale = originalScale * 1.3f;

		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			transform.localScale =
				Vector3.Lerp(originalScale, targetScale, timer / duration);
			yield return null;
		}

		if (D_Effect != null)
		{
			GameObject effect = Instantiate(D_Effect, transform.position, Quaternion.identity);
			Destroy(effect, 0.3f);
		}

		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
	}


}
