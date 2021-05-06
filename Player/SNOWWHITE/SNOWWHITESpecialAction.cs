using UnityEngine;

public class SNOWWHITESpecialAction : XXXCtrl {

    //這裡就負責放每隻腳色的特性
    //例:衝刺時無敵、三段跳之類的
    //可利用動畫或狀態來判斷
    //可用動畫控制器裡面的新增功能


    //====控制參數============

    public ATKColliderData DraftRocketATK;
    public ATKColliderData DraftRocketExplodeATK;
    public ATKColliderData SelfExplodeATK;
    public ATKColliderData AppleMineATK;
    public ATKColliderData MushroomSpringGATK;
    public ATKColliderData MushroomSpringAATK;

    public GameObject effectObjectDraft;
    public GameObject effectObjectSlefExplosion;
    public GameObject effectObjectAppleMine;
    public GameObject effectObjectMushroomSpring;

    public float ULT1MP = 30.0f;
	public float MushRoomJumpMP  = 5.0f;
	public float DraftRocketMP   = 7.0f;
	public float AppleMineMP     = 7.0f;
	
	public float ForwardATKDashSpeed   = 10.0f;
	public float ArialAceJumpForce     = 500.0f;
	public float ArialAceDashSpeed     = 4.0f;
	public float StoneStrikeJumpForce  = 300.0f;
	public float StoneStrikeDashSpeed  = 3.0f;
	public float ULTJumpForce     = 650.0f;
	public float ULTDashSpeed     = 4.0f;
	
	public float DraftSpeedX;
	public float DraftFlyForce;
	public float DraftGravity;
	public float AirDraftSpeedX;
	public float AirDraftFlyForce;
	public float AirDraftGravity;
	public float AppleMineSpeedX;
	public float AppleMineFlyForce;
	public float AppleMineRollTime;
	public float AppleMineERT;// AppleMineExplodeReadyTime
	public float UpAppleMineSpeedX;
	public float UpAppleMineFlyForce;
	public float UpAppleMineRollTime;
	public float UpAppleMineERT;// AppleMineExplodeReadyTime
	public float DownAppleMineSpeedX;
	public float DownAppleMineFlyForce;
	public float DownAppleMineRollTime;
	public float DownAppleMineERT;// AppleMineExplodeReadyTime
	public float MushroomJumpForce = 800.0f;

	//====音效參數============
	public AudioClip normalATK1SE;
	public float normalATK1SEPitch;
	public AudioClip normalATK2SE;
	public float normalATK2SEPitch;
	public AudioClip normalATK3SE;
	public float normalATK3SEPitch;
	public AudioClip upATKSE;
	public float upATKSEPitch;
	public AudioClip DraftRocketSE;
	public float DraftRocketSEPitch;
	public AudioClip CrazyAnimalsSE;
	public float CrazyAnimalsSEPitch;
	
	//====開關============
	bool MushroomJumpEnable = false;
	public float MushroomColdTime;
	float MushroomColdCTime = 0.0f;
	bool canPutMushroom = true;

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

    protected override void Update(){
        base.Update();

        //測試用
        AtkDataDic.Clear();
        setStartATKData();

        if (grounded) {
			MushroomJumpEnable = true;
		}

		if (!canPutMushroom) {
			MushroomColdCTime += Time.deltaTime;
			if(MushroomColdCTime >= MushroomColdTime){
				canPutMushroom = true;
				MushroomColdCTime = 0.0f;
			}
		}
	}
	
	//===招式1  DraftRocket====================================
	
	public void actionSPAttack1(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking) {

			animator.SetTrigger ("DraftRocket");
			//裡面可以依腳色寫一些判斷或加強

			//攻擊移動
			if(!grounded){
				canAttackMove = true;
			}
		}
		
		else if(isInCombo && atkInputEnabled){   //普攻接技可接招式

			if(grounded)nextATKName = "XXX_SPAttack1";
            else nextATKName = "XXX_SPAttack4";
            //攻擊移動
            if (!grounded)
            {
                canAttackMove = true;
            }
            if (atkInputEnabled) {
				atkInputEnabled = false;
				atkInputNow = true;
			}
			
		}
		
	}	

	public void actionFireDraft(){

		if (MP >= DraftRocketMP) {
            SetPadVibration(0.3f, 0.7f);
            MPCalculation (DraftRocketMP);
			GameObject effect = Instantiate (effectObjectDraft, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y + 0.5f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<StoneCollider> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<StoneCtrl> ().owner = transform;
		
			//狀態控制
			if (grounded) {
				effect.GetComponent<StoneCtrl> ().StoneSpeedX = DraftSpeedX;
				effect.GetComponent<StoneCtrl> ().StoneFlyForce = DraftFlyForce;
				effect.GetComponent<StoneCtrl> ().StoneGravity = DraftGravity;
			} else {
				effect.GetComponent<StoneCtrl> ().StoneSpeedX = AirDraftSpeedX;
				effect.GetComponent<StoneCtrl> ().StoneFlyForce = AirDraftFlyForce;
				effect.GetComponent<StoneCtrl> ().StoneGravity = AirDraftGravity;
			}
		}

		else{
			GameObject effect = Instantiate (effectObjectSlefExplosion, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
			effect.GetComponentInChildren<SelfExplodeCollider> ().owner = transform;
		}
		
	}

	
	/*
	public void ForwardATKDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(ForwardATK());
	}
	
	public void ForwardATKStopDash(){
		stopDash = true;
	}
	*/
	
	//===招式2  AppleMine=====================================
	
	public void actionSPAttack2(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && !isAirAttacking) {

			animator.SetTrigger ("AppleMine");

			//攻擊移動
			if(!grounded){
				canAttackMove = true;
			}
			
		}
		
		
		else if(isInCombo && atkInputEnabled){
			
			if (MP >= AppleMineMP) {
                if (grounded) nextATKName = "XXX_SPAttack2";
                else nextATKName = "XXX_SPAttack5";
                if (!grounded)
                {
                    canAttackMove = true;
                }
                if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}
		
		
	}

	public void actionFireAppleMine(){

		if (MP >= AppleMineMP) {
            SetPadVibration(0.2f, 0.4f);
			MPCalculation (AppleMineMP);

			GameObject effect = Instantiate (effectObjectAppleMine, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y + 0.5f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<AppleMineDetectCollider> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<AppleMineCtrl> ().owner = transform;

            //狀態控制
            if (!UpState && !DownState) {
				effect.GetComponent<AppleMineCtrl> ().AppleSpeedX = AppleMineSpeedX;
				effect.GetComponent<AppleMineCtrl> ().FlyForce = AppleMineFlyForce;
				effect.GetComponent<AppleMineCtrl> ().rollTime = AppleMineRollTime;
				effect.GetComponent<AppleMineCtrl> ().explodeReadyTime = AppleMineERT;
                effect.GetComponent<AppleMineCtrl>().AnimAppleRollSpeed = 1.0f;
            } else if (UpState && !DownState) {
				effect.GetComponent<AppleMineCtrl> ().AppleSpeedX = UpAppleMineSpeedX;
				effect.GetComponent<AppleMineCtrl> ().FlyForce = UpAppleMineFlyForce;
				effect.GetComponent<AppleMineCtrl> ().rollTime = UpAppleMineRollTime;
				effect.GetComponent<AppleMineCtrl> ().explodeReadyTime = UpAppleMineERT;
                effect.GetComponent<AppleMineCtrl>().AnimAppleRollSpeed = 0.5f;
            } else if (!UpState && DownState) {
				effect.GetComponent<AppleMineCtrl> ().AppleSpeedX = DownAppleMineSpeedX;
				effect.GetComponent<AppleMineCtrl> ().FlyForce = DownAppleMineFlyForce;
				effect.GetComponent<AppleMineCtrl> ().rollTime = DownAppleMineRollTime;
				effect.GetComponent<AppleMineCtrl> ().explodeReadyTime = DownAppleMineERT;
                effect.GetComponent<AppleMineCtrl>().AnimAppleRollSpeed = 2.0f;
            }
		}

	}

	
	//===招式3  MushroomSpring====================================
	
	public void actionSPAttack3(){
		if (MoveEnable && !isAttacking && !isDamaged && 
		    !isDashing && !isDashJump && grounded && canPutMushroom ) {
			if (MP >= MushRoomJumpMP) {
				MPCalculation(MushRoomJumpMP);
				canAttackMove = true;
				animator.SetTrigger ("MushRoomJump");
			}
		}

		else if(isInCombo && atkInputEnabled && !grounded)
        {
			
			if (MP >= MushRoomJumpMP) {
				MPCalculation(MushRoomJumpMP);
				nextATKName = "XXX_SPAttack3";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
			
		}

	}

	public void actionFireMushroomSpring(){
        SetPadVibration(0.2f, 0.5f);
        canPutMushroom = false;
		GameObject effect = Instantiate (effectObjectMushroomSpring, new Vector3 (transform.position.x + 1.5f * transform.lossyScale.x, transform.position.y - 0.3f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
		
	}

	

	//===招式6  MushroomSpringAir====================================

	public void actionSPAttackMushroomJump(){
		if (MushroomJumpEnable && MoveEnable && !isDashing && 
		    !isDamaged && !isAttacking && !isDashJump &&
		    !grounded && !isAirAttacking && canPutMushroom) {
            SetPadVibration(0.2f, 0.5f);
            MushroomJumpEnable = false;
			toSetUpForce = true;
			setUpForce = MushroomJumpForce;
            setATKData("MushroomSpringAATK");

			canPutMushroom = false;
			GameObject effect = Instantiate (effectObjectMushroomSpring, new Vector3 (transform.position.x - 0.1f * transform.lossyScale.x, transform.position.y -1.0f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut>().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
			effect.GetComponent<DirectionEffectCtrl>().owner = transform;
		}
		
	}
	

	//===ULT=======================================
	
	public void actionULT1(){
		if (MoveEnable && !isAttacking &&
		    !isDamaged && !isDashing && !isDashJump) {
			if (MP >= ULT1MP) {
				AirJumpEnabled = false;
				animator.SetTrigger ("CrazyAnimals");
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
	

	
	public void PlaySEDraftRocketSE(){
		audioCtrl.pitch = DraftRocketSEPitch;
		audioCtrl.PlayOneShot(DraftRocketSE);
	}

	public void PlaySECrazyAnimalsSE(){
		audioCtrl.pitch = CrazyAnimalsSEPitch;
		audioCtrl.PlayOneShot(CrazyAnimalsSE);
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
        AtkDataDic.Add("DraftRocketATK", DraftRocketATK);
        AtkDataDic.Add("DraftRocketExplodeATK", DraftRocketExplodeATK); 
        AtkDataDic.Add("SelfExplodeATK", SelfExplodeATK); 
        AtkDataDic.Add("AppleMineATK", AppleMineATK);
        AtkDataDic.Add("MushroomSpringGATK", MushroomSpringGATK);
        AtkDataDic.Add("MushroomSpringAATK", MushroomSpringAATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
        AtkDataDic.Add("ULT2ATK", ULT2ATK);
    }

    

}
