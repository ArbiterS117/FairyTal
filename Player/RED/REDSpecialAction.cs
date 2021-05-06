using UnityEngine;
using System.Collections;


public class REDSpecialAction : XXXCtrl {
	
	//這裡就負責放每隻腳色的特性
	//例:衝刺時無敵、三段跳之類的
	//可利用動畫或狀態來判斷
	//可用動畫控制器裡面的新增功能
	
    //====控制參數============
 
    public ATKColliderData ForwardATK1ATK  ;
    public ATKColliderData ForwardATK2ATK  ;
    public ATKColliderData ArialAce1ATK    ;
    public ATKColliderData ArialAce2ATK    ;
    public ATKColliderData StoneStrikeATK  ;

    //public ATKColliderData[] PlayerATKData = new ATKColliderData[14];//腳色全部的攻擊資訊 (攻擊總數目)

    public GameObject effectObjectStone1;

    public float ULT1MP = 30.0f;
	public float ULT2MP = 25.0f;
	public float BasketStrikeMP = 5.0f;
	public float ForwardATKMP   = 7.0f;
	public float ArialAceMP     = 7.0f;

	public float ForwardATKDashSpeed   = 10.0f;
	public float ArialAceJumpForce     = 500.0f;
	public float ArialAceDashSpeed     = 4.0f;
	public float StoneStrikeJumpForce  = 300.0f;
	public float StoneStrikeDashSpeed  = 3.0f;
	public float ULTJumpForce     = 650.0f;
	public float ULTDashSpeed     = 4.0f;

	public float StoneSpeedX;
	public float StoneFlyForce;
	public float StoneGravity;
	public float StoneUpStateSpeedX;
	public float StoneUpStateFlyForce;
	public float StoneUpStateGravity;
	public float StoneDownStateSpeedX;
	public float StoneDownStateFlyForce;
	public float StoneDownStateGravity;
	
	//====音效參數============
	public AudioClip normalATK1SE;
	public float normalATK1SEPitch;
	public AudioClip normalATK2SE;
	public float normalATK2SEPitch;
	public AudioClip normalATK3SE;
	public float normalATK3SEPitch;
	public AudioClip upATKSE;
	public float upATKSEPitch;
	public AudioClip ArialAce1SE;
	public float ArialAce1SEPitch;
	public AudioClip ArialAce2SE;
	public float ArialAce2SEPitch;
	public AudioClip ForwardATK1SE;
	public float ForwardATK1SEPitch;
	public AudioClip ForwardATK2SE;
	public float ForwardATK2SEPitch;
	public AudioClip ForceRelease1SE;
	public float ForceRelease1SEPitch;
	public AudioClip ForceRelease2SE;
	public float ForceRelease2SEPitch;
	public AudioClip MeteorStrikeSE;
	public float MeteorStrikeSEPitch;
	public AudioClip StoneStrikeSE;
	public float StoneStrikeSEPitch;
	
	//====開關============
	protected override void Awake() {
        base.Awake();
	}

    protected override void Start()
    {
        base.Start();
        setStartATKData();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    protected override void Update()
    {
        base.Update();

        //測試用
        AtkDataDic.Clear();
        setStartATKData();
    }
    
    //===招式1  ForwardATK====================================

    public void actionSPAttack1(){
		if (MoveEnable && !isAttacking && !isDamaged && 
			!isDashing && !isDashJump && !isAirAttacking) {

			if (MP >= ForwardATKMP) {
				MPCalculation(ForwardATKMP);
				AirJumpEnabled = false;
				animator.SetTrigger ("ForwardATK");
				//裡面可以依腳色寫一些判斷或加強
			}
		}

		else if(isInCombo && atkInputEnabled){   //普攻接技可接招式

			if (MP >= ForwardATKMP) {
				MPCalculation(ForwardATKMP);
				nextATKName = "XXX_SPAttack1";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}

		}

	}	

	public void ForwardATKDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(ForwardATK());
	}

	public void ForwardATKStopDash(){
		stopDash = true;
	}

	//===招式2  ArialAce=====================================

	public void actionSPAttack2(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking) {

			if (MP >= ArialAceMP) {
				MPCalculation(ArialAceMP);
				animator.SetTrigger ("ArialAce");
			}

		}


		else if(isInCombo && atkInputEnabled){

			if (MP >= ArialAceMP) {
				MPCalculation(ArialAceMP);
				nextATKName = "XXX_SPAttack2";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}

		}


	}

	public void ArialAceJump(){
		AirJumpEnabled = false;
		toSetUpForce = true;
		setUpForce = ArialAceJumpForce;
	}

	public void ArialAceDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(ArialAce());
	}
	
	public void ArialAceStopDash(){
		stopDash = true;
	}

	//===招式3  StoneStrike====================================

	public void actionSPAttack3(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking
		    ) {

			if (MP >= BasketStrikeMP) {
				MPCalculation(BasketStrikeMP);
				canAttackMove = true;
				animator.SetTrigger ("StoneAttack");
			}
		}

		else if(isInCombo && atkInputEnabled){
			
			if (MP >= BasketStrikeMP) {
				MPCalculation(BasketStrikeMP);
				if(grounded)nextATKName = "XXX_SPAttack3";
                else nextATKName = "XXX_SPAttack4";
                canAttackMove = true;
                if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}
	}


	public void StoneStrikeDash(){
		if (stopDash)stopDash = false;
		toSetUpForce = true;
		setUpForce = StoneStrikeJumpForce;
		StartCoroutine(StoneStrike());
	}
	
	public void StoneStrikeStopDash(){
		stopDash = true;
	}
	
	public void actionFireStone(){

        SetPadVibration(0.2f, 0.5f);

		GameObject effect = Instantiate (effectObjectStone1, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y - 0.7f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponentInChildren<StoneCollider> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		effect.GetComponent<StoneCtrl> ().owner = transform;
		
		//狀態控制
		if (!UpState && !DownState) {
			effect.GetComponent<StoneCtrl> ().StoneSpeedX = StoneSpeedX;
			effect.GetComponent<StoneCtrl> ().StoneFlyForce = StoneFlyForce;
			effect.GetComponent<StoneCtrl> ().StoneGravity = StoneGravity;
		} else if (UpState && !DownState) {
			effect.GetComponent<StoneCtrl> ().StoneSpeedX = StoneUpStateSpeedX;
			effect.GetComponent<StoneCtrl> ().StoneFlyForce = StoneUpStateFlyForce;
			effect.GetComponent<StoneCtrl> ().StoneGravity = StoneUpStateGravity;
		} else if (!UpState && DownState) {
			effect.GetComponent<StoneCtrl> ().StoneSpeedX = StoneDownStateSpeedX;
			effect.GetComponent<StoneCtrl> ().StoneFlyForce = StoneDownStateFlyForce;
			effect.GetComponent<StoneCtrl> ().StoneGravity = StoneDownStateGravity;
		}

	}

	//===ULT=======================================

	public void actionULT1(){
		if (MoveEnable && !isAttacking &&
			!isDamaged && !isDashing && !isDashJump) {
			if (MP >= ULT1MP) {
				AirJumpEnabled = false;
				animator.SetTrigger ("ForceRelease");
				MPCalculation(ULT1MP);
			}
		}
	}

	public void actionHakki(){
		hakki = true;
	}

	public void actionStopHakki(){
		hakki = false;
	}

	public void actionULT2(){
		if (MoveEnable && grounded && !isAttacking &&
			!isDamaged && !isDashing && !isDashJump) {
			if (MP >= ULT2MP) {
				AirJumpEnabled = false;
				animator.SetTrigger ("MeteorStrike");
				MPCalculation(ULT2MP);
			}
		}
	}

	public void ULTJump(){
		AirJumpEnabled = false;
		toSetUpForce = true;
		setUpForce = ULTJumpForce;
	}
	
	public void ULTDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(EULTDash());
	}


	//===實作=========================================

	//StoneStrike
	IEnumerator StoneStrike() {
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted) break;
			speedX = StoneStrikeDashSpeed * dir;
			yield return null;
		}
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}

	//ForwardATK
	IEnumerator ForwardATK() {
		GetComponent<Rigidbody2D> ().gravityScale = 0;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted || isDead) break;
			speedX = ForwardATKDashSpeed * dir;
			toSetVelocityY = true;
			setVelocityY = 0.0f;	
			yield return null;
		}
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}

	//ArialAce
	IEnumerator ArialAce() {
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted) break;
			speedX = ArialAceDashSpeed * dir;
			yield return null;
		}
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}

	//ULT1
	IEnumerator EULTDash() {
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted) break;
			speedX = ULTDashSpeed * dir;
			yield return null;
		}
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}
   
    //===音效=========================================

    public void PlaySENormalATK1SE(){
		audioCtrl.pitch = normalATK1SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(normalATK1SE);
	}

	public void PlaySENormalATK2SE(){
		audioCtrl.pitch = normalATK2SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(normalATK2SE);
	}

	public void PlaySENormalATK3SE(){
		audioCtrl.pitch = normalATK3SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(normalATK3SE);
	}

	public void PlaySEUpATKSE(){
		audioCtrl.pitch = upATKSEPitch;
		audioCtrl.PlayOneShot(upATKSE);
	}

	public void PlaySEArialAce1SE(){
		audioCtrl.pitch = ArialAce1SEPitch;
		audioCtrl.PlayOneShot(ArialAce1SE);
	}

	public void PlaySEArialAce2SE(){
		audioCtrl.pitch = ArialAce2SEPitch;
		audioCtrl.PlayOneShot(ArialAce2SE);
	}

	public void PlaySEForwardATK1SE(){
		audioCtrl.pitch = ForwardATK1SEPitch;
		audioCtrl.PlayOneShot(ForwardATK1SE);
	}

	public void PlaySEForwardATK2SE(){
		audioCtrl.pitch = ForwardATK2SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(ForwardATK2SE);
	}

	public void PlaySEForceRelease1SE(){
		audioCtrl.pitch = ForceRelease1SEPitch;
		audioCtrl.PlayOneShot(ForceRelease1SE);
	}

	public void PlaySEForceRelease2SE(){
		audioCtrl.pitch = ForceRelease2SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(ForceRelease2SE);
	}

	public void PlaySEMeteorStrikeSE(){
		audioCtrl.pitch = MeteorStrikeSEPitch;
		audioCtrl.PlayOneShot(MeteorStrikeSE);
	}

	public void PlaySEStoneStrikeSE(){
		audioCtrl.pitch = StoneStrikeSEPitch;
		audioCtrl.PlayOneShot(StoneStrikeSE);
	}

    //===設置攻擊力============================
    //傳送給碰撞器的資料
    public override void setATKData(string s)
    {
        ATKData = AtkDataDic[s];
    }

    //設置初始攻擊資料
    private void setStartATKData()
    {
        AtkDataDic.Add("Attack1ATK", Attack1ATK);
        AtkDataDic.Add("Attack2ATK", Attack2ATK);
        AtkDataDic.Add("Attack3ATK", Attack3ATK);
        AtkDataDic.Add("DownAttackATK", DownAttackATK);
        AtkDataDic.Add("UpAttackATK", UpAttackATK);
        AtkDataDic.Add("AirAttackATK", AirAttackATK);
        AtkDataDic.Add("DashAttackATK", DashAttackATK);
        AtkDataDic.Add("ForwardATK1ATK", ForwardATK1ATK);
        AtkDataDic.Add("ForwardATK2ATK", ForwardATK2ATK);
        AtkDataDic.Add("ArialAce1ATK", ArialAce1ATK);
        AtkDataDic.Add("ArialAce2ATK", ArialAce2ATK);
        AtkDataDic.Add("StoneStrikeATK", StoneStrikeATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
        AtkDataDic.Add("ULT2ATK", ULT2ATK);
    }

   

}
