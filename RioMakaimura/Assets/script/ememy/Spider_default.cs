using UnityEngine;

public class Spider_default : MonoBehaviour
{
    public Transform player;
    public float triggerRangeX = 0.5f;
    public float dropStopY = 1.0f;
    public float moveDistance = 1.0f;
    public float moveSpeed = 2.0f;

    private Vector2 originalPosition;
    private Rigidbody2D rb;
    private bool hasDropped = false;
    private bool moving = false;
    private float moveDirection = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDropped)
        {
            // 真下にプレイヤーが来たら落下
            if (Mathf.Abs(player.position.x - transform.position.x) < triggerRangeX &&
                player.position.y < transform.position.y)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                hasDropped = true;
            }
        }
        else if (!moving)
        {
            // 指定Y位置に到達したら停止＆上下移動モードへ
            if (transform.position.y <= dropStopY)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                moving = true;
            }
        }
        else
        {
            // 上下に往復移動
            float newY = transform.position.y + moveDirection * moveSpeed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x, newY);

            if (newY > dropStopY + moveDistance)
            {
                moveDirection = -1;
            }
            else if (newY < dropStopY)
            {
                moveDirection = 1;
            }
        }
    }
}
