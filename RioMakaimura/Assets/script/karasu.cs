using UnityEngine;

public class karasu : MonoBehaviour
{
	public float e_moveSpeed;       // �G�̈ړ����x

	Rigidbody2D rb;                 //Rigidbody2D�̊i�[
	Renderer Ren;

	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();

	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(-e_moveSpeed, 0.0f, 0.0f);      //���Ɉړ�
		transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 0.3f), transform.position.z);      //�ӂ�ӂ���ł�悤�Ɍ�����
	}
	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}
}
