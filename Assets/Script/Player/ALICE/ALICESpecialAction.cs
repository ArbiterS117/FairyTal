using UnityEngine;
using System.Collections;

public class ALICESpecialAction : XXXCtrl {
	
	//這裡就負責放每隻腳色的特性
	//例:衝刺時無敵、三段跳之類的
	//可利用動畫或狀態來判斷
	//可用動畫控制器裡面的新增功能
	
	
    //====控制參數============
    public ATKColliderData WindWalkerGATK ;
    public ATKColliderData WindWalkerAATK ;
    public ATKColliderData AquaGrailG1ATK ;
    public ATKColliderData AquaGrailG2ATK ;
    public ATKColliderData AquaJumpATK ;
   

    public GameObject effectObjectSword1;
    public GameObject effectObjectSword2;
    public GameObject effectObjectWater1;
    public GameObject effectObjectWater2;
    public GameObject effectObjectWater3;


    public float ULT1MP = 15.0f;
	public float WindWalkerMP = 9.0f;
	public float WindWalkerAirMP = 6.0f;
	public float AquaGrailMP  = 6.0f;
	public float ForwardATKDashSpeed = 10.0f;
	public float AquaJumpForce  = 500.0f;

	public bool isLimitBreak = false;
	public float limitBreakGetMPTime  = 0.2f;
	       float limitBreakGetMPCTime = 0.0f;
	//====音效參數============
	public AudioClip normalATK1SE;
	public float normalATK1SEPitch;
	public AudioClip normalATK2SE;
	public float normalATK2SEPitch;
	public AudioClip normalATK3SE;
	public float normalATK3SEPitch;
	public AudioClip dashATKSE;
	public float dashATKSEPitch;
	public AudioClip magicATKSE;
	public float magicATKSEPitch;
	public AudioClip magicATKFastSE;
	public float magicATKFastSEPitch;
	public AudioClip magicATKSlowSE;
	public float magicATKSlowSEPitch;
	public AudioClip FlameScepterSE;
	public float FlameScepterSEPitch;
	public AudioClip LimitBreakSE;
	public float LimitBreakSEPitch;


	
	//====開關============
	public bool AquaJumpEnable = false;

    //===動畫用參數========
    volatile bool SPatkInputEnabled = false;
    volatile bool SPatkInputNow = false;
    protected string SPNextATKName;


    protected override void Awake()
    {
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

    protected override void Update(){

        base.Update();

        //測試用
        AtkDataDic.Clear();
        setStartATKData();

        if (grounded) {
			AquaJumpEnable = true;
		}
		if (!isAttacking)
			SPatkInputEnabled = false;

		if (isLimitBreak) {
            SetPadVibration(0.05f, 0.2f);
            animator.SetBool("LimitBreak", true);
			limitBreakGetMPCTime += Time.deltaTime;
			if(limitBreakGetMPCTime >= limitBreakGetMPTime){
				MP += 1;
				limitBreakGetMPCTime = 0.0f;
			}

			if(!MoveEnable || isDamaged || 
			   isDashing   || isDashJump){
				isLimitBreak = false;
			}
		}
		else{
			animator.SetBool("LimitBreak", false);
			limitBreakGetMPCTime = 0.0f;
		}

	}

	//====動畫用程式=======
	public void SPEnableAttackInput() {
		SPatkInputEnabled = true;
	}
	
	public void SPSetNextAttack() {
		if (SPatkInputNow == true) {
			SPatkInputNow = false;
			animator.Play(SPNextATKName);
		}
		
	}
	
	//===招式1  WindWalker 地上====================================
	
	public void actionSPAttack1(){
		if(MoveEnable && !isAttacking && !isDamaged && 
		   !isDashing && !isDashJump && grounded){
			animator.SetTrigger("WindWalkerG");
			//裡面可以依腳色寫一些判斷或加強
		}

		else if(isInCombo && atkInputEnabled){
			if(grounded)nextATKName = "XXX_SPAttack1";
            
            if (atkInputEnabled) {
				atkInputEnabled = false;
				atkInputNow = true;
			}
		}

	}	

	public void actionFireSword(){
		if (MP >= WindWalkerMP) {
            SetPadVibration(0.2f,0.7f);

            MPCalculation (WindWalkerMP);
			GameObject effect = Instantiate (effectObjectSword1, new Vector3 (transform.position.x + 3.5f * transform.lossyScale.x, transform.position.y + 0.35f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		}
	}
	
	//===招式2&3 AquaGrail 地上====================================
	
	public void actionSPAttack2(){
		if(!isDamaged && 
		   !isDashing && !isDashJump && grounded){

			if (!isAttacking) {
				animator.SetTrigger("AquaGrailG");
			}

			else if(isInCombo && atkInputEnabled){
				nextATKName = "XXX_SPAttack2";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}

			else {
				if (SPatkInputEnabled) {
                    SPNextATKName = "XXX_SPAttack3";
                    SPatkInputEnabled = false;
					SPatkInputNow = true;
				}
			}

		}

	}

	public void actionFire(){
		if (MP >= AquaGrailMP) {
            SetPadVibration(0.2f, 0.5f);
            MPCalculation (AquaGrailMP);
			GameObject effect = Instantiate (effectObjectWater1, new Vector3 (transform.position.x + 1.5f * transform.lossyScale.x, transform.position.y + 1.5f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		}
	}

	public void actionFire2(){
		if (MP >= AquaGrailMP) {
            SetPadVibration(0.2f, 0.5f);
            MPCalculation (AquaGrailMP);
			GameObject effect = Instantiate (effectObjectWater2, new Vector3 (transform.position.x + 1.5f * transform.lossyScale.x, transform.position.y + 1.5f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		}
	}

    //===招式4  WindWalker 空中====================================
    public void actionSPAttack4()
    {
        if (MoveEnable && !isAttacking && !isDamaged &&
           !isDashing && !isDashJump && !grounded &&
           !isAirAttacking)
        {
            animator.SetTrigger("WindWalkerA");
            //裡面可以依腳色寫一些判斷或加強
            //攻擊移動
            canAttackMove = true;
            //裡面可以依腳色寫一些判斷或加強
        }

        else if (isInCombo && atkInputEnabled)
        {
            if (!grounded) nextATKName = "XXX_SPAttack4";
            canAttackMove = true;
            if (atkInputEnabled)
            {
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
    }

	public void actionFireSword2(){
		if (MP >= WindWalkerAirMP) {
            SetPadVibration(0.2f, 0.6f);
            MPCalculation (WindWalkerAirMP);
			GameObject effect = Instantiate (effectObjectSword2, new Vector3 (transform.position.x + 2.5f * transform.lossyScale.x, transform.position.y - 1.4f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		}
	}

	//===招式  AquaJump ====================================

	public void actionSPAttackAquaJump(){
		if (AquaJumpEnable && MoveEnable && !isDashing && 
		    !isDamaged && !isAttacking && !isDashJump &&
		    !grounded && !isAirAttacking) {
            SetPadVibration(0.2f, 0.4f);
            AquaJumpEnable = false;
			toSetUpForce = true;
			setUpForce = AquaJumpForce;
            setATKData("AquaJumpATK");

			GameObject effect = Instantiate (effectObjectWater3, new Vector3 (transform.position.x - 0.1f * transform.lossyScale.x, transform.position.y -1.0f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponentInChildren<FireObjColliderKnockOut>().owner = transform;
			effect.GetComponent<DirectionEffectCtrl>().owner = transform;
			effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		}

	}

	//===招式5  limitbreak ====================================

	public void actionSPAttack5(bool b){
        if (MoveEnable && !isAttacking && !isDamaged && 
			!isDashing && !isDashJump) {
			isLimitBreak = b;
            canAttackMove = true;
        }
	
	}

	//===ULT=======================================
	
	public void actionULT1(){
		if (MoveEnable && !isAttacking &&
		    !isDamaged && !isDashing && !isDashJump) {
			if(MP >= ULT1MP){
				animator.SetTrigger ("FlameScepter");
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
		audioCtrl.pitch = normalATK3SEPitch;
		audioCtrl.PlayOneShot(normalATK3SE);
	}

	public void PlaySEDashATKSE(){
		audioCtrl.pitch = dashATKSEPitch;
		audioCtrl.PlayOneShot(dashATKSE);
	}

	public void playSEMagicATKSE(){
		audioCtrl.pitch = magicATKSEPitch;
		audioCtrl.PlayOneShot(magicATKSE);
	}

	public void playSEMagicATKFastSE(){
		audioCtrl.pitch = magicATKFastSEPitch;
		audioCtrl.PlayOneShot(magicATKFastSE);
	}

	public void playSEMagicATKSlowSE(){
		audioCtrl.pitch = magicATKSlowSEPitch;
		audioCtrl.PlayOneShot(magicATKSlowSE);
	}

	public void PlaySEFlameScepterSE(){
		audioCtrl.pitch = FlameScepterSEPitch;
		audioCtrl.PlayOneShot(FlameScepterSE);
	}

	public void PlaySELimitBreakSE(){
		audioCtrl.pitch = LimitBreakSEPitch;
		audioCtrl.PlayOneShot(LimitBreakSE);
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
        AtkDataDic.Add("WindWalkerGATK", WindWalkerGATK);
        AtkDataDic.Add("WindWalkerAATK", WindWalkerAATK);
        AtkDataDic.Add("AquaGrailG1ATK", AquaGrailG1ATK);
        AtkDataDic.Add("AquaGrailG2ATK", AquaGrailG2ATK);
        AtkDataDic.Add("AquaJumpATK", AquaJumpATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
        AtkDataDic.Add("ULT2ATK", ULT2ATK);
    }

   

}
