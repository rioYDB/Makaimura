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
		// 1. 相手がプレイヤーかチェック
		if (other.CompareTag("Player"))
		{
			player_control player = other.GetComponent<player_control>();
			if (player != null)
			{
				// 2. プレイヤーのダメージ処理を直接呼び出す
				// これにより、SE再生、無敵、フラッシュ、HP減少がすべて走ります
				player.playerHP(1);
				Debug.Log("衝撃波がプレイヤーに命中！");
			}
		}
	}
}