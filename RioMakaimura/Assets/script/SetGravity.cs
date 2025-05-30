using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SetGravity : MonoBehaviour
{
    public bool IsEnable { get; set; }

    Rigidbody2D rb; //Rigidbody2Dの格納をするための変数
	void Start()
    {
        IsEnable = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnable)
        {
            //重力の設定
            Vector2 SetGravity = new Vector2(0f, -25f);

            //Rigidbody2Dに重力を加える
            rb.AddForce(SetGravity);
        }
    }
}
