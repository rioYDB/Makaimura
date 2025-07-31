using UnityEngine;
using System.Collections;



public enum CerberusState
{
    Idle,       // 待機
    Move,       // 移動
    Attack1,    // 攻撃パターン1
    Attack2,    // 攻撃パターン2
    Attack3,    // 攻撃パターン3
    Hurt,       // 被弾
    Dead        // 死亡
}


public class Cerberus_Controller : MonoBehaviour
{
    public CerberusState currentState; // 現在のボスの状態
    public Transform playerTransform; // プレイヤーのTransform (ターゲット)

    private float attackCoolDownTimer = 0f;
    public float attackCoolDownTime = 3f; // 攻撃間のクールダウン時間

    public GameObject fireBreathPrefab; // 炎のプレハブ
    public Transform head1SpawnPoint; // 頭1の炎生成位置
    public Transform head2SpawnPoint; // 頭2の炎生成位置
    public Transform head3SpawnPoint; // 頭3の炎生成位置


    //移動処理
    public float moveSpeed = 2f; // 移動速度
    public float moveRange = 3f; // 左右に動く範囲 (初期位置から左右にこの距離)
    private Vector2 initialPosition; // ケルベロスの初期位置
    private int moveDirection = 1; // 1:右, -1:左

    void Start()
    {
        currentState = CerberusState.Move; // 最初は移動状態から始める
        initialPosition = transform.position; // 初期位置を記録
    }

    void Update()
    {
        // クールダウンタイマーを更新
        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        // 現在の状態に応じて処理を分岐
        switch (currentState)
        {
            case CerberusState.Idle:
                HandleIdleState();
                break;
            case CerberusState.Move:
                HandleMoveState();
                break;
            case CerberusState.Attack1:
                // 攻撃中は他の処理をブロックし、攻撃終了を待つ
                break;
            case CerberusState.Attack2:
                break;
            case CerberusState.Attack3:
                break;
            case CerberusState.Hurt:
                HandleHurtState();
                break;
            case CerberusState.Dead:
                HandleDeadState();
                break;
        }
    }

    // --- 各状態ごとの処理 ---



    void HandleIdleState()
    {
        // クールダウンが終わったら次の攻撃を選択
        if (attackCoolDownTimer <= 0)
        {
            ChooseNextAttack();
        }
        else
        {
            // 攻撃クールダウン中は移動状態に移行
            currentState = CerberusState.Move;
        }
    }

    void HandleMoveState()
    {
        // 左右移動の目標位置を計算
        float targetX = initialPosition.x + moveRange * moveDirection;
        Vector2 targetPosition = new Vector2(targetX, transform.position.y);

        // 現在位置から目標位置へ移動
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 目標位置に到達したら方向を反転
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f) // ほぼ到達したら
        {
            moveDirection *= -1; // 方向を反転
            // キャラクターの向きを反転させる場合はここで
            // transform.localScale = new Vector3(moveDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // クールダウンが終わったら攻撃に移行
        if (attackCoolDownTimer <= 0)
        {
            ChooseNextAttack();
        }
    }
    void HandleHurtState()
    {
        // 被弾アニメーションの再生、ノックバックなど
        // アニメーション終了後にIdleに戻る
    }

    void HandleDeadState()
    {
        // 死亡アニメーション、ゲームオーバー処理など
    }

    // --- 攻撃の選択ロジック ---

    void ChooseNextAttack()
    {
        // ランダム、またはHPに応じた攻撃選択ロジック
        int randomAttack = Random.Range(0, 3); // 0, 1, 2のいずれか

        switch (randomAttack)
        {
            case 0:
                StartAttack1();
                break;
            case 1:
                StartAttack2();
                break;
            case 2:
                StartAttack3();
                break;
        }
    }

    // --- 各攻撃パターンの開始メソッド ---

    void StartAttack1()
    {
        currentState = CerberusState.Attack1;
        Debug.Log("ケルベロス: 攻撃1を開始！ (三位方向への火炎ブレス)");
        StartCoroutine(Attack1Coroutine()); // コルーチンで攻撃処理を開始
    }

    void StartAttack2()
    {
        currentState = CerberusState.Attack2;
        Debug.Log("ケルベロス: 攻撃2を開始！ (地中に消えてプレイヤーの下から飛び出す)");
        StartCoroutine(Attack2Coroutine());
    }

    void StartAttack3()
    {
        currentState = CerberusState.Attack3;
        Debug.Log("ケルベロス: 攻撃3を開始！ (突進)");
        StartCoroutine(Attack3Coroutine());
    }

    // --- 各攻撃パターンの具体的な実装 (コルーチン) ---
    // ここに各攻撃のロジックを記述します。

    IEnumerator Attack1Coroutine()
    {
        // 例: アニメーション再生（炎を吐くアニメーション）
        // animator.SetTrigger("Attack1");

        yield return new WaitForSeconds(0.5f); // アニメーション開始からブレスが出るまでの待機

        // 炎の生成と発射
        // 各頭のTransformを基準に、角度を調整して炎を生成・発射する
        // 例: プレイヤーの方向を向かせたり、固定の3方向（前方、左斜め、右斜め）にしたり
        // ここでは仮に固定の3方向とします

        // 頭1から発射 (例: 正面)
        if (fireBreathPrefab != null && head1SpawnPoint != null)
        {
            GameObject fire1 = Instantiate(fireBreathPrefab, head1SpawnPoint.position, Quaternion.identity);
            // fire1 に付いているスクリプトで移動ロジックを制御するか、Rigidbody2Dで力を加える
            // fire1.GetComponent<FireBreathScript>().Initialize(Vector2.right); // 例
            // fire1.GetComponent<Rigidbody2D>().AddForce(Vector2.right * fireSpeed); // 例
        }

        // 頭2から発射 (例: 左斜め)
        if (fireBreathPrefab != null && head2SpawnPoint != null)
        {
            GameObject fire2 = Instantiate(fireBreathPrefab, head2SpawnPoint.position, Quaternion.Euler(0, 0, 45)); // 45度回転
            // fire2.GetComponent<FireBreathScript>().Initialize(Quaternion.Euler(0, 0, 45) * Vector2.right); // 例
        }

        // 頭3から発射 (例: 右斜め)
        if (fireBreathPrefab != null && head3SpawnPoint != null)
        {
            GameObject fire3 = Instantiate(fireBreathPrefab, head3SpawnPoint.position, Quaternion.Euler(0, 0, -45)); // -45度回転
            // fire3.GetComponent<FireBreathScript>().Initialize(Quaternion.Euler(0, 0, -45) * Vector2.right); // 例
        }

        // 炎のエフェクトが続く時間待機
        yield return new WaitForSeconds(2.0f); // 炎が持続する時間

        Debug.Log("攻撃1終了");
        EndAttack();
    }

    IEnumerator Attack2Coroutine()
    {
        Debug.Log("ケルベロス: 攻撃2を開始！ (地中に消えてプレイヤーの下から飛び出す)");

        // 1. 地中に消えるアニメーション/処理
        // animator.SetTrigger("Disappear");
        // GetComponent<SpriteRenderer>().enabled = false; // スプライトを非表示にする例
        // Rigidbody2Dがある場合、この間物理演算を無効にすると良い
        // GetComponent<Collider2D>().enabled = false; // コライダーも無効にする
        yield return new WaitForSeconds(1.0f); // 消えるアニメーションの時間

        // 2. プレイヤーの現在の位置を記憶（最終的な目標地点）
        Vector2 playerCurrentPos = playerTransform.position;

        // 3. 地中から出現するための開始位置を計算
        // プレイヤーの少し下（潜る深さ）にワープする地点を設定
        // ここが飛び出しの「開始点」になります
        float deepUndergroundYOffset = -5.0f; // 例: より深く潜るためのYオフセット
        Vector2 startUndergroundPos = new Vector2(playerCurrentPos.x + Random.Range(-0.5f, 0.5f), playerCurrentPos.y + deepUndergroundYOffset);
        transform.position = startUndergroundPos; // ケルベロスを地中の開始位置にワープさせる

        // 4. 再び出現するアニメーション/処理
        // GetComponent<SpriteRenderer>().enabled = true; // スプライトを再表示する例
        // animator.SetTrigger("Appear"); // 飛び出すアニメーションを再生

        // 5. 地中から最終的な出現位置まで移動する
        float emergeDuration = 0.3f; // 地上に出るまでの時間
        Vector2 emergeTargetPos = new Vector2(playerCurrentPos.x, playerCurrentPos.y); // 最終的に出現する地上での位置
        float timer = 0f;

        while (timer < emergeDuration)
        {
            transform.position = Vector2.Lerp(startUndergroundPos, emergeTargetPos, timer / emergeDuration);
            timer += Time.deltaTime;
            yield return null; // 1フレーム待機
        }
        transform.position = emergeTargetPos; // 確実に目標位置に到達させる

        // 6. 攻撃判定の発生 (飛び出した瞬間にダメージを与える)
        // GetComponent<Collider2D>().enabled = true; // コライダーを再有効化
        // CallAttackHitbox(); // 攻撃判定を有効にするメソッド
        // yield return new WaitForSeconds(0.2f); // 攻撃判定が持続する時間
        // DisableAttackHitbox(); // 攻撃判定を無効にするメソッド


        Debug.Log("攻撃2終了");
        EndAttack();
    }
    IEnumerator Attack3Coroutine()
    {
        Debug.Log("ケルベロス: 攻撃3を開始！ (突進)");

        // 例: 突進開始アニメーション
        // animator.SetTrigger("Dash");

        // プレイヤーに向く
        if (playerTransform.position.x < transform.position.x)
        {
            // プレイヤーが左にいる場合、左を向く
            // transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // プレイヤーが右にいる場合、右を向く
            // transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        //プレイヤーへの突進
        float dashDuration = 0.5f;
        Vector2 startPos = transform.position;
        Vector2 targetPos = playerTransform.position; // プレイヤーの現在位置へ突進

        // 突進距離を制限したい場合、TargetPosを調整することもできます
         float maxDashDistance = 5f;
         Vector2 direction = (targetPos - startPos).normalized;
         targetPos = startPos + direction * maxDashDistance; // 最大5mまで突進

        float timer = 0f;
        while (timer < dashDuration)
        {
            // 突進中は攻撃判定を有効にする
            // CallAttackHitbox();

            transform.position = Vector2.Lerp(startPos, targetPos, timer / dashDuration);
            timer += Time.deltaTime;
            yield return null; // 1フレーム待機
        }
        transform.position = targetPos; // 確実に到達

        // 突進終了後に攻撃判定を無効にする
        // DisableAttackHitbox();

        // 突進後の硬直時間など
        // yield return new WaitForSeconds(0.5f); // 例

        Debug.Log("攻撃3終了");
        EndAttack();
    }

    // --- 攻撃終了処理 ---

    void EndAttack()
    {
        currentState = CerberusState.Idle; // 攻撃終了後、待機状態に戻る
        attackCoolDownTimer = attackCoolDownTime; // クールダウン開始
    }

    // HP管理や被弾ロジックは別途実装
    public void TakeDamage(int damage)
    {
        // HP減少処理
        // currentState = BossState.Hurt; // 被弾状態へ移行するロジックを追加
    }
}
