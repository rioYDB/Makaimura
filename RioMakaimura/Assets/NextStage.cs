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
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�S�[���I�u�W�F�N�g�ɐG�ꂽ��S�[���V�[���ɐ؂�ւ��
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("ayabe5"); // �uGoal�v�V�[���֑J��
        }
    }
}
