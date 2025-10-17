using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("�v���C���[�̎Q��")]
    public GameObject playerInfo;

    [Header("�J�����ʒu�I�t�Z�b�g")]
    public float yOffset; // �� Y���W�̍�������

    [Header("Y���Ǐ]�ݒ�")]
    public float upFollowThreshold = 1.5f;    // ������ɂǂꂭ�炢���ꂽ��ǂ���
    public float downFollowThreshold = 0.5f;  // �������ɂǂꂭ�炢���ꂽ��ǂ���
    public float ySmoothSpeed = 2.0f;     // �� Y���ړ��̃X���[�Y��

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
            //originalOffset.y = yOffset; // �� ����������Y�����𔽉f
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo == null /*|| isShaking*/)
            return;
        //{
        //// �v���C���[�̈ʒu�ɒǏ]�iY���W�͒����l�𔽉f�j
        //Vector3 targetPos = playerInfo.transform.position + new Vector3(originalOffset.x, yOffset, originalOffset.z);
        //transform.position = targetPos;
        //}

        // �v���C���[�̌��݈ʒu
        Vector3 playerPos = playerInfo.transform.position;

        // X�EZ �͏�ɒǏ]
        float targetX = playerPos.x + originalOffset.x;
        float targetZ = playerPos.z + originalOffset.z;

        // ���݂̃J����Y�ʒu
        float cameraY = transform.position.y;
        // �v���C���[�̖ڕWY�ʒu
        float targetY = playerPos.y + yOffset + originalOffset.y;

        float yDiff = targetY - cameraY;

        // --- ������ɗ��ꂷ������Ǐ]�J�n ---
        if (yDiff > upFollowThreshold)
        {
            cameraY = Mathf.Lerp(cameraY, targetY - upFollowThreshold, Time.deltaTime * ySmoothSpeed);
        }
        // --- �������ɗ��ꂷ������Ǐ]�J�n ---
        else if (yDiff < -downFollowThreshold)
        {
            cameraY = Mathf.Lerp(cameraY, targetY + downFollowThreshold, Time.deltaTime * ySmoothSpeed);
        }

        transform.position = new Vector3(targetX, cameraY, targetZ);
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

            //Vector3 playerPos = playerInfo.transform.position;
            //Vector3 targetPos = playerPos + new Vector3(originalOffset.x, yOffset, originalOffset.z);
            //transform.position = targetPos + randomOffset;
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
