using UnityEngine;

public class BossCameraTrigger : MonoBehaviour
{
	private CameraMove cam;

	void Start()
	{
		cam = Camera.main.GetComponent<CameraMove>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			cam.LockCamera();
		}
	}
}
