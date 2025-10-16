using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    private Image flashImage;
    private Coroutine flashCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
        flashImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �t���b�V�����J�n����֐�
    public void Flash(Color color, float duration)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color color, float duration)
    {
        float half = duration / 2f;
        float timer = 0f;

        // �t�F�[�h�C���i�Ԃ��Z���Ȃ�j
        while (timer < half)
        {
            timer += Time.deltaTime;
            float t = timer / half;
            flashImage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, color.a, t));
            yield return null;
        }

        // �t�F�[�h�A�E�g�i�Ԃ�������j
        timer = 0f;
        while (timer < half)
        {
            timer += Time.deltaTime;
            float t = timer / half;
            flashImage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0f, t));
            yield return null;
        }

        flashImage.color = new Color(color.r, color.g, color.b, 0f); // �Ō�ɓ�����
    }
}
