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
		//�v���C���[�ɐG�ꂽ�牽�����Ȃ�
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("�v���C���[�Ƃ̏Փ˂𖳎����܂�");
			return;
		}

		//Enemy�ɐG�ꂽ��j�󂷂�
		if (collision.gameObject.tag == "Enemy")
		{

			Debug.Log("�G�Ƃ̏Փ˂����o");
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

}
