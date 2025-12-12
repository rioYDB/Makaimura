// StageManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageManager
{
    // PlayerPrefsに保存するキー
    private const string CurrentStageKey = "CurrentStageName";
    private const string ClearedStageKeyPrefix = "StageCleared_";

    // 現在のステージ名を保存する（ゲームオーバー/ゴール時に使用）
    public static void SetCurrentStage(string stageName)
    {
        PlayerPrefs.SetString(CurrentStageKey, stageName);
        PlayerPrefs.Save();
        Debug.Log("Current Stage set to: " + stageName);
    }

    // 現在のステージ名を取得する（タイトルやロード画面で使用）
    public static string GetCurrentStage()
    {
        // 取得できなければ初期ステージ名を返すなどのフォールバックが必要
        return PlayerPrefs.GetString(CurrentStageKey, "ayabe4"); // 初期ステージ名を指定
    }

    // ステージクリア情報を保存する
    public static void MarkStageAsCleared(string stageName)
    {
        // ステージ名をキーにしてクリア済みフラグ(1)を保存
        PlayerPrefs.SetInt(ClearedStageKeyPrefix + stageName, 1);
        PlayerPrefs.Save();
        Debug.Log("Stage Cleared: " + stageName);
    }

    // ステージがクリア済みか判定する
    public static bool IsStageCleared(string stageName)
    {
        // 0(未クリア)でなければクリア済みと判定
        return PlayerPrefs.GetInt(ClearedStageKeyPrefix + stageName, 0) == 1;
    }

    // デバッグ用: PlayerPrefsのデータを全てリセット
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All game progress has been reset.");
    }
}
