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

        //R�L�[��L�̎�O�g���K�[�Ń^�C�g����
        if (Input.GetKeyDown(KeyCode.R) ||Input.GetButtonDown("Respawn"))
        {
            SceneManager.LoadScene("Title");
            Debug.Log("title");
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("LetsPlay"))
        {
            SceneManager.LoadScene("Ayabe3");

            Debug.Log("main");
        }

    }
}
