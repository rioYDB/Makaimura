using UnityEngine;

public class Clown_Projectile : MonoBehaviour
{
    public float lifetime = 5f; // �e�������ŏ�����܂ł̎���


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ��莞�Ԍ�Ɏ����ŏ�����i�������ߖ�j
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ������player�ɑ����Ă���ꍇ
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // �Փ˂𖳎�����
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CircleCollider2D>());
        }

        //// �v���C���[�ɓ��������Ƃ��̏����i���j
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // ��F�_���[�W�����Ȃ�
        //    // collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
        //}

        //// �����ɓ���������e������
        //Destroy(gameObject);
    }
}
