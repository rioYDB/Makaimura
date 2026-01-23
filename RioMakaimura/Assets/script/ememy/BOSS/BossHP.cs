using UnityEngine;
using System.Collections;

public class BossHP : enemy_HP
{
    [Header("Boss Clear Action")]
    public doorcontroller openDoor;   // このボスが開くドア
    public GameObject goal;            // ゴール（必要なら）

    protected override IEnumerator DeathEffect()
    {
        // ★ このボス専用の処理
        if (openDoor != null)
        {
            openDoor.OpenDoor();
        }

        if (goal != null)
        {
            goal.SetActive(true);
        }

        // カメラ解除（共通）
        CameraMove cam = Camera.main.GetComponent<CameraMove>();
        if (cam != null)
        {
            cam.UnlockCamera();
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
