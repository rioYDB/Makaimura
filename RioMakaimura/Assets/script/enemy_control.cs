using UnityEngine;

public class enemy_control : MonoBehaviour
{
	public float e_moveSpeed;       // “G‚ÌˆÚ“®‘¬“x


	Rigidbody2D rb;                 //Rigidbody2D‚ÌŠi”[
									
	void Start()
    {
		//ƒAƒ^ƒbƒ`‚³‚ê‚Ä‚¢‚éComponent‚ðŽæ“¾
		rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
		transform.Translate(-e_moveSpeed, 0.0f, 0.0f);
	}
}
