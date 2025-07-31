using UnityEngine;

public class enemy_HP : MonoBehaviour
{
    public int maxHP;       // �ő�HP�i�C���X�y�N�^�[�Œ����j
    private int currentHP;

    private SpriteRenderer spriteRenderer;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer ���A�^�b�`����Ă��܂���: " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    protected virtual void Die()
    {
        // �A�j���[�V������G�t�F�N�g��ǉ����Ă��悢
        Destroy(gameObject);
    }
}
