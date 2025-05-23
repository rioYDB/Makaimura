using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetGravity : MonoBehaviour
{
    Rigidbody2D rb; //Rigidbody2Dの格納をするための変数
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //重力の設定
        Vector2 SetGravity = new Vector2(0f, -25f);

		//Rigidbody2Dに重力を加える
		rb.AddForce(SetGravity);
    }
}
