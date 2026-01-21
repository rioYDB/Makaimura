using UnityEngine;

public class Stan : MonoBehaviour
{
	[Header("スタン効果設定")]
	public float duration = 0.5f;
	public float stunTime = 2.0f;

	[Header("移動設定")]
	public float speed = 10f;               // 衝撃波の移動速度
	private Vector2 moveDirection;          // 移動方向

	// 方向をセットするためのメソッド
	public void SetDirection(Vector2 dir)
	{
		moveDirection = dir;

		// 進行方向を向くようにスプライトを反転させる（必要に応じて）
		if (dir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
	}

	void Start()
	{
		Destroy(gameObject, duration);
	}

	void Update()
	{
		// 毎フレーム指定された方向に移動させる
		transform.Translate(moveDirection * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			player_control playerController = other.GetComponent<player_control>();
			if (playerController != null)
			{
				
				playerController.playerHP(1);    // ダメージを与える
				Debug.Log("プレイヤーがスタンしました！");
			}
		}
	}
}
