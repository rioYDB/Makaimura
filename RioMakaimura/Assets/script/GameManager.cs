using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Boss")]
    public bool isBossDefeated = false;

    [Header("Door")]
    public doorcontroller bossDoor;

    [Header("Goal")]
    public GameObject goal;

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
        if (isBossDefeated) return; // 二重防止

        isBossDefeated = true;

        // ドアを開く
        if (bossDoor != null)
        {
            bossDoor.OpenDoor();
        }

        // ゴール出現
        if (goal != null)
        {
            goal.SetActive(true);
        }

        Debug.Log("ボス撃破：ドア＆ゴール解放！");
    }
}
