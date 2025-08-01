using UnityEngine;

public class Firepillar : bullet
{
    public float lifetime = 0.2f; // 爪攻撃の持続時間（短い）

    //protected override void BulletMoves(GameObject Enemy)
    //{
    //    Debug.Log("ヴァンパイアでアタック！！！");
    //    Destroy(Enemy);
    //    Destroy(gameObject);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime); // 自動で削除
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        // 敵にEnemy_HPスクリプトがあるか確認
        enemy_HP enemyHP = collision.GetComponent<enemy_HP>();
        if (enemyHP != null)
        {
            enemyHP.TakeDamage(1); // ダメージ量を1とする（必要に応じて変える）
        }

    }

}
