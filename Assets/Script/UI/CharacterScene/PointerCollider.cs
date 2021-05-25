using UnityEngine;
using Fungus;


public class PointerCollider : MonoBehaviour {

	CharacterSelectDataCtrl characterData;
	CharacterSelectSceneCtrl sceneCtrl;

    public Flowchart SettingFlowchart = null;

	PointerCtrl pointerCtrl;

	AudioSource audioCtrl;

	//儲存玩家資訊
	int playerNUM = 0;

    Collider2D InWatCollider = null;

	//控制變數
	bool isChooseAI;
	int  ChooseAINUM = 0;

	public Sprite normalPointerSprite;
	public Sprite selectAIPointerSprite;
	
	public AudioClip ChooseSE;
	public float ChooseSEPitch;
	public AudioClip CancelSE;
	public float CancelSEPitch;
	public AudioClip StartSE;
	public float StartSEPitch;

	bool pressedGameStart = false;

    string playerJumpString;
    string playerDashString;


    void Awake(){
		characterData = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
		sceneCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
		pointerCtrl = transform.parent.GetComponent<PointerCtrl>();
		audioCtrl = GetComponent<AudioSource>();
        InWatCollider = null;
        SettingFlowchart = GameObject.Find("Rules Flowchart").GetComponent<Flowchart>();

    }

	void Start() {
        startStringCheck();
	}

	void Update() {

		if (pointerCtrl.moveEnable && !pointerCtrl.SettingStopMove && !pointerCtrl.SettingMap) {

			if (characterData.isSelected [playerNUM - 1]) {
				if (Input.GetButtonDown (playerDashString) && !isChooseAI) {
					characterData.isSelected [playerNUM - 1] = false;
					sceneCtrl.setSceneSelectedFalse (playerNUM);

					audioCtrl.pitch = CancelSEPitch ;
					audioCtrl.PlayOneShot(CancelSE);

				}
				
			} else {
				if (Input.GetButtonDown (playerDashString) && !isChooseAI) {
					pointerCtrl.eject ();
					audioCtrl.pitch = CancelSEPitch ;
					audioCtrl.PlayOneShot(CancelSE);
                    InWatCollider = null;
                }
			}

			if (Input.GetButtonDown (playerDashString)) {
				if (isChooseAI) {
					isChooseAI = false;
					characterData.inChoosenAI [ChooseAINUM - 1] = false;
					sceneCtrl.setPlayerImageStartIdle (ChooseAINUM);

					audioCtrl.pitch = CancelSEPitch ;
					audioCtrl.PlayOneShot(CancelSE);
				}
			}


			if (isChooseAI) {
				if (characterData.isInCtrl [ChooseAINUM - 1] == true) {//選AI中 該玩家加入
					isChooseAI = false;
					characterData.inChoosenAI [ChooseAINUM - 1] = false;
					sceneCtrl.setPlayerImageStartIdle (ChooseAINUM);

					audioCtrl.pitch = CancelSEPitch ;
					audioCtrl.PlayOneShot(CancelSE);
				}
				transform.parent.GetComponentInChildren<SpriteRenderer> ().sprite = selectAIPointerSprite;
			}
			else {
				transform.parent.GetComponentInChildren<SpriteRenderer> ().sprite = normalPointerSprite;
			}

            //=========進入collider時候
            if (InWatCollider)
            {
                if (!isChooseAI && characterData.isInCtrl[playerNUM - 1]) PlayerImageStartScene(InWatCollider);
                else
                {
                    AIPlayerImageStartScene(InWatCollider);
                }
                //====選擇腳色
                if (!characterData.isSelected[playerNUM - 1] && !isChooseAI)
                {
                    if (Input.GetButtonDown(playerJumpString))
                    {
                        selectCharacter(InWatCollider);
                    }
                }

                //====開啟選AI部分
                if (!isChooseAI && Input.GetButtonDown(playerJumpString))
                {
                    startChooseAICharacter(InWatCollider);
                }
                if (isChooseAI && Input.GetButtonDown(playerJumpString))
                {
                    selectAICharacter(InWatCollider);

                }

                //====更換隊伍
                if (Input.GetButtonDown(playerJumpString)) TeamSelect(InWatCollider);

                //====遊戲開始
                if (Input.GetButton(playerJumpString)) PushGameStart(InWatCollider);

               
            }


        }

        if (pointerCtrl.moveEnable && !pointerCtrl.SettingStopMove && !pointerCtrl.SettingMap)
        {
            //先重製
            pointerCtrl.isPressedDown = false;
            pointerCtrl.isPressedUp = false;


            if (Input.GetButtonDown(playerJumpString))
            {
                pointerCtrl.isPressed = true;
                pointerCtrl.isPressedDown = true;
            }
            else if (Input.GetButtonUp(playerJumpString))
            {
                pointerCtrl.isPressed = false;
                pointerCtrl.isPressedUp = true;
                pointerCtrl.isPressedInbtn = false;
            }

            
        }
        else
        {
            pointerCtrl.isPressedDown = false;
            pointerCtrl.isPressedUp = false;
        }

        if(!pointerCtrl.SettingStopMove && !characterData.isInCtrl[playerNUM - 1] && !pointerCtrl.SettingMap)
        {
           if(Input.GetButtonDown(playerDashString)) pointerCtrl.backToMainMenuCtrl.SetIsBacking(true);
           if(Input.GetButtonUp(playerDashString)) pointerCtrl.backToMainMenuCtrl.SetIsBacking(false);
        }

    }

    //=================進入collider==========================
    void OnTriggerEnter2D(Collider2D other) {

        if (!other.CompareTag("Player")) // 玩家指標重疊
        {
            InWatCollider = other; 
        }


    }

	//=================離開collider==========================
	void OnTriggerExit2D(Collider2D other) {

        if (!other.CompareTag("Player"))
        {
            if (!isChooseAI) PlayerImageStartIdleScene(other);
            else
            {
                AIPlayerImageStartIdleScene(other);
            }
            InWatCollider = null;

        }

    }

	//=================自創==========================

	//=====判斷選角框腳色顯示

	//腳色滑入部分
	void PlayerImageStartScene(Collider2D other){
		if (other.transform.parent.CompareTag("REDCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,0);
		}
		else if (other.transform.parent.CompareTag("ALICECharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,1);
		}
		else if (other.transform.parent.CompareTag("MOMOTAROCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,2);
		}
		else if (other.transform.parent.CompareTag("SNOWWHITECharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,3);
		}
		else if (other.transform.parent.CompareTag("RAPUNZELCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,4);
		}
        else if (other.transform.parent.CompareTag("ALADDINCharacterSelectBlock"))
        {
            sceneCtrl.setPlayerImageStart(playerNUM,5);
        }
        else if (other.transform.parent.CompareTag("RANDOMCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(playerNUM,6);
		}
	}

	void PlayerImageStartIdleScene(Collider2D other){
		sceneCtrl.setPlayerImageStartIdle (playerNUM);
	}
	
	//=====判斷選角 && 選定腳色部分
	void selectCharacter(Collider2D other){
		if (other.transform.parent.CompareTag("REDCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,0);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,0);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("ALICECharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,2);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,1);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("MOMOTAROCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,4);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,2);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("SNOWWHITECharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,6);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,3);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("RAPUNZELCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,8);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,4);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
			
		}
        else if (other.transform.parent.CompareTag("ALADDINCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(playerNUM,10);
			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,5);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
			
		}

		else if (other.transform.parent.CompareTag("RANDOMCharacterSelectBlock")) {
			float randomNUM = Random.Range(0.0f,6.0f);
			if(randomNUM <= 1.0f)characterData.setSelectedCharacterIndex(playerNUM,0);
			else if(randomNUM <= 2.0f)characterData.setSelectedCharacterIndex(playerNUM,2);
			else if(randomNUM <= 3.0f)characterData.setSelectedCharacterIndex(playerNUM,4);
			else if(randomNUM <= 4.0f)characterData.setSelectedCharacterIndex(playerNUM,6);
			else if(randomNUM <= 5.0f)characterData.setSelectedCharacterIndex(playerNUM,8);
            else if(randomNUM <= 6.0f)characterData.setSelectedCharacterIndex(playerNUM,10);

			sceneCtrl.setSceneSelectedCharacterIndex(playerNUM,6);
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
			
		}
	}

	//=======判斷選AI腳色=============
	void startChooseAICharacter(Collider2D other){
		if (!characterData.isInCtrl[0] && other.transform.parent.CompareTag("AIPlayer1Block")) {
			if (!characterData.inChoosenAI [0] && !characterData.isSelected[0]) {
				AutoChooseAI(1);
			}
			else if (!characterData.inChoosenAI [0] && characterData.isSelected[0]){
				isChooseAI = true;
				ChooseAINUM = 1;
				characterData.inChoosenAI [0] = true;
				characterData.isSelected [ChooseAINUM - 1] = false;
				sceneCtrl.setSceneSelectedFalse (ChooseAINUM);
			}
		} 

		else if (!characterData.isInCtrl[1] && other.transform.parent.CompareTag("AIPlayer2Block")) {
			if (!characterData.inChoosenAI [1] && !characterData.isSelected[1]) {
				AutoChooseAI(2);
			}
			else if (!characterData.inChoosenAI [1] && characterData.isSelected[1]){
				isChooseAI = true;
				ChooseAINUM = 2;
				characterData.inChoosenAI [1] = true;
				characterData.isSelected [ChooseAINUM - 1] = false;
				sceneCtrl.setSceneSelectedFalse (ChooseAINUM);
			}

		} 

		else if (!characterData.isInCtrl[2] && other.transform.parent.CompareTag("AIPlayer3Block")) {
			if (!characterData.inChoosenAI [2] && !characterData.isSelected[2]) {
				AutoChooseAI(3);
			}
			else if (!characterData.inChoosenAI [2] && characterData.isSelected[2]){
				isChooseAI = true;
				ChooseAINUM = 3;
				characterData.inChoosenAI [2] = true;
				characterData.isSelected [ChooseAINUM - 1] = false;
				sceneCtrl.setSceneSelectedFalse (ChooseAINUM);
			}
		}

		else if (!characterData.isInCtrl[3] && other.transform.parent.CompareTag("AIPlayer4Block")) {
			if (!characterData.inChoosenAI [3] && !characterData.isSelected[3]) {
				AutoChooseAI(4);
			}
			else if (!characterData.inChoosenAI [3] && characterData.isSelected[3]){
				isChooseAI = true;
				ChooseAINUM = 4;
				characterData.inChoosenAI [3] = true;
				characterData.isSelected [ChooseAINUM - 1] = false;
				sceneCtrl.setSceneSelectedFalse (ChooseAINUM);
			}
		}
	}

	void selectAICharacter(Collider2D other){
		if (other.transform.parent.CompareTag("REDCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,1);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,0);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;

			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("ALICECharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,3);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,1);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}
		else if (other.transform.parent.CompareTag("MOMOTAROCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,5);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,2);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);

		}

		else if (other.transform.parent.CompareTag("SNOWWHITECharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,7);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,3);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;

			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
		}
		else if (other.transform.parent.CompareTag("RAPUNZELCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,9);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,4);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
		}
        else if (other.transform.parent.CompareTag("ALADDINCharacterSelectBlock")) {
			characterData.setSelectedCharacterIndex(ChooseAINUM,11);
			sceneCtrl.setSceneSelectedCharacterIndex(ChooseAINUM,5);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
		}

		else if (other.transform.parent.CompareTag("RANDOMCharacterSelectBlock")) {
			AutoChooseAI(ChooseAINUM);
			isChooseAI = false;
			characterData.inChoosenAI [ChooseAINUM - 1] = false;
			
			audioCtrl.pitch = ChooseSEPitch ;
			audioCtrl.PlayOneShot(ChooseSE);
		}

	}

    //=======選AI腳色滑入部分=============
    void AIPlayerImageStartScene(Collider2D other){
		if (other.transform.parent.CompareTag("REDCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,0);
		}
		else if (other.transform.parent.CompareTag("ALICECharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,1);
		}
		else if (other.transform.parent.CompareTag("MOMOTAROCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,2);
		}
		else if (other.transform.parent.CompareTag("SNOWWHITECharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,3);
		}
		else if (other.transform.parent.CompareTag("RAPUNZELCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,4);
		}
        else if (other.transform.parent.CompareTag("ALADDINCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,5);
		}
		else if (other.transform.parent.CompareTag("RANDOMCharacterSelectBlock")) {
			sceneCtrl.setPlayerImageStart(ChooseAINUM,6);
		}
	}
	
	void AIPlayerImageStartIdleScene(Collider2D other){
		sceneCtrl.setPlayerImageStartIdle (ChooseAINUM);
	}

    //=======判斷選擇隊伍部分=============
    void TeamSelect(Collider2D other)
    {
        ColliderID id = other.GetComponent<ColliderID>();
        if (id != null)
        {
            if(id.ID == "Team") characterData.ChangeTeam(id.playerNum);
        }
    }

	//====判斷遊戲開始

	void PushGameStart(Collider2D other){
		if (other.transform.parent.CompareTag("GameStartButton")) {
			if(!pressedGameStart){
				pressedGameStart = true;
				characterData.GameStart();
			
				audioCtrl.pitch = StartSEPitch ;
				audioCtrl.PlayOneShot(StartSE);
			}
		}
	}

    public void SetPressedGameStart(bool b)
    {
        pressedGameStart = b;
    }

    //====AutoChooseAI
    void AutoChooseAI(int AINUM){
		float randomNUM = Random.Range(0.0f,6.0f);
		if(randomNUM <= 1.0f)characterData.setSelectedCharacterIndex(AINUM,1);
		else if(randomNUM <= 2.0f)characterData.setSelectedCharacterIndex(AINUM,3);
		else if(randomNUM <= 3.0f)characterData.setSelectedCharacterIndex(AINUM,5);
		else if(randomNUM <= 4.0f)characterData.setSelectedCharacterIndex(AINUM,7);
		else if(randomNUM <= 5.0f)characterData.setSelectedCharacterIndex(AINUM,9);
        else if(randomNUM <= 6.0f)characterData.setSelectedCharacterIndex(AINUM,11);

		sceneCtrl.setSceneSelectedCharacterIndex(AINUM,6);
	}

    void startStringCheck()
    {
        if (this.transform.parent.CompareTag("Player1"))
        {
            playerNUM = 1;
            playerJumpString = "P1Jump";
            playerDashString = "P1Dash";
        }
        else if (this.transform.parent.CompareTag("Player2"))
        {
            playerNUM = 2;
            playerJumpString = "P2Jump";
            playerDashString = "P2Dash";
        }
        else if (this.transform.parent.CompareTag("Player3"))
        {
            playerNUM = 3;
            playerJumpString = "P3Jump";
            playerDashString = "P3Dash";
        }
        else if (this.transform.parent.CompareTag("Player4"))
        {
            playerNUM = 4;
            playerJumpString = "P4Jump";
            playerDashString = "P4Dash";
        }
    }

}
