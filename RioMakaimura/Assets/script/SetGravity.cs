using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SetGravity : MonoBehaviour
{
    public bool IsEnable { get; set; }

    Rigidbody2D rb; //Rigidbody2D�̊i�[�����邽�߂̕ϐ�
	void Start()
    {
        IsEnable = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnable)
        {
            //�d�͂̐ݒ�
            Vector2 SetGravity = new Vector2(0f, -25f);

            //Rigidbody2D�ɏd�͂�������
            rb.AddForce(SetGravity);
        }
    }
}
