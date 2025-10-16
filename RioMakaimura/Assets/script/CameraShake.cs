using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalOffset;   // �v���C���[����̏������Έʒu
    private Transform target;         // �v���C���[�i���S�ɂȂ�Ώہj
    private Coroutine shakeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �V�[�����̃v���C���[�������擾
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

            // �v���C���[�̈ʒu�𒆐S�ɗh�炷
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                0
            );

            transform.position = target.position + originalOffset + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �Ō�Ɍ��̑��Έʒu�֖߂�
        if (target != null)
        {
            transform.position = target.position + originalOffset;
        }
    }
}
