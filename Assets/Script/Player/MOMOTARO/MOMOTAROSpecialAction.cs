using UnityEngine;
using System.Collections;

public class MOMOTAROSpecialAction : XXXCtrl
{

	//這裡就負責放每隻腳色的特性
	//例:衝刺時無敵、三段跳之類的
	//可利用動畫或狀態來判斷
	//可用動畫控制器裡面的新增功能
	
	
    //====控制參數============
    public ATKColliderData RisingFlashATK;
    public ATKColliderData DogStrikeATK;
    public ATKColliderData BirdStrikeATK;
    public ATKColliderData SerialPierce1ATK;
    public ATKColliderData SerialPierce2ATK;
    public ATKColliderData SerialPierceAATK;

    public GameObject effectObjectSlash1;
    public GameObject effectObjectDog1;
    public GameObject effectObjectBird1;

    public float ULT1MP                = 20.0f;
	public float SerialPierceMP        = 7.0f;
	public float DogStrikeMP           = 5.0f;
	public float BirdStrikeMP          = 4.0f;
	public float RisingFlashMP         = 6.0f;

	public float SerialPierceDashSpeed = 10.0f;
	public float RisingFlashDashSpeed  = 2.0f;
	public float RisingFlashJumpForce  = 600.0f;

	public float DogSpeedX = 7.0f;
	public float DogFlyForce = 150.0f;
	public float DogGravity = 1.0f;

	public float UltraSerialPierceDashSpeed = 20.0f;

	//====音效參數============
	public AudioClip normalATK1SE;
	public float normalATK1SEPitch;
	public AudioClip normalATK2SE;
	public float normalATK2SEPitch;
	public AudioClip normalATK3SE;
	public float normalATK3SEPitch;
	public AudioClip SerialPierce1SE;
	public float SerialPierce1SEPitch;
	public AudioClip SerialPierce2SE;
	public float SerialPierce2SEPitch;
	public AudioClip RisingFlashSE;
	public float RisingFlashSEPitch;

	
	//====開關============
	public bool RisingFlashJumpEnable = false;

    //volatile bool SPatkInputEnabled = false;
    //volatile bool SPatkInputNow    = false;
   // protected string SPNextATKName;

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
			RisingFlashJumpEnable = true;
		}
		if (isDamaged || isKnockBack || isKnockOuted ||
		    isKnockDown || isDashing) {
			canAttackMove = false;
		}

	}

    //====動畫用程式=======
    /*
	public void SPEnableAttackInput() {
		SPatkInputEnabled = true;
	}
	
	public void SPSetNextAttack() {
		if (SPatkInputNow == true) {
			SPatkInputNow = false;
			animator.Play(SPNextATKName);
		}
		
	}
	*/

    //===招式1  RisingFlash====================================

    public void actionSPAttack1(){
		if (RisingFlashJumpEnable && MoveEnable && !isAttacking 
		    && !isDamaged && !isDashing && !isDashJump &&
		    !isAirAttacking) {

			if (MP >= RisingFlashMP) {
				MPCalculation(RisingFlashMP);
				animator.SetTrigger ("RisingFlash");
			}

		}

		else if(RisingFlashJumpEnable && isInCombo && atkInputEnabled ){   //普攻接技可接招式

			if (MP >= RisingFlashMP) {
				MPCalculation(RisingFlashMP);
				nextATKName = "XXX_SPAttack1";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
		}
	}	

	public void actionFireSlash(){
        SetPadVibration(0.3f, 0.6f);
        GameObject effect = Instantiate (effectObjectSlash1, new Vector3 (transform.position.x + 1.5f * transform.lossyScale.x, transform.position.y + 1.2f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponentInChildren<FireObjColliderKnockOut>().owner = transform;
		effect.GetComponent<DirectionEffectCtrl>().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
	}

	public void RisingFlashDash(){
		if (stopDash)stopDash = false;
		AirJumpEnabled = false;
		RisingFlashJumpEnable = false;
		toSetUpForce = true;
		setUpForce = RisingFlashJumpForce;
		StartCoroutine(RisingFlash());
	}
	
	public void RisingFlashStopDash(){
		stopDash = true;
	}
	
	//===招式2  DogStrike=====================================
	public void actionSPAttack2(){
		if(MoveEnable && !isAttacking && !isDamaged && 
		   !isDashing && !isDashJump && grounded){

			if (MP >= DogStrikeMP) {
				MPCalculation(DogStrikeMP);
				animator.SetTrigger("DogStrike");
				//裡面可以依腳色寫一些判斷或加強
			}

		}

		else if(isInCombo && atkInputEnabled){   //普攻接技可接招式
			
			nextATKName = "XXX_SPAttack2";
			if (atkInputEnabled) {
				atkInputEnabled = false;
				atkInputNow = true;
			}
		}

	}	
	
	public void actionFireDog(){
        SetPadVibration(0.2f, 0.4f);
        GameObject effect = Instantiate (effectObjectDog1, new Vector3 (transform.position.x + 1.0f * transform.lossyScale.x, transform.position.y - 0.5f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
		effect.GetComponent<DogCtrl> ().owner = transform;

		effect.GetComponent<DogCtrl> ().DogSpeedX = DogSpeedX;
		effect.GetComponent<DogCtrl> ().DogFlyForce = DogFlyForce;
		effect.GetComponent<DogCtrl> ().DogGravity = DogGravity;
	}
	

	
	//===招式3  BirdStrike 空中====================================
	public void actionSPAttack3(){
		if(MoveEnable && !isAttacking && !isDamaged && 
		   !isDashing && !isDashJump && !grounded &&
		   !isAirAttacking){
			if (MP >= BirdStrikeMP) {
               
                MPCalculation(BirdStrikeMP);
				animator.SetTrigger("BirdStrike");

				//攻擊移動
				canAttackMove = true;
				//裡面可以依腳色寫一些判斷或加強
			}

		}

        else if (isInCombo && atkInputEnabled)
        {   //普攻接技可接招式

            if (MP >= BirdStrikeMP)
            {
                MPCalculation(BirdStrikeMP);
                nextATKName = "XXX_SPAttack3";

                //攻擊移動
                canAttackMove = true;
                //裡面可以依腳色寫一些判斷或加強
            }
            if (atkInputEnabled)
            {
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }


    }	
	
	public void actionFireBird(){
        SetPadVibration(0.2f, 0.4f);
        GameObject effect = Instantiate (effectObjectBird1, new Vector3 (transform.position.x + 1.3f * transform.lossyScale.x, transform.position.y + 0.2f * transform.lossyScale.y, transform.position.z), Quaternion.identity) as GameObject;
		effect.GetComponentInChildren<FireObjColliderKnockOut> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().owner = transform;
		effect.GetComponent<DirectionEffectCtrl> ().isFront = isFront;
	}

	//===招式4  SerialPierce ====================================
	public void actionSPAttack4(){
		if (MoveEnable && !isAttacking && !isDamaged && 
			!isDashing && !isDashJump && grounded) {

			if (MP >= SerialPierceMP) {
				MPCalculation(SerialPierceMP);
				animator.SetTrigger ("SerialPierce");
			}

		} else if (isInCombo && atkInputEnabled && grounded) {   //普攻接技可接招式

			if (MP >= SerialPierceMP) {
				MPCalculation(SerialPierceMP);
				nextATKName = "XXX_SPAttack4";
				if (atkInputEnabled) {
					atkInputEnabled = false;
					atkInputNow = true;
				}
			}
		}
	}	

	public void SerialPierceDash(){
		if (stopDash)stopDash = false;
		StartCoroutine(SerialPierce());
	}
	
	public void SerialPierceStopDash(){
		stopDash = true;
	}

	//===招式5  SerialPierceAir ====================================
	public void actionSPAttack5(){
		if(MoveEnable && !isAttacking && !isDamaged && 
		   !isDashing && !isDashJump && !grounded && !isAirAttacking)
        {
			animator.SetTrigger("SerialPierceAir");
			canAttackMove = true;
		}
        else if (isInCombo && atkInputEnabled && !grounded)
        {   //普攻接技可接招式
            nextATKName = "XXX_SPAttack5";
            canAttackMove = true;
            if (atkInputEnabled)
            {
                atkInputEnabled = false;
                atkInputNow = true;
            }

        }
    }	

	//===ULT=======================================
	
	public void actionULT1(){
		if (MoveEnable && !isAttacking &&
		    !isDamaged && !isDashing && !isDashJump) {
			if(MP >= ULT1MP){
				animator.SetTrigger ("UltraSerialPierce");
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
	
	//SerialPierce
	IEnumerator SerialPierce() {
		GetComponent<Rigidbody2D> ().gravityScale = 0;
		while (!stopDash) {
			if(isDamaged || isKnockOuted || isDead) break;
			speedX = SerialPierceDashSpeed * dir;
			toSetVelocityY = true;
			setVelocityY = 0.0f;	
			yield return null;
		}
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}

	//RisingFlash
	IEnumerator RisingFlash() {
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
		while (!stopDash) {
			if(isDamaged ||isKnockOuted) break;
			speedX = RisingFlashDashSpeed * dir;
			yield return null;
		}
		RisingFlashJumpEnable = false;  // 會有還沒跳起來就已經偵測到落地 防止用
		stopDash = false;
		GetComponent<Rigidbody2D> ().gravityScale = initGravity;
	}

	//UltraSerialPierce
	IEnumerator UltraSerialPierce() {
		float time = 0.0f;
		while (!stopDash) {
			time += Time.deltaTime;
			if(isDamaged ||isKnockOuted) break;
			speedX = (UltraSerialPierceDashSpeed - time * 50.0f) * dir; //time*50減速度用
			toSetVelocityY = true;
			setVelocityY = 0.0f;	
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
		audioCtrl.pitch = normalATK3SEPitch;
		audioCtrl.PlayOneShot(normalATK3SE);
	}

	public void PlaySESerialPierce1SE(){
		audioCtrl.pitch = SerialPierce1SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(SerialPierce1SE);
	}

	public void PlaySESerialPierce2SE(){
		audioCtrl.pitch = SerialPierce2SEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(SerialPierce2SE);
	}

	public void PlaySERisingFlashSE(){
		audioCtrl.pitch = RisingFlashSEPitch + Random.Range(-0.05f,0.05f);
		audioCtrl.PlayOneShot(RisingFlashSE);
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
        AtkDataDic.Add("RisingFlashATK", RisingFlashATK);
        AtkDataDic.Add("DogStrikeATK", DogStrikeATK);
        AtkDataDic.Add("BirdStrikeATK", BirdStrikeATK);
        AtkDataDic.Add("SerialPierce1ATK", SerialPierce1ATK);
        AtkDataDic.Add("SerialPierce2ATK", SerialPierce2ATK);
        AtkDataDic.Add("SerialPierceAATK", SerialPierceAATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
        AtkDataDic.Add("ULT2ATK", ULT2ATK);
    }

  

}
