using UnityEngine;

public class Bullet_Vampire : bullet
{

    public GameObject firepillarPrefab;     //�Β����Ăяo���v���n�u
    public LayerMask Ground;           //�O���E���h��������郌�C���[
    protected override void Start()
    {

    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("���@���p�C�A��ԂŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //�e�̏Փ˔�����Ă�
        base.OnTriggerEnter2D(collision);

        //�e(��)���n�ʂɐG�ꂽ��Β����Ăяo��
        //��������ray���΂��āA�w�肵�����C���[�̃I�u�W�F�N�g�ƐڐG���Ă��邩�ǂ������ʂ���
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, Ground);
        //�q�b�g���Ă��Ȃ��ꍇ��null���Ԃ����
        if (hit.collider != null)
        {
            Vector3 Firepillarpos = new Vector3(transform.position.x,collision.bounds.max.y,transform.position.z);
            Instantiate(firepillarPrefab, Firepillarpos, Quaternion.identity);
            Destroy(gameObject);
        }
        

    }


    protected override void Update()
    {
        base.Update();//�e�N���X��Update()���J�n
    }
}