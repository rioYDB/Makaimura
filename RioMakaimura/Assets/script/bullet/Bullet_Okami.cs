using UnityEngine;

public class Bullet_Okami : bullet
{
    public float lifetime = 0.2f; // �܍U���̎������ԁi�Z���j

    protected override void Start()
    {
        base.Start(); //�e�N���X��Start()���J�n

        //�q�N���X��Start()���e����������

        

    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("�T�j�ŃA�^�b�N�I�I�I");
        //Destroy(Enemy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "tendril" || collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        //if(collision.gameObject.tag == "Enemy")
        //{
        //    // �G�iEnemy�X�N���v�g�����Ă���́j�ɓ�������������
        //    enemy_HP enemy = collision.GetComponent<enemy_HP>();

        //    if (enemy != null)
        //    {
        //        enemy.TakeDamage(1); // 1�_���[�W�^����i�K�v�Ȃ�ϐ�������OK�j
        //    }
        //}
    }


    protected override void Update()
    {
        //base.Update();//�e�N���X��Update()���J�n


        Destroy(gameObject, lifetime); // �����ō폜

    }
}
