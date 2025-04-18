using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetGravity : MonoBehaviour
{
    Rigidbody2D rb; //Rigidbody2D‚ÌŠi”[‚ğ‚·‚é‚½‚ß‚Ì•Ï”
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //d—Í‚Ìİ’è
        Vector2 SetGravity = new Vector2(0f, -25f);

		//Rigidbody2D‚Éd—Í‚ğ‰Á‚¦‚é
		rb.AddForce(SetGravity);
    }
}
