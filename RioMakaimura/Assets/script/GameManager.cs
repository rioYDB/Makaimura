using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("ボス撃破フラグ")]
	public bool isBossDefeated = false;

	private void Awake()
	{
		// Singleton 保証
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject); // シーン跨ぎたいなら
	}
}
