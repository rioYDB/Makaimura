using UnityEngine;

public class Cerberus_HP : MonoBehaviour
{
    public int maxHP;
    private int currentHP;

    // ���ǉ�: �|������ɊJ�����̎Q��
    public Exitdoor exitDoor; // Exitdoor.cs ���Q��

    //�F�ω�
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
        Debug.Log("�G���_���[�W���󂯂��B�c��HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

        StartCoroutine(FlashRed());
    }


    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "���|�ꂽ�I");

        //�����J���閽�߂��m���ɍs��
        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }




        //Cerberus_Controller�Ɏ��S��ʒm (�R�����g�A�E�g���������ėL����)
        Cerberus_Controller cerberus = GetComponent<Cerberus_Controller>();
        if (cerberus != null)
        {
            cerberus.StartDeathSequence(); // ���R�����g�A�E�g���������ėL������
        }
        else
        {
            // ��ʓI�ȓG�̏ꍇ
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
