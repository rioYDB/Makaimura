using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // プレイヤーの情報
    public GameObject playerInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInfo != null)
        {
            // X座標だけプレイヤーの位置に合わせる
            transform.position = new Vector3(playerInfo.transform.position.x, 0.0f, transform.position.z);
        }
        
    }
}
