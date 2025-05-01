using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;     //‘„‚ÌˆÚ“®‘¬“x
	private float time;         //‘„‚ÌÁ–ÅŠÔ

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//‘„‚ÌˆÚ“®
		transform.Translate(movespeed, 0.0f, 0.0f);


	}
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Activearea")
			Destroy(collision.gameObject);
	}
}
