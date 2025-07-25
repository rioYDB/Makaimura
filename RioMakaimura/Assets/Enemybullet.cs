using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    public float moveSpeed = 10.0f; // �e����ԑ��x
    public float lifetime = 5.0f;   // ��ʊO�ɏo�邩�A��莞�Ԃŏ����鎞��

    private int directionX; // �e�����X���� (1:�E, -1:��)

    // ���ˎ���Launcher����������󂯎�邽�߂̃��\�b�h
    public void SetDirection(int dir)
    {
        directionX = dir;
        // �e�̌����ڂ�����ɍ��킹�Ĕ��]������
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * directionX, transform.localScale.y, transform.localScale.z);
    }

    void Start()
    {
        Destroy(gameObject, lifetime); // ��莞�Ԍ�Ɏ��g������
    }

    void Update()
    {
        // �e���ړ�������
        transform.Translate(Vector3.right * directionX * moveSpeed * Time.deltaTime, Space.World);
    }

    // �v���C���[�Ƀ_���[�W��^����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���ɐG��ă_���[�W�I");
            other.GetComponent<player_control>().playerHP(1); // player_control��TakeDamage���\�b�h�������
            Destroy(gameObject); // �v���C���[�ɓ��������瑄�͏�����
        }
    }

    // ��ʊO�ɏo���玩���I�ɏ����� (����lifetime��葁����ʊO�ɏo��ꍇ)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
