using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#


public class XXXCtrl : BaseCharCtrl {

    //控制參數
    public float HP       				   = 50.0f;
	public float MaxHP                     = 50.0f;
	public float MP                        = 50.0f;
	public float MaxMP                     = 50.0f;
	public float characterSpeed  		   = 5.0f;
	public float characterAcceleration     = 1.0f;
	public float dashSpeed       		   = 10.0f;
	
	public float dashTime       		   = 0.5f;
	public float dashAcceleration 		   = 1.0f;
	public float dashCooldown    		   = 0.0f;
	public float jumpForce       		   = 600.0f;
	
	public float doubleJumpForce       	   = 500.0f;
	public float BackDamageJumpForce       = 300.0f;

	public float DashCancelMP              = 5.0f;
	public float DMGToMPRate               = 1.5f;
	public float HPValueToRecoverMP        = 20.0f;
	public float MPRecoverTimeRate         = 0.5f;
	       float MPRecoverCTime            = 0.0f;

    public float WallATKBackForce          = 600.0f;

    public float invincibleTime            = 0.0f;
	       float InvincibleCtime           = 0.0f;

	public float UntouchableMoveSpeed      = 10.0f;
	public float UntouchableDashSpeed      = 15.0f;
	public float UntouchableJumpForce      = 700.0f;

    protected float DeadDestroyTime           = 2.0f;
	          float DeadDestroyCTime         = 0.0f;

	public bool[] hittedPlayer = new bool[4];

    //內部控制變數
    [System.NonSerialized] public AnimatorStateInfo stateInfo;
    [System.NonSerialized] public float deltaTime     = 0.0f;

    private Transform _transform           = null;

    private float initAcceleration		   = 1.0f;
    private float initSpeed       		   = 5.0f;
    private float initUntouchableMoveSpeed = 10.0f;
    private float initJumpForce            = 600.0f;
    private float initDashSpeed            = 10.0f;

    bool bInvincibleColliderCtrl = true;

    //遊戲變數
    public int teamNum;

    //狀態變數
    [System.NonSerialized] public bool  MoveEnable      		   = true;
    [System.NonSerialized] public bool  CancelDashEnable           = true;
    [System.NonSerialized] public bool  DashEnabled     		   = true;
    [System.NonSerialized] public bool  AirJumpEnabled  		   = false;
	[System.NonSerialized] public bool  isJumping 		 		   = false;
	[System.NonSerialized] public bool  isDashing        		   = false;
	[System.NonSerialized] public bool  isDashJump	     		   = false;
    [System.NonSerialized] public bool  isAttacking     		   = false;
    [System.NonSerialized] public bool  isAirAttacking     	       = false;
	[System.NonSerialized] public bool  isUsingULT                 = false;
	[System.NonSerialized] public bool  isUsingULTPrev             = false;
    [System.NonSerialized] public bool  isDamaged       		   = false;
    [System.NonSerialized] public bool  isKnockOuted    		   = false;
    [System.NonSerialized] public bool  isKnockDown     		   = false;
    [System.NonSerialized] public bool  isKnockBack                = false;
    [System.NonSerialized] public bool  isTouchingWall  		   = false;
    [System.NonSerialized] public bool  isTriggeredWall  		   = false; // 被卡牆狂歐觸發用
	[System.NonSerialized] public bool  isInvincible               = false;
    [System.NonSerialized] public bool  stopKnockBack              = false;
	[System.NonSerialized] public bool  backDMGSwitch   		   = false;
	[System.NonSerialized] public bool  wallBackDamage  		   = false;
	[System.NonSerialized] public bool  isDead      		       = false;
	[System.NonSerialized] public bool  isDeadDown                 = false;
    [System.NonSerialized] public bool  hakki                      = false; // 霸體
    [System.NonSerialized] public bool  UpState                    = false;
    [System.NonSerialized] public bool  DownState                  = false;



    protected int   hitPlayer = 0;   //最後一次打到自己的玩家

    [System.NonSerialized] public bool  isUntouchable = false;
    [System.NonSerialized] public bool  canAttackMove = false;

    //接技專用變數
    [System.NonSerialized] public bool isInCombo = false;

    public string nextATKName;

    [System.NonSerialized] public bool CNormal2ATK = false;
    [System.NonSerialized] public bool CUpATK      = false;
    [System.NonSerialized] public bool CDownATK    = false;

	float comboDelayCTime = 0.0f;
	float comboDelayTime = 0.05f;

	//相機晃動功能變數
	float shakeTime;
	float shakePower;
	float MaxShakePower = 0.15f;
	float MaxShakeTime = 0.5f; 
	float OriShakePower;

    //動畫判斷變數
    public readonly static int ANISTS_Idle 	       = Animator.StringToHash("XXX_Idle");
	public readonly static int ANISTS_Move 	       = Animator.StringToHash("XXX_Move");
	public readonly static int ANISTS_Jump         = Animator.StringToHash("XXX_Jump");
	public readonly static int ANISTS_AirJump      = Animator.StringToHash("XXX_AirJump");
	public readonly static int ANISTS_Dash         = Animator.StringToHash("XXX_Dash");
	public readonly static int ANISTS_DashJump     = Animator.StringToHash("XXX_DashJump");
	public readonly static int ANISTS_OnAir        = Animator.StringToHash("XXX_OnAir");
	public readonly static int ANISTS_Attack1      = Animator.StringToHash("XXX_Attack1");
	public readonly static int ANISTS_Attack2      = Animator.StringToHash("XXX_Attack2");
	public readonly static int ANISTS_Attack3      = Animator.StringToHash("XXX_Attack3");
	public readonly static int ANISTS_DownAttack   = Animator.StringToHash("XXX_DownAttack");
	public readonly static int ANISTS_UpAttack     = Animator.StringToHash("XXX_UpAttack");
	public readonly static int ANISTS_AirAttack    = Animator.StringToHash("XXX_AirAttack");
	public readonly static int ANISTS_DashAttack   = Animator.StringToHash("XXX_DashAttack");
	public readonly static int ANISTS_SPAttack1    = Animator.StringToHash("XXX_SPAttack1");
	public readonly static int ANISTS_SPAttack2    = Animator.StringToHash("XXX_SPAttack2");
	public readonly static int ANISTS_SPAttack3    = Animator.StringToHash("XXX_SPAttack3");
	public readonly static int ANISTS_SPAttack4    = Animator.StringToHash("XXX_SPAttack4");
	public readonly static int ANISTS_SPAttack5    = Animator.StringToHash("XXX_SPAttack5");
	public readonly static int ANISTS_SPAttack6    = Animator.StringToHash("XXX_SPAttack6");
    public readonly static int ANISTS_SPAttack7    = Animator.StringToHash("XXX_SPAttack7");
    public readonly static int ANISTS_SPAttack8    = Animator.StringToHash("XXX_SPAttack8");
    public readonly static int ANISTS_SPAttack9    = Animator.StringToHash("XXX_SPAttack9");
    public readonly static int ANISTS_SPAttack10   = Animator.StringToHash("XXX_SPAttack10");
    public readonly static int ANISTS_SPAttack11   = Animator.StringToHash("XXX_SPAttack11");
    public readonly static int ANISTS_SPAttack12   = Animator.StringToHash("XXX_SPAttack12");
	public readonly static int ANISTS_ULT1         = Animator.StringToHash("XXX_ULT1");
	public readonly static int ANISTS_ULT2         = Animator.StringToHash("XXX_ULT2");
	public readonly static int ANISTS_Damaged1     = Animator.StringToHash("XXX_Damaged1");
	public readonly static int ANISTS_Damaged2     = Animator.StringToHash("XXX_Damaged2");
	public readonly static int ANISTS_KnockOutSide = Animator.StringToHash("XXX_KnockOutSide");
	public readonly static int ANISTS_KnockOutUp   = Animator.StringToHash("XXX_KnockOutUp");
	public readonly static int ANISTS_KnockDown    = Animator.StringToHash("XXX_KnockDown");
	public readonly static int ANISTS_DamagedDown1 = Animator.StringToHash("XXX_DamagedDown1");
	public readonly static int ANISTS_DamagedDown2 = Animator.StringToHash("XXX_DamagedDown2");
	public readonly static int ANISTS_DamagedDown3 = Animator.StringToHash("XXX_DamagedDown3");
	public readonly static int ANISTS_DamagedDown4 = Animator.StringToHash("XXX_DamagedDown4");
	public readonly static int ANISTS_DamagedDown5 = Animator.StringToHash("XXX_DamagedDown5");
	public readonly static int ANISTS_Dead         = Animator.StringToHash("XXX_Dead");

	public GameObject effectObjectJumpSmoke;
    public GameObject effectObjectAirJumpSmoke;
    public GameObject effectObjectDashSmoke;
    public GameObject effectObjectKnockOutSmoke;

    public GameObject effectObjectDeathEffectGround;

    public float initKSTime = 0.1f;
    float initCKSTime = 0.0f;

    //動畫編輯用變數

    enum State{
		normal,
		untouchable
	}
	State state;

    [System.NonSerialized] public bool atkInputEnabled = false;
    [System.NonSerialized] public volatile bool atkInputNow    = false;

    [System.NonSerialized] public bool stopAttackDash = false; //攻擊時滑動

    //hitPause
    [System.NonSerialized] public bool  animPause = false;
	public float animPauseTime  = 0.08f;
	       float animPauseCTime = 0.0f;
	       float tempKnockBackDir = 0.0f;
	       float tempKnockBackSpeed = 0.0f;

    //音效參數
    protected AudioSource audioCtrl;
    protected float SFXAudioVolume = 0.5f;

    public AudioClip jumpSE;
	public float jumpSEPitch;
	public AudioClip dashSE;
	public float dashSEPitch;
	public AudioClip knockDownSE;
	public float knockDownSEPitch;
	public AudioClip ULTChargeSE;
	public float ULTChargeSEPitch;


    //控制器震動
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState gamePadState;
    GamePadState prevState;

    public float padVibrationValue = 0.0f;
    public float padVibrationTime = 0.0f;

    //其他變數(防範BUG)
    float BugKnockDownBackCTime = 0.0f; // 防止倒地回歸不正 導致無法移動

    float deadToDestroyTime = 5.0f;    // 防止不預期沒有死亡
    float deadToDestroyCTime = 0.0f;

    //float gravityZeroTime = 5.0f;     // 防止不預期重力為0
    //float gravityZeroCTime = 0.0f;

    bool bknockOutSideMoveUp = true; // 側擊飛時往上調整距離用開關 碰到地重製
    float knockOutSideHitDir = 1.0f; // 側擊飛時瞬間轉身紀錄方向用
    bool bknockOutSideMoveDown = false; // 側擊飛時往下調整距離用開關 碰到地重製

    [System.NonSerialized] public bool stopDash = false; // 特殊攻擊時的衝刺實作 用此來跳離

    //關卡特殊用變數
    bool bCursedNoteSprite = false;

    //攻擊碰撞器專用
    [System.Serializable]
    public struct ATKColliderData
    {
        public int ATK;
        public GameObject effectObject;
        public float knockOutTime;
        public float knockBackSpeedX;//擊飛速度 包含KnockBack KnockDamagedDown KnockOut
        public float hitForceY;
        public bool canPauseAnim;

        public bool sideType;
        public bool twoSide;

        public AudioClip hittedSE;
        public float hittedSEPitch;

        public float knockOutDecressSpeed;
        public float knockOutGravity;
    }
    protected ATKColliderData ATKData;//腳色目前持有的攻擊的資訊
    public Dictionary<string, ATKColliderData> AtkDataDic = new Dictionary<string, ATKColliderData>();

    //留到Special區域一起存到dictionary裡
    public ATKColliderData Attack1ATK;
    public ATKColliderData Attack2ATK;
    public ATKColliderData Attack3ATK;
    public ATKColliderData UpAttackATK;
    public ATKColliderData DownAttackATK;
    public ATKColliderData DashAttackATK;
    public ATKColliderData AirAttackATK;
    public ATKColliderData ULT1ATK;
    public ATKColliderData ULT2ATK;

    ///
    //===============================================================================程式 Monobehavior基本功能==============================================================================
    ///

    protected virtual void Awake() {
		BaseAwake ();
		audioCtrl = GetComponent<AudioSource>();
	}
	
	protected virtual void Start() {
        _transform = this.transform;

        if (PlayerNUM == 2 || PlayerNUM == 4)actionFlip();

		initSpeed = characterSpeed;
		initAcceleration = characterAcceleration;
		initJumpForce = jumpForce;
		initDashSpeed = dashSpeed;
		MaxHP = HP;
		MaxMP = MP;

		canAttackMove = false;

        //遊戲變數用
        //if(!gameCtrl.TeamMode)teamNum = PlayerNUM;
        if (!gameCtrl.MPEnable)
        {
            MP = 0;
            MaxMP = 0;
        }
        if (gameCtrl.UnlimitMP)
        {
            MP = 999999;
            MaxMP = 999999;
            MPRecoverTimeRate = 0.01f;
        }


        //控制器震動 初期設定
        playerIndex = (PlayerIndex)PlayerNUM - 1;
       

    }


    protected virtual void FixedUpdate() {
		BaseFixedUpdate();
        if (isTouchingWall)
        {

        }

        //====控制器震動============
        #region
        if (gamePadState.IsConnected)
        {
            if (!gameCtrl.GameOver)
            {
                GamePad.SetVibration(playerIndex, padVibrationValue, padVibrationValue);
            }
        }
        #endregion
    }

    protected virtual void Update() {
		stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        deltaTime = Time.deltaTime; // 優化效能用 全內部使用同一個時間變化

        //控制器震動
        #region
        //註冊
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)PlayerNUM - 1;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
        }

        prevState = gamePadState;
        gamePadState = GamePad.GetState(playerIndex);

        //震動
        if (padVibrationTime > 0.0f)
        {
            if (!gameCtrl.GameOver) padVibrationTime -= deltaTime;
            else padVibrationTime -= deltaTime / gameCtrl.SlowModeTimeScale;
        }
        else
        {
            padVibrationTime = 0.0f;
            padVibrationValue = 0.0f;
        }
        #endregion

        //動畫暫停
        if (animPause) {
			animator.speed = 0;
			animPauseCTime += deltaTime;
			if(animPauseCTime >= animPauseTime){
				animPauseCTime = 0.0f;
				animPause = false;
				gravityScale = 1.0f;
				animator.speed = 1;
			}
		}

		//避免重複攻擊判斷
		if (!isAttacking && !isAirAttacking) {
			reDetectDMG();
		}

		//著地確認
		if (grounded) {
			AirJumpEnabled = false;
			DashEnabled = true;
			canAttackMove = false;
            if (stateInfo.shortNameHash == ANISTS_Idle)
            {
                bknockOutSideMoveUp = true;
                bknockOutSideMoveDown = false;
            }
        }

        
        //二段跳判定
        if (!isDashing && !isDashJump) {
			if (!grounded && groundedPrev) {
				AirJumpEnabled = true;
			}
		}

		//ULT處理
		if (isUsingULT && !isUsingULTPrev) {
			gameCtrl.isUsingULT += 1;
		}
		
		if (!isUsingULT && isUsingULTPrev) {
			gameCtrl.isUsingULT -= 1;
		}
		isUsingULTPrev = isUsingULT;

		//起跳中
		if (stateInfo.shortNameHash == ANISTS_Jump) {
			isJumping = true;
		}
		else {
			isJumping = false;
		}

		//是否攻擊中
		if (stateInfo.shortNameHash == ANISTS_Attack1       ||
		    stateInfo.shortNameHash == ANISTS_Attack2       ||
		    stateInfo.shortNameHash == ANISTS_Attack3       ||
		    stateInfo.shortNameHash == ANISTS_DownAttack    ||
		    stateInfo.shortNameHash == ANISTS_UpAttack      ||
		    stateInfo.shortNameHash == ANISTS_DashAttack    ||
		    stateInfo.shortNameHash == ANISTS_ULT1          ||
		    stateInfo.shortNameHash == ANISTS_ULT2          ||
		    stateInfo.shortNameHash == ANISTS_SPAttack1     ||
		    stateInfo.shortNameHash == ANISTS_SPAttack2     ||
		    stateInfo.shortNameHash == ANISTS_SPAttack3     ||
		    stateInfo.shortNameHash == ANISTS_SPAttack4     ||
		    stateInfo.shortNameHash == ANISTS_SPAttack5     ||
		    stateInfo.shortNameHash == ANISTS_SPAttack6     ||
            stateInfo.shortNameHash == ANISTS_SPAttack7     ||
            stateInfo.shortNameHash == ANISTS_SPAttack8     ||
            stateInfo.shortNameHash == ANISTS_SPAttack9     ||
            stateInfo.shortNameHash == ANISTS_SPAttack10    ||
            stateInfo.shortNameHash == ANISTS_SPAttack11    ||
            stateInfo.shortNameHash == ANISTS_SPAttack12) {
			isAttacking = true;
			animator.SetBool("IsAttacking", true);
			if(!canAttackMove)speedX = 0.0f;
		}
		else if (stateInfo.shortNameHash == ANISTS_AirAttack) { //攻擊移動
			isAirAttacking = true;
			animator.SetBool("IsAttacking", true);
		}
		else {
            if (isAttacking != false)  // 動畫從正常轉成攻擊時 會有一禎沒先計算到
            {
                CNormal2ATK = false;
                CUpATK = false;
                CDownATK = false;
            }
            isAttacking = false;
			isAirAttacking = false;

			if(isInCombo){ //防止卡在combo狀態
				comboDelayCTime += deltaTime;
				if(comboDelayCTime >= comboDelayTime){
					comboDelayCTime = 0.0f;
					isInCombo = false;
				}
			}
			stopAttackDash = true;
			

			atkInputEnabled = false;
			atkInputNow = false;
			animator.SetBool("IsAttacking", false);
		}

		//是否可以衝刺取消的技能
		if (stateInfo.shortNameHash == ANISTS_SPAttack1  ||
			stateInfo.shortNameHash == ANISTS_SPAttack2  ||
			stateInfo.shortNameHash == ANISTS_SPAttack3  ||
			stateInfo.shortNameHash == ANISTS_SPAttack4  ||
		    stateInfo.shortNameHash == ANISTS_SPAttack5  ||
		    stateInfo.shortNameHash == ANISTS_SPAttack6  ||
            stateInfo.shortNameHash == ANISTS_SPAttack7  ||
            stateInfo.shortNameHash == ANISTS_SPAttack8  ||
            stateInfo.shortNameHash == ANISTS_SPAttack9  ||
            stateInfo.shortNameHash == ANISTS_SPAttack10 ||
            stateInfo.shortNameHash == ANISTS_SPAttack11 ||
            stateInfo.shortNameHash == ANISTS_SPAttack12 ||
            stateInfo.shortNameHash == ANISTS_DashAttack ||
		    stateInfo.shortNameHash == ANISTS_ULT1||
		    stateInfo.shortNameHash == ANISTS_ULT2) {

			CancelDashEnable = false;

		}
		else{
			CancelDashEnable = true;
		}

		//是否使用ULT
		if (stateInfo.shortNameHash == ANISTS_ULT1 || stateInfo.shortNameHash == ANISTS_ULT2) {
			isUsingULT = true;
		}
		else {
			isUsingULT = false;
		}

		//是否受傷害中
		if (stateInfo.shortNameHash == ANISTS_Damaged1 ||
		    stateInfo.shortNameHash == ANISTS_Damaged2
		    ) {
			isDamaged = true;
			animator.SetBool("IsDamaged", true);
			speedX = 0.0f;
		} 
		else {
			isDamaged = false;
			animator.SetBool("IsDamaged", false);
		}

		
		//是否倒地
		if(stateInfo.shortNameHash == ANISTS_KnockDown) {
			wallBackDamage = false;
			BugKnockDownBackCTime = 0.0f;
		}
		else {
			if(isKnockDown == true){
				BugKnockDownBackCTime += deltaTime;
				if(BugKnockDownBackCTime >= 0.1f){
					isKnockDown = false;
					animator.SetBool("IsKnockDown", false);
					BugKnockDownBackCTime = 0.0f;
				}
			}
		}
		
		//無法移動狀態
		if (stateInfo.shortNameHash == ANISTS_KnockDown|| // 防止倒地還可移動
			isKnockOuted                               ||
            isKnockDown                                ||
            isDamaged                                  ||
		    isDead                                       ) {
			MoveEnable = false;
		}
		else {
			MoveEnable = true;
		}

        //=========特效相關=================================

        if (stateInfo.shortNameHash == ANISTS_KnockOutSide)
        {
            initCKSTime += deltaTime;
            if (initCKSTime >= initKSTime)
            {
                Instantiate(effectObjectKnockOutSmoke, new Vector3(_transform.position.x + 0.5f * _transform.lossyScale.x, _transform.position.y - 0.2f * _transform.lossyScale.y, _transform.position.z), Quaternion.identity);
                initCKSTime = 0.0f;
            }
        }
        else if(stateInfo.shortNameHash == ANISTS_KnockOutUp)
        {
            initCKSTime += deltaTime;
            if (initCKSTime >= initKSTime)
            {
                Instantiate(effectObjectKnockOutSmoke, new Vector3(_transform.position.x - 0.1f * _transform.lossyScale.x, _transform.position.y - 0.6f * _transform.lossyScale.y, _transform.position.z), Quaternion.identity);
                initCKSTime = 0.0f;
            }
        }
        else initCKSTime = 0.0f;

        //=========腳色相關==================================
        //==死亡====

        if (HP <= 0){
			isDead = true;
			MoveEnable = false;
			animator.SetBool("IsDead", true);

			//防止不預期沒有死亡
			deadToDestroyCTime += deltaTime;
			if(deadToDestroyCTime >= deadToDestroyTime){
				dead();
			}
			//

			if(!isKnockOuted && !isKnockDown && !isDeadDown){
				isDeadDown = true;
				StopAllCoroutines();
				actionKnockDamagedDown(0, 2.0f, -dir, 3.0f, 350.0f, 0 , initGravity,0.0f);
			}

			if(isKnockOuted){
				isDeadDown = true;
			}

			if(isDeadDown && grounded){
				DeadDestroyCTime += deltaTime;
				if(DeadDestroyCTime >= DeadDestroyTime){
					dead();
				}
			}
		}
		
		//==自動回復MP==
		if (HP <= HPValueToRecoverMP && MP <= MaxMP) {
			MPRecoverCTime += deltaTime;
			if (MPRecoverCTime >= MPRecoverTimeRate) {
				MP += 1;
				MPRecoverCTime = 0.0f;
			}
		} 
		else if (MP <= MaxMP && HP > HPValueToRecoverMP) {
			MPRecoverCTime += deltaTime;
			if (MPRecoverCTime >= MPRecoverTimeRate * 2.5f) {
				MP += 1;
				MPRecoverCTime = 0.0f;
			}
		}

        //控制HP MP
        if (MP <= 0)
        {
            MP = 0.0f;
        }

        if (HP >= MaxHP) HP = MaxHP;
        if (MP >= MaxMP) MP = MaxMP;
        //==無敵狀態控制==
        if (isUntouchable) {
			state = State.untouchable;
		}
		else
		{
			state = State.normal;
		}

		if(state == State.untouchable){
			characterSpeed = UntouchableMoveSpeed;
			dashSpeed = UntouchableDashSpeed;
			jumpForce = UntouchableJumpForce;
		}

		if (state == State.normal && !isDashJump) {
			characterSpeed = initSpeed;
			dashSpeed = initDashSpeed;
			jumpForce = initJumpForce;
		}

		//==無敵=====

		if (isInvincible) {
				
			if (InvincibleCtime <= invincibleTime) {
				InvincibleCtime += deltaTime;
			}
			else{
				InvincibleCtime = 0.0f;
				isUntouchable = false;
				isInvincible = false;
			}
		}

		//==倒地內部變數控制====
		if (isKnockDown) {
			speedX = 0.0f; // 防止滑動
		}

        
		//==預防用==========
        /*
		if(_rigidbody.gravityScale == 0){
			gravityZeroCTime += deltaTime;
			if(gravityZeroCTime >= gravityZeroTime){
				_rigidbody.gravityScale = initGravity;
				gravityZeroCTime = 0.0f;
			}
		}
		else{
			gravityZeroCTime = 0.0f;
		}
        */

        //無敵狀態碰撞器
        if (isInvincible || isUntouchable || isKnockDown || isDead)
        {
            if (bInvincibleColliderCtrl)
            {
                bInvincibleColliderCtrl = false;
                transform.Find("Collider_ReceiveDMG").GetComponent<BoxCollider2D>().enabled = false;

            }
        }
        else
        {
            if (!bInvincibleColliderCtrl)
            {
                bInvincibleColliderCtrl = true;
                transform.Find("Collider_ReceiveDMG").GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //===特殊關卡用=============================================
        //魔鏡城詛咒
        if (isCursed)
        {
            if (!bCursedNoteSprite)
            {
                bCursedNoteSprite = true;
                transform.Find("NoteSprite").GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            if (bCursedNoteSprite)
            {
                bCursedNoteSprite = false;
                transform.Find("NoteSprite").GetComponent<SpriteRenderer>().enabled = false;
            }
        }

    }

    protected virtual void LateUpdate()
    {
        //動畫Update完之後 再更新玩家圖片 才能移動
        //腳色晃動功能
        if (shakeTime > 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakePower;
            _transform.Find("PlayerSprite").transform.localPosition = new Vector3(ShakePos.x, ShakePos.y, 0);
            shakeTime -= deltaTime;
        }
        else
        {
            _transform.Find("PlayerSprite").transform.localPosition = new Vector3(0, 0, 0);
        }
        
    }

    //===程式 動畫用程式==============================================

    public void EnableAttackInput() {
		atkInputEnabled = true;
	}

	public void NSetNextAttack(){     //接技專用
		if (atkInputNow == true) {
			atkInputNow = false;
			animator.Play(nextATKName);
		}
	}

	public void animStopKnockBack(){
		stopKnockBack = true;		
	}

    //為了符合地板位置與腳色圖片 側擊飛時會先轉身圖片才被打飛 會有一閃的不順暢感 側擊飛時的往上移動和轉身動畫交給動畫來呼叫方法執行
    public void knockOutSideMoveOn()
    {
        if (bknockOutSideMoveUp)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z);
            bknockOutSideMoveUp = false;
            bknockOutSideMoveDown = true;

        }
        if(dir == knockOutSideHitDir)
        {
            dir = dir * -1.0f;
            Vector3 theScale = _transform.localScale;
            theScale.x *= -1.0f;
            _transform.localScale = theScale;
        }
    }


    //===程式 基本動作====================================================

    // 移動
    public void actionMove(float n) {
		if(MoveEnable && !isDamaged && !isDashing && (!isAttacking || canAttackMove)){ // canMoveAttack 在SPAction控制開關狀態
			animator.SetFloat("SpeedX", Mathf.Abs(n));
			if (n != 0.0f) {
				if(Mathf.Abs(speedX) < characterSpeed){
					speedX = (Mathf.Abs(speedX) + characterAcceleration) * n;
				}
				else if(Mathf.Abs(speedX) >= characterSpeed){
					speedX = characterSpeed * n; 
				}
			} 
			else {
				speedX = 0.0f;
			}
		}
        else if (isDashing)
        {
            animator.SetFloat("SpeedX", Mathf.Abs(n));//編輯器會先更新 在此處理玩家輸入狀態以處理動畫
        }
		else {
			animator.SetFloat("SpeedX", 0);
		}

        if (isTouchingWall)
        {
            speedX = 0.0f;
        }

	}

	// 上下狀態判定
	public void actionState(float n){
		if (!isDamaged) {
			if(n > 0.99){
				UpState = true;
			}
			else if(n < -0.99){
				DownState = true;
			}
			else {
				UpState = false;
				DownState = false;
			}
		}
	}

	//跳躍
	public void actionJump() {
		//一般跳躍
		if (MoveEnable && grounded && !isDashing && !isDamaged && !isAttacking && !isAirAttacking) {
			isJumping = true;
			toSetUpForce = true;
			setUpForce = jumpForce;
			GameObject effect = Instantiate(effectObjectJumpSmoke, new Vector3(_transform.position.x - 0.1f * _transform.lossyScale.x ,_transform.position.y - 0.6f * _transform.lossyScale.y ,_transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
			PlaySEJumpSE();
		} 
		//二段跳
		else if (MoveEnable && !isKnockOuted && AirJumpEnabled && !grounded && !isAttacking && !isAirAttacking && !isDamaged) {
			toSetUpForce = true;
			setUpForce = doubleJumpForce;
            GameObject effect = Instantiate(effectObjectAirJumpSmoke, new Vector3(_transform.position.x - 0.1f * _transform.lossyScale.x, _transform.position.y - 0.6f * _transform.lossyScale.y, _transform.position.z), Quaternion.identity) as GameObject;
            effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
            AirJumpEnabled = false;
			PlaySEJumpSE();
		}
		//衝刺跳躍
		else if (MoveEnable && grounded && isDashing) {
			isDashJump = true;
			toSetUpForce = true;
			setUpForce = jumpForce;
			GameObject effect = Instantiate(effectObjectJumpSmoke, new Vector3(_transform.position.x - 0.1f * Mathf.Abs(_transform.lossyScale.x) ,_transform.position.y - 0.6f * _transform.lossyScale.y ,_transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
			StartCoroutine(dashJump());
			PlaySEJumpSE();
		}

	}

	public void actionJumpHelf() {
		if (MoveEnable && grounded && !isDashing && !isDamaged && !isAttacking && !isAirAttacking) {
			isJumping = true;
			toSetUpForce = true;
			setUpForce = jumpForce/1.5f;
			GameObject effect = Instantiate(effectObjectJumpSmoke, new Vector3(_transform.position.x - 0.1f * _transform.lossyScale.x ,_transform.position.y - 0.6f * _transform.lossyScale.y ,_transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
			PlaySEJumpSE();
		} 
		//二段跳
		else if (MoveEnable && !isKnockOuted && AirJumpEnabled && !grounded && !isAttacking && !isAirAttacking && !isDamaged) {
			toSetUpForce = true;
			setUpForce = jumpForce;
			AirJumpEnabled = false;
			PlaySEJumpSE();
		}
		//衝刺跳躍
		else if (MoveEnable && grounded && isDashing) {
			isDashJump = true;
			toSetUpForce = true;
			setUpForce = jumpForce;
			GameObject effect = Instantiate(effectObjectJumpSmoke, new Vector3(_transform.position.x - 0.1f * Mathf.Abs(_transform.lossyScale.x) ,_transform.position.y - 0.6f * _transform.lossyScale.y ,_transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
			StartCoroutine(dashJump());
			PlaySEJumpSE();
		}
	}

	//轉身
	public void actionFlip() {
		if (MoveEnable && !isAttacking && !isDamaged && !isAirAttacking) {
			dir = dir * -1.0f;
			Vector3 theScale = _transform.localScale;
			theScale.x *= -1.0f;
			_transform.localScale = theScale;
		}
	}

	//衝刺
	public void actionDash() {
		if(MoveEnable && !isDamaged && CancelDashEnable && !isDashJump){
			if (DashEnabled == true && isDashing == false) {
				animator.SetBool("Dash", true);
				if(isAttacking)MP -= DashCancelMP;
				//if(grounded){
					GameObject effect = Instantiate(effectObjectDashSmoke, new Vector3(_transform.position.x - 1.2f * _transform.lossyScale.x ,_transform.position.y + 0.7f * _transform.lossyScale.y,_transform.position.z), Quaternion.identity) as GameObject;
					effect.GetComponent<DirectionEffectCtrl>().owner = _transform;
				//}
				StartCoroutine(dash(dashTime));
				PlaySEDashSE();
			}
		}
	}

	//攻擊
	public virtual void actionAttack() {
		if (MoveEnable) {
			if (grounded && !isDashing) {
				//地面攻擊程式 動畫
				if (!isAttacking) {
					isAttacking = true;
					isInCombo = true;
					animator.SetTrigger("NormalAttack");

                }
				else {
					if(isInCombo && !CNormal2ATK && atkInputEnabled){
						nextATKName = "XXX_Attack2";
						CNormal2ATK = true;
					}

					else if(isInCombo && CNormal2ATK && atkInputEnabled){
						nextATKName = "XXX_Attack3";
					}

					if (isInCombo && atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
			}
			else if (!grounded && !isAttacking && !isAirAttacking) {
				//空中攻擊程式 動畫
				isAirAttacking = true;
				isDashing = false;
				animator.SetTrigger ("AirAttack");
                isInCombo = true;

            }
			else if (grounded && isDashing) {
				//衝刺攻擊程式 動畫
				isAttacking = true;
				isDashing = false;
				animator.SetTrigger ("DashAttack");
			}
		}
	}

	public virtual void actionUpAttack() {
		if (MoveEnable) {
			if(grounded){
				if(isInCombo && atkInputEnabled && !CUpATK)
                {
                    CUpATK = true;
					nextATKName = "XXX_UpAttack";
					if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
				else if(isInCombo && atkInputEnabled && CUpATK && !CNormal2ATK){//防止手殘來不及放開方向鍵也能順利接技
					nextATKName = "XXX_Attack2";
                    CNormal2ATK = true;
					if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
				else if(isInCombo && atkInputEnabled && CUpATK && CNormal2ATK){
					nextATKName = "XXX_Attack3"; 
                    if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
				else if(!isInCombo && !isAttacking && !isDashing && !isDashJump){
					animator.SetTrigger("UpAttack");
                    CUpATK = true;
                    isInCombo = true;

                }

			}
			else{
				isAirAttacking = true;
				isDashing = false;
				animator.SetTrigger ("AirAttack");
                isInCombo = true;
			}
		}
	}

	public virtual void actionDownAttack() {
		if (MoveEnable) {
			if(grounded){
				if(isInCombo && atkInputEnabled && !CDownATK){
					CDownATK = true;
					nextATKName = "XXX_DownAttack";
					if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
				else if(isInCombo && atkInputEnabled && CDownATK && !CNormal2ATK){//防止手殘來不及放開方向鍵也能順利接技
					nextATKName = "XXX_Attack2";
					CNormal2ATK = true;
					if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}
				else if(isInCombo && atkInputEnabled && CDownATK && CNormal2ATK){
					nextATKName = "XXX_Attack3";
					if (atkInputEnabled) {
						atkInputEnabled = false;
						atkInputNow = true;
					}
				}

				else if(!isInCombo && !isAttacking && !isDashing && !isDashJump){
					animator.SetTrigger("DownAttack");
                    CDownATK = true;
                    isInCombo = true;
                }
				
			}
			else{
				isAirAttacking = true;
				isDashing = false;
				animator.SetTrigger ("AirAttack");
                isInCombo = true;

            }
		}
	}

	//無敵
	public void actionInvincible(float n){
		isInvincible = true;
		invincibleTime = n;
		//暫時
		if(isDead)invincibleTime = 10000.0f;
	}

	//受身
	public void actionBackDamage() {
		if (isKnockOuted && backDMGSwitch || isKnockDown && grounded) {
			stopDamagedAnim();
			isKnockOuted = false;
			backDMGSwitch = false;
			isKnockDown = false;
            _rigidbody.gravityScale = initGravity;
            StopAllCoroutines();
			//AirJumpEnabled = false;

			toSetUpForce = true;
			setUpForce = BackDamageJumpForce;

			isKnockBack = false;
			actionInvincible(1.0f);
		}
	}

	//===程式 接受傷害======================================================

	//一般接受傷害
	public void actionTakeDMG(int DMG, float knockOutTime, float hitDir, float knockBackSpeedX, float hitForceY, int fromPlayer, float _knockOutGravity, float _knockOutDecressSpeed) {
		HP -= DMG;
		ShakeCharacter (DMG/70.0f,Mathf.Max(knockBackSpeedX/1000.0f , hitForceY/1000.0f));
        //SetPadVibration(Mathf.Max(Mathf.Abs(knockBackSpeedX / 1500.0f), Mathf.Abs(hitForceY / 1500.0f)), DMG / 5.0f);
        //MP += Mathf.Floor (DMG * DMGToMPRate); // 被攻擊補MP
        DashEnabled = true;
		if (!hakki || HP <= 0.0f) {  // 霸體
			MoveEnable = false;
			hakki = false;
			if (fromPlayer != 0)
				hitPlayer = fromPlayer;
			if (dir == hitDir) {
				dir = dir * -1.0f;
				Vector3 theScale = _transform.localScale;
				theScale.x *= -1.0f;
				_transform.localScale = theScale;
			}

			if (grounded) {
				isDamaged = true;
				if (DMG < 2)
					animator.SetTrigger ("Damaged1");
				else if (DMG >= 2)
					animator.SetTrigger ("Damaged2");
				if (stopKnockBack)
					stopKnockBack = false;
				if (isKnockBack) {
					StopAllCoroutines ();
                    _rigidbody.gravityScale = initGravity;
                    backDMGSwitch = false;
				}
				tempKnockBackDir = hitDir;
				tempKnockBackSpeed = knockBackSpeedX;
			} else if (!grounded) {
				stopDamagedAnim ();
				animator.SetTrigger ("Damaged3");
				animator.SetBool ("IsDamagedDown", true);
				toSetUpForce = true;
				setUpForce = hitForceY;
				if (isKnockOuted || isKnockBack) {
					StopAllCoroutines ();
					stopKnockBack = true;
					backDMGSwitch = false;
				}      
				StartCoroutine (knockOuted (knockOutTime, hitDir, knockBackSpeedX, _knockOutGravity, _knockOutDecressSpeed));
			}
		}
	}

	//小擊飛
	public void actionKnockDamagedDown(int DMG, float knockOutTime, float hitDir, float knockDamagedDownSpeedX, float hitForceY, int fromPlayer, float _knockOutGravity, float _knockOutDecressSpeed)
    {
		HP -= DMG;
		ShakeCharacter (DMG/80.0f,Mathf.Max(knockDamagedDownSpeedX/1200.0f , hitForceY/1200.0f));
        SetPadVibration(Mathf.Max(Mathf.Abs(knockDamagedDownSpeedX / 1500.0f), Mathf.Abs(hitForceY / 1500.0f)), DMG / 5.0f);
        //MP += Mathf.Floor (DMG * DMGToMPRate);
        DashEnabled = true;
		if (!hakki || HP <= 0.0f) {  // 霸體
			MoveEnable = false;
			hakki = false;
			if (fromPlayer != 0)
				hitPlayer = fromPlayer;
			if (dir == hitDir) {
				dir = dir * -1.0f;
				Vector3 theScale = _transform.localScale;
				theScale.x *= -1.0f;
				_transform.localScale = theScale;
			}
			if (isKnockOuted || isKnockBack) {
				StopAllCoroutines ();
				stopKnockBack = true;
				backDMGSwitch = false;
			}      

			stopDamagedAnim ();
			animator.SetTrigger ("Damaged3");
			animator.SetBool ("IsDamagedDown", true);
			toSetUpForce = true;
			setUpForce = hitForceY;
			StartCoroutine (knockOuted (knockOutTime, hitDir, knockDamagedDownSpeedX, _knockOutGravity, _knockOutDecressSpeed));
		}
	}


	//擊飛
	public void actionKnockOuted(bool sideType, int DMG, float knockOutTime, float hitDir, float knockOutSpeed, float hitForceY, int fromPlayer, float _knockOutGravity, float _knockOutDecressSpeed)
    {
		HP -= DMG;
		if(gameCtrl.MainCamera) gameCtrl.MainCamera.ShakeCamera(DMG/35.0f,Mathf.Max(Mathf.Abs(knockOutSpeed/1500.0f) , Mathf.Abs(hitForceY/1500.0f)));
        SetPadVibration(Mathf.Max(Mathf.Abs(knockOutSpeed / 1500.0f), Mathf.Abs(hitForceY / 1500.0f)), DMG / 5.0f);
        //MP += Mathf.Floor (DMG * DMGToMPRate);
        DashEnabled = true;
		if (!hakki || HP <= 0.0f) {  // 霸體
			MoveEnable = false;
			hakki = false;
			if (fromPlayer != 0)
				hitPlayer = fromPlayer;
			if (dir == hitDir) {
                if (!sideType)
                {
                    dir = dir * -1.0f;
                    Vector3 theScale = _transform.localScale;
                    theScale.x *= -1.0f;
                    _transform.localScale = theScale;
                }
                
			}
			if (isKnockOuted || isKnockBack) {
				StopAllCoroutines ();
				stopKnockBack = true;
				backDMGSwitch = false;
			}         
			if (sideType) {
                knockOutSideHitDir = hitDir;
                stopDamagedAnim();
                animator.SetBool("IsDamagedDown", true);
                animator.SetBool ("KnockOutSide", true);
            }
            else {
				stopDamagedAnim ();
				animator.SetBool ("IsDamagedDown", true);
				animator.SetBool ("KnockOutUp", true);
			}
			toSetUpForce = true;
			setUpForce = hitForceY;
			StartCoroutine (knockOuted (knockOutTime, hitDir, knockOutSpeed , _knockOutGravity, _knockOutDecressSpeed));
		}
	}

	//===程式 持續性動作做法======================================================
	//衝刺做法
	IEnumerator dash(float dashTime) {
		float time = 0.0f;
		float prevDir = dir;
		_rigidbody.gravityScale = 0;
		isDashing = true;
		DashEnabled = false;
		AirJumpEnabled = false;
		while (dashTime > time) {
			if(!isDashing || isDashJump || isDamaged || isKnockOuted || prevDir != dir || isDead) break;
			time += deltaTime;
			if(Mathf.Abs(speedX) < dashSpeed){
				speedX = (Mathf.Abs(speedX) + dashAcceleration) * dir;
			}
			else if(Mathf.Abs(speedX) >= dashSpeed){
				speedX = dashSpeed * dir;
			}
			toSetVelocityY = true;
			setVelocityY = 0.0f;	
			yield return null;
		}
		_rigidbody.gravityScale = initGravity;
		isDashing = false;
		animator.SetBool("Dash", false);
		if (!isDashJump) {
			yield return new WaitForSeconds (dashCooldown);
			//DashEnabled = true;
		}
	}

	//衝刺跳躍做法
	IEnumerator dashJump(){
		float time = 0.0f;
		while (!grounded || 0.1f > time ) {     //還沒跳起來就偵測倒貼地了       
			if(!isDashJump || isDamaged || isKnockOuted || isDead) break;
			time += deltaTime;
			characterSpeed = dashSpeed;
			characterAcceleration   = dashSpeed;
			yield return null;
		}
		characterSpeed = initSpeed;
		if (isUntouchable)
			characterSpeed = initUntouchableMoveSpeed;
		characterAcceleration = initAcceleration;
		isDashJump = false;
		yield return new WaitForSeconds(dashCooldown);
		//DashEnabled = true;
	}

	//普通受擊做法
	IEnumerator knockBack(float dir, float knockBackSpeed){
		float time = 0.0f;
		isKnockBack = true;
		while (!stopKnockBack && isKnockBack && time < 0.5f && !isDead) {   // TIME防止往後退問題
			time += deltaTime;
			speedX = knockBackSpeed * dir;
			yield return null;
		}
		isKnockBack = false;
	}

	//擊飛做法
	IEnumerator knockOuted(float knockOutTime, float dir, float knockOutSpeed , float _knockOutGravity, float _knockOutDecressSpeed)
    {
		float time = 0.0f;
		isKnockOuted = true;
		isKnockBack = false;
		_rigidbody.gravityScale = _knockOutGravity;
		animator.SetBool ("Dash", false);//防止動畫BUG
		while (!grounded || 0.1f > time ) {     //還沒跳起來就偵測倒貼地了  
			if(wallBackDamage || !isKnockOuted)break;
			if(time > knockOutTime) backDMGSwitch = true;
            if (_rigidbody.velocity.y <= -2.0f && time > 0.1f) { animator.SetBool("KnockOutUp", false); _rigidbody.gravityScale = initGravity; }//time > 0.1 : 防止往下擊落動畫出問題
            if (Mathf.Abs(_rigidbody.velocity.x) <= 4.0f && time > 0.1f) {
                animator.SetBool("KnockOutSide", false);
                if (bknockOutSideMoveDown) {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.45f, this.transform.position.z);
                    bknockOutSideMoveDown = false;
                }
            }//time > 0.1 : 防止往下擊落動畫出問題                                                                                                          //因為擊飛後玩家上移 要移動回去
            time += deltaTime;
            knockOutSpeed -= deltaTime * _knockOutDecressSpeed;
            if (knockOutSpeed <= 1.0f) knockOutSpeed = 1.0f;
            speedX = knockOutSpeed * dir;
			yield return null;
		}
		speedX = 0.0f;
		isKnockOuted = false;
		backDMGSwitch = false;
		stopDamagedAnim ();
        _rigidbody.gravityScale = initGravity;
        if (grounded) {
			isKnockDown = true;
			animator.SetBool ("IsKnockDown", true);
		}
        if (wallBackDamage) {
			wallBackDamage = false;
			actionInvincible(1.0f);
		}

    }

	//攻擊滑動做法
	IEnumerator AttackDashIE(float attackDashSpeed) {
		_rigidbody.gravityScale = 0;
		while (!stopAttackDash) {
			if(isDamaged ||isKnockOuted || isDead) break;
			speedX = attackDashSpeed * dir;
			toSetVelocityY = true;
			setVelocityY = 0.0f;	
			yield return null;
		}
		stopAttackDash = false;
		_rigidbody.gravityScale = initGravity;
	}


	//===音效==========================================================

	public void PlaySEJumpSE(){
		audioCtrl.pitch = jumpSEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(jumpSE);
	}
	
	public void PlaySEDashSE(){
		audioCtrl.pitch = dashSEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(dashSE);
	}
	
	public void PlaySEKnockDownSE(){
		audioCtrl.pitch = knockDownSEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(knockDownSE);
	}

	public void PlaySEULTChargeSE(){
		audioCtrl.pitch = ULTChargeSEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(ULTChargeSE);
	}


	//===程式 其他======================================================

	public void dead(){
		if (hitPlayer == 0) {
			gameCtrl.playerPoint [PlayerNUM - 1] -= 1;
		} 
		else {
			gameCtrl.playerPoint [hitPlayer - 1] += 1;
		}

        if (isUsingULT) gameCtrl.isUsingULT -= 1;

        Instantiate(effectObjectDeathEffectGround, new Vector3(_transform.position.x - 0.1f * _transform.lossyScale.x, _transform.position.y - 0.6f * _transform.lossyScale.y, _transform.position.z), Quaternion.identity);
        Destroy (this.gameObject);
	}

	public void MPCalculation(float n){
		MP -= n;
	}

	void stopDamagedAnim(){
		animator.SetBool("KnockOutSide", false);
		animator.SetBool("KnockOutUp", false);
		animator.SetBool ("IsDamagedDown", false);
		animator.SetBool("IsKnockDown", false);
	}

	public void stopKnockDown(){
		isKnockDown = false;
		animator.SetBool("IsKnockDown", false);
	}

	public void stopCanAttackMove(){
		canAttackMove = false;
	}

    public void StartCanAttackMove()
    {
        canAttackMove = true;
    }
	
	public void attackDash(float attackDashSpeed){
		if (stopAttackDash)stopAttackDash = false;
		StartCoroutine(AttackDashIE(attackDashSpeed));
	}
	
	public void attackDashStopDash(){
		stopAttackDash = true;
	}

	public void reDetectDMG(){
		hittedPlayer [0] = false;
		hittedPlayer [1] = false;
		hittedPlayer [2] = false;
		hittedPlayer [3] = false;
	}

	public void actionKnockBack(){
		StartCoroutine (knockBack (tempKnockBackDir, tempKnockBackSpeed));
	}

	public void ShakeCharacter(float shakePwr, float shakeDur){
		if ((shakePwr > OriShakePower && shakeTime > 0.0f) || shakeTime <= 0.0f) {
			shakePower = shakePwr;
			shakeTime = shakeDur;
			if(shakePwr > MaxShakePower)shakePwr = MaxShakePower;
			if(shakeDur > MaxShakeTime)shakeTime = MaxShakeTime;
			
			OriShakePower = shakePwr;
		}
	}

    public void StopDash()
    {
        stopDash = true;
    }


    //====設置攻擊資訊==============
    public virtual void setATKData(string s)
    {
        
    }

    public ATKColliderData getATKData()
    {
        return ATKData;
    }

    public ATKColliderData getATKDataInDic(string s)
    {
        return AtkDataDic[s];
    }

    //==設定控制器震動資訊==============================
    public void SetPadVibration(float t , float v)
    {
        if(padVibrationTime < t)
        {
            padVibrationTime = t;
        }
        if (v > 1.0f) v = 1.0f;
        padVibrationValue = v;
    }

    public void AnimSetPadVibration(float t) // 動畫事件只准許一ㄍ變數
    {
        if (padVibrationTime < t)
        {
            padVibrationTime = t;
        }
        
        padVibrationValue = 0.7f;
    }

}
