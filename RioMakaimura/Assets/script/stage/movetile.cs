using UnityEngine;

public class movetile : MonoBehaviour
{
	[SerializeField] private Vector3 leftEndPoint;
	[SerializeField] private Vector3 rightEndPoint;
	[SerializeField] private float moveSpeed = 2f;

	private float journeyLength;
	private float startTime;

	private Rigidbody2D rb;
	private Vector3 prevPosition;

	private Rigidbody2D playerRb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		transform.position = leftEndPoint;

		journeyLength = Vector3.Distance(leftEndPoint, rightEndPoint);
		startTime = Time.time;

		prevPosition = rb.position;
	}

	void FixedUpdate()
	{
		float t = Mathf.PingPong(
			(Time.time - startTime) * moveSpeed / journeyLength, 1f);

		Vector3 targetPos = Vector3.Lerp(leftEndPoint, rightEndPoint, t);
		rb.MovePosition(targetPos);

		// Åö è∞ÇÃà⁄ìÆó 
		Vector3 delta = targetPos - prevPosition;

		// Åö ÉvÉåÉCÉÑÅ[ÇàÍèèÇ…ìÆÇ©Ç∑
		if (playerRb != null)
		{
			playerRb.transform.position += delta;
		}

		prevPosition = targetPos;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerRb = null;
		}
	}
}
