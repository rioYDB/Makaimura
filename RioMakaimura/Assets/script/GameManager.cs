using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

  


    [Header("BossSpawn")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public float bossSpawnDelay = 1.0f;
    public bool isBossDefeated = false;
    private bool bossEventStarted = false;

    [Header("Door")]
    public doorcontroller OpenDoor;
    public Downdoor CloseDoor;

    [Header("Goal")]
    public GameObject goal;

    // カメラ解除用コルーチン
    [Header("Camera After Boss")]
    public float cameraUnlockDelay = 1.5f; // ← 溜め時間

    void Awake()
    {

        Debug.Log("GameManager Awake 呼ばれた");

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
	


	// ボスイベント開始用メソッド
	public void StartBossEvent()
    {
        if (bossEventStarted) return;
        bossEventStarted = true;

        StartCoroutine(BossEventSequence());
    }

	// カメラ解除用コルーチン
	private IEnumerator UnlockCameraAfterDelay()
	{
		yield return new WaitForSeconds(cameraUnlockDelay);

		CameraMove cam = Camera.main?.GetComponent<CameraMove>();
		if (cam != null)
		{
			cam.UnlockCamera();
		}

		Debug.Log("カメラ固定解除");
	}


	// ボス戦開始時
	private IEnumerator BossEventSequence()
	{
		// ① 扉を閉める
		if (CloseDoor != null)
		{
			CloseDoor.StartFalling();
		}

		// ② カメラ固定（★ここ！）
		CameraMove cam = Camera.main?.GetComponent<CameraMove>();
		if (cam != null)
		{
			cam.LockCamera();
		}
		else
		{
			Debug.LogWarning("CameraMove が見つからない（BossEventSequence）");
		}

		Debug.Log("① BossEventSequence 開始");

		// ③ 演出待ち
		yield return new WaitForSeconds(bossSpawnDelay);

		// ④ ボス出現
		if (bossPrefab != null && bossSpawnPoint != null)
		{
			Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
		}

		Debug.Log("ボスイベント開始！");
	}


	// ボス撃破後の流れ
	public void OnBossDefeated()
    {
        if (isBossDefeated) return; // 二重防止

        isBossDefeated = true;

        // ドアを開く
        if (OpenDoor != null)
        {
            OpenDoor.OpenDoor();
        }

        // ゴール出現
        if (goal != null)
        {
            goal.SetActive(true);
        }

        // 少し溜めてからカメラ解除
        StartCoroutine(UnlockCameraAfterDelay());

        Debug.Log("ボス撃破：ドア＆ゴール解放！");
    }
}
