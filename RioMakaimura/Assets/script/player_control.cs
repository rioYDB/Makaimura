using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class player_control : MonoBehaviour
{
	//�ϐ��錾
	public float moveSpeed;         //�ړ����x
	public float jumpPower;         //�W�����v��
	public LayerMask Ground;        //�n�ʂ𔻕ʂ���I�u�W�F�N�g���C���[
	public GameObject bulletPrefab;	//���̃v���n�u



	Rigidbody2D rb;                 //Rigidbody2D�̊i�[

	void Start()
    {
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update()
	{
		//���L�[�������ꂽ��
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			//�v���C���[�����Ɉړ�������
			transform.Translate(-moveSpeed, 0.0f, 0.0f);
		}
		//�E�L�[�������ꂽ��
		if (Input.GetKey(KeyCode.RightArrow))
		{
			//�v���C���[���E�Ɉړ�������
			transform.Translate(moveSpeed, 0.0f, 0.0f);
		}

		//�W�����v����
		if (IsGrounded() == true)
		{
			Jump();
		}

		//�U������
		if (Input.GetKeyDown(KeyCode.Z))
		{
			GameObject bullet =Instantiate(bulletPrefab);
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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector2.up * jumpPower);
			//++jumpcount;
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
		return ret;
	}
}

