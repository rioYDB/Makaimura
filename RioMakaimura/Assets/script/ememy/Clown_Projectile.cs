using UnityEngine;

public class Clown_Projectile : MonoBehaviour
{
    public float lifetime = 5f; // �e�������ŏ�����܂ł̎���


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ��莞�Ԍ�Ɏ����ŏ�����i�������ߖ�j
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
