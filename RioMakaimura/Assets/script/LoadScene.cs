using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //RキーかLの手前トリガーでリスポーン
        if (Input.GetKeyDown(KeyCode.R) ||Input.GetButtonDown("Respawn"))
        {
            SceneManager.LoadScene("Ayabe3");
        }
    }
}
