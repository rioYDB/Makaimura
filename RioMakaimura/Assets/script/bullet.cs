using UnityEngine;

public class bullet : MonoBehaviour
{
	public float movespeed;     //���̈ړ����x
	private float time;         //���̏��Ŏ���
	private float direction;    // ���˕���

	// Start is called before the first frame update
	void Start()
	{
		//�v���C���[����n���ꂽ�����Ɋ�Â��Ēe�𔭎�
		direction = transform.localScale.x;  // �v���C���[�̌����ɍ��킹��
	}

	// Update is called once per frame
	void Update()
	{


		//�v���C���[�̌����ɍ��킹�Ēe�̂̈ړ�
		transform.Translate(movespeed * direction , 0.0f, 0.0f);


	}

	//��ʊO�ɏo���炱�̃I�u�W�F�N�g��j��
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	//�G�ɐG�ꂽ�炱�̃I�u�W�F�N�g��j��
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Enemy�ɐG�ꂽ��j�󂷂�
		if (collision.gameObject.tag == "Enemy")
		{
			Debug.Log("�G�Ƃ̏Փ˂����o");
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

}
