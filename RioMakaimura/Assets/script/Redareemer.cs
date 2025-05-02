using UnityEngine;

public class Redareemer : MonoBehaviour
{
	public Transform player;             // プレイヤーのTransform
	public float jumpForce = 8f;         // ジャンプ力
	public float flySpeed = 3f;          // ホバリング速度
	public float flyDuration = 2f;       // ホバリング時間
	public float jumpCooldown = 3f;      // 次のジャンプまでの時間
	public float lastJumpTime;			 //今は飛んだ時増える数字になってる
	public float flyTimer;				 //飛んでいる時間にしようと思ってる

	public float radius = 3f;
	public float angularSpeed = 2f; // ラジアン/秒
	private bool isFlying = false;   //今飛んでいるかどうか

	private Vector3 centerOffset;	//オフセット

	private Rigidbody2D rb;
	private Animator anim;               // 必要ならアニメーションも
	private enum State { Idle, Jumping, Flying,Feint,Shoot }
	private State currentState = State.Idle;
	private float stateTimer = 0f;

	private bool isGrounded = true;



	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		centerOffset = transform.position - player.position;
		centerOffset = centerOffset.normalized * radius;
	}

	void Update()
	{
		switch (currentState)
		{
			case State.Idle:
				stateTimer += Time.deltaTime;
				if (stateTimer >= jumpCooldown && isGrounded)
				{
					if (Vector2.Distance(player.position, transform.position) < 9f)
					{
						if (Time.time > lastJumpTime + jumpCooldown)
						{
							Jump();
							lastJumpTime = Time.time;
						}
					}
				}
				break;

			case State.Jumping:
				if (rb.linearVelocity.y < 0) // 落下に転じたら飛行開始
				{
					currentState = State.Flying;
					stateTimer = 0f;
				}
				break;

			case State.Flying:
				stateTimer += Time.deltaTime;

				Vector2 target = player.position;
				Vector2 direction = (target - (Vector2)transform.position).normalized;
				rb.linearVelocity = direction * flySpeed;
				float hoverOffset = Mathf.Sin(Time.time * 5f) * 0.5f;
				rb.linearVelocity = new Vector2(direction.x * flySpeed, hoverOffset);

				if (stateTimer < flyDuration / 2f)
				{
					// プレイヤー方向へ
					direction = (player.position - transform.position).normalized;
				}
				else
				{
					// フェイント的に逆方向へ
					direction = -(player.position - transform.position).normalized;
				}

				if (stateTimer >= flyDuration)
				{
					rb.linearVelocity = Vector2.zero;
					currentState = State.Idle;
				}
				break;
		}

	}

	void Jump()
	{
		if (isGrounded)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
			currentState = State.Jumping;
			isGrounded = false;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{
			isGrounded = true;
		}
	}
}