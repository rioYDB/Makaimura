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
        //背景の移動（カメラの動きと背景の動きを関連付ける）
        this.transform.position = new Vector2(cam.transform.position.x * scrollSpeed, 0.0f);

        //背景画像を回り込ませる
        for (int i = 0; i < bg.Length; ++i)
        {
            //カメラのX座標と背景画像もX座標の距離が一定値を超えたら回り込ませる
            if (bg[i].transform.position.x < cam.transform.position.x - 30.0f)
            {
                //右端に回り込ませる
                bg[i].transform.localPosition = new Vector2(bg[i].transform.localPosition.x + 60.0f, 0.0f);
            }
            else if (bg[i].transform.position.x > cam.transform.position.x + 30.0f)
            {
                //左端に回り込ませる
                bg[i].transform.localPosition = new Vector2(bg[i].transform.localPosition.x - 60.0f, 0.0f);
            }

        }
    }
}
