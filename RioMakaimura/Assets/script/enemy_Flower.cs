using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class enemy_Flower : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform player;
	public float bulletSpeed ;
	public float fireRate ; // 1�b�Ԃɉ�������
	private float nextFireTime =0f;

	void Update()
	{
		// �v���C���[��null�łȂ����m�F

		if (player == null)
		{
			return; // �v���C���[���j������Ă���̂Ŕ��˂��Ȃ�
		}

		// �G�L�����N�^�[����ʓ��ɂ��邩�m�F
		Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
		if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
		{
			// ��ʊO�ɂ���ꍇ�͔��˂��Ȃ�
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