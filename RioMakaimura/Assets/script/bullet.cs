using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;�@	//���̈ړ����x
	
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//���̈ړ�
		transform.Translate(movespeed, 0.0f, 0.0f);




	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		//�������Ă����Ώە��́utag�v��ActiveArea�������ꍇ�͏�������
		if (collision.gameObject.tag == "ActiveArea")
		{
			//�������g�́uGameObject�v��j������
			Destroy(gameObject);
		}
	}
}
