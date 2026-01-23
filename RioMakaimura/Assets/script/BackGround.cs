using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera cam;
    public GameObject bgObj;
    public float scrollSpeed;

    [Header("背景のサイズ設定")]
    public float bgWidth = 50.0f;

    private GameObject[] bg;
    private bool isInitialized = false;

    // ★重要：エディタで設定した「高さ」や「奥行き」を保存する変数
    private float startY;
    private float startZ;
    private float startX;

    void Start()
    {
        // 1. ゲーム開始時の位置（Y軸やZ軸）を記録する
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;

        bg = new GameObject[3];
        for (int i = 0; i < bg.Length; ++i)
        {
            if (bgObj == null) return;

            // 2. 子オブジェクト（背景）を生成する際、親のY,Zに合わせる
            bg[i] = Instantiate(bgObj, new Vector3((float)(i - 1) * bgWidth, 0.0f, 0.0f), Quaternion.identity);
            bg[i].transform.SetParent(transform);

            // 子のローカル座標を確実に(0,0,0)ベースにする
            bg[i].transform.localPosition = new Vector3((float)(i - 1) * bgWidth, 0.0f, 0.0f);

            SpriteRenderer sr = bg[i].GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = -1;
        }
        isInitialized = true;
    }

    void Update()
    {
        if (cam == null || !isInitialized) return;

        // 3. 【位置調整の核心】
        // X座標はカメラに合わせて動かすが、YとZは「最初にエディタで置いた値」を維持する
        float targetX = startX + (cam.transform.position.x * scrollSpeed);

        // ここで startY と startZ を使うことで、エディタでの調整が反映されます
        this.transform.position = new Vector3(targetX, startY, startZ);

        for (int i = 0; i < bg.Length; ++i)
        {
            if (bg[i] == null) continue;

            float localX = bg[i].transform.localPosition.x;
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