using UnityEngine;

public class enemy_control : MonoBehaviour
{
	public float z_moveSpeed;       // �G�̈ړ����x
    public float riseDuration = 1f;      // �n�ʂ���オ�鎞��
    public float riseHeight = 0.5f;      // �㏸���鍂���i�n�ʂ���̋����j

    private Transform player;      // �v���C���[�̍��W�擾
	private Vector2 moveDirection; // �ړ��������L�^����

	Rigidbody2D rb;                 //Rigidbody2D�̊i�[
    private BoxCollider2D boxCollider;   // Collider�̎Q��

    private float riseElapsed = 0f;
    private bool isRising = true;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector3 originalPosition;

    void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

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

        // �R���C�_�[�̌��̃T�C�Y�ƃI�t�Z�b�g��ۑ�
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;

        // ������Ԃ̓R���C�_�[�����߁��������ɂ��炷�i�n�ʂɖ��܂��Ă��ԁj
        boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
        boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - originalColliderSize.y / 4f);

        // �|�W�V����������������
        originalPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, originalPosition.y - riseHeight, originalPosition.z);


        // 3�b��Ɏ�����j�󂷂�
        Destroy(gameObject, 3f);
	}

	// Update is called once per frame
	void Update()
	{
        if (isRising)
        {
            // �㏸����
            riseElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(riseElapsed / riseDuration);

            // �R���C�_�[�T�C�Y�ƃI�t�Z�b�g���Ԃ��Ė߂�
            boxCollider.size = new Vector2(originalColliderSize.x, Mathf.Lerp(originalColliderSize.y / 2f, originalColliderSize.y, t));
            boxCollider.offset = new Vector2(originalColliderOffset.x, Mathf.Lerp(originalColliderOffset.y - originalColliderSize.y / 4f, originalColliderOffset.y, t));

            // �|�W�V�������㏸������
            float newY = Mathf.Lerp(originalPosition.y - riseHeight, originalPosition.y, t);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (t >= 1f)
            {
                isRising = false;
                // �㏸���������̂ňړ��J�n
            }
        }
        else
        {
            if (player == null) return;

            // �㏸������̓v���C���[�Ɍ������Ĉړ�����
            transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;
        }

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

		// ������player�ɑ����Ă���ꍇ
		if (collision.gameObject.CompareTag("Player"))
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
