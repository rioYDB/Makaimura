using UnityEngine;

public class enemy_control : MonoBehaviour
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
}
