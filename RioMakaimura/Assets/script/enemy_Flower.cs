using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class enemy_Flower : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform player;
	public float bulletSpeed ;
	public float fireRate ; // 1•bŠÔ‚É‰½”­Œ‚‚Â
	private float nextFireTime =0f;

	void Update()
	{
		if (Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + fireRate ;
			GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			Vector3 direction = (player.position - transform.position).normalized;
			bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;

		}
		
	}
}