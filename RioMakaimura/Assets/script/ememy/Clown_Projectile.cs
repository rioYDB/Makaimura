using UnityEngine;

public class Clown_Projectile : MonoBehaviour
{
    public float lifetime = 5f; // ’e‚ª©“®‚ÅÁ‚¦‚é‚Ü‚Å‚ÌŠÔ


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ˆê’èŠÔŒã‚É©“®‚ÅÁ‚¦‚éiƒƒ‚ƒŠß–ñj
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
