using UnityEngine;

public class Bullet_Vampire : bullet
{

    public GameObject firepillarPrefab;     //火柱を呼び出すプレハブ
    public LayerMask Ground;           //グラウンド判定をするレイヤー
    protected override void Start()
    {

    }

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("ヴァンパイア状態でアタック！！！");
        Destroy(Enemy);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //親の衝突判定を呼ぶ
        base.OnTriggerEnter2D(collision);

        //弾(炎)が地面に触れたら火柱を呼び出す
        //下方向にrayを飛ばして、指定したレイヤーのオブジェクトと接触しているかどうか判別する
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, Ground);
        //ヒットしていない場合はnullが返される
        if (hit.collider != null)
        {
            Vector3 Firepillarpos = new Vector3(transform.position.x,collision.bounds.max.y,transform.position.z);
            Instantiate(firepillarPrefab, Firepillarpos, Quaternion.identity);
            Destroy(gameObject);
        }
        

    }


    protected override void Update()
    {
        base.Update();//親クラスのUpdate()を開始
    }
}