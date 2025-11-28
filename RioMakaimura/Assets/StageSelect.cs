using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelect : MonoBehaviour
{
    //•Ï”éŒ¾
    public int Stage_num;
    public GameObject Stage_1;
    public GameObject Stage_2;
    public GameObject Stage_3;
    public GameObject Stage_4;
    public GameObject Stage_5;

    void Start()
    {
        //Œ»İ‚Ìstage_num‚ğŒÄ‚Ño‚·
        Stage_num = PlayerPrefs.GetInt("SCORE", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
