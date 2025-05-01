using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class enemy_Flower : MonoBehaviour
{
	//bulletのプレハブ
	public GameObject bullet;
	public float bulletSpeed;       // 敵の移動速度

	//プレイヤーの座標
	public Transform player;
	// 移動方向を記録する
	private Vector2 moveDirection;

	//弾の発射レート
	public float targetTime ;
	public float currentTime ;
	// Start is called before the first frame update
	void Start()
	{

	}
	// Update is called once per frame
	void Update()
	{
		//何秒かに発射されるか
		currentTime += Time.deltaTime;
		if (targetTime < currentTime)
		{
			currentTime = 0;
			//敵の座標を取得
			var pos = this.gameObject.transform.position;
			//弾のプレハブ
			var t = Instantiate(bullet) as GameObject;
			//弾を敵の位置に出す
			t.transform.position = pos;
			//敵からプレイヤーに向かうようにする
			//プレイヤーから敵の位置を引く
			if (player == null) return;

			// 移動方向を保持し続ける
			transform.position += (Vector3)moveDirection * bulletSpeed * Time.deltaTime;

			// 方向転換をしないようにするため、回転処理は無効化
			transform.rotation = Quaternion.identity; // 常に回転をリセット（固定）
		}
	}
}