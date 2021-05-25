using UnityEngine;
using System.Collections;

public class RAPUNZELSpecialAction : XXXCtrl {

    //這裡就負責放每隻腳色的特性
    //例:衝刺時無敵、三段跳之類的
    //可利用動畫或狀態來判斷
    //可用動畫控制器裡面的新增功能

    //====控制參數============

    public ATKColliderData HairCombo1ATK;
    public ATKColliderData HairCombo2ATK;
    public ATKColliderData ArialHairGATK;
    public ATKColliderData ArialHairA1ATK;
    public ATKColliderData ArialHairA2ATK;
    public ATKColliderData HairWindATK;

    public GameObject effectObjectWind;

    public float ULT1MP = 30.0f;
	public float HairWindMP = 5.0f;
	public float HairComboMP    = 5.0f;
	public float HairComboAirMP = 5.0f;
	public float ArialHairMP    = 7.0f;

	public float ArialHairJumpForce     = 500.0f;
	public float ArialHairDashSpeed     = 4.0f;
	public float ArialHairGravity       = 1.0f;

	public float HairComboAirJumpForce     = 500.0f;
	public float HairComboAirDashSpeed     = 4.0f;
	public float HairComboAirGravity       = 1.0f;


	public float ULTJumpForce     = 650.0f;
	public float ULTDashSpeed     = 4.0f;
	
	public float WindSpeedX;
	public float WindFlyForce;
	public float WindUpStateSpeedX;
	public float WindUpStateFlyForce;
	public float WindDownStateSpeedX;
	public float WindDownStateFlyForce;

	//====音效參數============
	public AudioClip normalATK1SE;
	public float normalATK1SEPitch;
	public AudioClip normalATK2SE;
	public float normalATK2SEPitch;
	public AudioClip normalATK3SE;
	public float normalATK3SEPitch;
	public AudioClip upATKSE;
	public float upATKSEPitch;
	public AudioClip ArialHairSE;
	public float ArialHairSEPitch;
	public AudioClip HairCombo1SE;
	public float HairCombo1SEPitch;
	public AudioClip HairCombo2SE;
	public float HairCombo2SEPitch;
	public AudioClip HairWindSE;
	public float HairWindSEPitch;
	
	//====開關============
	volatile bool SPatkInputEnabled = false;
	volatile bool SPatkInputNow    = false;
    protected string SPNextATKName;

    protected override void Awake() {
        base.Awake();
        audioCtrl = GetComponent<AudioSource>();
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

        //測試用
        AtkDataDic.Clear();
        setStartATKData();

        base.Update();
    }
        //====動畫用程式=======
        public void SPEnableAttackInput() {
		SPatkInputEnabled = true;
	}
	
	public void SPSetNextAttack(string name) {
		if (SPatkInputNow == true) {
			SPatkInputNow = false;
			animator.Play(name,-1,0f);
		}
		
	}
	//===招式1&2&6  HairCombo====================================

	int SP1Count = 0;

	public void actionSPAttack1(){
		if (MoveEnable && !isAttacking && !isDamaged && 
			!isDashing && !isDashJump && !isAirAttacking) {
			
			if (MP >= HairComboMP) {
				MPCalculation (HairComboMP);
				AirJumpEnabled = false;
				animator.SetTrigger ("HairCombo");
				SP1Count = 0;
				//裡面可以依腳色寫一些判斷或加強
			}
		}
		
		else if(isInCombo && atkInputEnabled){   //普攻接技可接招式
			
			if (MP >= HairComboMP) {
				MPCalculation(HairComboMP);
				nextATKName = "XXX_SPAttack1";
				SP1Count = 0;
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}

		else if(SP1Count < 3 && MP >= HairComboMP){
			if (SPatkInputEnabled) {
				SPatkInputEnabled = false;
				SPatkInputNow = true;
			}
		}
		
	}

	public void AnimSPATK1_MP_Count(){
		MPCalculation(HairComboMP);
		SP1Count ++;
	}

	//===招式3&5  ArialHair=====================================
	
	public void actionSPAttack3(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking) {
			
			if (MP >= ArialHairMP) {
				MPCalculation(ArialHairMP);
				animator.SetTrigger ("ArialHair");
			}
			
		}
		
		
		else if(isInCombo && atkInputEnabled){
			
			if (MP >= ArialHairMP) {
				MPCalculation(ArialHairMP);
				if(grounded)nextATKName = "XXX_SPAttack3";
                else nextATKName = "XXX_SPAttack5";
                if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}
		
		
	}
	
	public void ArialHairJump(){
		AirJumpEnabled = false;
		toSetUpForce = true;
		setUpForce = ArialHairJumpForce;
	}
	
	public void ArialHairDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(ArialHair());
	}
	
	public void ArialHairStopDash(){
		stopDash = true;
	}
	
	//===招式4  HairWind====================================
	
	public void actionSPAttack4(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking
		    ) {
			
			if (MP >= HairWindMP) {
				MPCalculation(HairWindMP);
				canAttackMove = true;
				animator.SetTrigger ("HairWind");
			}
		}
		
		else if(isInCombo && atkInputEnabled){
			
			if (MP >= HairWindMP) {
				MPCalculation(HairWindMP);
                canAttackMove = true;
                nextATKName = "XXX_SPAttack4";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}
	}
	
	public void actionFireWind(){
        SetPadVibration(0.2f, 0.6f);

        GameObject effect = Instantiate (effectObjectWind, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		effect.GetComponent<HairWindCtrl> ().owner = transform;
		
		//狀態控制
		if (!UpState && !DownState) {
			effect.GetComponent<HairWindCtrl> ().WindSpeedX = WindSpeedX;
			effect.GetComponent<HairWindCtrl> ().WindFlyForce = WindFlyForce;
		} else if (UpState && !DownState) {
			effect.GetComponent<HairWindCtrl> ().WindSpeedX = WindUpStateSpeedX;
			effect.GetComponent<HairWindCtrl> ().WindFlyForce = WindUpStateFlyForce;
		} else if (!UpState && DownState) {
			effect.GetComponent<HairWindCtrl> ().WindSpeedX = WindDownStateSpeedX;
			effect.GetComponent<HairWindCtrl> ().WindFlyForce = WindDownStateFlyForce;
		}
		
	}
	
	//===ULT=======================================
	
	public void actionULT1(){
		if (MoveEnable && !isAttacking &&
		    !isDamaged && !isDashing && !isDashJump) {
			if (MP >= ULT1MP) {
				AirJumpEnabled = false;
				animator.SetTrigger ("HairOmni");
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
	
	//===實作=========================================

	//ArialHair
	IEnumerator ArialHair() {
		GetComponent<Rigidbody2D> ().gravityScale = ArialHairGravity;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted) break;
			speedX = ArialHairDashSpeed * dir;
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
	
	public void PlaySEHairCombo1SE(){
		audioCtrl.pitch = HairCombo1SEPitch;
		audioCtrl.PlayOneShot(HairCombo1SE);
	}
	
	public void PlaySEHairCombo2SE(){
		audioCtrl.pitch = HairCombo2SEPitch;
		audioCtrl.PlayOneShot(HairCombo2SE);
	}
	
	public void PlaySEArialHairSE(){
		audioCtrl.pitch = ArialHairSEPitch;
		audioCtrl.PlayOneShot(ArialHairSE);
	}

	public void PlaySEHairWindSE(){
		audioCtrl.pitch = HairWindSEPitch;
		audioCtrl.PlayOneShot(HairWindSE);
	}

    //===設置攻擊力============================
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
        AtkDataDic.Add("HairCombo1ATK", HairCombo1ATK);
        AtkDataDic.Add("HairCombo2ATK", HairCombo2ATK);
        AtkDataDic.Add("ArialHairGATK", ArialHairGATK);
        AtkDataDic.Add("ArialHairA1ATK", ArialHairA1ATK);
        AtkDataDic.Add("ArialHairA2ATK", ArialHairA2ATK);
        AtkDataDic.Add("HairWindATK", HairWindATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
        AtkDataDic.Add("ULT2ATK", ULT2ATK);
    }

   

}
