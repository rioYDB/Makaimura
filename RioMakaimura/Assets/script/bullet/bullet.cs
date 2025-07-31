using UnityEngine;

public abstract class bullet : MonoBehaviour�@�@�@//���ۃN���X(�p���O��̃N���X)
{
    public float movespeed;     //�U���A�C�e���̈ړ����x
    protected float direction;    // ���˕���

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //�v���C���[����n���ꂽ�����Ɋ�Â��Ēe�𔭎�
        direction = transform.localScale.x;  // �v���C���[�̌����ɍ��킹��
    }

    // Update is called once per frame
    protected virtual void Update()
    {


        //�v���C���[�̌����ɍ��킹�Ēe�̂̈ړ�
        transform.Translate(movespeed * direction, 0.0f, 0.0f);


    }


    //�G�ɐG�ꂽ�炱�̃I�u�W�F�N�g��j��

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy�ɐG�ꂽ��j�󂷂�
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("�G�Ƃ̏Փ˂����o");
            BulletMoves(collision.gameObject); //�G������
            Destroy(gameObject);//�e������
        }
    }


    //��ʊO�ɏo���炱�̃I�u�W�F�N�g��j��
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


    protected abstract void BulletMoves(GameObject Enemy);   //���ۃ��\�b�h(�q�N���X�ɋ�̓I�ȏ�������������)

}
