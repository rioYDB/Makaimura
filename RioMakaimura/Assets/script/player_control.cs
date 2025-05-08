using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Diagnostics;
public class player_control : MonoBehaviour
{
	//�ϐ��錾
	public float moveSpeed;															//�ړ����x
	public float jumpPower;															//�W�����v��
	public LayerMask Ground;														//�n�ʂ𔻕ʂ���I�u�W�F�N�g���C���[
	public GameObject bulletPrefab;												//���̃v���n�u
	public float KnockbackForce;													//�m�b�N�o�b�N
	public float invincibleTime;                                                        //���G����
    public int maxBulletsOnScreen = 3;                                          //��ʓ��ɏo��v���C���[�U���̍ő�̐�
    public float ClimbSpeed = 0.0f;												//�͂����̈ړ��X�s�[�h


    public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //�����Ă鎞�̃T�C�Y
	public Vector3 CrouchSize = new Vector3(1.7f, 1.9f, 1f);             //���Ⴊ�񂾎��̃T�C�Y

	private int HP = 2;                                                                 //HP
	private int AttackCount;                                                            //�U�����J�E���g����ϐ�
	private bool IsSquat = false;													//���Ⴊ�ݔ���
	private bool IsJumping;															//�󒆂ɂ��邩����
	private bool IsAttacking = true;												//�U���ł��邩����
	private bool IsInvincible = false;                                              //���G��Ԃ�����
    private bool IsClimbing = false;												//�͂�����o���Ă��邩
    private bool IsOnLadder = false;												//�͂����ɐG��Ă��邩
    private bool IsCrouching = false;												//���Ⴊ�ݒ���
    private float Moveinput;                                                          //�ړ�����
	private float InvincibleTimer;													//���G���ԃ^�C�}�[
	private Vector2 Movedirection = Vector2.zero;							// �ړ��������L�����Ă���




	private Rigidbody2D rb;															//Rigidbody2D�̊i�[
	private BoxCollider2D bc;														//BoxCollider2D�̊i�[��
	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
		
	}

	// Update is called once per frame
	void Update()
	{
		////�㉺�̃L�[�̔���
        //float vertical = Input.GetAxisRaw("Vertical");

        //�ړ�����
        if (/*IsGrounded() == true && */ IsSquat == false /*&& IsJumping == false*/)
        {
            Move();
        }

        ////�W�����v����
        //if (IsGrounded() == true && IsSquat == false)
        //{
        //	Jump();
        //}

        //      // �� �͂�������
        //      if (IsOnLadder)
        //      {
        //          if (Mathf.Abs(vertical) > 0f)
        //          {
        //              IsClimbing = true;
        //          }
        //      }
        //      else
        //      {
        //          IsClimbing = false;
        //      }

        //      if (IsClimbing)
        //      {
        //          rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * ClimbSpeed);
        //          rb.gravityScale = 0f;
        //      }
        //      else
        //      {
        //          rb.gravityScale = 1f;
        //      }

        //      // �� ���Ⴊ�ݏ����i�͂����ɐG��Ă��Ȃ��Ƃ��̂݁j
        //      if (!IsOnLadder && vertical < 0)
        //      {
        //          if (!IsCrouching)
        //          {
        //              IsCrouching = true;
        //              bc.size = CrouchSize; // ����������
        //          }
        //      }
        //      else
        //      {
        //          if (IsCrouching)
        //          {
        //              IsCrouching = false;
        //              bc.size = StandSize; // ���ɖ߂�
        //          }
        //      }



        //Z�L�[�������ꂽ��U��
        if (Input.GetKeyDown(KeyCode.Z))
		{
           
			Attack();

        }

        //���G�^�C�}�[
        if (IsInvincible==true)
		{
			InvincibleTimer -= Time.deltaTime;
			if (InvincibleTimer <= 0)
			{
				IsInvincible = false;
			}
		}

		//���S����
		if (HP==0)
		{
			//�v���C���[��j��
			Destroy(gameObject);

			//Debug.Log("����wwwwwwwwww");

			//Scene�����Z�b�g����
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    //�͂����ɐG��Ă��邩
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Ladder"))
    //    {
    //        IsOnLadder = true;
    //    }
    //}

    ////�͂������痣�ꂽ��
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Ladder"))
    //    {
    //        IsOnLadder = false;
    //        IsClimbing = false;
    //    }
    //}


    //�֐����FAttack()
    //�p�r�F�U������
    //�����F�Ȃ�
    //�߂�l�F�Ȃ�
    private void Attack()
    {


        //���I�u�W�F�N�g�����ׂĎ擾���邽�߂ɔz����쐬
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Spear");

        if (bullets.Length >= maxBulletsOnScreen)
        {
            // ��ʂ̍ő吔�ɒB���Ă���̂Ŕ��˂��Ȃ�

            return;
        }

        // �U������
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // �v���C���[�̌����ɍ��킹�Ĕ��]
        bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);


    }


    //��ʊO�ɏo���炱�̃I�u�W�F�N�g��j��
    private void OnBecameInvisible()
    {
        Destroy(gameObject);

        //Scene�����Z�b�g����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    //�_���[�W����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Enemy��EnemyBullet�ɓ���������v���C���[��j�󂷂�
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"))
        {

            HP -= 1;
            //Debug.Log("�ɂ�");



            // �m�b�N�o�b�N����
            Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
            GetComponent<Rigidbody2D>().AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);

            // ���G��Ԃ��J�n
            StartInvincibility();
        }
    }


    //�֐����FStartInvincibility()
    //�p�r�F���G���ԏ���
    //�����F�Ȃ�
    //�߂�l�F�Ȃ�
    void StartInvincibility()
	{
		IsInvincible = true;
		InvincibleTimer = invincibleTime;

		// ���G���A�G�Ƃ̏Փ˂𖳎�����
		StartCoroutine(IgnoreEnemyCollisionDuringInvincibility());

		// �v���C���[��_�ł����邽�߂̃R���[�`�����J�n
		StartCoroutine(InvincibilityFlash());
	}



	//�֐����FInvincibilityFlash()
	//�p�r�F�_�ŏ���
	//�����F�Ȃ�
	//�߂�l�F�Ȃ�
	// �v���C���[��_�ł�����R���[�`��
	IEnumerator InvincibilityFlash()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		while (IsInvincible)
		{
			sr.enabled = !sr.enabled; // �_�Łi�X�v���C�g�̕\���E��\���؂�ւ��j
			yield return new WaitForSeconds(0.1f); // �_�ł̊Ԋu
		}
		sr.enabled = true; // �Ō�ɃX�v���C�g��\��
	}


	// ���G���Ԓ��A�v���C���[�ƓG�̏Փ˂𖳎�����
	IEnumerator IgnoreEnemyCollisionDuringInvincibility()
	{
		// ���G���Ԃ��I������܂ŌJ��Ԃ�
		while (IsInvincible)
		{
			// "Enemy" �^�O�̂����I�u�W�F�N�g�����ׂĎ擾
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			// �G�Ƃ̏Փ˂𖳎�
			foreach (GameObject enemy in enemies)
			{
				Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), true);
			}

			// ���G���Ԓ��͏Փ˂𖳎���������
			yield return null;
		}

		// ���G���Ԃ��I��������A�Փ˂��ĂїL���ɂ���
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in allEnemies)
		{
			Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), false);
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


