using UnityEngine;
using UnityEngine.SceneManagement;


public class GoalFlag : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールオブジェクトに触れたらゴールシーンに切り替わる
        if (collision.gameObject.tag == "Player")
        {

            string clearedStageName = SceneManager.GetActiveScene().name;
            StageManager.MarkStageAsCleared(clearedStageName); // クリア情報を保存
            // --- ★ここまで追加・変更★ ---

            FadeManager.Instance.LoadScene("Goal _1",1.0f); // 「Goal」シーンへ遷移

        }
    }
}
