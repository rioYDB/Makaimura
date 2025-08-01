using UnityEngine;

public class enemy_HP : MonoBehaviour
{
    public int maxHP;       // �ő�HP�i�C���X�y�N�^�[�Œ����j
    private int currentHP;

    //�F�ω�
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ���擾
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("�G���_���[�W���󂯂��B�c��HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

        StartCoroutine(FlashRed()); // �Ԃ��t���b�V��
    }


    protected virtual void Die()
    {
        Debug.Log("�G���|�ꂽ�I");
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;

            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration); // �����҂�
            spriteRenderer.color = originalColor;
        }
    }
}
