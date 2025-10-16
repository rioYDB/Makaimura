using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("�v���C���[�̎Q��")]
    public GameObject playerInfo;

    [Header("�J�����h��ݒ�")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalOffset; // �v���C���[�Ƃ̋���
    private Coroutine shakeCoroutine;
    private bool isShaking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerInfo != null)
        {
            // �v���C���[�ƃJ�����̏����������L�^
            originalOffset = transform.position - playerInfo.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo != null && !isShaking)
        {
            // �Ǐ]���i��Ƀv���C���[�̈ʒu�ɍ��킹��j
            transform.position = playerInfo.transform.position + originalOffset;
        }
    }

    /// <summary>
    /// �O������Ăяo���ăJ������h�炷
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

        // �h��I����A�Ǐ]�ʒu�ɖ߂�
        if (playerInfo != null)
        {
            transform.position = playerInfo.transform.position + originalOffset;
        }

        isShaking = false;
    }
}
