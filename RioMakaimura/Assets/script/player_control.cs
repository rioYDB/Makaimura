using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public enum AttackType { Human, Okami, Which, Vampire };                                //攻撃種類を管理する

[System.Serializable]
public class AnimationSet
{
    public Sprite idle;         // 待機時のスプライト
    public Sprite[] walk;       // 歩行時（複数枚でアニメーション）
    public Sprite jump;         // ジャンプ中
    public Sprite attack;       // 攻撃時
}

public class player_control : MonoBehaviour
{
    //変数宣言
    public float moveSpeed;                                                         //移動速度
    public float jumpPower;                                                         //ジャンプ力
    private float jumpCooldown = 0.1f; // ジャンプ後の無効時間
    private float jumpTimer = 0f;
    private bool isFlying = false;
    private float flyingTime = 0f;
    private float currentMoveSpeed;
    public float maxFlyingTime = 2.0f; // 飛行の最大時間
    public LayerMask Ground;                                                        //地面を判別するオブジェクトレイヤー
    public LayerMask LadderLayer;                                                   //hasigoを判別するオブジェクトレイヤー
    bool jumpPressed;
    bool jumpHeld;


    //攻撃オブジェクトをプレイヤーからどれだけ上に生成するか (Y軸)
    public float attackSpawnYOffset = 0.5f; // Inspectorで調整してください (この値を大きくすると弾の位置が高くなります)

    //---------------------------------------------------------------------------------------------------------------
    //攻撃パターンと変身
    //攻撃判定の変数
    private AttackType currentAttack = AttackType.Human;                        //攻撃する種類がはじめは人間状態の攻撃にするため
    public void ChangeAttack(AttackType newType)
    {


        currentAttack = newType;
    }

    public AttackType attackType;

    public GameObject HumanWeapon;                                             //人状態で攻撃する（槍）のプレハブ
    public GameObject OkamiWeapon;                                             //狼男状態で攻撃する（爪）のプレハブ
    public GameObject WhichWeapon;                                             //魔女状態で攻撃する（魔法）のプレハブ
    public GameObject VampireWeapon;										   //ヴァンパイア状態で攻撃する(火柱)のプレハブ

    private GameObject spearToShoot;




    // 外部から currentAttack の値を安全に読み取るための公開プロパティ
    public AttackType CurrentAttackType
    {
        get { return currentAttack; }
    }


    public float KnockbackForce;                                                    //ノックバック
    public float invincibleTime;                                                        //無敵時間
    public int maxBulletsOnScreen = 3;                                          //画面内に出るプレイヤー攻撃の最大の数

    public int FirePillarCnt = 0;                                               //火柱を何連させるか
    public float FirePillarDelay = 0.0f;                                        //火柱の出現間隔
    public float FirePillarSpread = 0.0f;                                       //火柱がどのくらい広がるか
    public float FirePillarOffset = 0.0f;                                       //プレイヤーから火柱を出現させる距離
    public int maxFirePillarOnScreen = 0;                                       //火柱の最大出現数

    //-------------------------------------------------------------------------------------------------------------------
    public Vector3 StandSize = new Vector3(3.4f, 3.8f, 1f);             //立ってる時のサイズ
    public Vector3 SquatSize = new Vector3(1.7f, 1.9f, 1f);             //しゃがんだ時のサイズ

    private ScreenFlash screenFlash;
    //private CameraShake cameraShake;

    public int maxHP = 2;       // 最大HP
    private int HP = 2;         // げんざいのHP 
    //------------------------------------------------ 
    // HP表示用Text（インスペクターでセット）
    [SerializeField] private TMP_Text hpText;
    private Color defaultColor; // 元の色を保存しておく
    //------------------------------------------------ 
    private int AttackCount;                                                            //攻撃をカウントする変数
    private bool IsSquat = false;                                                   //しゃがみ判定
    private bool IsJumping;                                                         //空中にいるか判定
    private bool IsAttacking = true;                                                //攻撃できるか判定
    private bool IsInvincible = false;                                              //無敵状態か判定
    private float Moveinput;                                                          //移動入力
    private float LastAttackTime;                                                   //最後に攻撃した時間
    private float InvincibleTimer;                                                  //無敵時間タイマー
    private Vector2 Movedirection = Vector2.zero;                           // 移動方向を記憶しておく
    private float FlyingTime = 0.0f;   //滑空時間を制限する変数
    private Rigidbody2D rb;                                                         //Rigidbody2Dの格納

    public Rigidbody2D Rb { get { return rb; } } // Rigidbody2Dを外部に公開

    private int playerLayer;
    private int platformLayer;
    private BoxCollider2D bc;                                                       //BoxCollider2Dの格納庫
    private SpriteRenderer SpriteRenderer;                                          //SpriteRendererを扱うための格納庫
   //-----------------------------------------------------------------------------------------------------------------------
                                                                                    //仮想チェンジ用
    public Sprite Okami;        //当たった時に画像を変えるため
    public Sprite Which;        //当たった時に画像を変えるため
    public Sprite Vampire;		//当たった時に画像を変えるため


    private bool HumanChange = true;        //人間状態の判定をする
    private bool OkamiChange = false;       //狼男状態の判定をする
    private bool WhichChange = false;		//魔女状態の判定をする


    //SetGravityコンポーネントへの参照
    private SetGravity setGravityComponent;

    private Image image;            //画像の管理
    bool text1enableKey = true;
    
    //アニメーション関連
    //-----------------------------------------------------------------------------------------
    // 各変身状態のアニメーションセットをInspectorで登録
    public AnimationSet humanAnim;
    public AnimationSet okamiAnim;
    public AnimationSet witchAnim;
    public AnimationSet vampireAnim;

    // 現在の状態のアニメーションを保持する変数
    private AnimationSet currentAnim;

    private SpriteRenderer sr;       // スプライトを描画するためのコンポーネント

    private float walkAnimTimer = 0f;    // 歩行アニメのタイマー
    private int walkAnimIndex = 0;       // 歩行アニメのフレームインデックス

    //狼状態の壁登り-----------------------------------------------------------------------------------------
    public LayerMask WallClim;          //壁登りレイヤーの変数
    //-----------------------------------------------------------------------------------------------------------------------
    //はしご登り関連の追加変数
    public float ladderClimbSpeed = 3.0f; // はしごを登る速度
    private bool isClimbingLadder = false; // はしごを登っている最中かどうか
    private bool canClimbLadder = false;   // はしごに触れていて、登れる状態にあるかどうか
    private float originalGravityScale;    // 元の重力値を保持
    private Collider2D currentLadderCollider; // 今触れているはしご
    private Collider2D lastLadderCollider; // 最後に登ったはしごを保存
    private bool isAtLadderBottom = false;

    //-----------------------------------------------------------------------------------------------------------------------
    //しゃがむ処理の変数
    private Vector2 OriginColliderSize;
    private Vector2 OriginClliderOffset;
    private Vector3 OriginLocalScale;
    //-----------------------------------------------------------------------------------------------------------------------
    //魔女の能力（隠しエリアに入れる）処理の変数
    public LayerMask SecretArea;
    public string FadeWallTag = "Fadewall";
    public float FadeDuration;
    private bool InSecretArea;
    //-----------------------------------------------------------------------------------------------------------------------

    public bool isFacingRight = true; // カメラが参照する向きフラグ




    void Start()
    {
        //アタッチされているComponentを取得
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        /*SpriteRenderer.color = Color.green; */                                    //プレイヤーの色を緑色にする

        // SpriteRendererコンポーネントを取得します
        image = GetComponent<Image>();
        // SpriteのSpriteRendererコンポーネントを取得
        sr = gameObject.GetComponent<SpriteRenderer>();
        // 初期状態（Human）のアニメーションセットを使用
        SetCurrentAnimSet();

        //元の重力値を保持
        originalGravityScale = rb.gravityScale;

        // HP関連
        // --------------------------------------------
        // HPマックスからスタート
        HP = maxHP;

        screenFlash = FindAnyObjectByType<ScreenFlash>(); // 画面フラッシュを探す
        //cameraShake = FindAnyObjectByType<CameraShake>(); // カメラ振動

        if (hpText != null)
            defaultColor = hpText.color; // 初期色（通常は白）

        UpdateHPText(); // 起動時に表示を更新
        // --------------------------------------------

        //しゃがみ機能の初期設定
        OriginColliderSize = bc.size;
        OriginClliderOffset = bc.offset;

        OriginLocalScale = transform.localScale;

        BulletChange("Human");

        // レイヤーを取得する処理
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");

        Debug.Log("PlayerLayer = " + playerLayer + ", PlatformLayer = " + platformLayer);
    }

    // Update is called once per frame
    void Update()
    {
       

        if (IsGrounded())
        {
            IsJumping = false;
        }

        // はしごの一番下 + 地面 のときの特例処理
        bool bottomAndGrounded = (canClimbLadder && isAtLadderBottom && IsGrounded());

        // 上方向入力
        bool wantToClimb = Input.GetAxisRaw("Vertical") > 0.1f;

        // ★通常行動モードにするか？
        bool isFreeFromLadder = bottomAndGrounded && !wantToClimb;

        // ---------------------------------------------------------------
        // ● “通常行動だけを通したい状況” では、はしご処理をスキップする
        // ---------------------------------------------------------------

        if (!isFreeFromLadder)
        {
            // ★はしご登り中（すでに登っている最中）
            if (isClimbingLadder)
            {
                ClimbLadder();
                return; // ← 通常行動には進まない
            }

            // ★はしごに触れていて、上 or 下入力で登り開始
            if (canClimbLadder && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
            {
                StartClimbingLadder(currentLadderCollider);
                return; // ← 通常行動には進まない
            }
        }



        if (Input.GetKeyDown(KeyCode.DownArrow) && IsOnPlatform())
        {
            Debug.Log("↓キー入力検出！");
            StartCoroutine(DisableColliderMomentarily());
        }

       

        //移動処理
        if (IsSquat == false )
        {
            Move();
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LadderLayer);

        //ジャンプ処理
        // タイマー更新
        if (jumpTimer > 0)
            jumpTimer -= Time.deltaTime;

        // ジャンプボタン入力を記録
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        jumpHeld = Input.GetButton("Jump");

        if (/*(IsGrounded() == true || hit.collider != null) && */IsSquat == false && !isClimbingLadder)
        {
            Jump();
        }

        // 変更後（空中でも可変ジャンプ処理が動くようにする）
        if (!isClimbingLadder && IsSquat == false)
        {
            Jump();
        }

        //Zキーが押されたら
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))
        {
            Attack();
        }



       

        //無敵タイマー
        if (IsInvincible == true)
        {
            InvincibleTimer -= Time.deltaTime;
            if (InvincibleTimer <= 0)
            {
                IsInvincible = false;
            }
        }

        // アニメーションの更新
        SetAnimation();


        //死亡条件
        if (HP == 0)
        {
            //プレイヤーを破壊
            Destroy(gameObject);

            Debug.Log("死ぬwwwwwwwwww");

            //Sceneをリセットする
            SceneManager.LoadScene("GameOver");
        }

        // =================== デバッグキー ===================
        if (Input.GetKeyDown(KeyCode.F1))
        {
            IsInvincible = !IsInvincible;  // 無敵トグル
            if (IsInvincible)
            {
                InvincibleTimer = 999f; // 十分に長い時間を設定
                StartCoroutine(InvincibilityFlash());
                StartCoroutine(IgnoreEnemyCollisionDuringInvincibility());
                Debug.Log("無敵モード ON");
            }
            else
            {
                InvincibleTimer = 0f;
                Debug.Log("無敵モード OFF");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            sr.sprite = Okami;
            BulletChange("Okami");
            currentAttack = AttackType.Okami;
            SetCurrentAnimSet();
            Debug.Log("狼に変身");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            sr.sprite = Which;
            BulletChange("Which");
            currentAttack = AttackType.Which;
            SetCurrentAnimSet();
            Debug.Log("魔女に変身");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            sr.sprite = Vampire;
            BulletChange("Vampire");
            currentAttack = AttackType.Vampire;
            SetCurrentAnimSet();
            Debug.Log("ヴァンパイアに変身");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            sr.sprite = humanAnim.idle; // idleスプライトに戻す（または任意のSprite）
            BulletChange("Human");
            currentAttack = AttackType.Human;
            SetCurrentAnimSet();
            Debug.Log("人間に戻る");
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("シーンリセット");
        }

        // ↓キー入力時に物理設定を変更する前後でログを出力
        #if false
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("↓キーが押された: 衝突無効化を開始");
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            rb.gravityScale = 0f;  // 重力を無効化
        }
        else
        {
            Debug.Log("↓キーが離された: 衝突無効化を終了");
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
            rb.gravityScale = originalGravityScale;  // 元の重力に戻す
        }
        #endif
    }

    // 変身状態に応じて使うアニメーションセットを切り替える
    void SetCurrentAnimSet()
    {
        switch (currentAttack)
        {
            case AttackType.Okami:
                currentAnim = okamiAnim;
                break;
            case AttackType.Which:
                currentAnim = witchAnim;
                break;
            case AttackType.Vampire:
                currentAnim = vampireAnim;
                break;
            default:
                currentAnim = humanAnim;
                break;
        }
    }

    void SetAnimation()
    {
        // 攻撃中のスプライトを優先表示（例としてキー入力で判定）
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButton("Fire1"))
        {
            sr.sprite = currentAnim.attack;
            return;
        }

        // ジャンプ中はジャンプ用スプライト
        if (IsJumping && !IsGrounded())
        {
            sr.sprite = currentAnim.jump;
            return;
        }

        // 横移動中（地面にいる場合）のスプライト切り替え（歩行アニメ）
        if (Mathf.Abs(Moveinput) > 0.01f && IsGrounded())
        {
            walkAnimTimer += Time.deltaTime;
            if (walkAnimTimer > 0.2f) // アニメーション速度（0.2秒ごとに切り替え）
            {
                walkAnimIndex = (walkAnimIndex + 1) % currentAnim.walk.Length;
                walkAnimTimer = 0f;
            }
            sr.sprite = currentAnim.walk[walkAnimIndex];
            return;
        }

        // それ以外（待機状態）のスプライト
        sr.sprite = currentAnim.idle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        
        //狼男アイテムに触れたら自分が狼男になる
        if (collision.gameObject.tag == "Okami")
        {

            sr.sprite = Okami;
            BulletChange("Okami");

            SetCurrentAnimSet();

            Debug.Log("狼男に変身");

            Destroy(collision.gameObject);
        }


        //魔女アイテムに触れたら自分が魔女になる
        if (collision.gameObject.tag == "Which")
        {

            sr.sprite = Which;
            BulletChange("Which");

            SetCurrentAnimSet();

            Debug.Log("魔女に変身");

            Destroy(collision.gameObject);
        }

        /*
        //魔女が隠し空間に入れるようにする
        if (currentAttack == AttackType.Which && ((1 << collision.gameObject.layer) & SecretArea) != 0)
        {
            // 衝突したオブジェクトがFadeWallタグを持っているか確認（壁を透かす対象か）
            if (collision.CompareTag(FadeWallTag))
            {
                Debug.Log("隠しエリアクランクインです (魔女)");
                if (!InSecretArea) // 既に入っている状態ではない場合のみ実行
                {
                    InSecretArea = true;

                    // ★修正: 透過アニメーション開始前に、コライダーを即座に無効化し通り抜けを保証★
                    Collider2D wallCollider = collision.GetComponent<Collider2D>();
                    if (wallCollider != null)
                    {
                        wallCollider.enabled = false; // すぐにすり抜け可能にする
                        // Debug.Log("壁コライダー即時無効化"); 
                    }

                    // 透過アニメーション開始
                    StartCoroutine(FadeWalls(true));
                }
            }
        }
        
        */
        //ヴァンパイアアイテムに触れたら自分が魔女になる
        if (collision.gameObject.tag == "Vampire")
        {

            sr.sprite = Vampire;
            BulletChange("Vampire");

            SetCurrentAnimSet();

            Debug.Log("ヴァンパイアに変身");

            Destroy(collision.gameObject);
        }



        //はしごに触れたら登れるようになる
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimbLadder = true;
            currentLadderCollider = collision; // どのはしごか記録！（現在）
            
        }

        //EnemyとEnemyBulletに当たったらプレイヤーを破壊する
        if ((collision.gameObject.tag == "AttackZone" || collision.gameObject.tag == "EnemyBullet"))
        {

            playerHP(1);
        }
    }
    //「梯子の一番下にいるか」判定
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            Bounds b = collision.bounds;

            // プレイヤーの足元の座標
            float playerFeet = transform.position.y - (bc.size.y / 2f);

            // 梯子の最下点
            float ladderBottom = b.min.y;

            // プレイヤーの足の方が梯子の底付近なら「一番下にいる」
            isAtLadderBottom = playerFeet <= ladderBottom + 0.3f;
        }
    }
    //はしごから離れた処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimbLadder = false;
            isAtLadderBottom = false;

            // はしごのコライダーから完全に離れたら、はしご登り状態を強制終了する
            // これで、横に移動してはしごから外れた場合も対応できる
            if (isClimbingLadder)
            {
                StopClimbingLadder();
            }
        }




        //隠しエリアから出る


        
        // 衝突したオブジェクトのレイヤーがSecretAreaに含まれているかチェック
        if (((1 << collision.gameObject.layer) & SecretArea) != 0)
        {
            Debug.Log("隠しエリアクランクアップ！");
            InSecretArea = false;

            if (gameObject.activeInHierarchy)
            {
                // ★修正: 透過解除とコライダー有効化を同時に行う★
                //StartCoroutine(FadeWalls(false));
            }
            else
            {
                Debug.LogWarning("コルーチンを開始できませんでした: プレイヤーオブジェクトが非アクティブです。", this);
            }
        }
        
    }

    


    //接触判定（敵やアイテム）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Activearea")
        {
            HP = 0;
        }
    }



    //関数名：StartInvincibility()
    //用途：無敵時間処理
    //引数：なし
    //戻り値：なし
    void StartInvincibility()
    {
        IsInvincible = true;
        InvincibleTimer = invincibleTime;

        // 無敵時、敵との衝突を無視する
        StartCoroutine(IgnoreEnemyCollisionDuringInvincibility());


        // プレイヤーを点滅させるためのコルーチンを開始
        StartCoroutine(InvincibilityFlash());
    }



    //関数名：InvincibilityFlash()
    //用途：点滅処理
    //引数：なし
    //戻り値：なし
    // プレイヤーを点滅させるコルーチン
    IEnumerator InvincibilityFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (IsInvincible)
        {
            sr.enabled = !sr.enabled; // 点滅（スプライトの表示・非表示切り替え）
            yield return new WaitForSeconds(0.1f); // 点滅の間隔
        }
        sr.enabled = true; // 最後にスプライトを表示
    }


    // 無敵時間中、プレイヤーと敵の衝突を無視する
    IEnumerator IgnoreEnemyCollisionDuringInvincibility()
    {
        // 無敵時間が終了するまで繰り返し
        while (IsInvincible)
        {
            // "Enemy" タグのついたオブジェクトをすべて取得
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // 敵との衝突を無視
            foreach (GameObject enemy in enemies)
            {
                Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), true);
            }

            // 無敵時間中は衝突を無視し続ける
            yield return null;
        }

        // 無敵時間が終了したら、衝突を再び有効にする
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            Physics2D.IgnoreCollision(bc, enemy.GetComponent<Collider2D>(), false);
        }
    }

    //関数名：PlayerColor()
    //用途：プレイヤーのHPによる色の変更
    //引数：なし
    //戻り値：なし
    void PlayerColor()
    {
        switch (HP)
        {
            case 3:

            //プレイヤーのHPが2の時
            case 2:

                //プレイヤーの色を緑色にする
                SpriteRenderer.color = Color.green;
                break;

            //プレイヤーのHPが2の時
            case 1:

                //プレイヤーの色を黄色にする
                SpriteRenderer.color = Color.yellow;
                break;

            //プレイヤーのHPが2の時
            case 0:

                //プレイヤーの色を赤色にする
                SpriteRenderer.color = Color.red;
                break;
        }
    }


    //関数名：Jump()
    //用途：ジャンプ処理
    //引数：なし
    //戻り値：なし
    void Jump()
    {
        // ジャンプ可能条件
        bool canJump = IsGrounded() && jumpTimer <= 0f;

        // 飛行中はジャンプ入力を無効化
        if (isFlying) return;

        //ジャンプ処理
        if ((Input.GetKeyDown(KeyCode.Space) || jumpPressed) && canJump/*Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")*/)
        {
         
            //Debug.Log(jumpHeld);

            //プレイヤーの状態に併せてジャンプ力を変更する
            float currentJumpPower = jumpPower;

            //狼男の状態だとジャンプ力が上がる
            if (currentAttack == AttackType.Okami)
            {
                currentJumpPower *= 1.2f;
            }

            //ヴァンパイア状態だとジャンプ力が少し上がる
            if (currentAttack == AttackType.Vampire)
            {
                currentJumpPower *= 1.1f;
            }


            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpPower);
            //ジャンプ状態にする
            IsJumping = true;
            jumpPressed = false;

            //rb.AddForce(Vector2.up * jumpPower);
            ////ジャンプ状態にする
            //IsJumping = true;

            ////ジャンプ中は移動速度を制限する
            //Moveinput *= 0.7f;

            // ジャンプ中は SetGravity を一時停止
            GetComponent<SetGravity>().IsEnable = false;

            // 次のジャンプまで待機時間を設定
            jumpTimer = jumpCooldown;
        }


        // 可変ジャンプ（ボタン離したら上昇打ち切り）
        if (!jumpHeld && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // 落下時と上昇時で重力を調整
        if (rb.linearVelocity.y < 0)
        {
            // 落下中：速く落ちる
            rb.gravityScale = 2.5f;  // ← 落下を速く（ズドン感UP）
        }
        else if (rb.linearVelocity.y > 0 && !jumpHeld)
        {
            // ジャンプボタンを離したら少しだけ重くする
            rb.gravityScale = 1.5f;
        }
        else
        {
            // 上昇中（押してる間）は軽くしてふわっと
            rb.gravityScale = 0.7f;  // ← 上昇を軽く（ふんわり感UP）
        }

        // 地面についたら SetGravity を再開
        if (IsGrounded())
        {
            GetComponent<SetGravity>().IsEnable = true;
        }
    }


    //関数名：Move()
    //用途：移動処理
    //引数：なし
    //戻り値：なし
    void Move()
    {
       
            GetComponent<SetGravity>().IsEnable = true;
            rb.gravityScale = originalGravityScale;
        

        //地上にいないときは入力を受け付けない
        if (IsGrounded() == true )
        {


            //地面についたら滑空時間をリセットする
            FlyingTime = 0.0f;

            Moveinput = Input.GetAxisRaw("Horizontal");



            //プレイヤーの向きを変更
            if (Moveinput != 0)
            {
                Movedirection = new Vector2(Moveinput, 0f);

                // 向きを反転させる処理
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(Moveinput); // 左ならマイナス、右ならプラス
                transform.localScale = scale;

                // 向きフラグを更新
                isFacingRight = (Moveinput > 0);
            }
        }




        currentMoveSpeed = moveSpeed;  //moveSpeedを一時的な変数に保管
        float PlayerDirection = transform.localScale.x > 0 ? 1f : -1f; // Move()内で使うplayerDirectionを定義


        //狼男になった時に壁を登れるようになる
        if (currentAttack == AttackType.Okami)
        {
            currentMoveSpeed *= 1.5f;

            float WallCheck = 0.01f;

            // Y軸オフセットを追加
            float verticalCenterOffset = bc.size.y * 0.50f; // プレイヤーの高さの1/4分上にずらす

            Vector2 wallRayOrigin = (Vector2)transform.position + bc.offset + new Vector2(bc.size.x / 2f * PlayerDirection, 0f);



            // 壁のチェック (横方向)
            RaycastHit2D wallHit = Physics2D.Raycast(wallRayOrigin, Vector2.right * PlayerDirection, WallCheck, WallClim);

            // 壁に触れていて、かつ上方向の入力がある場合 (壁登りの継続/開始条件)
            if (wallHit.collider != null && Input.GetAxisRaw("Vertical") > 0.1f)
            {
                // 壁登りの実行
                GetComponent<SetGravity>().IsEnable = false;
                rb.gravityScale = 0f;

                float climbSpeed = currentMoveSpeed * 0.8f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed);

                return; // 壁登り中は他の移動処理をスキップ
            }

            // ★★★ 修正箇所: 横方向の壁がなくなった時のチェック ★★★

            // 壁登りモード中に、横の壁がなくなった（wallHit.collider == null）場合
            // ただし、Playerが空中にいる場合のみこのロジックが適用されるようにする（地面にいる場合は通常の移動ロジックで制御される）
            if (rb.gravityScale == 0f && !IsGrounded())
            {
                // 空中で、かつ壁がなくなった（Raycastがヒットしなかった）場合
                if (wallHit.collider == null)
                {
                    Debug.Log("横の壁が途切れたため、壁登りモードを解除します。");

                    // 重力と移動を復帰させる
                    GetComponent<SetGravity>().IsEnable = true;
                    rb.gravityScale = originalGravityScale;

                    // プレイヤーが向いている方向（Moveinput方向）にジャンプさせる
                    rb.linearVelocity = new Vector2(PlayerDirection * jumpPower * 0.5f, jumpPower * 0.8f);

                    return; // 処理を終了し、通常の移動処理をスキップ
                }
            }

            // 壁に触れていない、または上入力がない場合は重力を元に戻す
            else
            {
                GetComponent<SetGravity>().IsEnable = true;
                rb.gravityScale = originalGravityScale;
            }

        }


        //魔女になった時に魔女限定で動かしたい時に使う
        else if (currentAttack == AttackType.Which)
        {

        }

        //ヴァンパイアになったときに飛べるようになる
        else if (currentAttack == AttackType.Vampire)
        {
            currentMoveSpeed *= 1.2f;


            HandleVampireFlight();
            //// Wキー長押しで飛行モードに入る例
            //if ((Input.GetKey(KeyCode.Space) || jumpHeld) && !isClimbingLadder) // はしご登り中は飛ばない
            //{

            //    FlyingTime += Time.deltaTime;
            //    Debug.Log(("経過") + FlyingTime);

            //    //Gravityのスクリプトも止める
            //    GetComponent<SetGravity>().IsEnable = false;

            //    rb.gravityScale = 0f; // 重力を無効にする



            //    // Y軸の速度は0に固定する
            //    // 左右の移動はMoveinputで制御、Y軸はInput.GetAxisRaw("Vertical")の影響を受けないようにする
            //    rb.linearVelocity = new Vector2(Moveinput * currentMoveSpeed, 0f); // Y軸の速度を0に固定！
            //}

            //if (FlyingTime >= 2.0f) //3秒たったら重力を元に戻し徐々に下降していく
            //{
            //    SpriteRenderer.color = Color.magenta;   //プレイヤーに下降している状態とわかりやすく伝えるために色を変える

            //    GetComponent<SetGravity>().IsEnable = true;
            //    rb.gravityScale = originalGravityScale;
            //}

            //else if (Input.GetKeyUp(KeyCode.Space)|| !jumpHeld) // Wキーを離したら重力を元に戻す
            //{
            //    PlayerColor();                 //元の体力状態の色に戻す
            //    GetComponent<SetGravity>().IsEnable = true;
            //    rb.gravityScale = originalGravityScale;



            //}
        }


        //プレイヤーを移動させる
        rb.linearVelocity = new Vector2(Moveinput * currentMoveSpeed, rb.linearVelocity.y);



        if (Moveinput != 0)
        {
            Movedirection = new Vector2(Moveinput, 0f);
        }

    }

    // ヴァンパイアの飛行の関数
    void HandleVampireFlight()
    {
        // ヴァンパイア形態でなければスキップ
        if (currentAttack != AttackType.Vampire)
        {
            isFlying = false;
            return;
        }

        // 空中でジャンプボタンを押し続けている間に飛行へ移行
        if (!IsGrounded() && (Input.GetKey(KeyCode.Space) || jumpHeld) && !isFlying)
        {
            // 飛行に入る条件：上昇が止まってから（自然な感じに）
            if (rb.linearVelocity.y <= 0)
            {
                isFlying = true;
                flyingTime = 0f;

                GetComponent<SetGravity>().IsEnable = false;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;

                //// 見た目で分かるように色変化など
                //SpriteRenderer.color = Color.magenta;
            }
        }

        // 飛行中の処理
        if (isFlying)
        {
            flyingTime += Time.deltaTime;
            rb.linearVelocity = new Vector2(Moveinput * currentMoveSpeed, 0f);

            // 離す or 時間切れで飛行終了
            if ((Input.GetKeyUp(KeyCode.Space) || !jumpHeld) || flyingTime > maxFlyingTime)
            {
                isFlying = false;
                GetComponent<SetGravity>().IsEnable = true;
                rb.gravityScale = 1f;
                /*PlayerColor();*/ // 元の色に戻す
            }
        }
    }


    //関数名：Attack()
    //用途：攻撃処理
    //引数：なし
    //戻り値：なし
    private void Attack()
    {
        ////槍オブジェクトをすべて取得するために配列を作成
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Spear");


        // プレイヤーの向きを取得
        float playerDirection = transform.localScale.x > 0 ? 1f : -1f;

        //発射位置を制御
        float Offsetx = 1.0f;
        Vector3 SpwanPos = transform.position + new Vector3(transform.localScale.x > 0 ? Offsetx : -Offsetx, 0, 0);

        //火柱の位置を設定
        Vector3 spawnPosition = transform.position + new Vector3(playerDirection * FirePillarOffset, 0f, 0f);


        if (currentAttack == AttackType.Vampire)
        {
            //火柱の処理
            //数の制限
            GameObject[] Pillars = GameObject.FindGameObjectsWithTag(VampireWeapon.tag);
            if (Pillars.Length > maxFirePillarOnScreen)
            {
                Debug.Log("火柱出すぎてアチアチやでぇ...");
                return;
            }

            //火柱を出力
            StartCoroutine(SpawnFirePillarsRoutine(spawnPosition, playerDirection, FirePillarCnt, FirePillarDelay, FirePillarSpread, Ground));

        }

        else //ヴァンパイア以外の状態の攻撃
        {

            if (bullets.Length >= maxBulletsOnScreen)
            {
                // 画面の最大数に達しているので発射しない

                return;
            }


            // 攻撃処理
            GameObject bullet = Instantiate(spearToShoot, SpwanPos, Quaternion.identity);

            // プレイヤーの向きに合わせて反転
            bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);



            //変更箇所: Y軸オフセットを加算して生成位置を計算
            Vector3 SpawnPosition = transform.position + new Vector3(
                playerDirection * Offsetx, // X軸のオフセット
                attackSpawnYOffset,                  // Y軸のオフセット
                0f
            );

        }



    }

    //関数名：BulletChange()
    //用途：攻撃処理
    //引数：string
    //戻り値：なし
    private void BulletChange(string BulletName)
    {
        /*
        // ★修正: 攻撃タイプが変更されたら、壁のルールを強制適用★
        EnforceSecretWallRule();
        */

        //発射する弾の種類を管理する（switchで）
        spearToShoot = HumanWeapon;

        switch (BulletName)
        {
            case "Okami":
                currentAttack = AttackType.Okami; // currentAttack もここで設定
                spearToShoot = OkamiWeapon;
                break;
            case "Which":
                currentAttack = AttackType.Which;
                spearToShoot = WhichWeapon;
                break;

            case "Vampire":
                currentAttack = AttackType.Vampire;
                spearToShoot = VampireWeapon;
                break;

            default:
                currentAttack = AttackType.Human;
                spearToShoot = HumanWeapon;
                break;

        }
    }


    //関数名：IsGrounded()
    //用途：接地判定処理
    //引数：なし
    //戻り値：接地している場合はtrue、していない場合はfalse
    public bool IsGrounded()
    {
        bool ret = false;
        // 「Ground」+「Platform」両方を対象にする
        int groundAndPlatformMask = Ground | (1 << LayerMask.NameToLayer("Platform"));


        //下方向にrayを飛ばして、指定したレイヤーのオブジェクトと接触しているかどうか判別する
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundAndPlatformMask);

        // レイを可視化
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.yellow);

        //ヒットしていない場合はnullが返される
        if (hit.collider != null)
        {
            ret = true;
        }
        if (ret == true)
        {
            //IsJumping = false;
        }

        return ret;
    }

    //関数名：StartClimbingLadder()
    //用途：はしごを登り始める処理
    //引数：void
    //戻り値：なし
    void StartClimbingLadder(Collider2D ladderCollider)
    {
        isClimbingLadder = true;
        rb.gravityScale = 0f; // 重力を無効にする
        rb.linearVelocity = Vector2.zero; // 現在の移動をリセット

        //Gravityのスクリプトも止める
        GetComponent<SetGravity>().IsEnable = false;
        // はしごの中央に位置をスナップ
        Vector3 pos = transform.position;
        pos.x = ladderCollider.bounds.center.x;
        transform.position = pos;

        currentLadderCollider = ladderCollider; // 今登っているはしごを保存
    }

    //関数名：ClimbLadder()
    //用途：はしごを登る処理
    //引数：void
    //戻り値：なし
    void ClimbLadder()
    {
        if (!isClimbingLadder)
        {
            return;
        }

        // Y軸の入力（上矢印キーまたはLスティックの上下）を取得
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 上下移動
        rb.linearVelocity = new Vector2(0f, verticalInput * ladderClimbSpeed);
        //Debug.Log("Input = " + verticalInput + " velocity = " + rb.linearVelocity + " verticalInput * ladderClimbSpeed= " + verticalInput * ladderClimbSpeed);

    }


    //関数名：StopClimbingLadder()
    //用途：はしご登り終了処理
    //引数：void
    //戻り値：なし
    void StopClimbingLadder()
    {
        isClimbingLadder = false;
        rb.gravityScale = originalGravityScale;


        //Gravityのスクリプトは再開
        GetComponent<SetGravity>().IsEnable = true;
    }

   

    //関数名：playerHP()
    //用途：ダメージ処理
    //引数：void
    //戻り値：なし
    public void  playerHP(int Damege)
    {
        if (!IsInvincible)
        {
            HP -= Damege;
            Debug.Log("痛い");


            //PlayerColor();

            //// ノックバック処理
            //Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
            //GetComponent<Rigidbody2D>().AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);
            // 無敵状態を開始
            StartInvincibility();

            UpdateHPText(); // 起動時に表示を更新
            StartCoroutine(HPTextEffect(Color.red)); //HP変動時に一瞬拡大する演出(赤)

            // ダメージ時に画面フラッシュ！
            if (screenFlash != null)
            {
                screenFlash.Flash(new Color(1, 0, 0, 0.4f), 0.3f);
            }

            //// カメラを揺らす！
            // カメラシェイク呼び出し
            FindAnyObjectByType<CameraMove>()?.Shake();
            //CameraMove cameraMove = FindAnyObjectByType<CameraMove>();
            //if (cameraMove != null)
            //{
            //    cameraMove.Shake();
            //}
            //if (cameraShake != null)
            //{
            //    cameraShake.Shake(0.15f, 0.08f);
            //}
        }
    }

    //HPを回復する関数
    public void Heal(int amount)
    {
        HP += amount;
        if (HP > maxHP)
        {
            HP = maxHP; // 最大値を超えないように
        }

        Debug.Log("HP回復！ 現在HP：" + HP);
        // ここに回復エフェクトや音を追加してもOK

        UpdateHPText(); // 起動時に表示を更新
        StartCoroutine(HPTextEffect(Color.green)); //HP変動時に一瞬拡大する演出(緑)
    }

    // HPを画面に表示するための関数
    private void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = "HP:" + HP + " / " + maxHP;
        }
    }

    //HP変動時に一瞬拡大する演出を起こす関数
    private IEnumerator HPTextEffect(Color effectColor)
    {
        if (hpText == null) yield break;

        float duration = 0.1f;
        Vector3 originalScale = hpText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        // 色とスケールのアニメーション
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            hpText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            hpText.color = Color.Lerp(defaultColor, effectColor, t);
            yield return null;
        }

        // 戻す処理
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            hpText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            hpText.color = Color.Lerp(effectColor, defaultColor, t);
            yield return null;
        }

        hpText.color = defaultColor; // 念のため元の色に戻す
    }

    //火柱を放つコルーチン
    IEnumerator SpawnFirePillarsRoutine(Vector3 basePosition, float playerDirection, int count, float delay, float spread, LayerMask Ground)
    {
     
        //修正：火柱の基準となるY座標をプレイヤーの足元から取得する
        float baseFirePillarY = transform.position.y; // デフォルトはプレイヤーのY座標

        // プレイヤーの足元からRaycastを飛ばし、立っている地面のY座標を取得する
        Vector2 playerFeetRayOrigin = (Vector2)transform.position + bc.offset + Vector2.down * (bc.size.y / 2f - 0.05f); // プレイヤーの足元
        float playerFeetRayLength = 0.5f; // プレイヤーの足元から地面を探す長さ（調整が必要）
        RaycastHit2D playerGroundHit = Physics2D.Raycast(playerFeetRayOrigin, Vector2.down, playerFeetRayLength, Ground);

        // Debug.DrawRay(playerFeetRayOrigin, Vector2.down * playerFeetRayLength, Color.green, 1f); // デバッグ用

        if (playerGroundHit.collider != null)
        {
            baseFirePillarY = playerGroundHit.point.y; // プレイヤーが立っている地面のY座標を取得
        }
        else
        {
            // プレイヤーが地面についていない場合（ジャンプ中など）は、最も近い地面を探す
            // あるいは、火柱を地面から出すのを諦めて、プレイヤーの足元から出す
            // ここでは、プレイヤーの足元Y座標を基準とする (または、より長いRaycastで地面を探す)
            Debug.LogWarning("プレイヤーが地面についていないため、火柱の正確な地面位置が特定できませんでした。プレイヤーの足元から出ます。", this);
        }

        for (int i = 0; i < count; i++)
        {
            float offsetX = (i - (count - 1) / 2.0f) * spread * playerDirection;
            // 火柱を生成するX座標は、プレイヤーの目の前を中心に広がるように計算する
            float targetX = basePosition.x + offsetX;

            // 火柱の最終的なY座標は、上で計算したbaseFirePillarYを使用する
            Vector3 firePillarSpawnPos = new Vector3(targetX, baseFirePillarY, basePosition.z);

            // 火柱を生成する
            GameObject pillarInstance = Instantiate(VampireWeapon, firePillarSpawnPos, Quaternion.identity);

            // 火柱の見た目もプレイヤーの向きに合わせる (この部分は変更なし)
            SpriteRenderer pillarSr = pillarInstance.GetComponent<SpriteRenderer>();
            if (pillarSr != null)
            {
                pillarSr.flipX = (playerDirection == -1);
            }
            else
            {
                Vector3 pillarScale = pillarInstance.transform.localScale;
                pillarScale.x = Mathf.Abs(pillarScale.x) * playerDirection;
                pillarInstance.transform.localScale = pillarScale;
            }

            // 次の火柱が出るまで少し待つ
            yield return new WaitForSeconds(delay);
        }
    }


    //一旦削除

    
    /*

    //魔女の隠しエリアのかべのコルーチン
    IEnumerator FadeWalls(bool FadeOut)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag(FadeWallTag); // FadingWallタグの壁を全て取得
        
        // フェードアウト（壁に入る）時は、まずコライダーを無効化
        if (FadeOut)
        {
            foreach (GameObject wall in walls)
            {
                Collider2D wallCollider = wall.GetComponent<Collider2D>();
                if (wallCollider != null)
                {
                    wallCollider.enabled = false; // コライダーを無効にし、すり抜け可能にする
                }
            }
        }
        

        foreach (GameObject wall in walls)
        {
            SpriteRenderer wallRenderer = wall.GetComponent<SpriteRenderer>();
            Collider2D wallCollider = wall.GetComponent<Collider2D>(); // コライダーを再取得

            if (wallRenderer != null)
            {
                Color startColor = wallRenderer.color;
                Color targetColor = startColor;

                if (FadeOut) // 透かす場合
                {
                    targetColor.a = 0.3f; // 完全に透明にせず、少しだけ見えるように
                }
                else // 元に戻す場合
                {
                    targetColor.a = 1.0f; // 不透明に戻す
                }

                float timer = 0f;
                while (timer < FadeDuration)
                {
                    timer += Time.deltaTime;
                    float progress = timer / FadeDuration;
                    wallRenderer.color = Color.Lerp(startColor, targetColor, progress);
                    yield return null;
                }
                wallRenderer.color = targetColor; // 最終的な色を確実に適用
            }
        }

        // フェードイン（壁から出る）時は、フェードが完了してからコライダーを有効化
        if (!FadeOut)
        {
            foreach (GameObject wall in walls)
            {
                Collider2D wallCollider = wall.GetComponent<Collider2D>();
                if (wallCollider != null)
                {
                    wallCollider.enabled = true; // コライダーを有効にし、壁として機能させる
                }
            }
        }

    }
    


    /// <summary>
    /// プレイヤーの状態が変わったとき、隠し壁の規則を強制的に適用する。
    /// </summary>
    private void EnforceSecretWallRule()
    {
        // プレイヤーが現在、隠しエリアの境界内にいる、または壁が透けている状態の場合にチェック
        if (InSecretArea && currentAttack != AttackType.Which)
        {
            Debug.Log("変身検知: 非魔女状態のため壁を即時復帰します。");
            // 非魔女がエリア内にいる場合、壁を元に戻す（SOLIDにする）
            // FadeOut=falseでコライダーを有効にし、見た目を不透明に戻す
            StartCoroutine(FadeWalls(false));
            InSecretArea = false; // 状態をリセット
        }
    }

    */

    //すり抜け床の上にいるかどうかを判定する関数 すり抜け床用
    bool IsOnPlatform()
    {
        // Platformレイヤーだけを判定
        int platformMask = 1 << LayerMask.NameToLayer("Platform");

        // 親切に少し長めにRaycast（下方向）
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, platformMask);

        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.cyan);

        return hit.collider != null;
    }

    //プレイヤーのコライダーを一瞬だけOFFにするコルーチン　すり抜け床用
    IEnumerator DisableColliderMomentarily()
    {
        // プレイヤーのCollider2D（BoxCollider2Dなど）
        Collider2D col = GetComponent<Collider2D>();

        col.enabled = false;      // 一瞬無効化する
        yield return new WaitForSeconds(0.2f); // 落ちる時間だけ待つ
        col.enabled = true;       // 元に戻す
    }
}