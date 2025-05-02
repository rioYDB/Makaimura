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
	public GameObject bulletPrefab;                                     //���̃v���n�u
	private float LastAttackTime;										//�Ō�ɍU����������
	public float AttackRate;                                            //�U�����o
	private int AttackCount;                                             //�U�����J�E���g����ϐ�
	public float CoolDown=2.0f;												//�U���̃N�[���_�E��



	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //�����Ă鎞�̃T�C�Y
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);				//���Ⴊ�񂾎��̃T�C�Y

	private bool IsSquat = false;                                       //���Ⴊ�ݔ���
	private bool IsJumping;                                             //�󒆂ɂ��邩����
	private bool IsAttacking = true;
	private float Moveinput;											//�ړ�����
	private Vector2 Movedirection = Vector2.zero;						// �ړ��������L�����Ă���

	private Rigidbody2D rb;												//Rigidbody2D�̊i�[
	private BoxCollider2D bc;											//BoxCollider2D�̊i�[��
	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
		LastAttackTime = -AttackRate;  // �ŏ��̔��˂������ł���悤�ɐݒ�

	}

	// Update is called once per frame
	void Update()
	{

		//�ړ�����
		if (/*IsGrounded() == true && */ IsSquat == false /*&& IsJumping == false*/)
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
			// ���ˉ\���A���ˉ񐔂�3��ȉ��̏ꍇ�ɔ���
			if (IsAttacking && AttackCount < 3)
			{
				Attack();
			}
			// ���ˉ񐔂�3��ɒB�����ꍇ�A������������
			else if (AttackCount >= 3)
			{
				StartCoroutine(AttackCoolDown());
			}
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

			//�W�����v���͈ړ����x�𐧌�����
			Moveinput *= 0.7f;
		}
	}


	//�֐����FMove()
	//�p�r�F�ړ�����
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	void Move()
	{

		//�n��ɂ��Ȃ��Ƃ��͓��͂��󂯕t���Ȃ�
		if (IsGrounded() == true)
		{
			Moveinput = Input.GetAxisRaw("Horizontal");

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


		//�v���C���[���ړ�������
		transform.Translate(Moveinput * moveSpeed, 0.0f, 0.0f);

		if (Moveinput != 0)
		{
			Movedirection = new Vector2(Moveinput, 0f);
		}

		
	}

	//�֐����FCoolDown()
	//�p�r�F�U���Ԋu��݂��鏈��
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	private IEnumerator AttackCoolDown()
	{
		// ���ː������̏���
		IsAttacking = false;
		Debug.Log("���ː�����...");

		// �������Ԃ�҂�
		yield return new WaitForSeconds(CoolDown);

		// ���ː�������
		IsAttacking = true;
		AttackCount = 0; // ���ˉ񐔂����Z�b�g
		Debug.Log("���ˍĊJ�I");
	}


	//�֐����FAttack()
	//�p�r�F�U������
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	private void Attack()
	{
		// �U������
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1); // �v���C���[�̌����ɍ��킹�Ĕ��]

		// ���ˉ񐔂��J�E���g
		AttackCount++;

		// �Ō�ɔ��˂����������X�V
		LastAttackTime = Time.time;

		// 3��ڂ̔��ˌ�ɔ��ː�����������
		if (AttackCount >= 3)
		{
			StartCoroutine(AttackCoolDown());
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


