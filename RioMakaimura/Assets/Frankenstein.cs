using UnityEngine;
using System.Collections;

public enum FrankenState
{
  Idle,                 //待機
  Move,                 //移動
  Defend,               //防御（弾破壊）
  Attack_main,          //攻撃（基本攻撃）
  Attack_Stan,          //スタン攻撃
  Hit,                  //ダメージ
  Dead                  //死亡
}
public class Frankenstein : MonoBehaviour
{


    public FrankenState currentState = FrankenState.Idle;
    public Transform playerTransform;

    [Header("攻撃設定")]
    public float detectRange = 5f;          // 近接攻撃の検知距離
    public float attackCooldown = 2.0f;     // 攻撃間のクールダウン
    private float cooldownTimer = 0f;
    public Collider2D hammerHitbox;


    [Header("防御設定")]
    public float defenseDuration = 0.7f;     // 防御アニメーション時間
    public LayerMask playerBulletLayer;      // プレイヤーの弾のLayerMask
    public float defenseRadius = 3.0f;       // 弾を破壊する検知エリアの半径

    [Header("スタン攻撃設定")]
    public float jumpForce = 15f;
    public float slamHardnessTime = 0.5f;    // 叩きつけ後の硬直時間
    public GameObject stunWavePrefab;        // スタン衝撃波のプレハブ

    [Header("ハンマー制御")]
    public Transform hammerVisual;       // ハンマーの見た目のTransform
    public Vector3 swingStartRotation = new Vector3(0, 0, 90); // 振り上げ開始時の角度
    public Vector3 swingEndRotation = new Vector3(0, 0, -45);  // 振り下ろし終了時の角度
    public float swingDuration = 0.15f;  // 振り下ろし動作の時間

    [Header("接地判定設定")]
    public float groundCheckDistance = 0.1f; // 足元からレイを飛ばす
    public LayerMask groundLayer;            // 地面判定用レイヤー


    private Rigidbody2D rb;
    private Collider2D enemyCollider; // 自身のコライダーを格納
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        rb = GetComponent<Rigidbody2D>();
        //自身のコライダーを取得
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        //プレイヤーのTransformを取得
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            // プレイヤーが見つからない場合は警告を出し、攻撃ロジックをスキップできるようにする
            Debug.LogError("Playerタグのオブジェクトが見つかりません。攻撃ロジックが機能しません。", this);
        }

        //ハンマーを非表示
        if (hammerVisual != null)
        {
            hammerVisual.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // クールダウン処理
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        switch (currentState)
        {
            case FrankenState.Idle:
            case FrankenState.Move:
                CheckAttacks();
                break;
                // 攻撃中は他の処理をブロック
        }
    }


    // 攻撃選択ロジック
    void CheckAttacks()
    {
        if (cooldownTimer > 0) return;

        // 1. 弾の接近を優先的にチェック (防御ロジック)
        if (Physics2D.OverlapCircle(transform.position, defenseRadius, playerBulletLayer))
        {
            StartCoroutine(Defend());
            return;
        }

        // 2. プレイヤーが近接距離にいるかチェック (ハンマー振り下ろし)
        if (Vector2.Distance(transform.position, playerTransform.position) < detectRange)
        {
            // 一定の確率で近接攻撃とスタン攻撃を使い分ける
            if (Random.Range(0f, 1f) < 0.6f) // 60%の確率で近接攻撃
            {
                StartCoroutine(Attack_main());
            }
            else // 40%の確率でスタン攻撃
            {
                StartCoroutine(Attack_Stan());
            }
        }
        else
        {
            currentState = FrankenState.Move; // 遠い場合は移動など
        }
    }



    // 弾が接近した際の防御行動
    IEnumerator Defend()
    {
        currentState = FrankenState.Defend;
        // anim.SetTrigger("HammerBlock"); 

        // 予備動作時間（ハンマーを振り上げる時間）
        yield return new WaitForSeconds(0.2f);

        // --- 弾の破壊処理 ---
        // 検知エリア内のすべての弾を破壊
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, defenseRadius, playerBulletLayer);

        foreach (Collider2D col in hitColliders)
        {
            // 弾であることを確認して破壊
            if (col.CompareTag("PlayerBullet") || col.gameObject.GetComponent<bullet>() != null)
            {
                Debug.Log("破壊");
                Destroy(col.gameObject);
            }
        }

        // 防御後の硬直
        yield return new WaitForSeconds(defenseDuration);

        cooldownTimer = attackCooldown;
        currentState = FrankenState.Idle;
    }

    // プレイヤーへのハンマー振り下ろし攻撃
    IEnumerator Attack_main()
    {
        currentState = FrankenState.Attack_main;
        // anim.SetTrigger("HammerSwing");

        Debug.Log("通常攻撃");

        // 予備動作中に見た目をプレイヤーに向かせる（オプション）
        if (playerTransform != null)
        {
            // 振り下ろす前にプレイヤーの方向を向く
            bool lookLeft = playerTransform.position.x < transform.position.x;
            // SRが存在するとして、見た目を反転させる
            // if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().flipX = lookLeft; 
        }

        Debug.Log("通常攻撃: 予備動作開始");



        //ハンマーを表示する
        if (hammerVisual != null)
        {
            hammerVisual.gameObject.SetActive(true);
            hammerVisual.localRotation = Quaternion.Euler(swingStartRotation); // 振り上げ開始位置
        }

        // 1. 振り下ろし動作の予備動作 (チャージ)
        // 予備動作時間: 0.4秒
        yield return new WaitForSeconds(0.4f);


        // --- ★攻撃判定の開始★ ---
        // 振り下ろし開始時刻を記録
        float startTime = Time.time;

        // 振り下ろしが完了するまでループ
        while (Time.time < startTime + swingDuration)
        {
            // 進行度 (0.0 から 1.0)
            float t = (Time.time - startTime) / swingDuration;

            // 開始角度から終了角度へ補間して回転させる
            Quaternion targetRotation = Quaternion.Euler(swingEndRotation);
            Quaternion currentRotation = Quaternion.Euler(swingStartRotation);

            hammerVisual.localRotation = Quaternion.Slerp(currentRotation, targetRotation, t);

            // 攻撃判定を有効化 (動作中に当たり判定ON)
            if (t >= 0.2f && hammerHitbox != null && !hammerHitbox.enabled)
            {
                Debug.Log("通常攻撃: ヒットボックスON");
                hammerHitbox.enabled = true;
            }

            yield return null;
        }

        // 動作完了後、角度を正確に終了位置に設定
        if (hammerVisual != null)
        {
            hammerVisual.localRotation = Quaternion.Euler(swingEndRotation);
        }

        // --- ★攻撃判定の終了★ ---

        if (hammerHitbox != null)
        {
            Debug.Log("通常攻撃: ヒットボックスOFF");
            hammerHitbox.enabled = false; // 攻撃判定を無効化
        }

        // 攻撃後の硬直
        yield return new WaitForSeconds(0.7f);

        cooldownTimer = attackCooldown;
        currentState = FrankenState.Idle;
    }


    // ジャンプして叩きつけ、スタン波を発生させる攻撃
    IEnumerator Attack_Stan()
    {
        currentState = FrankenState.Attack_Stan;
        // anim.SetTrigger("JumpSlam");

        // 1. ジャンプ
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // 一旦速度リセット
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 2. 着地を待つ
        // 実際のゲームでは IsGrounded() などのカスタム判定メソッドを使います
        yield return new WaitUntil(() => IsGrounded());

        // 3. 地面叩きつけ (Stun Waveの生成)
        // anim.SetTrigger("GroundSlam");

        // 衝撃波を生成（ケルベロスと同様、衝撃波プレハブに当たり判定ロジックを持たせる）
        if (stunWavePrefab != null)
        {
            Debug.Log("スタン開始");

            // フランケンの足元（またはコライダーの下端）に生成
            Vector3 spawnPos = transform.position;
            if (GetComponent<Collider2D>() != null)
            {
                spawnPos.y = GetComponent<Collider2D>().bounds.min.y;
            }
            Instantiate(stunWavePrefab, spawnPos, Quaternion.identity);
        }

        // 4. 叩きつけ後の硬直時間
        yield return new WaitForSeconds(slamHardnessTime);

        Debug.Log("スタン終わり");

        cooldownTimer = attackCooldown;
        currentState = FrankenState.Idle;
    }


    // --- ヘルパーメソッド ---

    /// <summary>
    /// ボスが地面に接地しているかを判定します。
    /// </summary>
    /// <returns>接地していれば true</returns>
    bool IsGrounded()
    {
        if (enemyCollider == null)
        {
            // コライダーがない場合は常に非接地とみなす
            return false;
        }

        // コライダーの下端の中心からレイを飛ばして地面をチェック
        Vector2 origin = enemyCollider.bounds.center;
        origin.y = enemyCollider.bounds.min.y; // コライダーの真下を開始点とする

        // レイの長さをコライダーのサイズ + チェック距離とする
        float rayLength = groundCheckDistance;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);

        // デバッグ表示 (確認用にSceneビューにラインを表示)
        // Debug.DrawRay(origin, Vector2.down * rayLength, hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }






    /// <summary>
    /// 攻撃シーケンスを終了し、クールダウンを開始します。
    /// </summary>
    void EndAttack()
    {
        // 攻撃終了時に必ず速度をリセット
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        cooldownTimer = attackCooldown;
        currentState = FrankenState.Idle;
    }


}
