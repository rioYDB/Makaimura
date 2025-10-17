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

    // �v���C���[�ւ̎Q�Ƃ�ǉ�
    private player_control playerHealth;

    // �R���[�`����1�������������߂̕ϐ���ǉ�
    private Coroutine scaleCoroutine;
    private Vector3 originalScale;

    // �Q�[���J�n����1�񂾂��Ă΂��
    private void Awake()
    {
        // ���̃X�N���v�g��B��̃C���X�^���X�Ƃ��ēo�^
        instance = this;
    }

    void Start()
    {
        // �v���C���[���������Ď擾
        playerHealth = FindAnyObjectByType<player_control>();

        scoreText.text = "Coins: " + score.ToString();

        originalScale = scoreText.transform.localScale; // �� ���̑傫�����L�^
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
        // ���ɃA�j�����Ȃ�~�߂Ă���ăX�^�[�g
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScoreTextEffect());

        //10���W�߂���HP�񕜁����Z�b�g
        if (score % 10 == 0)
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(1); // HP��1�񕜁I
            }

            // �����ŃX�R�A�����Z�b�g
            ResetScore();

            // �K�v�Ȃ�u�{�[�i�X���o�v��u���v�������Œǉ��ł���
            Debug.Log("10���W�߂��IHP�񕜁I�X�R�A�����Z�b�g���܂����B");
        }
    }

    private IEnumerator ScoreTextEffect()
    {
        float duration = 0.1f;
        Vector3 targetScale = originalScale * 1.2f;
        // �X�P�[�������Z�b�g���Ă���g��
        scoreText.transform.localScale = originalScale;

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

        scoreText.transform.localScale = originalScale; // �O�̂��ߊ��S�ɖ߂�
        scaleCoroutine = null; // �R���[�`������
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
