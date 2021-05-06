using UnityEngine;
using System.Collections;

public class SNOWWHITEMain : MonoBehaviour {

    GameCtrl gameCtrl;
	SNOWWHITESpecialAction playerCtrl;
	
	public bool  inputEnabled      = true;
	float move              = 0.0f;
	
	bool  ULT1Trigger       = false;
	bool  ULT1Ready         = false;
	int   ULT1InputNum      = 0;
	float ULTHPValue        = 20.0f;
	float ULT1InputTime     = 0.7f;
	float ULT1InputCTime    = 0.0f;
	
	bool  DashReadyL        = false;
	bool  DashReadyR        = false;
	float DashInputTime     = 0.2f;
	float DashInputCTime    = 0.0f;
	bool  DashReadyMid      = false;
	float DashReadyMidTime  = 0.2f;
	float DashReadyMidCTime = 0.0f;
	float PrevMoveState     = 0.0f;

    bool inputDelay = true;
    float inputDelayCTime = 0.0f;

    string playerVerticalString;
    string playerVerticalKeyString;
    string playerHorizontalString;
    string playerHorizontalKeyString;
    string playerJumpString;
    string playerAttackString;
    string playerDashString;
    string playerULTString;
    string playerFSString;

    void Awake() {
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        playerCtrl = GetComponent<SNOWWHITESpecialAction>();
	}
	
	void Start(){
        startStringCheck();
	}
	
	void Update() {

        if (gameCtrl.isPaused) return;

     
        //===程式 接受輸入===========================

        if (inputDelay == false)
        {
            inputDelayCTime += Time.deltaTime;
            if (inputDelayCTime >= 0.05f)
            {
                inputDelayCTime = 0.0f;
                inputDelay = true;
            }
        }


        if (inputEnabled) {
			
			
			//上下狀態
			float state = 0.0f;
			state += Input.GetAxis(playerVerticalString);
			state += Input.GetAxis(playerVerticalKeyString);
			if(state >= 1.0f)state = 1.0f;
			else if(state <= -1.0f)state = -1.0f;
			playerCtrl.actionState(state);
			
			//移動
			move = 0.0f;
			move += Input.GetAxis (playerHorizontalString);
			move += Input.GetAxis (playerHorizontalKeyString);
			if(move >= 1.0f)move = 1.0f;
			else if(move <= -1.0f)move = -1.0f;
			playerCtrl.actionMove (move);
			
			//轉身
			if (move > 0 && playerCtrl.dir < 0){
				playerCtrl.actionFlip ();
                DashReadyMid = false;
                DashReadyR = false;
                DashReadyL = false;
            }
			else if (move < 0 && playerCtrl.dir > 0){
				playerCtrl.actionFlip ();
                DashReadyMid = false;
                DashReadyR = false;
                DashReadyL = false;
            }

            //輸入區

            if (inputDelay)
            {

                //跳躍&受身

                if (Input.GetButtonDown(playerJumpString) && !Input.GetButtonDown(playerAttackString) && !Input.GetButtonDown(playerULTString))
                {
                    if (!playerCtrl.DownState) playerCtrl.actionJump();
                    else playerCtrl.actionJumpHelf();
                    playerCtrl.actionBackDamage();

                    inputDelay = false;
                }

                //衝刺
                if (Input.GetButtonDown(playerDashString) || (DashReadyR && move > 0) || (DashReadyL && move < 0))
                {
                    if (playerCtrl.MoveEnable && playerCtrl.CancelDashEnable)
                    {
                        if (move > 0 && playerCtrl.dir < 0)
                        {
                            playerCtrl.dir = playerCtrl.dir * -1.0f;
                            Vector3 theScale = transform.localScale;
                            theScale.x *= -1.0f;
                            transform.localScale = theScale;
                        }
                        else if (move < 0 && playerCtrl.dir > 0)
                        {
                            playerCtrl.dir = playerCtrl.dir * -1.0f;
                            Vector3 theScale = transform.localScale;
                            theScale.x *= -1.0f;
                            transform.localScale = theScale;
                        }
                    }
                    playerCtrl.actionDash();
                    DashReadyR = false;
                    DashReadyL = false;
                }
                else if (move == 0 && Input.GetButtonUp(playerDashString))
                {
                    playerCtrl.isDashing = false;
                }
                if (move == 0 && !Input.GetButton(playerDashString))
                {
                    playerCtrl.isDashing = false;
                }

                if (playerCtrl.isDashing) DashInputCTime = 0.0f;
                if (move == 0) DashReadyMidCTime = 0.0f;

                //攻擊
                if (Input.GetButtonDown(playerAttackString) && !playerCtrl.UpState && !playerCtrl.DownState)
                {
                    playerCtrl.actionAttack();

                    inputDelay = false;
                }

                if (Input.GetButtonDown(playerAttackString) && playerCtrl.DownState)
                {
                    playerCtrl.actionDownAttack();

                    inputDelay = false;
                }

                if (Input.GetButtonDown(playerAttackString) && playerCtrl.UpState)
                {
                    playerCtrl.actionUpAttack();

                    inputDelay = false;
                }

                //特殊衝刺攻擊判斷
                if (playerCtrl.isDashing && playerCtrl.grounded)
                {
                    if (Input.GetButtonDown(playerAttackString))
                    {
                        playerCtrl.actionAttack();

                        inputDelay = false;
                    }
                }

                //招式

                if (Input.GetButtonDown(playerULTString) && !playerCtrl.UpState && !playerCtrl.DownState)
                {
                    if (!ULT1Ready) playerCtrl.actionSPAttack1();
                    else playerCtrl.actionULT1();

                    inputDelay = false;
                }

                if (Input.GetButtonDown(playerULTString) && playerCtrl.UpState)
                {
                    if (playerCtrl.grounded)
                    {
                        if (!ULT1Ready) playerCtrl.actionSPAttack3();
                        else playerCtrl.actionULT1();

                        inputDelay = false;
                    }
                    else
                    {
                        if (!ULT1Ready) playerCtrl.actionSPAttackMushroomJump();
                        else playerCtrl.actionULT1();

                        inputDelay = false;
                    }
                }

                if (Input.GetButtonDown(playerULTString) && playerCtrl.DownState)
                {
                    if (!ULT1Ready) playerCtrl.actionSPAttack2();
                    else playerCtrl.actionULT1();

                    inputDelay = false;
                }

                if (Input.GetButtonDown(playerFSString))
                {
                    if (playerCtrl.HP <= ULTHPValue) playerCtrl.actionULT1();
                    inputDelay = false;
                }
            }
			//===程式 判斷==============================
			
			if(playerCtrl.HP <= 0){
				inputEnabled = false;
			}
			
			if(playerCtrl.HP <= ULTHPValue)ULT1Inputdetermine();         //ULT判斷
            if (!playerCtrl.isDashing) DashInputdetermine();
			PrevMoveState = move;        //處理前一偵的移動狀態
			
		}
		
	}
	
	//=========自訂
	
	void DashInputdetermine(){
		if((move >0 && PrevMoveState <=0) || (move <0 && PrevMoveState >=0)){ //雙按移動衝刺判斷
			DashReadyMid = true;
		}
		
		if((DashReadyMid && move == 0 && PrevMoveState >0)){ //雙按移動衝刺判斷
			DashReadyR = true;
			DashReadyMid = false;
		}
		
		if((DashReadyMid && move == 0 && PrevMoveState <0)){ //雙按移動衝刺判斷
			DashReadyL = true;
			DashReadyMid = false;
		}
		
		if(DashReadyMid){
			DashReadyMidCTime += Time.deltaTime;
			if(DashReadyMidCTime >= DashReadyMidTime){
				DashReadyMidCTime = 0.0f;
				DashReadyMid = false;
			}
		}
		if(DashReadyR || DashReadyL){
			DashInputCTime += Time.deltaTime;
			if(DashInputCTime >= DashInputTime){
				DashInputCTime = 0.0f;
				DashReadyR = false;
				DashReadyL = false;
			}
		}
		
	}
	
	void ULT1Inputdetermine(){
		if(playerCtrl.DownState){
			if(!ULT1Trigger){
				ULT1Trigger = true;
				ULT1InputNum +=1;
			}
		}
		else{
			if(ULT1Trigger){
				ULT1Trigger = false;
			}
		}
		
		if(ULT1InputNum > 0 || ULT1Ready){
			ULT1InputCTime += Time.deltaTime;
			if(ULT1InputCTime >= ULT1InputTime){
				ULT1InputCTime = 0.0f;
				ULT1Trigger = false;
				ULT1InputNum = 0;
			}
		}
		
		if (ULT1InputNum >= 2) {
			ULT1Ready = true;
		}
		else{
			ULT1Ready = false;
		}
		
	}

    void startStringCheck()
    {
        if (playerCtrl.PlayerNUM == 1)
        {
            tag = "Player1";
            playerAttackString = "P1Attack";
            playerJumpString = "P1Jump";
            playerDashString = "P1Dash";
            playerULTString = "P1ULT";
            playerVerticalString = "P1Vertical";
            playerVerticalKeyString = "P1VerticalKey";
            playerHorizontalString = "P1Horizontal";
            playerHorizontalKeyString = "P1HorizontalKey";
            playerFSString = "P1FinalSmash";
        }
        else if (playerCtrl.PlayerNUM == 2)
        {
            tag = "Player2";
            playerAttackString = "P2Attack";
            playerJumpString = "P2Jump";
            playerDashString = "P2Dash";
            playerULTString = "P2ULT";
            playerVerticalString = "P2Vertical";
            playerVerticalKeyString = "P2VerticalKey";
            playerHorizontalString = "P2Horizontal";
            playerHorizontalKeyString = "P2HorizontalKey";
            playerFSString = "P2FinalSmash";
        }
        else if (playerCtrl.PlayerNUM == 3)
        {
            tag = "Player3";
            playerAttackString = "P3Attack";
            playerJumpString = "P3Jump";
            playerDashString = "P3Dash";
            playerULTString = "P3ULT";
            playerVerticalString = "P3Vertical";
            playerVerticalKeyString = "P3VerticalKey";
            playerHorizontalString = "P3Horizontal";
            playerHorizontalKeyString = "P3HorizontalKey";
            playerFSString = "P3FinalSmash";
        }
        else if (playerCtrl.PlayerNUM == 4)
        {
            tag = "Player4";
            playerAttackString = "P4Attack";
            playerJumpString = "P4Jump";
            playerDashString = "P4Dash";
            playerULTString = "P4ULT";
            playerVerticalString = "P4Vertical";
            playerVerticalKeyString = "P4VerticalKey";
            playerHorizontalString = "P4Horizontal";
            playerHorizontalKeyString = "P4HorizontalKey";
            playerFSString = "P4FinalSmash";
        }
    }
	
}
