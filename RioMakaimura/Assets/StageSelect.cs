using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelect : MonoBehaviour
{
    //変数宣言
    public int Stage_num;
    public GameObject Stage_1;
    public GameObject Stage_2;
    public GameObject Stage_3;
    public GameObject Stage_4;
    public GameObject Stage_5;

    void Start()
    {
        //現在のstage_numを呼び出す
        Stage_num = PlayerPrefs.GetInt("SCORE", 0);
    }

    // Update is called once per frame
    void Update()
    {

        //stage_numが２以上のとき、ステージ２を解放する。以下同様
        if (Stage_num >= 2)
        {
            Stage_2.SetActive(true);
        }

        if (Stage_num >= 3)
        {
            Stage_3.SetActive(true);
        }

        if (Stage_num >= 4)
        {
            Stage_4.SetActive(true);
        }

        if (Stage_num >= 5)
        {
            Stage_5.SetActive(true);
        }
    }


}
