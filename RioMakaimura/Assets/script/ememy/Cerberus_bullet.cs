using UnityEngine;

public class Cerberus_bullet : MonoBehaviour
{
    public float moveSpeed = 5f; // ���̈ړ����x
    public float lifeTime = 3f; // ����������܂ł̎���
    public int damageAmount = 2; // �����^����_���[�W��

    private Vector2 moveDirection; // ���̐i�s����

    // �P���x���X����Ă΂�āA���̐i�s������ݒ肷��
    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized; // �����𐳋K�����ēn��
        // �������ɂ����Ɉړ����J�n����
    }

    void Start()
    {
        // ��莞�Ԍ�Ɏ��g�����ł�����
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // �i�s�����Ɉړ�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    // �v���C���[�Ƃ̏Փ˔���
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �v���C���[�̃^�O��"Player"�ł��邱�Ƃ��m�F
        {
            // �v���C���[�Ƀ_���[�W��^���鏈�����Ăяo��
            // ��: other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            Debug.Log("�v���C���[�ɉ������������I");

            // ���͈�x���������������悤�ɂ���
            Destroy(gameObject);
        }
        
    }
}
