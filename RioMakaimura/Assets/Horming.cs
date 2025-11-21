using UnityEngine;
using System.Collections;

public class Horming : MonoBehaviour
{
    // Rigidbody2Dの参照を格納するプライベート変数
    private Rigidbody2D rb;

    /// 旋回速度
    float _rotSpeed = 3.0f;
    /// 移動速度
    float _speed = 3.0f;

    /// 移動角度
    float Direction
    {
        // ★修正: rb.velocity を使用★
        get { return Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg; }
    }

    // ★追加: Startメソッドでコンポーネントを取得★
    void Start()
    {
        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("HormingスクリプトにはRigidbody2Dコンポーネントが必要です！");
            enabled = false; // コンポーネントがなければスクリプトを無効化
        }
    }


    /// 角度と速度から移動速度を設定する
    void SetVelocity(float direction, float speed)
    {
        var vx = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
        var vy = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;

        // ★修正: rb.velocity を使用★
        rb.linearVelocity = new Vector2(vx, vy);
    }

    /// 更新
    void Update()
    {
        if (rb == null) return; // Rigidbodyがない場合は処理しない

        // 画像の角度を移動方向に向ける
        var renderer = GetComponent<SpriteRenderer>();
        // ※ Quaternion.Eulerは非推奨ではありませんが、transform.rotationを使うのがより一般的
        renderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Direction));

        // ターゲット座標を取得（マウスの座標に向かって移動する）
        var mousePosition = Input.mousePosition;
        // Z座標はカメラの距離に合わせる必要があるため、修正
        mousePosition.z = (transform.position - Camera.main.transform.position).magnitude;

        Vector3 next = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 now = transform.position;

        // 目的となる角度を取得する
        var d = next - now;
        var targetAngle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

        // 角度差を求める
        var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);
        var newAngle = Direction;

        if (Mathf.Abs(deltaAngle) < _rotSpeed)
        {
            // 旋回速度を下回る角度差なので何もしない
        }
        else if (deltaAngle > 0)
        {
            // 左回り
            newAngle += _rotSpeed;
        }
        else
        {
            // 右回り
            newAngle -= _rotSpeed;
        }

        // 新しい速度を設定する
        SetVelocity(newAngle, _speed);
    }
}