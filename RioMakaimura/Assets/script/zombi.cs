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

	//void Enemy_Karasu(Collider2D collision)
	//{
	//	transform.position = new Vector3(-e_moveSpeed, Mathf.Sin(Time.time) * 2.0f + transform.position.y, transform.position.z);
	//}
}
