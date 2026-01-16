using UnityEngine;
using System.Collections;

//public class BossHP : enemy_HP
//{
//	protected override IEnumerator DeathEffect()
//	{
//		// 共通の死亡処理（無効化など）
//		yield return StartCoroutine(base.DeathEffect());

//		// ★ ボス撃破フラグ
//		GameManager.Instance.isBossDefeated = true;

//		// ★ カメラ解除
//		CameraMove cam = Camera.main.GetComponent<CameraMove>();
//		if (cam != null)
//		{
//			cam.UnlockCamera();
//		}

//		// ★ 余韻
//		yield return new WaitForSeconds(0.5f);

//		Destroy(gameObject);
//	}
//}
