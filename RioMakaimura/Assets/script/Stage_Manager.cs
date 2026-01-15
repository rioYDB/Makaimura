using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage_Manager : MonoBehaviour
{

  
	/// <summary>
	/// ステージ選択ボタン押下時処理
	/// </summary>
	/// <param name="bossID">該当ボスID</param>
	public void OnStageSelectButtonPressed(int bossID)
	{
		// シーン切り替え
		SceneManager.LoadScene(bossID + 1);
	}

   
}
