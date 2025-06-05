using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    //�ϐ��錾
    public Camera cam;      //�J�������(Inspector)
    public GameObject bgObj; //�w�i�摜�̃v���n�u�iInspector�Őݒ�j
    public float scrollSpeed;   //�X�N���[�����x

    GameObject[] bg;    //���ۂɓ������w�i�̃I�u�W�F�N�g
    void Start()
    {
        //3����GameObject��p��
        bg = new GameObject[3];
        //�w�i�摜���i�[����
        for (int i = 0; i < bg.Length; ++i)
        {
            //�v���n�u����z��ɃC���X�^���X������
            bg[i] = Instantiate(bgObj, new Vector3((float)(i - 1) * 20.0f, 0.0f, 0.0f), Quaternion.identity);
            //�e�I�u�W�F�N�g��BG�ɂ���
            bg[i].transform.SetParent(transform);
            //sortingOder(Oder In Leyer)��-1�ɐݒ�
            bg[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�w�i�̈ړ��i�J�����̓����Ɣw�i�̓������֘A�t����j
        this.transform.position = new Vector2(cam.transform.position.x * scrollSpeed, 0.0f);

        //�w�i�摜����荞�܂���
        for (int i = 0; i < bg.Length; ++i)
        {
            //�J������X���W�Ɣw�i�摜��X���W�̋��������l�𒴂������荞�܂���
            if (bg[i].transform.position.x < cam.transform.position.x - 30.0f)
            {
                //�E�[�ɉ�荞�܂���
                bg[i].transform.localPosition = new Vector2(bg[i].transform.localPosition.x + 60.0f, 0.0f);
            }
            else if (bg[i].transform.position.x > cam.transform.position.x + 30.0f)
            {
                //���[�ɉ�荞�܂���
                bg[i].transform.localPosition = new Vector2(bg[i].transform.localPosition.x - 60.0f, 0.0f);
            }

        }
    }
}
