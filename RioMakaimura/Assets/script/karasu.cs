using UnityEngine;

public class karasu : MonoBehaviour
{
	public float e_moveSpeed;       // �G�̈ړ����x


	Rigidbody2D rb;                 //Rigidbody2D�̊i�[

	void Start()
	{
		//�A�^�b�`����Ă���Component���擾
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(-e_moveSpeed, 0.0f, 0.0f);
	}

	void Enemy_Karasu(Collider2D collision)
	{
		transform.position = new Vector3(-e_moveSpeed, Mathf.Sin(Time.time) * 2.0f + transform.position.y, transform.position.z);
	}
}
