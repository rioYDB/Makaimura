using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;     //���̈ړ����x
	private float time;         //���̏��Ŏ���

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

	//��ʊO�ɏo���炱�̃I�u�W�F�N�g��j��
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	//�G�ɐG�ꂽ�炱�̃I�u�W�F�N�g��j��
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			return;
		}


		if (collision.gameObject.tag == "Enemy")
		{
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

}
