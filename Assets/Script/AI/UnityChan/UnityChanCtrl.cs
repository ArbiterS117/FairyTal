using UnityEngine;
using System.Collections;

public class UnityChanCtrl : NPCEnemyBase {

	[System.NonSerialized] public Animator animator;
	GameCtrl gameCtrl;

	public Transform TargetTransform;

    //一般參數
    public GameObject effectObjectBullet;
    
    //控制參數
    public float   normalSpeed            =  10.0f;
	public float   jumpForce              = 500.0f;

	public float changeTargetTime = 5.0f;
	float changeTargetCTime = 0.0f;

	public float followDistance = 2.0f;
	public float MeeleATKDistance = 2.0f;
	
	public float followColdTime   = 0.0f;
	float followCTime = 0.0f;
	
	public float attackColdTime = 0.0f;
	float attackCTime = 0.0f;
	public float attackContinueColdTime = 0.1f; // 避免短時間內連續出招
	float attackContinueCTime = 0.0f;

    public float BladeATKDashSpeed = 7.0f;

    public Transform fireBulletTransform; // 子彈發射位置

    public float bulletSpeed = 4.0f;
    public float bulletLifeTime = 2.0f;

    // 攻擊資訊

    public ATKColliderData BladeATK1ATK;
    public ATKColliderData BladeATK2ATK;
    public ATKColliderData BladeATK3ATK;
    public ATKColliderData BulletATK_ATK;

    //狀態參數

    [System.NonSerialized] public bool isDead = false;
    [System.NonSerialized]  public bool isAttacking = false;

    [System.NonSerialized]  public bool isTargeted = false;
    [System.NonSerialized]  public int  targetNUM = 0;

	public enum AIstate{
		move,
		Attack,
		ready,
		follow,
		dead
	}
	public AIstate aiState;

	//動畫判斷參數
	public readonly static int ANISTS_BladeAttack = Animator.StringToHash("BladeAttack");
	public readonly static int ANISTS_GunAttack = Animator.StringToHash("GunAttack");
	public readonly static int ANISTS_DoubleGunAttack = Animator.StringToHash("DoubleGunAttack");

    //====開關============
    bool stopDash = false;




    protected override void Awake(){
		base.Awake();

		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		animator = GetComponent<Animator>();
        fireBulletTransform = transform.Find("FireBulletTransform").transform;

    }

    protected override void Start () {
		base.Start();
        setStartATKData();

    }

	protected override void FixedUpdate(){
		base.FixedUpdate();
	}

	protected override void Update () {
		base.Update();

		if (HP <= 0) {
			if(!isDead){
				aiState = AIstate.dead;
				animator.SetTrigger("Dead");
				isDead = true;
                stopDash = true;
            }
		}

		//動畫狀態判定
		animState();

		if (TargetTransform != null && !isDead) {
			if (this.transform.position.x - TargetTransform.position.x <= -followDistance 
			    || this.transform.position.x - TargetTransform.position.x >= followDistance) {
				if (aiState != AIstate.follow) {
					followCTime += Time.deltaTime;
					if (followCTime >= followColdTime + Random.Range (0.0f, 1.0f)) {
						aiState = AIstate.follow;
						followCTime = 0.0f;
					}
				}
			}
		}
		//============準備狀態==================
		
		if (aiState == AIstate.ready) {
			if (TargetTransform != null) {
				if (this.transform.position.x - TargetTransform.position.x >= -followDistance 
				    && this.transform.position.x - TargetTransform.position.x <= followDistance) {
					attackCTime += Time.deltaTime;
					if (attackCTime >= attackColdTime) {
						aiState = AIstate.Attack;
						attackCTime = 0.0f;
					}
				}
			}
		}
		
		//============抓取目標==================
		if(gameCtrl.DebugAITrace){//debug

			if(!isDead){
				if (!isTargeted) {
					if(FindTarget()) isTargeted = true;
					changeTargetCTime = 0.0f;
				}
				
				if (isTargeted) {
					if(gameCtrl.isDead[targetNUM]){
						TargetTransform = null;
						isTargeted = false;
					}

					changeTargetCTime += Time.deltaTime;
					if(changeTargetCTime >= changeTargetTime){
						TargetTransform = null;
						isTargeted = false;
						changeTargetCTime = 0.0f;
					}
				}
			}
			
		}//debug
		
		
		//================跟隨===================
		if (aiState == AIstate.follow) {
			followTarget();
		}
		
		//================攻擊====================
		if (aiState == AIstate.Attack) {
			if (TargetTransform != null) {
				if(this.transform.position.x - TargetTransform.position.x >= -followDistance 
				   && this.transform.position.x - TargetTransform.position.x <= followDistance){
					
					//判斷敵方位於前or後
					if(this.transform.position.x > TargetTransform.position.x && dir > 0 ||
					   this.transform.position.x < TargetTransform.position.x && dir < 0){
						actionFlip();
					}
					attackContinueCTime += Time.deltaTime;
					if(attackContinueCTime >= attackContinueColdTime){
						
						if(Mathf.Abs(this.transform.position.x - TargetTransform.position.x) <= followDistance &&
						   Mathf.Abs(this.transform.position.x - TargetTransform.position.x) > MeeleATKDistance)
						{
							float randomNUM = Random.Range(0.0f,6.0f);
							
							if(randomNUM <= 3.0f){
								actionGunATK();
							}
							else{
								actionDoubleGunATK();
							}
							
							attackContinueCTime = 0.0f;
						}
						
						else if(Mathf.Abs(this.transform.position.x - TargetTransform.position.x) <= MeeleATKDistance){
							float randomNUM = Random.Range(0.0f,7.0f);
							if(randomNUM <= 3.0f){
								actionBladeATK();
							}
							else if(randomNUM <= 5.0f){
								//playerCtrl.animator.SetTrigger ("DashAttack");
							}
							else{
								//playerSpecial.actionSPAttack2();
							}
							attackContinueCTime = 0.0f;
						}
						
					}
					
				}
				else{
					aiState = AIstate.ready;
				}
			}
		}

		//================死亡====================
		if (aiState == AIstate.dead) {
			actionNormalMove (0);
			transform.Find("Collider_ReceiveDMG").GetComponent<BoxCollider2D>().enabled = false;
		}



	}

	//==============================自創功能=================================
	
	public void actionNormalMove(float n) {
		if (!isAttacking) {
			if (n != 0.0f) {
				speedX = normalSpeed * n;
				animator.SetFloat ("SpeedX", Mathf.Abs (n));
			} else {
				speedX = 0.0f;
				animator.SetFloat ("SpeedX", 0);
			}
		}
	}

	public void actionFlip() {
		if (!isAttacking) {
			dir = dir * -1.0f;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1.0f;
			transform.localScale = theScale;
		}
	}
	
	public void actionJump(){
		toSetUpForce = true;
		setUpForce = jumpForce;
	}

	public void DstroyObj(){
		Destroy(this.gameObject);
	}

	//==========抓取目標
	bool FindTarget(){
		int randomNUM = 0;
		randomNUM = Mathf.FloorToInt(Random.Range(0.0f, 4.0f));
		if (randomNUM == 4)	randomNUM = 3; // 怕數值剛好取到4.0
		targetNUM = randomNUM;
		
		if (gameCtrl.isDead [targetNUM]) {
			return false;
		}
		else {
			if(gameCtrl.player[targetNUM]!=null)TargetTransform = gameCtrl.player[targetNUM].transform;
			return true;
		}
		
	}
	
	//==========跟隨目標
	void followTarget(){
		if (TargetTransform != null) {
			if (this.transform.position.x - TargetTransform.position.x <= -followDistance) {
				actionNormalMove (1);
				if (dir < 0)
					actionFlip ();
				aiState = AIstate.follow;
			} else if (this.transform.position.x - TargetTransform.position.x >= followDistance) {
				actionNormalMove (-1);
				if (dir > 0)
					actionFlip ();
				aiState = AIstate.follow;
			} else {
				actionNormalMove (0);
				aiState = AIstate.ready;
			}
		}
	}
	
	//==========================================================

	//====動畫狀態判斷
	void animState(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (stateInfo.shortNameHash == ANISTS_BladeAttack ||
			stateInfo.shortNameHash == ANISTS_GunAttack ||
			stateInfo.shortNameHash == ANISTS_DoubleGunAttack) 
		{
			isAttacking = true;
		}
		else 
		{
			isAttacking = false;
		}

	}

    //====動畫用程式=======

    //===BladeATK========================================================
    public void actionBladeATK()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BladeATK");
        }
    }

    public void BladeATKDashStopDash()
    {
        stopDash = true;
        speedX = 0.0f;
    }



    //===GunATK====================================================
    public void actionGunATK()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("GunATK");
        }
    }

    //===DoubleGunATK=====================================================
    public void actionDoubleGunATK()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("DoubleGunATK");
        }
    }

    public void actionFireBullet(float angle)
    {
        GameObject effect = Instantiate(effectObjectBullet, new Vector3(fireBulletTransform.position.x, fireBulletTransform.position.y, fireBulletTransform.position.z), Quaternion.identity) as GameObject;
        effect.GetComponentInChildren<BulletCollider>().owner = transform;
        effect.GetComponent<UnityChanBullet>().owner = transform;
        effect.GetComponent<UnityChanBullet>().angle = angle;
        effect.GetComponent<DirectionEffectCtrl>().owner = transform;

    }

    //=====================實作部分================
    IEnumerator BladeATKDash()
    {
        while (!stopDash)
        {
            speedX = BladeATKDashSpeed * dir;
            toSetVelocityY = true;
            setVelocityY = 0.0f;
            yield return 0;
        }
        stopDash = false;
    }

    //===設置攻擊力============================
    //設置初始攻擊資料
    private void setStartATKData()
    {
        AtkDataDic.Add("BladeATK1ATK", BladeATK1ATK);
        AtkDataDic.Add("BladeATK2ATK", BladeATK2ATK);
        AtkDataDic.Add("BladeATK3ATK", BladeATK3ATK);
        AtkDataDic.Add("BulletATK_ATK", BulletATK_ATK);

    }

}
