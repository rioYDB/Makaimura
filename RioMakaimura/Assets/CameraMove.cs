using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // �v���C���[�̏��
    public GameObject playerInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInfo != null)
        {
            // X���W�����v���C���[�̈ʒu�ɍ��킹��
            transform.position = new Vector3(playerInfo.transform.position.x, 0.0f, transform.position.z);
        }
        
    }
}
