using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//�ϐ��錾
	public float moveSpeed;												//�ړ����x
	public float jumpPower;												//�W�����v��
	public LayerMask Ground;											//�n�ʂ𔻕ʂ���I�u�W�F�N�g���C���[
	public GameObject bulletPrefab;										//���̃v���n�u


	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //�����Ă鎞�̃T�C�Y
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);				//���Ⴊ�񂾎��̃T�C�Y

	private bool IsSquat = false;                                       //���Ⴊ�ݔ���
	private bool IsJumping;												//�󒆂ɂ��邩����
	private float Moveinput;											//�ړ�����
	private Vector2 Movedirection = Vector2.zero;						// �ړ��������L�����Ă���

	private Rigidbody2D rb;												//Rigidbody2D�̊i�[
	private BoxCollider2D bc;											//BoxCollider2D�̊i�[��
	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{

		//�ړ�����
		if (/*IsGrounded() == true && */IsSquat == false /*&& IsJumping == false*/)
		{

			Move();
		}

		//�W�����v����
		if (IsGrounded() == true && IsSquat == false)
		{
			Jump();
		}

		//Z�L�[�������ꂽ��
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//�U������
			GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

			bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1); // �v���C���[�̌����ɍ��킹�Ĕ��]
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Enemy��EnemyBullet�ɓ���������v���C���[��j�󂷂�
		if ((collision.gameObject.tag=="Enemy"|| collision.gameObject.tag == "EnemyBullet"))
		{
			Destroy(gameObject);

			//Scene�����Z�b�g����
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// �g���K�[�������������̏���
	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	// �ڐG�����I�u�W�F�N�g��tag����Enemy�̏ꍇ��
	//	if (collision.gameObject.tag == "Enemy")
	//	{

	//		//// �ŐV�X�^�C���̌Ăяo��
	//		//GameManager.Instance.RespawnPlayer();

	//		// Player�I�u�W�F�N�g����������
	//		Destroy(gameObject);

	//		// ���݂̃V�[���������[�h�i�ŏ������蒼���j
	//		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	//	}


	//}



	//�֐����FJump()
	//�p�r�F�W�����v����
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	void Jump()
	{
		//�W�����v����
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector2.up * jumpPower);

			//�W�����v��Ԃɂ���
			IsJumping = true;

		}


		//�W�����v�O�̈ړ����L��������
		if (IsJumping == true)
		{

			rb.linearVelocity = new Vector2(Movedirection.x * moveSpeed, rb.linearVelocity.y);
		}

	}


	//�֐����FMove()
	//�p�r�F�ړ�����
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	void Move()
	{

		Moveinput = Input.GetAxisRaw("Horizontal");


		//���ړ��������L��������



		//�v���C���[���ړ�������
		transform.Translate(Moveinput * moveSpeed, 0.0f, 0.0f);
		if (Moveinput != 0)
		{
			Movedirection = new Vector2(Moveinput, 0f);
		}


		//�v���C���[�̌�����ύX
		if (Moveinput != 0)
		{
			Movedirection = new Vector2(Moveinput, 0f);

			// �����𔽓]�����鏈��
			Vector3 scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * Mathf.Sign(Moveinput); // ���Ȃ�}�C�i�X�A�E�Ȃ�v���X
			transform.localScale = scale;
		}


	}



	//�֐����FIsGrounded()
	//�p�r�F�ڒn���菈��
	//�����F�Ȃ�
	//�߂�l�F�ڒn���Ă���ꍇ��true�A���Ă��Ȃ��ꍇ��false
	bool IsGrounded()
	{
		bool ret = false;
		//��������ray���΂��āA�w�肵�����C���[�̃I�u�W�F�N�g�ƐڐG���Ă��邩�ǂ������ʂ���
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, Ground);
		//�q�b�g���Ă��Ȃ��ꍇ��null���Ԃ����
		if (hit.collider != null)
		{
			ret = true;
		}
		if (ret == true)
		{
			IsJumping = false;
		}

		return ret;
	}
}

