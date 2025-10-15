using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class switchcontroller : MonoBehaviour
{
    
    public doorcontroller targetDoor; // 開きたい扉のDoorControllerスクリプトの参照をドラッグ＆ドロップ

    public string playerTag = "Player"; // プレイヤーのタグ
    public string playerAttackTag = "Spear"; // プレイヤーの攻撃のタグ (例: 槍なら"Spear")

    public Color pressedColor = Color.gray; // スイッチが押された時の色 (デフォルトは灰色)
    private Color defaultColor; // スイッチの初期色を保存

    private SpriteRenderer spriteRenderer; // スイッチのSpriteRenderer

    private bool isActivated = false; // スイッチが既に起動しているか

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color; // 初期色を保存
        }

        if (targetDoor == null)
        {
            Debug.LogWarning("SwitchTrigger: 開く扉が設定されていません！", this);
        }
    }

    // プレイヤーや攻撃がスイッチに触れたことを検出する
    private void OnTriggerEnter2D(Collider2D other)
    {
        // スイッチが既に起動している場合は何もしない
        if (isActivated)
        {
            return;
        }

        // 接触したのがプレイヤー、またはプレイヤーの攻撃かチェック
        bool isPlayer = other.CompareTag(playerTag);
        bool isPlayerAttack = other.CompareTag(playerAttackTag);

        if (isPlayer || isPlayerAttack)
        {
            Debug.Log("スイッチが押された！");
            ActivateSwitch();
        }
    }

    // スイッチを起動するメソッド
    void ActivateSwitch()
    {
        isActivated = true; // スイッチを起動済みに設定

        //スイッチの見た目を色に変更する (オプション)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = pressedColor; // 押された時の色に変更
        }

        // 扉を開く処理
        if (targetDoor != null)
        {
            targetDoor.OpenDoor(); // DoorControllerのOpenDoorメソッドを呼び出す
        }
        else
        {
            Debug.LogWarning("SwitchTrigger: 開く扉（DoorController）が見つかりません。", this);
        }

        // スイッチを一度触れるだけで良いので、このスイッチ自身はコライダーを無効化 (オプション)
        // GetComponent<Collider2D>().enabled = false;
        // this.enabled = false; // スクリプト全体を無効化
    }

    // スイッチの状態をリセットしたい場合（必要であれば追加）
    public void ResetSwitch()
    {
        isActivated = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor; // 色を初期状態に戻す
        }
        // GetComponent<Collider2D>().enabled = true; // コライダーを有効化
    }
}

