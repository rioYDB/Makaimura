using UnityEngine;
using UnityEngine.SceneManagement;


public class NextStage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("LetsPlay"))
        {
            Debug.Log("a");

            SceneManager.LoadScene("ayabe5");

            Debug.Log("main");
        }


        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("Respawn"))
        {
            SceneManager.LoadScene("title");

        }

    }


}
