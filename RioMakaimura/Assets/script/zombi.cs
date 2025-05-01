using UnityEngine;

public class enemy_control : MonoBehaviour
{
	public float z_moveSpeed;       // �G�̈ړ����x
	private Transform player;      // �v���C���[�̍��W�擾
	private Vector2 moveDirection; // �ړ��������L�^����

	Rigidbody2D rb;                 //Rigidbody2D�̊i�[

	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();

		GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			player = playerObj.transform;
		}

		// �ŏ��Ɍ������������v�Z���ČŒ�
		if (player != null)
		{
			moveDirection = (player.position - transform.position).normalized;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (player == null) return;

		// �ړ�������ێ���������
		transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

		// �����]�������Ȃ��悤�ɂ��邽�߁A��]�����͖�����
		transform.rotation = Quaternion.identity; // ��ɉ�]�����Z�b�g�i�Œ�j
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// �������G���C���[�ɑ����Ă���ꍇ
		if (collision.gameObject.CompareTag("Enemy"))
		{
			// �Փ˂𖳎�����
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}

		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Haka")
		{
			Debug.Log("aya");

			// �������Ȃ��i�������Ȃ��j
			return;
		}
	}

	void OnBecameInvisible()
	{
		// ��ʊO�ɏo���玩���I�ɓG��j�󂷂�
		Destroy(gameObject);
	}
}
