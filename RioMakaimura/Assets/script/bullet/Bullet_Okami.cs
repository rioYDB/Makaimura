using UnityEngine;

public class Bullet_Okami : bullet
{
    public float lifetime = 0.2f; // �܍U���̎������ԁi�Z���j

    protected override void Start()
    {
        base.Start(); //�e�N���X��Start()���J�n

        //�q�N���X��Start()���e����������

        

    }

    //protected override void BulletMoves(GameObject Enemy)
    //{
    //    Debug.Log("�T�j�ŃA�^�b�N�I�I�I");

     

    //    //Destroy(gameObject); // �e�̏���
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "tendril" )
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            // �G��Enemy_HP�X�N���v�g�����邩�m�F
            enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(1); // �_���[�W�ʂ�1�Ƃ���i�K�v�ɉ����ĕς���j
            }
        }


    }


    protected override void Update()
    {
        //base.Update();//�e�N���X��Update()���J�n


        Destroy(gameObject, lifetime); // �����ō폜

    }
}
