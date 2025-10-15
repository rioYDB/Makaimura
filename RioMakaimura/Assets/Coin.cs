using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{

    public int value = 1; // �R�C���̉��l�i�X�R�A�p�j
    private bool isCollected = false; // �A���Ŏ���Ȃ��悤�ɂ��邽��
    public GameObject sparkleEffect; // �� �p�[�e�B�N���v���n�u��o�^����p
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[���G�ꂽ��
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // �X�R�A�����Z����i�X�R�A�Ǘ��N���X������΁j
            CoinScoreManager.instance.AddScore(value);

            // �R�C���̊g�剉�o�����s
            StartCoroutine(CollectEffect());

            // �擾���i�C�Ӂj
            // AudioSource.PlayClipAtPoint(coinSound, transform.position);


        }
    }

    private IEnumerator CollectEffect()
    {
        float duration = 0.05f; // �g��ɂ����鎞��
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        //�p�[�e�B�N�����o���I
        if (sparkleEffect != null)
        {
            GameObject effect = Instantiate(sparkleEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f); // 0.5�b�ŏ���
        }

        // �����҂��Ă��������
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }
}
