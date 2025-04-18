using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;　	//槍の移動速度
	
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//槍の移動
		transform.Translate(movespeed, 0.0f, 0.0f);




	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		//当たっていた対象物の「tag」がActiveAreaだった場合は処理する
		if (collision.gameObject.tag == "ActiveArea")
		{
			//自分自身の「GameObject」を破棄する
			Destroy(gameObject);
		}
	}
}
