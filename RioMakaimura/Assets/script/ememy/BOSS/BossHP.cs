using UnityEngine;
using System.Collections;   // Å© Åö ïKê{ Åö


public class BossHP : enemy_HP
{


	protected override IEnumerator DeathEffect()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnBossDefeated();
		}

		CameraMove cam = Camera.main.GetComponent<CameraMove>();
		if (cam != null)
		{
			cam.UnlockCamera();
		}

		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}
}
