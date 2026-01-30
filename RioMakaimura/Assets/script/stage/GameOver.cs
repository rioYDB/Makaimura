using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        // ゲームオーバーになったステージ名を取得
        string failedStageName = StageManager.GetCurrentStage();
        Debug.Log("Game Over at stage: " + failedStageName);
    }
    
    // Update is called once per frame
    void Update()
    {



        //RキーかLの手前トリガーでタイトルへ
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Respawn"))
        {
            GoTitle();
        }

        // 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Retry();
        }

    }

    public void Retry()
    {
        // 保存しておいたシーン名を取得
        string retryScene = PlayerPrefs.GetString("RetryScene", "Title");

        SceneManager.LoadScene(retryScene);
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }

}
