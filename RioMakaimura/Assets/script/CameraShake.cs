using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalOffset;   // プレイヤーからの初期相対位置
    private Transform target;         // プレイヤー（中心になる対象）
    private Coroutine shakeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン内のプレイヤーを自動取得
        target = FindAnyObjectByType<player_control>()?.transform;
        if (target != null)
        {
            originalOffset = transform.position - target.position;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (target == null)
                yield break;

            // プレイヤーの位置を中心に揺らす
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                0
            );

            transform.position = target.position + originalOffset + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 最後に元の相対位置へ戻す
        if (target != null)
        {
            transform.position = target.position + originalOffset;
        }
    }
}
