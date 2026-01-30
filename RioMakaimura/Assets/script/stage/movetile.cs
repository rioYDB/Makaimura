using UnityEngine;

public class movetile : MonoBehaviour
{
	[SerializeField] private Vector3 leftEndPoint;
	[SerializeField] private Vector3 rightEndPoint;
	[SerializeField] private float moveSpeed = 2f;

	private float journeyLength;
	private float startTime;
	private Rigidbody2D rb;
	private Vector2 lastPosition;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Kinematic; // 物理で動かされないように固定
		transform.position = leftEndPoint;

		journeyLength = Vector3.Distance(leftEndPoint, rightEndPoint);
		startTime = Time.time;
		lastPosition = rb.position;
	}

	void FixedUpdate()
	{
		// 往復移動の計算
		float t = Mathf.PingPong((Time.time - startTime) * moveSpeed / journeyLength, 1f);
		Vector2 targetPos = Vector2.Lerp(leftEndPoint, rightEndPoint, t);

		// ★このフレームの床の速度を計算する
		Vector2 currentVelocity = (targetPos - lastPosition) / Time.fixedDeltaTime;

		// 床を動かす
		rb.MovePosition(targetPos);
		lastPosition = targetPos;

		// 判定：上に乗っているプレイヤーを探す
		// 親子関係にはせず、速度だけを伝える
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0, Vector2.up, 0.5f, LayerMask.GetMask("Player"));

		if (hit.collider != null)
		{
			player_control pc = hit.collider.GetComponent<player_control>();
			if (pc != null)
			{
				pc.SetPlatformVelocity(currentVelocity);
			}
		}
	}

	// プレイヤーが離れたら速度をリセットさせる
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			player_control pc = collision.gameObject.GetComponent<player_control>();
			if (pc != null) pc.SetPlatformVelocity(Vector2.zero);
		}
	}
}