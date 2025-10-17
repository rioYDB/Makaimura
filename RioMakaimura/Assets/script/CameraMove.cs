using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    [Header("�v���C���[�̎Q��")]
    //public GameObject playerInfo;
    public player_control player;

    [Header("�J�����ʒu�I�t�Z�b�g")]
    public float yOffset; // �� Y���W�̍�������

    [Header("Y���Ǐ]�ݒ�")]
    public float upFollowThreshold = 1.5f;    // ������ɂǂꂭ�炢���ꂽ��ǂ���
    public float downFollowThreshold = 0.5f;  // �������ɂǂꂭ�炢���ꂽ��ǂ���
    public float ySmoothSpeed = 2.0f;     // �� Y���ړ��̃X���[�Y��

    [Header("�J�����h��ݒ�")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.1f;

    [Header("�������i��ǂ݁j�ݒ�")]
    public float lookAheadDistance = 2.0f;  // �����Ă�������ɂ��炷����
    public float lookAheadSmoothSpeed = 3.0f; // �X���[�Y���i�����قǑf�����j

    private Vector3 originalOffset; // �v���C���[�Ƃ̋���
    private float currentLookAheadX = 0f;
    private float targetLookAheadX = 0f;

    private Coroutine shakeCoroutine;
    private bool isShaking = false;

    private Rigidbody2D playerRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
        {
            // �v���C���[�ƃJ�����̏����������L�^
            originalOffset = transform.position - player.transform.position;
            //originalOffset.y = yOffset; // �� ����������Y�����𔽉f

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null /*|| isShaking*/)
            return;

        if (shakeCoroutine == null) // �� �h�ꒆ�͒ʏ�Ǐ]���~�߂�
        {
            UpdateCameraFollow();
        }
    }

    private void UpdateCameraFollow()
    {
        // �v���C���[�̌��݈ʒu
        Vector3 playerPos = player.transform.position;



        // === �v���C���[�̌����ɉ�������ǂݕ��� ===
        float dir = player.isFacingRight ? 1f : -1f;
        targetLookAheadX = lookAheadDistance * dir;
        currentLookAheadX = Mathf.Lerp(currentLookAheadX, targetLookAheadX, Time.deltaTime * lookAheadSmoothSpeed);

        // === �c�����̒Ǐ] ===
        float cameraY = transform.position.y;
        float targetY = playerPos.y + yOffset + originalOffset.y;
        float yDiff = targetY - cameraY;

        if (yDiff > upFollowThreshold)
            cameraY = Mathf.Lerp(cameraY, targetY, Time.deltaTime * 2.0f);
        else if (yDiff < -downFollowThreshold)
            cameraY = Mathf.Lerp(cameraY, targetY, Time.deltaTime * 5.0f);

        // === �ŏI�I�ȃJ�����ʒu ===
        float targetX = playerPos.x + originalOffset.x + currentLookAheadX;
        float targetZ = playerPos.z + originalOffset.z;

        transform.position = new Vector3(targetX, cameraY, targetZ);
    }

    /// <summary>
    /// �O������Ăяo���ăJ������h�炷
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
        Vector3 originalPos = transform.position; // �J�������̂̌��݈ʒu
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // �J�������S����Ƀ����_���ɓ�����
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                0
            );

            transform.position = originalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos; // ���̈ʒu�ɖ߂�
        shakeCoroutine = null; // �I�����L�^
    }
}
