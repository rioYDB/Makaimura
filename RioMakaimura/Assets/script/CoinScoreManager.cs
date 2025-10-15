using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CoinScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // ���̃X�N���v�g����ȒP�ɃA�N�Z�X�ł���悤�ɂ��邽�߂́u�V���O���g���v�ݒ�
    public static CoinScoreManager instance;

    // �X�R�A��\������TextMeshPro UI
    public TMP_Text scoreText;

    // ���݂̃X�R�A
    private int score = 0;

    // �Q�[���J�n����1�񂾂��Ă΂��
    private void Awake()
    {
        // ���̃X�N���v�g��B��̃C���X�^���X�Ƃ��ēo�^
        instance = this;
    }

    void Start()
    {
        scoreText.text = "Coins: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �X�R�A�����Z����֐�
    public void AddScore(int value)
    {
        // �X�R�A��value���𑫂�
        score += value;
        
        // Text�ɃX�R�A��\���iUI���X�V�j
        scoreText.text = "Coins: " + score.ToString();

        // �X�R�AUI���s�������Ɠ��������o
        StartCoroutine(ScoreTextEffect());

        //10���W�߂��烊�Z�b�g
        if (score % 10 == 0)
        {
            // �����ŃX�R�A�����Z�b�g
            ResetScore();

            // �K�v�Ȃ�u�{�[�i�X���o�v��u���v�������Œǉ��ł���
            Debug.Log("10���W�߂��I�X�R�A�����Z�b�g���܂����B");
        }
    }

    private IEnumerator ScoreTextEffect()
    {
        float duration = 0.1f;
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // �����҂��Ă��猳�ɖ߂�
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }

    // �X�R�A�����Z�b�g�������Ƃ��Ɏg����֐��i�C�Ӂj
    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Coins: " + score.ToString();
    }

    // ���݂̃X�R�A���擾�������Ƃ��Ɏg���i�C�Ӂj
    public int GetScore()
    {
        return score;
    }
  
}
