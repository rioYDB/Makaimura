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
            

            SceneManager.LoadScene("Goal _1"); // 「Goal」シーンへ遷移
        }
    }
}
