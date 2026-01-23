using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera cam;
    public GameObject bgObj;
    public float scrollSpeed;

    [Header("背景のサイズ設定")]
    public float bgWidth = 50.0f; // ★インスペクターで画像の横幅に合わせて調整してください

    private GameObject[] bg;
    private bool isInitialized = false;

    void Start()
    {
        bg = new GameObject[3];
        for (int i = 0; i < bg.Length; ++i)
        {
            if (bgObj == null) return;

            // ★固定の20.0fではなくbgWidthを使用
            bg[i] = Instantiate(bgObj, new Vector3((float)(i - 1) * bgWidth, 0.0f, 0.0f), Quaternion.identity);
            bg[i].transform.SetParent(transform);

            SpriteRenderer sr = bg[i].GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = -1;
        }
        isInitialized = true;
    }

    void Update()
    {
        if (cam == null || !isInitialized) return;

        this.transform.position = new Vector2(cam.transform.position.x * scrollSpeed, 0.0f);

        for (int i = 0; i < bg.Length; ++i)
        {
            if (bg[i] == null) continue;

            float localX = bg[i].transform.localPosition.x;

            // ★ループ判定もbgWidthに合わせて自動計算
            // 3枚並んでいるので、中心から「幅×1.5」以上離れたら「幅×3」移動させる
            float limit = bgWidth * 1.5f;
            float moveStep = bgWidth * 3.0f;

            if (localX < -limit)
            {
                bg[i].transform.localPosition += new Vector3(moveStep, 0, 0);
            }
            else if (localX > limit)
            {
                bg[i].transform.localPosition -= new Vector3(moveStep, 0, 0);
            }
        }
    }
}