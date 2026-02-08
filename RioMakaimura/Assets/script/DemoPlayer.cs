//using UnityEngine;
//using UnityEngine.Video;
//using UnityEngine.SceneManagement; // SceneManagerは使わないが念のため
//using UnityEngine.UI; // Imageコンポーネントを操作するために必要
//using TMPro; // TextMeshProを使うために必要
//using System.Collections; // コルーチンのために必要


//public partial class DemoPlayer : MonoBehaviour
//{
//    [Header("設定")]
//    public float waitTime = 30f;
//    public VideoPlayer videoPlayer;
//    public GameObject titleUI;
//    public Image fadePanel;
//    public TextMeshProUGUI demoText;

//    [Header("フェード設定")]
//    public float fadeDuration = 1.0f;
//    public float endFadeStartBefore = 3.0f; // 終了何秒前からフェードを始めるか

//    private float timer = 0f;
//    private bool isPlayingDemo = false;
//    private bool isFading = false;
//    private bool isEndingHandled = false; // 終了処理が二重に走らないためのフラグ

//    void Start()
//    {
//        videoPlayer.gameObject.SetActive(false);
//        fadePanel.gameObject.SetActive(false);
//        demoText.gameObject.SetActive(false);

//        Color fadeColor = fadePanel.color;
//        fadeColor.a = 0;
//        fadePanel.color = fadeColor;
//    }

//    void Update()
//    {
//        if (isFading) return;

//        if (isPlayingDemo)
//        {
//            // 1. ボタン入力があったら即タイトルへ戻るフェード開始
//            if (Input.anyKeyDown)
//            {
//                StopDemo();
//                return;
//            }

//            // 2. 映像終了の3秒前になったら自動で戻るフェード開始
//            double timeLeft = videoPlayer.length - videoPlayer.time;
//            if (!isEndingHandled && timeLeft <= endFadeStartBefore)
//            {
//                isEndingHandled = true; // フェード開始
//                StopDemo();
//            }
//            return;
//        }

//        // タイトル画面での放置タイマー
//        if (Input.anyKey) timer = 0f;
//        else timer += Time.deltaTime;

//        if (timer >= waitTime) StartDemo();
//    }

//    void StartDemo()
//    {
//        isEndingHandled = false;
//        StartCoroutine(FadeOutAndPlayDemo());
//    }

//    void StopDemo()
//    {
//        // 映像を止める前にフェードアウトを開始する
//        StartCoroutine(FadeOutAndReturnTitle());
//    }

//    IEnumerator FadeOutAndPlayDemo()
//    {
//        isFading = true;
//        fadePanel.gameObject.SetActive(true);

//        // 画面を黒くする
//        yield return StartCoroutine(UpdateFade(0, 1));

//        titleUI.SetActive(false);
//        videoPlayer.gameObject.SetActive(true);
//        videoPlayer.Play();
//        demoText.gameObject.SetActive(true);

//        isPlayingDemo = true;
//        isFading = false;

//        // 黒から透明に戻す（映像が見えるようになる）
//        yield return StartCoroutine(FadeIn());
//    }

//    IEnumerator FadeOutAndReturnTitle()
//    {
//        isFading = true;
//        fadePanel.gameObject.SetActive(true);

//        // ★先に画面を黒くする（映像やテキストは出たままでOK）
//        yield return StartCoroutine(UpdateFade(0, 1));

//        // 画面が真っ黒になったタイミングで映像とテキストを消す
//        videoPlayer.Stop();
//        videoPlayer.gameObject.SetActive(false);
//        demoText.gameObject.SetActive(false);

//        titleUI.SetActive(true);
//        isPlayingDemo = false;
//        timer = 0f;

//        // 透明に戻す（タイトルUIが見えるようになる）
//        yield return StartCoroutine(FadeIn());
//    }

//    IEnumerator FadeIn()
//    {
//        yield return StartCoroutine(UpdateFade(1, 0));
//        fadePanel.gameObject.SetActive(false);
//        isFading = false;
//    }

//    // フェード計算の共通化
//    IEnumerator UpdateFade(float startAlpha, float targetAlpha)
//    {
//        float elapsedTime = 0f;
//        Color color = fadePanel.color;

//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
//            fadePanel.color = color;
//            yield return null;
//        }
//        color.a = targetAlpha;
//        fadePanel.color = color;
//    }

//    // VideoPlayerのイベントからは直接StopDemoを呼ばず、Updateで監視するように変更
//    void OnVideoEnd(VideoPlayer vp) { /* Updateで制御するため空にするか削除 */ }
//}
