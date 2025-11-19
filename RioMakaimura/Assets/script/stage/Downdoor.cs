using UnityEngine;

public class Downdoor : MonoBehaviour
{
    // ★ Inspectorで設定する項目
    public float fallSpeed = 5.0f;          // 扉が落ちる速度
    public float fallDistance = 10.0f;      // 扉が落ちる距離
    public bool isFallen = false;           // 扉が落ちたかどうかのフラグ

    // ★ 扉の挙動に必要なコンポーネント
    private Collider2D doorCollider;
    private Vector3 initialPosition;         // 初期位置
    private Vector3 targetPosition;          // 最終的な目標位置

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        initialPosition = transform.position;

        // 落下目標位置をStart時に計算しておく
        targetPosition = initialPosition + Vector3.down * fallDistance;

        // 初期状態ではコライダーを有効にしておく (床として機能させるため)
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }

        // 補足：もしRigidbody2Dが付いていたら、ここでKinematicにして重力を無効にしてください
        // Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // 扉が落ちる状態になったら、毎フレーム実行
        if (isFallen)
        {
            FallDown();
        }
    }

    // トリガーゾーンから呼び出され、落下を開始する公開メソッド
    public void StartFalling()
    {
        if (!isFallen) // まだ落ちていなければ実行
        {
            Debug.Log("トラップ発動！床が抜けました。");
            isFallen = true;
        }
    }

    // 実際の落下処理（Y軸方向の移動）
    private void FallDown()
    {
        // 目標位置へ移動 (transform.positionを直接操作)
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            fallSpeed * Time.deltaTime
        );

        // 落ち切ったら (目標位置に到達したら) 機能を停止
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // 落ち切った後の処理
            // コライダーを無効化することで、完全に消えたように見せる
       

            // スクリプトのUpdateを停止 (CPU負荷軽減)
            enabled = false;
        }
    }
}
