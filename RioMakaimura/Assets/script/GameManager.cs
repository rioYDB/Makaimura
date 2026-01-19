using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Boss")]
	public bool isBossDefeated = false;

	[Header("Goal")]
	public GameObject goal;   // ƒS[ƒ‹‚ğ“ü‚ê‚é

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void OnBossDefeated()
	{
		isBossDefeated = true;

		if (goal != null)
		{
			goal.SetActive(true);
		}
	}
}
