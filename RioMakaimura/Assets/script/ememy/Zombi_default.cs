using UnityEngine;

public class Zombi_default : MonoBehaviour
{
    public float z_moveSpeed;       // 敵の移動速度
   

    private Transform player;      // プレイヤーの座標取得
    private Vector2 moveDirection; // 移動方向を記録する

    Rigidbody2D rb;                 //Rigidbody2Dの格納
    private CapsuleCollider2D capsuleCollider;   // ColliderをCapsuleに変更

    public LayerMask Ground; // ← インスペクターから地面レイヤーを指定
    //private bool isGrounded = false;


 
   

    private float originalHeight;
    private float originalWidth;
    private Vector2 originalOffset;
    private Vector3 originalPosition;

    

    public delegate void OnDesloyDelegate(GameObject g);
    public OnDesloyDelegate mOnDestly = new OnDesloyDelegate((GameObject g) => { });
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //アタッチされているComponentを取得
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();


        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 最初に向かう方向を計算して固定
        if (player != null)
        {
            moveDirection = (player.position - transform.position).normalized;

            moveDirection.y = 0f;            // Y成分をゼロにする
            moveDirection = moveDirection.normalized; // 正規化する
        }

     
    }

    // Update is called once per frame
    void Update()
    {

        // 敵キャラクターが画面内にいるか確認
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            // 画面外にいる場合は岸辺露伴は動かない
            return;
        }


        // 方向転換をしないようにするため、回転処理は無効化
        transform.rotation = Quaternion.identity; // 常に回転をリセット（固定）

        transform.position += (Vector3)moveDirection * z_moveSpeed * Time.deltaTime;

    }

    private void OnDestroy()
    {
        //破棄直前のデリゲートを呼び出す
        mOnDestly(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       

        //// 自分がplayerに属している場合
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // 衝突を無視する
        //    Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
        //}
    }

    void OnBecameInvisible()
    {
        // 画面外に出たら自動的に敵を破壊する

        //Destroy(gameObject);
    }




}
