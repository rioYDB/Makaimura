using UnityEngine;
using UnityEngine.UI;

public class player_change : MonoBehaviour
{

    public Sprite Okami;        //当たった時に画像を変えるため
    public Sprite Which;        //当たった時に画像を変えるため

    private Image image;            //画像の管理
    bool text1enableKey = true;

    // 画像描画用のコンポーネント
    SpriteRenderer sr;

    private int Human;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SpriteRendererコンポーネントを取得します
        image = GetComponent<Image>();
        // SpriteのSpriteRendererコンポーネントを取得
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
