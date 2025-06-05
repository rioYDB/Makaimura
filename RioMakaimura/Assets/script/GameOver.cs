using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        //RキーかLの手前トリガーでタイトルへ
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Respawn"))
        {
            SceneManager.LoadScene("Title");
            Debug.Log("title");
        }

    }
}
