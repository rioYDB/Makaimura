using UnityEngine;

public class karasu : MonoBehaviour
{
	public float e_moveSpeed;       // “G‚ÌˆÚ“®‘¬“x

	Rigidbody2D rb;                 //Rigidbody2D‚ÌŠi”[
	Renderer Ren;

	void Start()
	{
		//ƒAƒ^ƒbƒ`‚³‚ê‚Ä‚¢‚éComponent‚ðŽæ“¾
		rb = GetComponent<Rigidbody2D>();

	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(-e_moveSpeed, 0.0f, 0.0f);      //‰¡‚ÉˆÚ“®
		transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 0.3f), transform.position.z);      //‚Ó‚í‚Ó‚í”ò‚ñ‚Å‚é‚æ‚¤‚ÉŒ©‚¹‚é
	}
	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}
}
