using UnityEngine;

public class Clown_Projectile : MonoBehaviour
{
    public float lifetime = 5f; // 弾が自動で消えるまでの時間


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 一定時間後に自動で消える（メモリ節約）
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
