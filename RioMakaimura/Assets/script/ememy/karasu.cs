using UnityEngine;
using UnityEngine.EventSystems;

public class karasu : MonoBehaviour
{
    public float e_moveSpeed = 3.0f;       // 敵の移動速度
    public float floatHeight = 0.5f;       // 上下のふわふわの高さ
    public float floatSpeed = 1.0f;        // ふわふわの速さ

    float startY;                          // 初期Y座標を保存

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {

        // 敵キャラクターが画面内にいるか確認
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            // 画面外にいる場合は岸辺露伴は動かない
            return;
        }


        // 移動させたX座標を取得
        float newX = transform.position.x - e_moveSpeed * Time.deltaTime;

        // Yはふわふわの分だけ上下に揺れる
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        float newY = startY + offsetY;

        // 新しい位置に更新
        transform.position = new Vector3(newX, newY, transform.position.z);



    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}


