using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Camera")]
    public CameraMove cameraMove;


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

        if (cameraMove != null)
        {
            cameraMove.UnlockCamera();
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

        // ② カメラ固定
        if (cameraMove != null)
        {
            cameraMove.LockCamera();
        }
        Debug.Log("① BossEventSequence 開始");

        if (CloseDoor == null) Debug.LogWarning("CloseDoor が null");
        if (cameraMove == null) Debug.LogWarning("cameraMove が null");
        if (bossPrefab == null) Debug.LogWarning("bossPrefab が null");
        if (bossSpawnPoint == null) Debug.LogWarning("bossSpawnPoint が null");

        // ③ 演出待ち
        yield return new WaitForSeconds(bossSpawnDelay);

        Debug.Log("② ボス生成直前");

        // ④ ボス出現
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        }
        Debug.Log("③ ボス生成完了: " );

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
