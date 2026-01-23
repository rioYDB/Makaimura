using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class doorcontroller : MonoBehaviour
{
    public float openSpeed = 1.0f; // 扉が開く速度
    public float openDistanceY = 5.0f; //上に動く距離
    public AudioClip openSound; // 開く音 (オプション)

    private Vector3 initialPosition;
    private Vector3 targetOpenPosition; //開いた時の目標位置
    private bool isOpening = false;
    private AudioSource audioSource; // 音を鳴らすため

    void Start()
    {
        initialPosition = transform.position;
        //開いた時の目標位置を計算 (現在の位置からY方向にopenDistanceYだけ上に移動)
        targetOpenPosition = initialPosition + new Vector3(0, openDistanceY, 0);
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
    }

    void Update()
    {
        if (isOpening)
        {
            // 扉を徐々に開く（目標位置まで移動させる）
            transform.position = Vector3.MoveTowards(transform.position, targetOpenPosition, openSpeed * Time.deltaTime);

            // 完全に開いたらisOpeningをfalseにする
            if (Vector3.Distance(transform.position, targetOpenPosition) < 0.01f)
            {
                isOpening = false;
                // Colliderを無効化して通れるようにする
                Collider2D doorCollider = GetComponent<Collider2D>();
                if (doorCollider != null)
                {
                    doorCollider.enabled = false; // 扉が完全に開いたら衝突判定を無効にする
                }
            }
        }
    }

    // スイッチから呼び出されるメソッド
    public void OpenDoor()
    {
        Debug.Log("★★ OpenDoor 呼ばれた ★★", this);
        isOpening = true;

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

}
