using System;
using UnityEngine;

public class enemy_control : MonoBehaviour
{
    public float z_moveSpeed;       // �G�̈ړ����x
    public float riseDuration;      // �n�ʂ���オ�鎞��
    public float riseHeight;      // �㏸���鍂���i�n�ʂ���̋����j

    private Transform player;      // �v���C���[�̍��W�擾
    private Vector2 moveDirection; // �ړ��������L�^����

    Rigidbody2D rb;                 //Rigidbody2D�̊i�[
    private CapsuleCollider2D capsuleCollider;   // Collider��Capsule�ɕύX

    public LayerMask Ground; // �� �C���X�y�N�^�[����n�ʃ��C���[���w��
    private bool isGrounded = false;


    private float riseElapsed = 0f;
    private bool isRising = true;
    private bool isSinking = false;
    private float sinkElapsed = 0f;
    public float sinkDuration = 1f;  // ���ގ���
    private bool sinkRequested = false; // �n�ʂɂ����璾�ނ悤�\��

    private float originalHeight;
    private float originalWidth;
    private Vector2 originalOffset;
    private Vector3 originalPosition;

    private Vector3 sinkStartPosition; // ���݈ʒu�𒾉��J�n�n�_�Ƃ��ċL�^

    public delegate void OnDesloyDelegate(GameObject g);
    public OnDesloyDelegate mOnDestly = new  OnDesloyDelegate((GameObject g) =>{ });

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

        // ���̍����E���E�I�t�Z�b�g��ۑ�
        originalHeight = capsuleCollider.size.y; // size������ꍇ
        originalWidth = capsuleCollider.size.x;  // size������ꍇ

        // ���� size �v���p�e�B���Ȃ����Ȃ� capsuleCollider.height, capsuleCollider.width���g��
        // capsuleCollider.height = originalHeight; capsuleCollider.width = originalWidth; 



        // ������Ԃ̓R���C�_�[�����������������ɂ��炷�i�n�ʂɖ��܂�����ԁj
        capsuleCollider.size = new Vector2(originalWidth, originalHeight / 2f);
        capsuleCollider.offset = new Vector2(originalOffset.x, originalOffset.y - originalHeight / 4f);

        // �|�W�V����������������
        originalPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, originalPosition.y - riseHeight, originalPosition.z);

        //�ŏ��͏㏸���Ȃ̂ŃL�l�}�e�B�b�N�Ő���
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 5�b��Ɏ�����j�󂷂�
        Invoke("StartSinking", 5f);
    }

	// Update is called once per frame
	void Update()
	{
        Vector2 colliderSize = capsuleCollider.size * transform.lossyScale;

        // �ڒn�`�F�b�N�i�]���r�̑�������^���� Ray ���΂��j
        //Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - capsuleCollider.size.y / 2f + capsuleCollider.offset.y);
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - colliderSize.y / 2f);
        isGrounded = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, Ground);

        if (isRising)
        {
            // �㏸����
            riseElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(riseElapsed / riseDuration);

            // �R���C�_�[�T�C�Y�ƃI�t�Z�b�g���Ԃ��Ė߂�
            capsuleCollider.size = new Vector2(originalWidth, Mathf.Lerp(originalHeight / 2f, originalHeight, t));
            capsuleCollider.offset = new Vector2(originalOffset.x, Mathf.Lerp(originalOffset.y - originalHeight / 4f, originalOffset.y, t));

            // �|�W�V�������㏸������
            float newY = Mathf.Lerp(originalPosition.y - riseHeight, originalPosition.y, t);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (t >= 1f)
            {
                //�㏸���������̂�Dynamic�ɕύX
                rb.bodyType = RigidbodyType2D.Dynamic;

                isRising = false;
                // �㏸���������̂ňړ��J�n
            }
        }
        else if (isSinking)
        {
            // ��������
            sinkElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(sinkElapsed / sinkDuration);

            // �R���C�_�[�k���ƈʒu���~
            capsuleCollider.size = new Vector2(originalWidth, Mathf.Lerp(originalHeight, originalHeight / 2f, t));
            capsuleCollider.offset = new Vector2(originalOffset.x, Mathf.Lerp(originalOffset.y, originalOffset.y - originalHeight / 4f, t));


            float newY = Mathf.Lerp(sinkStartPosition.y, sinkStartPosition.y - riseHeight, t);
            transform.position = new Vector3(sinkStartPosition.x, newY, sinkStartPosition.z);


            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (player == null) return;

            // �㏸������̓v���C���[�Ɍ������Ĉړ�����
            transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

            // �n�ʂɐڒn���Ă��Ē����\�񂪂���Β���
            if (sinkRequested && isGrounded && !isSinking)
            {
                StartSinking();
                sinkRequested = false; // ��x�J�n������t���O��߂�
            }
        }

        // �����]�������Ȃ��悤�ɂ��邽�߁A��]�����͖�����
        transform.rotation = Quaternion.identity; // ��ɉ�]�����Z�b�g�i�Œ�j
	}

    private void OnDestroy()
    {
        //�j�����O�̃f���Q�[�g���Ăяo��
        mOnDestly(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
        // �������G���C���[�ɑ����Ă���ꍇ
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //	// �Փ˂𖳎�����
        //	Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
        //}

        // ������player�ɑ����Ă���ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �Փ˂𖳎�����
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
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
        if (!isSinking && !isRising)
        {
            StartSinking();
        }
    }

    void StartSinking()
    {
        Debug.Log("StartSinking called - grounded? " + isGrounded);
        if (isSinking) return;

        if (isGrounded)
        {
            isSinking = true;
            sinkElapsed = 0f;
            sinkStartPosition = transform.position; // ���݈ʒu����ɒ���
            Debug.Log("Sink started!");
        }
        else
        {
            sinkRequested = true; // ���͋󒆂Ȃ̂ŁA�n�ʂɂ����璾�܂���
            Debug.Log("Sink requested but not grounded yet.");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (capsuleCollider == null) return;

        // �ڒn�`�F�b�N�i�]���r�̑�������^���� Ray ���΂��j
        Vector2 colliderSize = capsuleCollider.size * transform.lossyScale;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - colliderSize.y / 2f);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * 0.1f);
    }


}
