using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting : MonoBehaviour
{
	private void Awake()
	{
		// FPS��60�ɌŒ�
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
}
