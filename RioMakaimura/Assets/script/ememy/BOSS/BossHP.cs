using UnityEngine;
using System.Collections;

public class BossHP : enemy_HP
{
    public override void Die()
    {
        // ★ 先にボス撃破を通知
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossDefeated();
        }

        // 親クラスの死亡処理も実行
        base.Die();
    }
    //   [Header("Boss Clear Action")]
    //   public doorcontroller openDoor;   // このボスが開くドア
    //   public GameObject goal;            // ゴール（必要なら）

    //[Header("Camera")]
    //public GameObject cameraTrigger; // ボス部屋のカメラ固定トリガー


    //protected override IEnumerator DeathEffect()
    //{
    //	// ★ このボス専用の処理
    //	if (openDoor != null)
    //	{
    //		openDoor.OpenDoor();
    //	}

    //	if (goal != null)
    //	{
    //		goal.SetActive(true);
    //	}

    //	// ★ カメラ固定トリガーを無効化
    //	if (cameraTrigger != null)
    //	{
    //		cameraTrigger.SetActive(false);
    //	}

    //	// カメラ解除（共通）
    //	CameraMove cam = Camera.main.GetComponent<CameraMove>();
    //	if (cam != null)
    //	{
    //		cam.UnlockCamera();
    //	}

    //	yield return new WaitForSeconds(0.2f);
    //	Destroy(gameObject);
    //}

}
