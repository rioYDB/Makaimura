using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI.Imageを使うためこれは残す
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
// using Unity.VisualScripting; // 使っていない場合は削除してOK
using System; // ★この行が原因で'Image'が曖昧になるため、削除します (必要なければ)
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
    public LayerMask Ground;                                                        //地面を判別するオブジェクトレイヤー
    public LayerMask LadderLayer;                                                   //hasigoを判別するオブジェクトレイヤー

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

    private int HP = 2;                                                                 //HP
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


    // ★追加：SetGravityコンポーネントへの参照
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

    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------------------------------------
    //はしご登り関連の追加変数
    public float ladderClimbSpeed = 3.0f; // はしごを登る速度
    private bool isClimbingLadder = false; // はしごを登っている最中かどうか
    private bool canClimbLadder = false;   // はしごに触れていて、登れる状態にあるかどうか
    private float originalGravityScale;    // 元の重力値を保持
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




    void Start()
    {
        //アタッチされているComponentを取得
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = Color.green;                                     //プレイヤーの色を緑色にする

        // SpriteRendererコンポーネントを取得します
        image = GetComponent<Image>();
        // SpriteのSpriteRendererコンポーネントを取得
        sr = gameObject.GetComponent<SpriteRenderer>();
        // 初期状態（Human）のアニメーションセットを使用
        SetCurrentAnimSet();

        //元の重力値を保持
        originalGravityScale = rb.gravityScale;

        //しゃがみ機能の初期設定
        OriginColliderSize = bc.size;
        OriginClliderOffset = bc.offset;

        OriginLocalScale = transform.localScale;

        BulletChange("Human");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            IsJumping = false;
        }

        // はしごを登る処理を最優先
        if (isClimbingLadder)
        {
            ClimbLadder(); // はしごを登る処理
            return; // はしごを登っている間は他の動作をスキップ
        }

        //移動処理
        if (IsSquat == false )
        {
            Move();
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LadderLayer);

        //ジャンプ処理
        if ((IsGrounded() == true || hit.collider != null) && IsSquat == false && !isClimbingLadder)
        {
            Jump();
        }

        //Zキーが押されたら
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))
        {
            Attack();
        }


        // はしごに触れていて、かつ上方向の入力がある場合
        if (canClimbLadder && (Input.GetAxisRaw("Vertical") > 0.3f))
        {
            StartClimbingLadder();
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

        //ゴールオブジェクトに触れたらゴールシーンに切り替わる
        if (collision.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene("Goal");
        }

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

        //魔女が隠し空間に入れるようにする
        if (currentAttack == AttackType.Which && ((1 << collision.gameObject.layer) & SecretArea) != 0)
        {
            Debug.Log("隠しエリアクランクインです");
            if (!InSecretArea) // 既に入っている状態ではない場合のみ実行
            {
                InSecretArea = true;
                StartCoroutine(FadeWalls(true)); // 壁を透かすコルーチンを開始
            }
        }

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
        }

        //EnemyとEnemyBulletに当たったらプレイヤーを破壊する
        if ((collision.gameObject.tag == "AttackZone" || collision.gameObject.tag == "EnemyBullet"))
        {

            playerHP(1);
        }
    }

    //はしごから離れた処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimbLadder = false;
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
            
            if (gameObject.activeInHierarchy) // 親を含めてHierarchy上でアクティブか
            {
                StartCoroutine(FadeWalls(false)); // 壁を元に戻すコルーチンを開始
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
        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            

            //プレイヤーの状態に併せてジャンプ力を変更する
            float currentJumpPower = jumpPower;

            //狼男の状態だとジャンプ力が上がる
            if (currentAttack == AttackType.Okami)
            {
                currentJumpPower *= 1.3f;
            }

            //ヴァンパイア状態だとジャンプ力が少し上がる
            if (currentAttack == AttackType.Vampire)
            {
                currentJumpPower *= 1.15f;
            }



            rb.AddForce(Vector2.up * jumpPower);
            //ジャンプ状態にする
            IsJumping = true;

            //ジャンプ中は移動速度を制限する
            Moveinput *= 0.7f;

            
        }
    }


    //関数名：Move()
    //用途：移動処理
    //引数：なし
    //戻り値：なし
    void Move()
    {
        //足元が梯子かどうかチェック
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LadderLayer);
        bool onLadder = false;



        //真下に梯子（はしご）がある
        if (hit.collider != null && !canClimbLadder)
        {
            onLadder = true;
            GetComponent<SetGravity>().IsEnable = false;
            rb.gravityScale = 0f; // 重力を無効にする
            rb.linearVelocityY = 0.0f;

            //下キーが押されている
            if (Input.GetAxisRaw("Vertical") < -0.3f)
            {
                //梯子（はしご）モードに遷移
                StartClimbingLadder();
                return;

            }

        }
        else
        {
            GetComponent<SetGravity>().IsEnable = true;
            rb.gravityScale = originalGravityScale;
        }

        //地上にいないときは入力を受け付けない
        if (IsGrounded() == true || onLadder)
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
            }
        }




        float currentMoveSpeed = moveSpeed;  //moveSpeedを一時的な変数に保管
        float PlayerDirection = transform.localScale.x > 0 ? 1f : -1f; // Move()内で使うplayerDirectionを定義


        //狼男になった時に壁を登れるようになる
        if (currentAttack == AttackType.Okami)
        {
            //移動速度アップ
            currentMoveSpeed *= 1.5f;


            //横方向にレイを飛ばし、壁があるか判定する
            float WallCheck = 0.01f; //壁を判定する距離

            Vector2 wallRayOrigin = (Vector2)transform.position + bc.offset + new Vector2(bc.size.x / 2f * PlayerDirection, 0f);
            RaycastHit2D wallHit = Physics2D.Raycast(wallRayOrigin, Vector2.right * PlayerDirection, WallCheck, Ground); // 進行方向へRayを飛ばす


            Debug.DrawRay(wallRayOrigin, Vector2.right * PlayerDirection * WallCheck, Color.blue); // デバッグ用

            // 壁に触れていて、かつ上方向の入力がある場合
            if (wallHit.collider != null && Input.GetAxisRaw("Vertical") > 0.1f)
            {

                //梯子（はしご）モードに遷移
                StartClimbingLadder();


                // 壁に沿って登るようにY軸速度を調整
                // climbSpeedは移動速度currentMoveSpeedに比例させると良い
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentMoveSpeed * 0.8f); // 登る速度を調整
            }
            else
            {
                GetComponent<SetGravity>().IsEnable = true;

                // 壁に触れていないか、上入力がない場合は重力に従う
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



            // Wキー長押しで飛行モードに入る例
            if (Input.GetKey(KeyCode.W) && !isClimbingLadder) // はしご登り中は飛ばない
            {

                FlyingTime += Time.deltaTime;
                Debug.Log(("経過") + FlyingTime);

                //Gravityのスクリプトも止める
                GetComponent<SetGravity>().IsEnable = false;

                rb.gravityScale = 0f; // 重力を無効にする



                // Y軸の速度は0に固定する
                // 左右の移動はMoveinputで制御、Y軸はInput.GetAxisRaw("Vertical")の影響を受けないようにする
                rb.linearVelocity = new Vector2(Moveinput * currentMoveSpeed, 0f); // Y軸の速度を0に固定！
            }

            if (FlyingTime >= 2.0f) //3秒たったら重力を元に戻し徐々に下降していく
            {
                SpriteRenderer.color = Color.magenta;   //プレイヤーに下降している状態とわかりやすく伝えるために色を変える

                GetComponent<SetGravity>().IsEnable = true;
                rb.gravityScale = originalGravityScale;
            }

            else if (Input.GetKeyUp(KeyCode.W)) // Wキーを離したら重力を元に戻す
            {
                PlayerColor();                 //元の体力状態の色に戻す
                GetComponent<SetGravity>().IsEnable = true;
                rb.gravityScale = originalGravityScale;



            }
        }


        //プレイヤーを移動させる
        rb.linearVelocity = new Vector2(Moveinput * currentMoveSpeed, rb.linearVelocity.y);



        if (Moveinput != 0)
        {
            Movedirection = new Vector2(Moveinput, 0f);
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

        }



    }

    //関数名：BulletChange()
    //用途：攻撃処理
    //引数：string
    //戻り値：なし
    private void BulletChange(string BulletName)
    {
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
        //下方向にrayを飛ばして、指定したレイヤーのオブジェクトと接触しているかどうか判別する
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, Ground);

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
    void StartClimbingLadder()
    {
        isClimbingLadder = true;
        rb.gravityScale = 0f; // 重力を無効にする
        rb.linearVelocity = Vector2.zero; // 現在の移動をリセット

        //Gravityのスクリプトも止める
        GetComponent<SetGravity>().IsEnable = false;
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

        //地上にいる、かつ、下方向に入力したら、梯子を解除してよい
        //もしくはジャンプボタンを押したら梯子を解除
        if ((verticalInput < -0.1f && IsGrounded()) || Input.GetButtonDown("Jump"))
        {
            StopClimbingLadder();
            return;
        }

        Debug.Log("Input = " + verticalInput + " velocity = " + rb.linearVelocity + " verticalInput * ladderClimbSpeed= " + verticalInput * ladderClimbSpeed);
        rb.linearVelocity = new Vector2(0f, verticalInput * ladderClimbSpeed);
    }


    //関数名：StopClimbingLadder()
    //用途：はしご登り終了処理
    //引数：void
    //戻り値：なし
    void StopClimbingLadder()
    {
        isClimbingLadder = false;
        rb.gravityScale = originalGravityScale; // 重力を元に戻す (この変数は使われないが念のため残す)

        // ★★★修正：X軸の速度も0にリセットするで！★★★
        rb.linearVelocity = Vector2.zero; // X軸速度もY軸速度も0にする

        // 地面にいる場合は、IsJumpingをfalseにリセット
        IsJumping = !IsGrounded();



        //Gravityのスクリプトは再開
        GetComponent<SetGravity>().IsEnable = true;
    }

    //関数名：playerHP()
    //用途：はしご登り終了処理
    //引数：void
    //戻り値：なし
   public void  playerHP(int Damege)
    {
        if (!IsInvincible)
        {
            HP -= Damege;
            Debug.Log("痛い");


            PlayerColor();

            //// ノックバック処理
            //Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
            //GetComponent<Rigidbody2D>().AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);
            // 無敵状態を開始
            StartInvincibility();
        }
    }


    //火柱を放つコルーチン
    IEnumerator SpawnFirePillarsRoutine(Vector3 basePosition, float playerDirection, int count, float delay, float spread, LayerMask Ground)
    {
        //for (int i = 0; i < count; i++)
        //{
        //    float offsetX = (i - (count - 1) / 2.0f) * spread * playerDirection;
        //    // 火柱を生成するX座標は、プレイヤーの目の前を中心に広がるように計算する
        //    float targetX = basePosition.x + offsetX;


        //    float groundY = basePosition.y; // 一旦プレイヤーのY座標を仮の地面として設定

        //    // 弾を真下（Vector2.down）に飛ばして、地面（groundLayer）に当たるかをチェックする
        //    // ここでのRaycastの長さ（例: 10f）は、プレイヤーの高さから確実に地面に届くように長めにする
        //    RaycastHit2D hit = Physics2D.Raycast(new Vector2(targetX, basePosition.y + 10f), Vector2.down, 20f, Ground); // ★Raycastの開始位置と長さを調整

        //    if (hit.collider != null)
        //    {
        //        // 地面が見つかったら、その地面の上端のY座標を取得する
        //        groundY = hit.point.y; // Raycastが当たった場所のY座標
        //    }
        //    else
        //    {
        //        // もし地面が見つからへんかったら（空中にRaycastが届かんとか）、デバッグログを出して、
        //        // 元のプレイヤーのY座標を使うとか、何らかのフォールバック処理を考える
        //        // 今回はDebug.Logしとくわ。
        //        Debug.LogWarning("火柱の足元に地面が見つかりませんでした！ 火柱はプレイヤーの高さから出ます。", this);
        //    }

        //    // 火柱を生成する最終的な位置やで！Y座標は地面の高さを使う
        //    Vector3 firePillarSpawnPos = new Vector3(targetX, groundY, basePosition.z);

        //    // 火柱を生成するで
        //    GameObject pillarInstance = Instantiate(VampireWeapon, firePillarSpawnPos, Quaternion.identity);

        //    // 火柱の見た目もプレイヤーの向きに合わせるで (この部分は変更なし)
        //    SpriteRenderer pillarSr = pillarInstance.GetComponent<SpriteRenderer>();
        //    if (pillarSr != null)
        //    {
        //        pillarSr.flipX = (playerDirection == -1);
        //    }
        //    else
        //    {
        //        Vector3 pillarScale = pillarInstance.transform.localScale;
        //        pillarScale.x = Mathf.Abs(pillarScale.x) * playerDirection;
        //        pillarInstance.transform.localScale = pillarScale;
        //    }

        //    // 次の火柱が出るまで少し待つ
        //    yield return new WaitForSeconds(delay);
        //}

        // ★★★修正：火柱の基準となるY座標をプレイヤーの足元から取得する★★★
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

    //魔女の隠しエリアのかべのコルーチン
    IEnumerator FadeWalls(bool FadeOut)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag(FadeWallTag); // FadingWallタグの壁を全て取得

        foreach (GameObject wall in walls)
        {
            SpriteRenderer wallRenderer = wall.GetComponent<SpriteRenderer>();
            if (wallRenderer != null)
            {
                Color startColor = wallRenderer.color;
                Color targetColor = startColor;

                if (FadeOut) // 透かす場合
                {
                    targetColor.a = 0.3f; // 完全に透明にせず、少しだけ見えるように (0.0fで完全透明)
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
                    wallRenderer.color = Color.Lerp(startColor, targetColor, progress); // 色を徐々に変化させる
                    yield return null; // 1フレーム待つ
                }
                wallRenderer.color = targetColor; // 最終的な色を確実に適用
            }

        }

    }
}