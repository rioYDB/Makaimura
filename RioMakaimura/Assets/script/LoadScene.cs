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

    

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("LetsPlay"))
        {
            SceneManager.LoadScene("ayabe4");

            Debug.Log("main");
        }

    }
}
