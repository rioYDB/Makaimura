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



        //R�L�[��L�̎�O�g���K�[�Ń^�C�g����
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Respawn"))
        {
            SceneManager.LoadScene("Title");
            Debug.Log("title");
        }

    }
}
