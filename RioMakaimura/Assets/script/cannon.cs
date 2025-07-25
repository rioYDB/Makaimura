using UnityEngine;
using System.Collections;

public class cannon : MonoBehaviour
{
    public GameObject BulletPrefab;       // 発射する槍のプレハブをInspectorから設定
    public float fireRate = 2.0f;        // 槍を発射する間隔 (秒)
    public int fireDirection = 1;        // 槍が飛ぶ方向 (1:右, -1:左)

    void Start()
    {
        StartCoroutine(EnemyBulletRoutine()); // 発射ルーチンを開始
    }

    IEnumerator EnemyBulletRoutine()
    {
        while (true) // 無限ループで定期的に発射
        {
            yield return new WaitForSeconds(fireRate); // 発射間隔を待つ

            // 槍を生成する位置は、この発射口の場所
            // 槍の回転はなし (Quaternion.identity)
            GameObject newSpear = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            // 生成した槍のスクリプトに方向を伝える
            Enemybullet spearScript = newSpear.GetComponent<Enemybullet>();
            if (spearScript != null)
            {
                spearScript.SetDirection(fireDirection);
            }
        }
    }
}
