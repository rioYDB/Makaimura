using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//�ϐ��錾
	public float moveSpeed;                                     //�ړ����x
	public float jumpPower;                                     //�W�����v��
	public LayerMask Ground;                                    //�n�ʂ𔻕ʂ���I�u�W�F�N�g���C���[
	public GameObject bulletPrefab;                             //���̃v���n�u

	public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //�����Ă鎞�̃T�C�Y
	public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);         //���Ⴊ�񂾎��̃T�C�Y

	private bool IsSquat = false;                                           //���Ⴊ�ݔ���
	private bool IsJumping;                                     //�󒆂ɂ��邩����
	private float Moveinput;                                    //�ړ�����
	private Vector2 Movedirection = Vector2.zero;             // �ړ��������L�����Ă���

	private Rigidbody2D rb;                                     //Rigidbody2D�̊i�[
	private BoxCollider2D bc;                                   //BoxCollider2D�̊i�[��
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
		if (IsGrounded() == true && IsSquat == false && IsJumping == false)
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
			Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		}


	}

	// �g���K�[�������������̏���
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �ڐG�����I�u�W�F�N�g��tag����Enemy�̏ꍇ��
		if (collision.gameObject.tag == "Enemy")
		{
			// Player�I�u�W�F�N�g����������
			Destroy(gameObject);
		}
	}



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

		/*
		
		//�����L�[�������ꂽ��
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			//�v���C���[�����Ⴊ�܂���
			Squat(true);
		}
		

		//�����L�[�𗣂�����
		if(Input.GetKeyUp(KeyCode.DownArrow))
		{
			Debug.Log("�ʂ���");
			//�v���C���[�̂��Ⴊ�݂����߂�����
			Squat(false);
			
		}
		*/
	}

	/*
	private void Squat(bool squat)
	{
		IsSquat=squat;

		//���Ⴊ�ݎ��̃T�C�Y�ύX
		transform.localScale = squat ? SquatSize : StandSize;

		//�R���C�_�[�̃T�C�Y���ύX����B
		if (squat)
		{
			bc.size = new Vector2(bc.size.x, SquatSize.y); //���Ⴊ�񂾎��̃T�C�Y�ɕύX
			Debug.Log("���Ⴊ�ݒ�");
		}
		
		else
		{
			bc.size = new Vector2(bc.size.x,StandSize.y); //���̃T�C�Y�ɖ߂�
		}
	}
	*/

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

