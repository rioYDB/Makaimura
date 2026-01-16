using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("プレイヤーの参照")]
    //public GameObject playerInfo;
    public player_control player;

    [Header("カメラ位置オフセット")]
    public float yOffset; // ← Y座標の高さ調整

    [Header("Y軸追従設定")]
    public float upFollowThreshold = 1.5f;    // 上方向にどれくらい離れたら追うか
    public float downFollowThreshold = 0.5f;  // 下方向にどれくらい離れたら追うか
    public float ySmoothSpeed = 2.0f;     // ← Y軸移動のスムーズさ

    [Header("カメラ揺れ設定")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.1f;

    [Header("横方向（先読み）設定")]
    public float lookAheadDistance = 2.0f;  // 向いている方向にずらす距離
    public float lookAheadSmoothSpeed = 3.0f; // スムーズさ（高いほど素早く）

    private Vector3 originalOffset; // プレイヤーとの距離
    private float currentLookAheadX = 0f;
    private float targetLookAheadX = 0f;

    private Coroutine shakeCoroutine;
    private bool isShaking = false;
	public bool isFollow = true;

	private Rigidbody2D playerRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
        {
            // プレイヤーとカメラの初期距離を記録
            originalOffset = transform.position - player.transform.position;
            //originalOffset.y = yOffset; // ← 初期化時にY調整を反映

        }
    }

	// Update is called once per frame
	//void LateUpdate()
	//{
	//    if (player == null /*|| isShaking*/)
	//        return;

	//    if (shakeCoroutine == null) // ← 揺れ中は通常追従を止める
	//    {
	//        UpdateCameraFollow();
	//    }
	//}

	void LateUpdate()
	{
		if (player == null)
			return;

		// ★ ボスエリアなどで固定中
		if (!isFollow)
			return;

		if (shakeCoroutine == null)
		{
			UpdateCameraFollow();
		}
	}

	public void LockCamera()
	{
		isFollow = false;
	}

	public void UnlockCamera()
	{
		isFollow = true;
	}



	private void UpdateCameraFollow()
    {
        // プレイヤーの現在位置
        Vector3 playerPos = player.transform.position;



        // === プレイヤーの向きに応じた先読み方向 ===
        float dir = player.isFacingRight ? 1f : -1f;
        targetLookAheadX = lookAheadDistance * dir;
        currentLookAheadX = Mathf.Lerp(currentLookAheadX, targetLookAheadX, Time.deltaTime * lookAheadSmoothSpeed);

        // === 縦方向の追従 ===
        float cameraY = transform.position.y;
        float targetY = playerPos.y + yOffset + originalOffset.y;
        float yDiff = targetY - cameraY;

        if (yDiff > upFollowThreshold)
            cameraY = Mathf.Lerp(cameraY, targetY, Time.deltaTime * 2.0f);
        else if (yDiff < -downFollowThreshold)
            cameraY = Mathf.Lerp(cameraY, targetY, Time.deltaTime * 5.0f);

        // === 最終的なカメラ位置 ===
        float targetX = playerPos.x + originalOffset.x + currentLookAheadX;
        float targetZ = playerPos.z + originalOffset.z;

        transform.position = new Vector3(targetX, cameraY, targetZ);
    }

    /// <summary>
    /// 外部から呼び出してカメラを揺らす
    /// </summary>
    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (duration < 0) duration = shakeDuration;
        if (magnitude < 0) magnitude = shakeMagnitude;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position; // カメラ自体の現在位置
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // カメラ中心を基準にランダムに動かす
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                0
            );

            transform.position = originalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos; // 元の位置に戻す
        shakeCoroutine = null; // 終了を記録
    }
}
