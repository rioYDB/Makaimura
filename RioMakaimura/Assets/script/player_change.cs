using UnityEngine;
using UnityEngine.UI;

public class player_change : MonoBehaviour
{

    public Sprite Okami;        //�����������ɉ摜��ς��邽��
    public Sprite Which;        //�����������ɉ摜��ς��邽��

    private Image image;            //�摜�̊Ǘ�
    bool text1enableKey = true;

    // �摜�`��p�̃R���|�[�l���g
    SpriteRenderer sr;

    private int Human;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SpriteRenderer�R���|�[�l���g���擾���܂�
        image = GetComponent<Image>();
        // Sprite��SpriteRenderer�R���|�[�l���g���擾
        sr = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
      
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Okami")
        {

            sr.sprite = Okami;

            Debug.Log("ooooooooooooo");

            Destroy(collision.gameObject);
        }
    }

}
