using UnityEngine;
using System.Collections;

public enum FrankenState
{
    Idle,                 //待機
    Move,                 //移動
    Defend,               //防御
    Attack_main,          //攻撃
    Attack_Stan,          //スタン攻撃
    Hit,                  //ダメージ
    Dead                  //死亡
}

public class Frankenstein : MonoBehaviour
{
    public FrankenState currentState = FrankenState.Idle;
    public Transform playerTransform;

    [Header("移動設定")]
    public float moveSpeed = 2.0f;       // ★追加：移動速度
    public float stopDistance = 3.0f;   // ★追加：この距離まで近づいたら止まって攻撃

    [Header("攻撃設定")]
    public float detectRange = 5f;
    public float attackCooldown = 2.0f;
    private float cooldownTimer = 0f;
    public Collider2D hammerHitbox;

    [Header("通常攻撃・判定設定")]
    public ContactFilter2D playerFilter;

    [Header("防御設定")]
    public float defenseDuration = 0.7f;
    public LayerMask playerBulletLayer;
    public float defenseRadius = 3.0f;

    [Header("スタン攻撃設定")]
    public float jumpForce = 15f;
    public float slamHardnessTime = 0.5f;
    public GameObject stunWavePrefab;

    [Header("ハンマー制御")]
    public Transform hammerVisual;
    public Vector3 swingStartRotation = new Vector3(0, 0, 90);
    public Vector3 swingEndRotation = new Vector3(0, 0, -45);
    public float swingDuration = 0.15f;

    [Header("接地判定設定")]
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        if (hammerVisual != null)
        {
            hammerVisual.gameObject.SetActive(false);
        }

        //ぶつかってもすり抜けられるようにする
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
	}

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

		if (currentState != FrankenState.Dead && currentState != FrankenState.Hit)
		{
			FlipTowardsPlayer();
		}

		// 状態に応じた処理
		switch (currentState)
        {
            case FrankenState.Idle:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 止まる
                CheckAttacks();
                break;

            case FrankenState.Move:
                MoveTowardsPlayer(); // ★移動処理を実行
                CheckAttacks();
                break;
        }
    }

    // ★追加：プレイヤーに向かって歩く処理
    void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 攻撃範囲より遠い場合のみ移動する
        if (distanceToPlayer > stopDistance)
        {
            // プレイヤーの方向（右か左か）を判定
            float direction = playerTransform.position.x > transform.position.x ? 1 : -1;

            // 移動速度をセット（Y軸は物理演算に任せる）
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            // 向きをプレイヤーの方に変える
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 近づきすぎたら止まる
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            currentState = FrankenState.Idle;
        }
    }

    void CheckAttacks()
    {
        if (cooldownTimer > 0) return;

        // 防御判定
        if (Physics2D.OverlapCircle(transform.position, defenseRadius, playerBulletLayer))
        {
            StartCoroutine(Defend());
            return;
        }

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 攻撃範囲内なら攻撃
        if (distance < detectRange)
        {
            if (Random.Range(0f, 1f) < 0.6f)
            {
                StartCoroutine(Attack_main());
            }
            else
            {
                StartCoroutine(Attack_Stan());
            }
        }
        else
        {
            // 遠ければ移動状態へ
            currentState = FrankenState.Move;
        }
    }


    IEnumerator Defend()
    {
        currentState = FrankenState.Defend;
        rb.linearVelocity = Vector2.zero; // 防御中は止まる
        yield return new WaitForSeconds(0.2f);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, defenseRadius, playerBulletLayer);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("PlayerBullet") || col.gameObject.GetComponent<bullet>() != null)
                Destroy(col.gameObject);
        }
        yield return new WaitForSeconds(defenseDuration);
        EndAttack();
    }

    IEnumerator Attack_main()
    {
        currentState = FrankenState.Attack_main;
        rb.linearVelocity = Vector2.zero; // 攻撃中は止まる

        if (hammerVisual != null)
        {
            hammerVisual.gameObject.SetActive(true);
            hammerVisual.localRotation = Quaternion.Euler(swingStartRotation);
        }
        yield return new WaitForSeconds(0.4f);
        float startTime = Time.time;
        bool damageApplied = false;
        while (Time.time < startTime + swingDuration)
        {
            float t = (Time.time - startTime) / swingDuration;
            hammerVisual.localRotation = Quaternion.Slerp(Quaternion.Euler(swingStartRotation), Quaternion.Euler(swingEndRotation), t);
            if (t >= 0.2f && !damageApplied && hammerHitbox != null)
            {
                Collider2D[] results = new Collider2D[5];
                int count = hammerHitbox.Overlap(playerFilter, results);
                for (int i = 0; i < count; i++)
                {
                    if (results[i].CompareTag("Player"))
                    {
                        player_control player = results[i].GetComponent<player_control>();
                        if (player != null) { player.playerHP(1); damageApplied = true; break; }
                    }
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        if (hammerVisual != null) hammerVisual.gameObject.SetActive(false);
        EndAttack();
    }

    IEnumerator Attack_Stan()
    {
        currentState = FrankenState.Attack_Stan;
        rb.linearVelocity = new Vector2(0, jumpForce);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => IsGrounded() && rb.linearVelocity.y <= 0.1f);
        if (stunWavePrefab != null)
        {
            float groundY = enemyCollider != null ? enemyCollider.bounds.min.y : transform.position.y;
            StartCoroutine(SpawnSlamWaves(new Vector3(transform.position.x, groundY, 0)));
        }
        yield return new WaitForSeconds(slamHardnessTime);
        EndAttack();
    }

    IEnumerator SpawnSlamWaves(Vector3 centerPos)
    {
        int waveCount = 4;
        float spread = 1.8f;
        float delay = 0.08f;
        for (int i = 1; i <= waveCount; i++)
        {
            Vector3 rightPos = centerPos + new Vector3(i * spread, 0.2f, 0);
            Vector3 leftPos = centerPos + new Vector3(-i * spread, 0.2f, 0);
            Instantiate(stunWavePrefab, rightPos, Quaternion.identity);
            Instantiate(stunWavePrefab, leftPos, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
    }

    bool IsGrounded()
    {
        if (enemyCollider == null) return false;
        Vector2 origin = new Vector2(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y);
        return Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer).collider != null;
    }

    void EndAttack()
    {
        rb.linearVelocity = Vector2.zero;
        cooldownTimer = attackCooldown;
        currentState = FrankenState.Idle;
    }

	void FlipTowardsPlayer()
	{
		if (playerTransform == null) return;

		float direction = playerTransform.position.x - transform.position.x;
		Vector3 scale = transform.localScale;

		if (direction > 0) scale.x = -Mathf.Abs(scale.x);
		else if (direction < 0) scale.x = Mathf.Abs(scale.x);

		transform.localScale = scale;
	}
}