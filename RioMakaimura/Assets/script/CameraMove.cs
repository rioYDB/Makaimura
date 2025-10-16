using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("プレイヤーの参照")]
    public GameObject playerInfo;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo != null && !isShaking)
        {
            // 追従中（常にプレイヤーの位置に合わせる）
            transform.position = playerInfo.transform.position + originalOffset;
        }
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
