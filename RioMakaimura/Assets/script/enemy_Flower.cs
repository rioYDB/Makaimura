using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class enemy_Flower : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform player;
	public float bulletSpeed ;
	public float fireRate ; // 1秒間に何発撃つ
	private float nextFireTime =0f;

	void Update()
	{
		// プレイヤーがnullでないか確認

		if (player == null)
		{
			return; // プレイヤーが破棄されているので発射しない
		}

		// 敵キャラクターが画面内にいるか確認
		Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
		if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
		{
			// 画面外にいる場合は発射しない
			return;
		}

		if (Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + fireRate ;
			GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			Vector3 direction = (player.position - transform.position).normalized;
			bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;

		}
		
	}
}