using UnityEngine;

public class Zombi_default : MonoBehaviour
{
    public float z_moveSpeed;       // �G�̈ړ����x
   

    private Transform player;      // �v���C���[�̍��W�擾
    private Vector2 moveDirection; // �ړ��������L�^����

    Rigidbody2D rb;                 //Rigidbody2D�̊i�[
    private CapsuleCollider2D capsuleCollider;   // Collider��Capsule�ɕύX

    public LayerMask Ground; // �� �C���X�y�N�^�[����n�ʃ��C���[���w��
    //private bool isGrounded = false;


 
   

    private float originalHeight;
    private float originalWidth;
    private Vector2 originalOffset;
    private Vector3 originalPosition;

    

    public delegate void OnDesloyDelegate(GameObject g);
    public OnDesloyDelegate mOnDestly = new OnDesloyDelegate((GameObject g) => { });
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�A�^�b�`����Ă���Component���擾
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();


        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // �ŏ��Ɍ������������v�Z���ČŒ�
        if (player != null)
        {
            moveDirection = (player.position - transform.position).normalized;

            moveDirection.y = 0f;            // Y�������[���ɂ���
            moveDirection = moveDirection.normalized; // ���K������
        }

     
    }

    // Update is called once per frame
    void Update()
    {

        // �G�L�����N�^�[����ʓ��ɂ��邩�m�F
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            // ��ʊO�ɂ���ꍇ�͊ݕӘI���͓����Ȃ�
            return;
        }


        // �����]�������Ȃ��悤�ɂ��邽�߁A��]�����͖�����
        transform.rotation = Quaternion.identity; // ��ɉ�]�����Z�b�g�i�Œ�j

        transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

    }

    private void OnDestroy()
    {
        //�j�����O�̃f���Q�[�g���Ăяo��
        mOnDestly(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       

        //// ������player�ɑ����Ă���ꍇ
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // �Փ˂𖳎�����
        //    Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
        //}
    }

    void OnBecameInvisible()
    {
        // ��ʊO�ɏo���玩���I�ɓG��j�󂷂�

        //Destroy(gameObject);
    }




}
