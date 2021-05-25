using UnityEngine;
using System.Collections;

public class CrazyOldMan : MonoBehaviour {

	[System.NonSerialized] public Animator animator;
	[System.NonSerialized] public StageObjectCtrlCrazyGrandHouse stageCtrl;
	[System.NonSerialized] public GameCtrl gameCtrl;

	public GameObject HeartMarkNotePrefab;
	public GameObject AngryMarkNotePrefab;
	
	//控制參數
	public float   normalSpeed            =   3.0f;
	public float   getMadSpeed            =  11.0f;
	public float   jumpForce              = 500.0f;

	public float CrazyOldManExistTime     = 20.0f; //待機、存在時間
	       float CrazyOldManExistCTime    =  0.0f;
	public float CrazyOldManGetMadTime    = 15.0f; //發狂持續時間
	       float CrazyOldManGetMadCTime   =  0.0f;

	public float CrazyOldManMoveTime      =  1.0f; //普通移動時間
	       float CrazyOldManMoveCTime     =  0.0f;
	public float CrazyOldManMoveColdTime  =  1.0f; //普通移動間格時間
	       float CrazyOldManMoveColdCTime =  0.0f;
	public float CrazyOldManMoveRate      = 50.0f; //普通移動機率

	public float GetMadFlipTime           =  1.0f; //發狂轉身時間
	public float GetMadFlipRate           = 70.0f; //發狂轉身機率
	[System.NonSerialized] public float GetMadFlipCTime = 0.0f;

	public float GetMadJumpColdTime       =  2.0f; //發狂跳躍冷卻時間
	public float GetMadJumpRate           = 50.0f; //發狂跳躍機率
	       float GetMadJumpColdCTime      =  0.0f;

	public float AngryValue               = 7.0f;  //發狂指數
	public float InitAngryValue           = 5.0f;  //初始發狂指數
	public float PlayerNUMAngryValue      = 3.0f;  //發狂指數隨著玩家數增減量
	       float currentAngryValue        = 0.0f;
	public float AngryDecreaseTime        = 1.0f;  //發狂指數隨著時間遞減
	       float AngryDecreaseCTime       = 0.0f;


	       bool CanIncreaseGiftTime      = false;
	public float GetGiftPriceTime         =  3.0f;  //送達探望禮物成功時間
	       float GetGiftPriceCTime        =  0.0f;
	public float GiftDecreaseAngryTime    =  0.5f;  //探望禮物成功遞減時間
	       float GiftDecreaseAngryCTime   =  0.0f;
    public Transform AngryBarCanvus       =  null;  //生氣條顯示 
    public Transform GiftBar              =  null;   
    public Transform WhiteBar             =  null;

           float getInHouseDis            = 1.0f;  //回歸時離門口距離

	//狀態參數

	[System.NonSerialized] public float speedX       = 0.0f;
	[System.NonSerialized] public float dir          = 1.0f;

	[System.NonSerialized] public bool isGetBack     = false;
	[System.NonSerialized] public bool isGetMad      = false;
    [System.NonSerialized] public bool isGetGift     = false;
    [System.NonSerialized] public bool isGettingGift = false;

	[System.NonSerialized] public bool  toSetUpForce = false;
	[System.NonSerialized] public float setUpForce   = 0.0f;

	public enum AIstate{
		idle,
		move,
		getmad,
		back
	}
	AIstate aiState;

    //攻擊變數
    public GameObject effectObject   = null;
    public AudioClip  hittedSE       = null;
    public float      hittedSEPitch  = 1.0f;
    public bool       sideType       = false;
    public int        Damage         = 3;
    public float      knockOutTime   = 1.0f;
    public float      knockOutSpeedX = 7.0f;
    public float      hitForceY      = 300.0f;
    public float      knockOutGravity = 2.5f;
    public float knockOutDecressSpeed = 0.0f;

    //其他變數
    Vector2 initPos;



	void Awake(){
		animator = GetComponent<Animator>();
		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl> ();
		stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlCrazyGrandHouse>();
	}

	void Start () {
		aiState = AIstate.idle;
		speedX = 0.0f;
		dir = (transform.localScale.x > 0.0f) ? 1.0f : -1.0f;
		initPos = new Vector2 (this.transform.position.x, this.transform.position.y);

		int playerNUM = 0;
		for(int i = 0; i<=3; i++){
			if(gameCtrl.isInBattle[i])playerNUM++;
		}
		AngryValue = InitAngryValue + PlayerNUMAngryValue * playerNUM;
	}
	
	void FixedUpdate(){
		//移動計算
		GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, GetComponent<Rigidbody2D>().velocity.y);

		//跳躍計算
		if (toSetUpForce) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, setUpForce));
			toSetUpForce = false;
		}
	}

	void Update () {

        float _deltaTime = Time.deltaTime;

        //======一般判斷===================

        if (aiState == AIstate.idle) {
			CrazyOldManExistCTime += _deltaTime;
			if (CrazyOldManExistCTime >= CrazyOldManExistTime) {
				isGetBack = true;
				aiState = AIstate.back;
			}

			CrazyOldManMoveColdCTime += _deltaTime;
			if(CrazyOldManMoveColdCTime >= CrazyOldManMoveColdTime){
				if(Random.Range (0.0f, 100.0f) <= CrazyOldManMoveRate) {
					if(Random.Range (0.0f, 2.0f) <= 1.0f) actionFlip(); //一半的機率轉身走
					aiState = AIstate.move;
					animator.SetTrigger("Move");
				}
				CrazyOldManMoveColdCTime = 0.0f;
			}
			actionNormalMove(0);

		}

		//======普通移動狀態================
		if (aiState == AIstate.move) {

			CrazyOldManExistCTime += _deltaTime;
			if (CrazyOldManExistCTime >= CrazyOldManExistTime) {
				isGetBack = true;
				aiState = AIstate.back;
			}

			CrazyOldManMoveCTime += _deltaTime;
			if(CrazyOldManMoveCTime >= CrazyOldManMoveTime + Random.Range (0.0f, 2.0f)){
				aiState = AIstate.idle;
				animator.SetTrigger("Idle");
				CrazyOldManMoveCTime = 0.0f;
			}

			if(dir >= 0.0f)actionNormalMove(1);
			else
			{
				actionNormalMove(-1);
			}

		}


		//======瘋狂狀態===================
		if (aiState == AIstate.getmad) {
			//===轉身
			GetMadFlipCTime += _deltaTime;
			if(GetMadFlipCTime >=GetMadFlipTime + Random.Range (0.0f, 2.0f)){
				if(Random.Range (0.0f, 100.0f) < GetMadFlipRate){
					actionFlip();
				}
				GetMadFlipCTime = 0.0f;
			}

			//===跳躍
			GetMadJumpColdCTime += _deltaTime;
			if(GetMadJumpColdCTime >= GetMadJumpColdTime + Random.Range (0.0f, 2.0f)){
				if(Random.Range (0.0f, 100.0f) < GetMadJumpRate){
					actionJump();
				}
				GetMadJumpColdCTime = 0.0f;
			}


			//===移動
			if(dir >= 0.0f)actionGetMadMove(1);
			else{
				actionGetMadMove(-1);
			}

			//===結束
			CrazyOldManGetMadCTime += _deltaTime;
			if(CrazyOldManGetMadCTime >= CrazyOldManGetMadTime){
				aiState = AIstate.idle;
				CrazyOldManGetMadCTime = 0.0f;
				actionGetMadMove(0);  //停下
				animator.SetTrigger("Move");
				isGetBack = true;
				aiState = AIstate.back;
				//結束GetMad狀態
			}


		}

		//======收禮物 && 處理共同狀態===================
		if (aiState == AIstate.idle || aiState == AIstate.move || aiState == AIstate.back) {

			//是否可撿取禮物
			if(currentAngryValue > 0)CanIncreaseGiftTime = false;
			else{
				CanIncreaseGiftTime = true;
			}

			//拿到禮物回歸
			if(GetGiftPriceCTime >= GetGiftPriceTime){
				isGetBack = true;
				aiState = AIstate.back;
				isGetGift = true;
				isGettingGift = false;
			}

			//是否拿著禮物
			if(!isGettingGift){

				if(GetGiftPriceCTime >= 0.0f){
					GetGiftPriceCTime -= _deltaTime;
				}
				else
				{
					GetGiftPriceCTime = 0.0f;
				}

				if(GiftDecreaseAngryCTime != 0.0f)GiftDecreaseAngryCTime = 0.0f;

			}

			else{
				if(!isGetGift){
					if(stageCtrl.Gift != null){
						if(!stageCtrl.Gift.GetComponent<GiftBox>().isCatched)isGettingGift = false;
					}

					if(CanIncreaseGiftTime)GetGiftPriceCTime += _deltaTime;
					GiftDecreaseAngryCTime += _deltaTime;
					if(GiftDecreaseAngryCTime >= GiftDecreaseAngryTime){
						Instantiate(HeartMarkNotePrefab, new Vector3(this.transform.position.x +Random.Range(-0.65f, 0.65f) ,this.transform.position.y +Random.Range(0.15f, 0.65f),this.transform.position.z), Quaternion.identity) ;
						currentAngryValue -= 1;
						GiftDecreaseAngryCTime = 0.0f;
					}
				}
				else{
					GiftDecreaseAngryCTime = 0.0f;
				}
			}

			//發狂數值計算
			if(!isGetBack && !isGetGift && currentAngryValue >= 1.0f){
				AngryDecreaseCTime += Time.deltaTime;
				if(AngryDecreaseCTime >= AngryDecreaseTime){
					currentAngryValue -= 1;
					AngryDecreaseCTime = 0.0f;
				}
			}

			if(currentAngryValue <= 0.0f)currentAngryValue = 0.0f;


			if (currentAngryValue >= AngryValue) {
				if(!isGetMad){
					isGetMad = true;
					actionGetMad();
				}
			}

            //生氣條變化
            if (currentAngryValue > 0)
            {
                WhiteBar.GetComponent<RectTransform>().localPosition = new Vector3(currentAngryValue / AngryValue, 0, 0);
            }

            else if (GetGiftPriceCTime > 0.0f)
            {
                WhiteBar.GetComponent<RectTransform>().localPosition = new Vector3(GetGiftPriceCTime / GetGiftPriceTime, 0, 0);
            }

            if (GetGiftPriceCTime > 0.0f)
            {
                GiftBar.GetComponent<RectTransform>().localPosition = new Vector3(GetGiftPriceCTime / GetGiftPriceTime - 1, 0, 0);
            }
            else
            {
                GiftBar.GetComponent<RectTransform>().localPosition = new Vector3(-1.0f, 0, 0);
            }

            //生氣條隱藏&反轉
            if (currentAngryValue <= 0 && GetGiftPriceCTime <= 0.0f || isGetBack)
            {
                AngryBarCanvus.GetComponent<Canvas>().enabled = false;
            }

            else if (currentAngryValue > 0)
            {
                AngryBardisplay();
            }

            else if (GetGiftPriceCTime > 0.0f)
            {
                AngryBardisplay();
            }


        }

		//======回歸狀態===================
		if (aiState == AIstate.back) {
			isGettingGift = false;
			CanIncreaseGiftTime = false;

			if (this.transform.position.x - initPos.x <= -getInHouseDis) {
				actionNormalMove(1);
				animator.SetTrigger("Move");
				if (dir < 0) actionFlip ();
			} 

			else if (this.transform.position.x - initPos.x >= getInHouseDis) {
				actionNormalMove(-1);
				animator.SetTrigger("Move");
				if (dir > 0) actionFlip ();
			}

			else{
				actionNormalMove(0);
				animator.SetTrigger("Back");
			}


		}


		//==========其他處理=======================
		if(currentAngryValue >= AngryValue)currentAngryValue = AngryValue;

	}

	//=========自創功能========

	public void actionNormalMove(float n) {
		if (n != 0.0f) {
			speedX = normalSpeed * n;
		} 
		else {
			speedX = 0.0f;
		}
	}

	public void actionGetMadMove(float n) {
		if (n != 0.0f) {
			speedX = getMadSpeed * n;
		} 
		else {
			speedX = 0.0f;
		}
	}

	public void actionFlip() {
		dir = dir * -1.0f;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1.0f;
		transform.localScale = theScale;
	}

	public void actionJump(){
		toSetUpForce = true;
		setUpForce = jumpForce;
	}

	public void actionGetMad(){
		aiState = AIstate.getmad;
		animator.SetTrigger("GetMad");
		CrazyOldManMoveCTime = 0.0f;
		CrazyOldManMoveColdCTime = 0.0f;
		CrazyOldManExistCTime = 0.0f;
	}

	public void DstroyObj(){
		stageCtrl.CrazyOldManIsLive = false;
		Destroy(this.gameObject);
	}
	 
	public void getAngry(){
		Instantiate(AngryMarkNotePrefab, new Vector3(this.transform.position.x +Random.Range(-0.65f, 0.65f) ,this.transform.position.y +Random.Range(0.15f, 0.65f),this.transform.position.z), Quaternion.identity) ;
		if (GetGiftPriceCTime >= 0.0f) {
			GetGiftPriceCTime -= 0.5f;
			if(GetGiftPriceCTime <= 0.0f)GetGiftPriceCTime = 0.0f;
		}
		if(GetGiftPriceCTime <= 0.0f && currentAngryValue <= AngryValue)currentAngryValue += 1; 

	}

    //=========自訂======================
    void AngryBardisplay()
    {
        if (!AngryBarCanvus.GetComponent<Canvas>().enabled) AngryBarCanvus.GetComponent<Canvas>().enabled = true;


        if (dir > 0)
            AngryBarCanvus.transform.localScale = new Vector3(1, 1, 1);
        else
        {
            AngryBarCanvus.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}
