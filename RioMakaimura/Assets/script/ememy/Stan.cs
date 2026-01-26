using UnityEngine;

public class Stan : MonoBehaviour
{
	public float lifeTime = 0.5f; // ヴァンパイアの火柱のように短時間で消す
	public float stunTime = 2.0f; // 後のスタン処理用

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			player_control player = other.GetComponent<player_control>();
			if (player != null)
			{
				// ダメージを与える
				player.playerHP(1);

				// ★スタンさせる（2秒間）
				player.Stun(stunTime);
			}
		}
	}
}