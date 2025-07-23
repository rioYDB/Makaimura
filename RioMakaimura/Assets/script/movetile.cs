using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class movetile : MonoBehaviour
{

    [SerializeField]
    private Vector3 leftEndPoint; // 左端の座標

    [SerializeField]
    private Vector3 rightEndPoint; // 右端の座標



    [SerializeField]
    private float moveSpeed = 2f;    // 床の移動速度

    private float journeyLength; // 移動する全体の距離
    private float startTime;     // 移動開始時間

    void Start()
    {
        // 現在の床の位置を、設定した開始地点の近くに移動させておく
        // エディタ上で設定したleftEndPointとrightEndPointが適用されるようにする
        transform.position = leftEndPoint;

        // 左端から右端までの距離を計算
        journeyLength = Vector3.Distance(leftEndPoint, rightEndPoint);
        startTime = Time.time; // 移動開始時間を記録
    }

    void Update()
    {
        // 往復の時間を計算 (PingPongで0～1の範囲で正規化された値を取得)
        float pingPongTime = Mathf.PingPong((Time.time - startTime) * moveSpeed / journeyLength, 1f);

        // 左端と右端の間を線形補間 (Lerp) で移動
        transform.position = Vector3.Lerp(leftEndPoint, rightEndPoint, pingPongTime);
    }

    // プレイヤーが乗ったときに一緒に動くようにする場合 (オプション)

    private void OnCollisionEnter2D(Collision2D collision) // 引数も Collision2D に変更
    {
        // "Player"タグのオブジェクトが衝突した場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーを床の子オブジェクトにする
            collision.collider.transform.SetParent(transform);
            Debug.Log("プレイヤーが動く床に乗った！"); // デバッグログ追加
        }
    }


    private void OnCollisionExit2D(Collision2D collision) // 引数も Collision2D に変更
    {
        // "Player"タグのオブジェクトが衝突から離れた場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーの子オブジェクト解除
            // SetParent(null) の前に、プレイヤーのTransformが有効かチェック
            if (collision.collider.transform.parent == transform) // 親がこの床であることを確認
            {
                collision.collider.transform.SetParent(null);
                Debug.Log("プレイヤーが動く床から降りた！"); // デバッグログ追加
            }
        }
    }

    // エディタ上で移動範囲を可視化するためのギズモ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // エディタ上で設定したleftEndPointとrightEndPointが反映されるように
        if (Application.isPlaying) // プレイ中なら現在の位置を使う
        {
            Gizmos.DrawLine(leftEndPoint, rightEndPoint);
            Gizmos.DrawSphere(leftEndPoint, 0.2f);
            Gizmos.DrawSphere(rightEndPoint, 0.2f);
        }
        else // エディタ上ならInspectorで設定された値を使う
        {
            // Inspectorで設定された値がnullでないことを確認 (Unity 2020.1以降で必要になることがある)
            if (leftEndPoint != Vector3.zero || rightEndPoint != Vector3.zero) // 適当なチェック
            {
                Gizmos.DrawLine(leftEndPoint, rightEndPoint);
                Gizmos.DrawSphere(leftEndPoint, 0.2f);
                Gizmos.DrawSphere(rightEndPoint, 0.2f);
            }
        }
    }
}
