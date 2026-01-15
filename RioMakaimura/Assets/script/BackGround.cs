using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera cam;
    public GameObject bgObj;
    public float scrollSpeed;

    GameObject[] bg;
    bool isInitialized = false; // ★追加：初期化フラグ

    void Start()
    {
        bg = new GameObject[3];
        for (int i = 0; i < bg.Length; ++i)
        {
            if (bgObj == null) return; // プレハブがない場合は中断

            bg[i] = Instantiate(bgObj, new Vector3((float)(i - 1) * 20.0f, 0.0f, 0.0f), Quaternion.identity);
            bg[i].transform.SetParent(transform);
            bg[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        isInitialized = true; // ★追加：準備完了
    }

    void Update()
    {
        // ★修正：カメラがない、または準備ができていない場合は何もしない
        if (cam == null || !isInitialized) return;

        this.transform.position = new Vector2(cam.transform.position.x * scrollSpeed, 0.0f);

        for (int i = 0; i < bg.Length; ++i)
        {
            // ★念のため個別のnullチェックを追加
            if (bg[i] == null) continue;

            float localX = bg[i].transform.localPosition.x;

            if (localX < -30.0f)
            {
                bg[i].transform.localPosition += new Vector3(60.0f, 0, 0);
            }
            else if (localX > 30.0f)
            {
                bg[i].transform.localPosition -= new Vector3(60.0f, 0, 0);
            }
        }
    }
}