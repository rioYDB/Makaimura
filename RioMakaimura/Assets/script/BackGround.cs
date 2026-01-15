using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    //変数宣言
    public Camera cam;      //カメラ情報(Inspector)
    public GameObject bgObj; //背景画像のプレハブ（Inspectorで設定）
    public float scrollSpeed;   //スクロール速度

    GameObject[] bg;    //実際に動かす背景のオブジェクト
    void Start()
    {
        //3つ分のGameObjectを用意
        bg = new GameObject[3];
        //背景画像を格納する
        for (int i = 0; i < bg.Length; ++i)
        {
            //プレハブから配列にインスタンス化する
            bg[i] = Instantiate(bgObj, new Vector3((float)(i - 1) * 20.0f, 0.0f, 0.0f), Quaternion.identity);
            //親オブジェクトをBGにする
            bg[i].transform.SetParent(transform);
            //sortingOder(Oder In Leyer)を-1に設定
            bg[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // カメラが未設定の場合の安全策
        if (cam == null) return;

        // 1. 背景全体の移動（カメラとの追従）
        this.transform.position = new Vector2(cam.transform.position.x * scrollSpeed, 0.0f);

        // 2. 背景個別の回り込み（ローカル座標で管理する）
        for (int i = 0; i < bg.Length; ++i)
        {
            // 親（this）から見た背景の相対位置（localPosition）を取得
            float localX = bg[i].transform.localPosition.x;

            // 親（背景システム全体の中心）から30以上離れたら移動させる
            // ※ 20.0f幅の画像3枚(計60f)をループさせる計算
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