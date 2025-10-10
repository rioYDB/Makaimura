using UnityEngine;
using System.Collections;

public enum CerberusState
{
    Idle,       // 待機
    Move,       // 移動
    Attack1,    // 攻撃パターン1 (三位方向への火炎ブレス)
    Attack2,    // 攻撃パターン2 (地中に消えてプレイヤーの下から飛び出す)
    Attack3,    // 攻撃パターン3 (突進)
    Hurt,       // 被弾
    Dead        // 死亡
}


public class Cerberus_Controller : MonoBehaviour
{
    public CerberusState currentState; // 現在のボスの状態
    public Transform playerTransform; // プレイヤーのTransform (ターゲット)

    public float attackDetectionRange = 8f; // プレイヤーを見つける距離 (画面外でもこの距離なら攻撃開始)

    private float attackCoolDownTimer = 0f;
    public float attackCoolDownTime = 3f; // 攻撃間のクールダウン時間

    // 攻撃1用の炎のプレハブと頭のTransform
    public GameObject fireBreathPrefab; // 炎のプレハブ
    public float fireSpeed = 10f; // 炎が飛ぶ速度
    public Transform head1SpawnPoint; // 頭1の炎生成位置
    public Transform head2SpawnPoint; // 頭2の炎生成位置
    public Transform head3SpawnPoint; // 頭3の炎生成位置
    public float fireSpreadAngle = 30f; // 炎の広がり角度

    // 地中に消える攻撃用の変数
    public float undergroundYOffset = -5.0f; // 地中に潜る深さ
    public float premonitionDelay = 2.0f; // プレイヤーの下から飛び出すまでの待機時間 (前兆)
    public float emergeSpeed = 10f; // 飛び出す速度
    public GameObject premonitionEffectPrefab; // 飛び出す前の前兆エフェクト（例：土煙のパーティクル）

    //移動処理
    public float moveSpeed = 2f; // 移動速度
    public float moveRange = 3f; // 左右に動く範囲 (初期位置から左右にこの距離)
    private Vector2 initialPosition; // ケルベロスの初期位置
    private int moveDirection = 1; // 1:右, -1:左

    // 地中から飛び出す際の、Y軸の最終的な調整値
    public float emergeTopOffset = 1.0f; //。この値を増やすとより上に出ます。

    // ケルベロスのコンポーネント
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D enemyCollider;
    private Animator animator;

    //レイヤー
    public string GroundTileMapName = "TileMap";

    public int TileMapLayer;

    void Start()
    {
        currentState = CerberusState.Move; // 最初は移動状態から始める
        initialPosition = transform.position; // 初期位置を記録

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // プレイヤーのTransformを取得 (タグが"Player"のオブジェクトを探す)
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        //Tilemapのレイヤー番号を取得
        TileMapLayer = LayerMask.NameToLayer(GroundTileMapName);
        if (TileMapLayer == -1)
        {
            Debug.LogError("指定されたレイヤー名 '" + GroundTileMapName + "' が見つかりません。Project Settingsを確認してください。");
        }
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
                // 攻撃中は他の処理をブロックし、攻撃終了を待つ
                break;
            case CerberusState.Attack3:
                // 攻撃中は他の処理をブロックし、攻撃終了を待つ
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
        if (attackCoolDownTimer <= 0)
        {
            ChooseNextAttack();
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
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            moveDirection *= -1;
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
    }

    void HandleDeadState()
    {
        // 死亡アニメーション、ゲームオーバー処理など
    }

    // --- 攻撃の選択ロジック ---

    void ChooseNextAttack()
    {
        if (IsPlayerVisibleOrInRange())
        {
            int randomAttack = Random.Range(0, 3);

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
        else
        {
            currentState = CerberusState.Move;
        }
    }

    // プレイヤーが画面内にいるか、または一定距離内にいるかを判定するメソッド
    bool IsPlayerVisibleOrInRange()
    {
        if (playerTransform == null || Camera.main == null)
        {
            Debug.LogWarning("Player Transform または Main Camera が設定されていません。");
            return false;
        }

        Vector3 cerberusViewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isCerberusVisible = cerberusViewportPoint.x >= 0 && cerberusViewportPoint.x <= 1 &&
                                 cerberusViewportPoint.y >= 0 && cerberusViewportPoint.y <= 1 &&
                                 cerberusViewportPoint.z > 0;

        if (!isCerberusVisible)
        {
            return false;
        }
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(playerTransform.position);
        bool isVisible = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                         viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                         viewportPoint.z > 0;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool isInRange = distanceToPlayer <= attackDetectionRange;

        return isVisible || isInRange;
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

    // 攻撃1: 三方向に炎を飛ばす
    IEnumerator Attack1Coroutine()
    {
        // animator.SetTrigger("Attack1");

        yield return new WaitForSeconds(0.5f);

        if (fireBreathPrefab != null)
        {
            Vector2 playerDirection = (playerTransform.position - transform.position).normalized;

            // 正面
            if (head1SpawnPoint != null)
            {
                GameObject fire1 = Instantiate(fireBreathPrefab, head1SpawnPoint.position, Quaternion.identity);
                Cerberus_bullet bullet1 = fire1.GetComponent<Cerberus_bullet>();
                if (bullet1 != null) bullet1.Initialize(playerDirection);
            }

            // 左斜め (プレイヤー方向から-fireSpreadAngle分ずらす)
            if (head2SpawnPoint != null)
            {
                Quaternion leftRotation = Quaternion.Euler(0, 0, fireSpreadAngle);
                Vector2 leftDirection = leftRotation * playerDirection;
                GameObject fire2 = Instantiate(fireBreathPrefab, head2SpawnPoint.position, Quaternion.identity);
                Cerberus_bullet bullet2 = fire2.GetComponent<Cerberus_bullet>();
                if (bullet2 != null) bullet2.Initialize(leftDirection);
            }

            // 右斜め (プレイヤー方向から+fireSpreadAngle分ずらす)
            if (head3SpawnPoint != null)
            {
                Quaternion rightRotation = Quaternion.Euler(0, 0, -fireSpreadAngle);
                Vector2 rightDirection = rightRotation * playerDirection;
                GameObject fire3 = Instantiate(fireBreathPrefab, head3SpawnPoint.position, Quaternion.identity);
                Cerberus_bullet bullet3 = fire3.GetComponent<Cerberus_bullet>();
                if (bullet3 != null) bullet3.Initialize(rightDirection);
            }
        }

        yield return new WaitForSeconds(1.0f); // 炎が持続する時間
        Debug.Log("攻撃1終了");
        EndAttack();
    }

    // 攻撃2: 地中攻撃
    IEnumerator Attack2Coroutine()
    {
        Debug.Log("ケルベロス: 攻撃2を開始！ (地中に消えてプレイヤーの下から飛び出す)");

        // 1. 物理影響とコライダーを無効化 (地中潜行と移動の開始)
        // animator.SetTrigger("Submerge");
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic; // 物理影響無効
        if (rb != null) rb.linearVelocity = Vector2.zero; // 速度をリセット
        if (enemyCollider != null) enemyCollider.enabled = false; // ★コライダーを無効化★
        if (sr != null) sr.enabled = false; // 見た目を非表示

        // ★★★ 修正箇所: Tilemapとの衝突を無視する ★★★
        if (TileMapLayer != -1)
        {
            // ケルベロスのレイヤーとTilemapのレイヤー間の衝突を無視
            Physics2D.IgnoreLayerCollision(gameObject.layer, TileMapLayer, true);
        }

        yield return new WaitForSeconds(1.0f); // 消えるアニメーションの時間（例）

        // 2. プレイヤーの現在の位置を記憶と地面Y座標の計算 (ロジック変更なし)
        Vector2 playerCurrentPos = playerTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(playerCurrentPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        float groundY = playerCurrentPos.y;
        if (hit.collider != null)
        {
            groundY = hit.point.y;
        }

        // 地面から少し下に潜った位置にワープ (ロジック変更なし)
        Vector2 startUndergroundPos = new Vector2(playerCurrentPos.x, groundY + undergroundYOffset);
        transform.position = startUndergroundPos;

        // 3. 飛び出すまでの前兆時間 (ロジック変更なし)
        if (premonitionEffectPrefab != null)
        {
            GameObject premonitionEffect = Instantiate(premonitionEffectPrefab, new Vector2(playerCurrentPos.x, groundY), Quaternion.identity);
            Destroy(premonitionEffect, premonitionDelay);
        }
        yield return new WaitForSeconds(premonitionDelay);

        // 4. 飛び出す処理
        if (sr != null) sr.enabled = true; // 見た目を有効化

        // 地面の上部Y座標を計算
        // Colliderの中心が地面のY座標に来るように計算
        float halfHeight = enemyCollider.bounds.size.y / 2f;

        
        // 飛び出しの目標位置に、さらに手動のオフセットを加える
        Vector2 emergeCenterPos = new Vector2(
            transform.position.x,
            groundY + halfHeight + emergeTopOffset // ★ここで持ち上げます★
        );
        


        // 物理影響を無視して、地中から地面の上までスムーズに移動
        while (transform.position.y < emergeCenterPos.y)
        {
            transform.position += Vector3.up * emergeSpeed * Time.deltaTime;
            yield return null; // 次のフレームまで待機
        }

        // 飛び出し終点に位置を固定
        transform.position = new Vector3(transform.position.x, emergeCenterPos.y, transform.position.z);



        // 5. 物理影響を再開
        if (enemyCollider != null) enemyCollider.enabled = true;

        // ★★★ 修正箇所: Tilemapとの衝突無視を解除 ★★★
        if (TileMapLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, TileMapLayer, false);
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // 物理影響を有効化
        }

        yield return new WaitForSeconds(0.5f); // 飛び出し後の硬直時間

        Debug.Log("攻撃2終了");
        EndAttack();
    }

    // 攻撃3: 突進
    IEnumerator Attack3Coroutine()
    {
        Debug.Log("ケルベロス: 攻撃3を開始！ (突進)");

        // ... (突進ロジックは変更なし) ...

        Debug.Log("攻撃3終了");
        EndAttack();
        yield return null; // コルーチンの終端
    }

    void EndAttack()
    {
        currentState = CerberusState.Idle;
        attackCoolDownTimer = attackCoolDownTime;
    }
}