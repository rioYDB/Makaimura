using UnityEngine;

public class Firepillar : bullet
{
    public float lifetime = 0.2f; // �܍U���̎������ԁi�Z���j

    //protected override void BulletMoves(GameObject Enemy)
    //{
    //    Debug.Log("���@���p�C�A�ŃA�^�b�N�I�I�I");
    //    Destroy(Enemy);
    //    Destroy(gameObject);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime); // �����ō폜
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        // �G��Enemy_HP�X�N���v�g�����邩�m�F
        enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
        if (enemyHP != null)
        {
            enemyHP.TakeDamage(1); // �_���[�W�ʂ�1�Ƃ���i�K�v�ɉ����ĕς���j
        }

    }

}
