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
		// Rigidbodyの速度から現在の移動角度を返す
		get
		{
			if (rb != null)
			{
				return Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
			}
			return 0f; // rbがない場合は0を返す
		}
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			Debug.LogError("HormingスクリプトにはRigidbody2Dコンポーネントが必要です！");
			enabled = false;
		}
		// ★修正: Rigidbodyが存在する場合、初期速度を設定★
		else
		{
			// 初期速度を右方向（Direction=0度）に設定して、ホーミングが始まるようにする
			SetVelocity(0f, _speed);
		}
	}


	/// 角度と速度から移動速度を設定する
	void SetVelocity(float direction, float speed)
	{
		var vx = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
		var vy = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;

		rb.linearVelocity = new Vector2(vx, vy);
	}

	/// 更新
	void Update()
	{
		if (rb == null) return;

		// 1. ターゲット座標を取得（マウスの座標に向かう）
		var mousePosition = Input.mousePosition;

		// Z座標はカメラの距離に合わせる必要がある
		mousePosition.z = (transform.position - Camera.main.transform.position).magnitude;

		Vector3 next = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3 now = transform.position;

		// 2. 目的となる角度を取得する
		var d = next - now;
		var targetAngle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

		// 3. 角度差を求めて新しい角度を計算する (ホーミングロジック)
		var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);
		var newAngle = Direction;

		if (Mathf.Abs(deltaAngle) > 0.01f) // 角度差が微小でなければ
		{
			if (Mathf.Abs(deltaAngle) < _rotSpeed)
			{
				// 旋回速度を下回る角度差なら、ターゲット角度に直接設定
				newAngle = targetAngle;
			}
			else if (deltaAngle > 0)
			{
				// 左回り (現在の角度を旋回速度分増やす)
				newAngle += _rotSpeed;
			}
			else
			{
				// 右回り (現在の角度を旋回速度分減らす)
				newAngle -= _rotSpeed;
			}
		}

		// ★★★ 4. 新しい速度をRigidbodyに設定し直す (移動処理を上書き) ★★★
		SetVelocity(newAngle, _speed);


		// 5. 画像の角度を移動方向に向ける (見た目の更新)
		var renderer = GetComponent<SpriteRenderer>();
		if (renderer != null)
		{
			// 角度を常に正規化しておく
			renderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
		}
	}
}