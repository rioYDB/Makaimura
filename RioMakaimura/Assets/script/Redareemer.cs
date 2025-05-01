using UnityEngine;

public class Redareemer : MonoBehaviour
{

	public Transform player;          // �v���C���[�̈ʒu
	public GameObject Player;
	public float moveSpeed = 2f;      // �ړ����x
	public float attackRange = 1.5f;  // �U���͈�
	public float attackDelay = 1f;    // �U���̊Ԋu
	private float nextAttackTime = 0f;

	public GameObject projectilePrefab;  // �e�̃v���n�u
	public float projectileSpeed = 5f;

	

	public enum AttackType
	{
		Melee,
		Ranged
	}

	public AttackType currentAttackType = AttackType.Melee;

	void AttackPlayer()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
		{
			nextAttackTime = Time.time + attackDelay;

			// �U���̎�ނ�؂�ւ�
			if (distanceToPlayer < 2f)
			{
				currentAttackType = AttackType.Melee;
			}
			else
			{
				currentAttackType = AttackType.Ranged;
			}

			switch (currentAttackType)
			{
				case AttackType.Melee:
					// �ߐڍU���A�j���[�V����
					Debug.Log("Red Arremer Melee Attack!");
					///////////////player.GetComponent<Player>().TakeDamage(20);  // �v���C���[�Ƀ_���[�W
					break;

				case AttackType.Ranged:
					// ��ѓ���U��
					ShootProjectile();
					break;
			}
		}
	}


	void ShootProjectile()
	{
		Vector3 direction = (player.position - transform.position).normalized;
		GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
		rb.linearVelocity = direction * projectileSpeed;
	}
	 void Update()
	{
		MoveTowardsPlayer();
		//////////////////////AttackPlayer();
	}
	// �v���C���[�Ɍ������Ĉړ�����
	void MoveTowardsPlayer()
	{
		Vector3 direction = (player.position - transform.position).normalized;
		transform.position += direction * moveSpeed * Time.deltaTime;
	}

	// �U���͈͓��Ƀv���C���[������ƍU�����s��
	//////////void AttackPlayer()
	//////////{
	//////////	float distanceToPlayer = Vector3.Distance(transform.position, player.position);

	//////////	if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
	//////////	{
	//////////		nextAttackTime = Time.time + attackDelay;
	//////////		// �U�����[�V�������Č�
	//////////		Debug.Log("Red Arremer Attacks!");
	//////////		// �����ɍU���̃A�j���[�V������_���[�W������ǉ�
	//////////	}
	//////////	// Start is called once before the first execution of Update after the MonoBehaviour is created
	//////////	void Start()
	//////////	{

	//////////	}

	//////////	// Update is called once per frame
	//////////}
}
