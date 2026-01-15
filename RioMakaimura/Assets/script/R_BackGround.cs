using UnityEngine;

public class R_BackGround : MonoBehaviour
{
    public Transform cam;          // ƒƒCƒ“ƒJƒƒ‰‚ÌTransform
    [Tooltip("0 = ƒJƒƒ‰‚ÉŒÅ’è, 1 = ¢ŠE‚ÉŒÅ’è, 0.5 = ”¼•ª’x‚ê‚Ä“®‚­")]
    public float relativeSpeed;

    private float length;          // ”wŒi‰æ‘œ‚Ì‰¡•
    private float startPos;        // ”wŒi‚Ì‰ŠúXÀ•W

    void Start()
    {
        // ƒJƒƒ‰‚ª–¢İ’è‚È‚ç©“®æ“¾
        if (cam == null) cam = Camera.main.transform;

        startPos = transform.position.x;

        // ‰æ‘œ‚Ì‰¡•‚ğæ“¾
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
    }

    void Update()
    {
        // 1. ”wŒi‚ÌˆÚ“®—Ê‚ğŒvZ
        // relativeSpeed‚ª0‚É‹ß‚¢‚Ù‚ÇƒJƒƒ‰‚É’Ç]‚µA1‚É‹ß‚¢‚Ù‚Ç‚»‚Ìê‚É—¯‚Ü‚è‚Ü‚·
        float distance = cam.position.x * (1 - relativeSpeed);

        // 2. ”wŒi‚ğˆÚ“®‚³‚¹‚é
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // 3. ƒ‹[ƒvˆ—i‰ñ‚è‚İj
        // ƒJƒƒ‰‚ªu”wŒi1–‡•ªv‚ğ’Ç‚¢‰z‚µ‚½‚çAŠJnˆÊ’u(startPos)‚ğ‚¸‚ç‚·
        float temp = cam.position.x * relativeSpeed;

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}