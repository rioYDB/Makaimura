using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System.Collections;
//using static System.Net.Mime.MediaTypeNames;
// ★ ここにあった using static System.Net.Mime... を削除しました

public class DemoPlayer : MonoBehaviour
{
    [Header("設定")]
    public float waitTime = 30f;
    public VideoPlayer videoPlayer;
    public GameObject titleUI;
    public Image fadePanel;
    public TextMeshProUGUI demoText;

    [Header("フェード設定")]
    public float fadeDuration = 1.0f;
    public float endFadeStartBefore = 3.0f;

    private float timer = 0f;
    private bool isPlayingDemo = false;
    private bool isFading = false;
    private bool isEndingHandled = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;

        videoPlayer.gameObject.SetActive(false);
        fadePanel.gameObject.SetActive(false);
        demoText.gameObject.SetActive(false);

        Color fadeColor = fadePanel.color;
        fadeColor.a = 0;
        fadePanel.color = fadeColor;
    }

    void Update()
    {
        if (isFading) return;

        if (isPlayingDemo)
        {
            if (Input.anyKeyDown)
            {
                StopDemo();
                return;
            }

            double duration = videoPlayer.length;
            double currentTime = videoPlayer.time;

            if (!isEndingHandled && duration > 0 && (duration - currentTime) <= (double)endFadeStartBefore)
            {
                isEndingHandled = true;
                StopDemo();
            }
            return;
        }

        if (Input.anyKey) timer = 0f;
        else timer += Time.deltaTime;

        if (timer >= waitTime) StartDemo();
    }

    public void StartDemo()
    {
        isEndingHandled = false;
        StartCoroutine(FadeOutAndPlayDemo());
    }

    public void StopDemo()
    {
        StartCoroutine(FadeOutAndReturnTitle());
    }

    IEnumerator FadeOutAndPlayDemo()
    {
        isFading = true;
        fadePanel.gameObject.SetActive(true);

        yield return StartCoroutine(UpdateFade(0, 1));

        titleUI.SetActive(false);

        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared) yield return null;

        videoPlayer.Play();
        demoText.gameObject.SetActive(true);

        isPlayingDemo = true;
        isFading = false;

        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeOutAndReturnTitle()
    {
        isFading = true;
        fadePanel.gameObject.SetActive(true);

        yield return StartCoroutine(UpdateFade(0, 1));

        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        demoText.gameObject.SetActive(false);

        titleUI.SetActive(true);
        isPlayingDemo = false;
        timer = 0f;

        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(UpdateFade(1, 0));
        fadePanel.gameObject.SetActive(false);
        isFading = false;
    }

    IEnumerator UpdateFade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        fadePanel.color = color;
    }

    void OnVideoEnd(VideoPlayer vp) { }
}