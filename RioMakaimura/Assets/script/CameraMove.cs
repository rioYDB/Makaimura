using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("プレイヤーの参照")]
    public GameObject playerInfo;

    [Header("カメラ位置オフセット")]
    public float yOffset; // ← Y座標の高さ調整

    [Header("Y軸追従設定")]
    public float upFollowThreshold = 1.5f;    // 上方向にどれくらい離れたら追うか
    public float downFollowThreshold = 0.5f;  // 下方向にどれくらい離れたら追うか
    public float ySmoothSpeed = 2.0f;     // ← Y軸移動のスムーズさ

    [Header("カメラ揺れ設定")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalOffset; // プレイヤーとの距離
    private Coroutine shakeCoroutine;
    private bool isShaking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerInfo != null)
        {
            // プレイヤーとカメラの初期距離を記録
            originalOffset = transform.position - playerInfo.transform.position;
            //originalOffset.y = yOffset; // ← 初期化時にY調整を反映
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo == null /*|| isShaking*/)
            return;
        //{
        //// プレイヤーの位置に追従（Y座標は調整値を反映）
        //Vector3 targetPos = playerInfo.transform.position + new Vector3(originalOffset.x, yOffset, originalOffset.z);
        //transform.position = targetPos;
        //}

        // プレイヤーの現在位置
        Vector3 playerPos = playerInfo.transform.position;

        // X・Z は常に追従
        float targetX = playerPos.x + originalOffset.x;
        float targetZ = playerPos.z + originalOffset.z;

        // 現在のカメラY位置
        float cameraY = transform.position.y;
        // プレイヤーの目標Y位置
        float targetY = playerPos.y + yOffset + originalOffset.y;

        float yDiff = targetY - cameraY;

        // --- 上方向に離れすぎたら追従開始 ---
        if (yDiff > upFollowThreshold)
        {
            cameraY = Mathf.Lerp(cameraY, targetY - upFollowThreshold, Time.deltaTime * ySmoothSpeed);
        }
        // --- 下方向に離れすぎたら追従開始 ---
        else if (yDiff < -downFollowThreshold)
        {
            cameraY = Mathf.Lerp(cameraY, targetY + downFollowThreshold, Time.deltaTime * ySmoothSpeed);
        }

        transform.position = new Vector3(targetX, cameraY, targetZ);
    }

    /// <summary>
    /// 外部から呼び出してカメラを揺らす
    /// </summary>
    public void Shake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            if (playerInfo == null) yield break;

            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                Random.Range(-1f, 1f) * shakeMagnitude,
                0
            );

            //Vector3 playerPos = playerInfo.transform.position;
            //Vector3 targetPos = playerPos + new Vector3(originalOffset.x, yOffset, originalOffset.z);
            //transform.position = targetPos + randomOffset;
            transform.position = playerInfo.transform.position + originalOffset + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 揺れ終了後、追従位置に戻す
        if (playerInfo != null)
        {
            transform.position = playerInfo.transform.position + originalOffset;
        }

        isShaking = false;
    }
}
