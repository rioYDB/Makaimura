using UnityEngine;

public class BossBodyDamege : MonoBehaviour
{
	[Header("ダメージ設定")]
	public int damageAmount = 1;
	public float damageInterval = 1.0f;
	private float nextDamageTime;

	private Collider2D myCollider;
	private ContactFilter2D filter;
	private Collider2D[] results = new Collider2D[5];

	void Start()
	{
		myCollider = GetComponent<Collider2D>();
		// プレイヤーレイヤーだけを検知する設定
		filter.useTriggers = true;
		filter.SetLayerMask(LayerMask.GetMask("Player"));
	}

	void Update()
	{
		// 物理エンジンの衝突判定を待たず、スクリプトで直接「重なり」をチェックする
		if (myCollider == null) return;

		int count = myCollider.Overlap(filter, results);
		if (count > 0 && Time.time >= nextDamageTime)
		{
			for (int i = 0; i < count; i++)
			{
				if (results[i].CompareTag("Player"))
				{
					player_control player = results[i].GetComponent<player_control>();
					if (player != null)
					{
						player.playerHP(damageAmount);
						nextDamageTime = Time.time + damageInterval;
						Debug.Log(gameObject.name + "に強制接触ダメージ！");
						break;
					}
				}
			}
		}
	}


	void OnDrawGizmos()
	{
		Collider2D col = GetComponent<Collider2D>();
		if (col != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
		}
	}
}
