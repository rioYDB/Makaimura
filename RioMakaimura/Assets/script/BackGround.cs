using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera cam;
    public GameObject bgObj;
    public float scrollSpeed; // 0ならカメラに完全固定、1なら動かない

    [Header("背景のサイズ設定")]
    public float bgWidth = 50.0f;

    private GameObject[] bg;
    private bool isInitialized = false;

    private float startZ;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
        startZ = transform.position.z;

        bg = new GameObject[3];
        for (int i = 0; i < bg.Length; ++i)
        {
            if (bgObj == null) return;

            // 最初は「左・中・右」の順に並べる (-1, 0, 1)
            bg[i] = Instantiate(bgObj, new Vector3((i - 1) * bgWidth, 0.0f, 0.0f), Quaternion.identity);
            bg[i].transform.SetParent(transform);
            bg[i].transform.localPosition = new Vector3((i - 1) * bgWidth, 0.0f, 0.0f);

            SpriteRenderer sr = bg[i].GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = -1;
        }
        isInitialized = true;
    }

    void Update()
    {
        if (cam == null || !isInitialized) return;

        // 1. パララックス移動量の計算
        // dist: カメラに合わせて背景をどれだけずらすか
        float dist = (cam.transform.position.x * scrollSpeed);

        // 2. ループ（回り込み）判定用の計算
        // temp: 視差を考慮した「実質的なカメラの移動量」
        float temp = (cam.transform.position.x * (1 - scrollSpeed));

        // 3. 親オブジェクトをカメラの高さに固定しつつ、Xは視差移動
        transform.position = new Vector3(startX + dist, cam.transform.position.y, startZ);

        // 4. 背景の回り込みロジック (ここが重要！)
        // カメラが今の背景の端を越えたら、startXを1枚分ずらす
        if (temp > startX + bgWidth)
        {
            startX += bgWidth;
        }
        else if (temp < startX - bgWidth)
        {
            startX -= bgWidth;
        }
    }
}