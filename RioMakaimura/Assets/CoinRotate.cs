using UnityEngine;

public class CoinRotate : MonoBehaviour
{

    public float rotationSpeed = 180f; // ��]�X�s�[�h�i�x/�b�j


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
