using UnityEngine;

public class Cerberus_HP : MonoBehaviour
{
    public int maxHP;
    private int currentHP;

    // ★追加: 倒した後に開く扉の参照
    public Exitdoor exitDoor; // Exitdoor.cs を参照

    //色変化
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;


    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

        StartCoroutine(FlashRed());
    }


    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "が倒れた！");

        //扉を開ける命令を確実に行う
        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }




        //Cerberus_Controllerに死亡を通知 (コメントアウトを解除して有効化)
        Cerberus_Controller cerberus = GetComponent<Cerberus_Controller>();
        if (cerberus != null)
        {
            cerberus.StartDeathSequence(); // ★コメントアウトを解除して有効化★
        }
        else
        {
            // 一般的な敵の場合
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;

            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }
}
