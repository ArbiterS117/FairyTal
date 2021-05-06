using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using XInputDotNetPure; // Required in C#
using Fungus;

public class GameCtrl : MonoBehaviour {

	DataCtrl dataCtrl;  // 儲存玩家選項資訊

    public Flowchart FunctionFlowChart;

	//角色相關

	public bool[] isInBattle                  = new bool[4];         //玩家是否加入戰局
	public GameObject[] player                = new GameObject[4];   //玩家物件

	public GameObject[] PlayerList;                                  //腳色列表
	public int[] SelectedCharacterIndex       = new int[4];          //玩家選擇腳色
	public Transform[] BirthPoint             = new Transform[4] ;   //玩家重生點
	public bool[] isDead                      = new bool[4];         //玩家是否死亡
	public float rebornTime = 2.0f;

	[System.NonSerialized] public bool[] isAI = new bool[4];         //玩家是否是AI 

	
	//計命
	public int[] playerStuck = new int[4];

    int[] teamStuck = new int[4];
    bool[] teamAlive = new bool[4];
    public int[] DebugTeam = new int[4];
    int liveTeam = 0; // 隊伍存活權

    //計時
	public float GameTime = 0.0f;
	public float finishTime = 60.0f;

    string TimeText;
    string GameoverTimeText;

	//計分
	public int[] playerPoint = new int[4]; 

	//系統相關
	public TRyCamera MainCamera;
	public float outOfTime = 30.0f;

	[System.NonSerialized] public bool isPaused = false;
    public bool GameOver = false;
    public bool CanPause = true;
	public float SlowModeTimeScale = 0.2f;

    public bool TeamMode = false;
    public bool SurvivalMode = false;
    public bool TimeModeOn = true;
    public bool MPEnable = true;
    public bool UnlimitMP = false;

    //遊戲開始倒數
    public bool CanCountDown = true;
    public bool CountDownComplete = false;

    //public float StartBornDelayTime = 0.1f;
    //float StartBornDelayCTime = 0.0f;
    bool canStartBorn = false;

    public GameObject StartBornEffect;
    public GameObject PlayerBornEffect;

    //Fade In Fade Out
    public bool canFadeOut = false;
	public float TimeUpToFadeOutTime =3.0f;
	float TimeUpToFadeOutCTime = 0.0f;

	public float FadeOutToExitTime = 2.0f;

	//世界溝通變數
	public int isUsingULT = 0;

	//文字顯示
	public Text GameTimeText;
	//public Text TimeUPText;
	//public Text WinText;
	//public Text ScoreText;

	//TIMEUP物件
	public GameObject TimeUP;

    //手把震動 參數
    float[] PVValue = new float[4];
    float[] PVDur = new float[4];
    GamePadState[] gamePadState = new GamePadState[4];


    //debug參數
    public bool DebugAITrace = false;
	public bool DebugCharacterData = false;

    
	void Awake(){
		GameTime = 0.0f;
		dataCtrl = GetComponent<DataCtrl> ();
        FunctionFlowChart = GameObject.Find("FunctionFlowChart").GetComponent<Flowchart>();
        //MainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<TRyCamera>();
    }

	void Start(){


        /*
		player[0] = Instantiate(PlayerList[SelectedCharacterIndex[0]], BirthPoint[0].position, Quaternion.identity) as GameObject;
		player[0].GetComponent<XXXCtrl>().PlayerNUM = 1;

		player[1] = Instantiate(PlayerList[SelectedCharacterIndex[1]], BirthPoint[1].position, Quaternion.identity) as GameObject;
		player[1].GetComponent<XXXCtrl>().PlayerNUM = 2;
		*/

        //抓取遊戲資訊
        if (DebugCharacterData)
        {
            for (int i = 0; i < 4; i++)
            {
                //抓取static資料 character
                if (DebugCharacterData)
                {
                    isInBattle[i] = dataCtrl.returnIsBattle(i);
                    SelectedCharacterIndex[i] = dataCtrl.returnCharacterIndex(i);
                }
            }
            TeamMode = dataCtrl.getTeamMode();

            MPEnable = dataCtrl.getMPEnable();
            UnlimitMP = dataCtrl.getUnlimitMP();

            CanPause = dataCtrl.getCanPause();
            SurvivalMode = dataCtrl.getSurvivalMode();

            if (!SurvivalMode)
            {
                finishTime = dataCtrl.getGameTime() * 60.0f; //  分轉秒
                TimeModeOn = true;
            }
            else
            {
                TimeModeOn = true;
                finishTime = dataCtrl.getSurvivalTime() * 60.0f;
                if (finishTime == 0) TimeModeOn = false;
            }


        }

        //產生玩家變數 => FlowChart倒數完控制
        //canStartBorn = false;

        //設定FLOWChart
        FunctionFlowChart.SetBooleanVariable("CanCountDown", CanCountDown);

        TimeText = "Time ";
        GameoverTimeText = "Time: 0.00";


    }

    void FixedUpdate(){

        float _fixedDeltaTime = Time.fixedDeltaTime;

        
        //產生玩家
        if (canStartBorn)
        {
            //StartBornDelayCTime += _fixedDeltaTime;
            //if (StartBornDelayCTime >= StartBornDelayTime)
            //{
                StartBorn();
                canStartBorn = false;
            //}
        }
        

        //=======關卡計時======================

        if(CountDownComplete)GameTime += _fixedDeltaTime;
        if (TimeModeOn) {
			if (GameTime < finishTime) {				
				GameTimeText.text = TimeText + (Mathf.Round ((finishTime - GameTime) * 100.0f) / 100.0f).ToString ();
			} else {

                if (SurvivalMode)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerStuck[i] > 0) dataCtrl.setPlayerLifeTime(i + 1, GameTime + 1);
                    }
                }

                GameOver = true;
				GameTimeText.text = GameoverTimeText;
			}
			
			if (finishTime - GameTime < outOfTime) {
				GameTimeText.color = new Color (1, 0, 0);
			}
		}
        //=======關卡計命======================
        if (SurvivalMode && CountDownComplete)
        {
            int liveNum = 4;
            for (int i = 0; i < 4; i++)
            {
                if (playerStuck[i] == 0)
                {
                    liveNum--;
                    if(dataCtrl.getPlayerLifeTime(i + 1) == 0) dataCtrl.setPlayerLifeTime(i + 1, GameTime);
                }
            }
            if(liveNum <= 1)
            {
                GameOver = true;
                GameTimeText.text = GameoverTimeText;
                for(int i = 0; i < 4; i++)
                {
                    if (playerStuck[i] > 0) dataCtrl.setPlayerLifeTime(i + 1, GameTime + 1);
                }
            }

            //隊伍模式
            if (TeamMode)
            {
                //玩家隊伍生命數歸零
                for(int i = 0; i < 4; i++)
                {
                    teamStuck[i] = 0;
                }
                //玩家隊伍生命數總計
                for (int i = 0; i < 4; i++)
                {                  
                    if (isInBattle[i])
                    {                       
                        if(DebugCharacterData)teamStuck[ dataCtrl.getTeamNum(i) - 1 ] += playerStuck[i]; // 隊伍生命數
                        else teamStuck[DebugTeam[i] - 1] += playerStuck[i]; 

                    }

                }
                ////玩家隊伍生命權
                liveTeam = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (teamStuck[i] == 0) teamAlive[i] = false;
                    if (teamAlive[i]) liveTeam++;
                }
                if(liveTeam < 2)
                {
                    GameOver = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerStuck[i] > 0) dataCtrl.setPlayerLifeTime(i + 1, GameTime + 1);
                    }
                }
            }

        }

        //====================控制器震動 死亡限定=================================
        #region
        for (int i =0; i < 4; i++)
        {
            gamePadState[i] = GamePad.GetState((PlayerIndex)i);

            if (isInBattle[i] && gamePadState[i].IsConnected && isDead[i])
            {
                if (PVDur[i] > 0)
                {
                    if (!GameOver) PVDur[i] -= _fixedDeltaTime;
                    else PVDur[i] -= _fixedDeltaTime / SlowModeTimeScale;
                    GamePad.SetVibration((PlayerIndex)i, PVValue[i], PVValue[i]);
                }
                else
                {
                    PVDur[i] = 0;
                    GamePad.SetVibration((PlayerIndex)i, 0.0f, 0.0f);
                }
            }
        }
        #endregion

    }

    void Update () {

		//=======玩家重生======================

		for (int i = 0; i < 4; i++) {
			if(isInBattle[i]){
				if (player[i] == null) {
					if(!isDead[i]){
						isDead[i] = true;
                        if (SurvivalMode)
                        {
                            playerStuck[i] -= 1;
                        }
						if(!SurvivalMode || SurvivalMode && playerStuck[i] > 0){
							StartCoroutine(reborn(rebornTime, i));
						}
					}
				}
			}
		}

		//=======遊戲結束======================
		if (GameOver == true) {
			Time.timeScale = SlowModeTimeScale;
            CanPause = false;
			TimeUP.SetActive (true);
			//TimeUPText.text = "Time UP".ToString();
			pointResult ();
			overFadeOutToExit ();
		}

		//=======重置=============================

		if (Input.GetKeyDown (KeyCode.F1))
        {  
			Time.timeScale = 1;
			SceneManager.LoadScene(0, LoadSceneMode.Single);  

		}

        //=====暫停===========================
        if (CanPause)
        {
            if (Input.GetKeyDown(KeyCode.P) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetButtonDown("Start"))
            {
                if (!isPaused)
                {
                    //isPaused = true;
                    //finishTime += 100.0f;
                    //Time.timeScale = 0.0f;
                    FunctionFlowChart.SendFungusMessage("PauseIn");
                }
                else
                {
                    //isPaused = false;
                    //Time.timeScale = 1;
                    FunctionFlowChart.SendFungusMessage("PauseOut");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!isPaused)
            {
                isPaused = true;
                //finishTime += 100.0f;
                Time.timeScale = 10.0f;
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1;
            }
        }


    }

	//=====功能========================================

    //產生玩家
    public void StartBorn()
    {
        //產生玩家
        for (int i = 0; i < 4; i++)
        {

            //處理
            if (isInBattle[i])
            {
                player[i] = Instantiate(PlayerList[SelectedCharacterIndex[i]], BirthPoint[i].position, Quaternion.identity) as GameObject;
                Instantiate(PlayerBornEffect, BirthPoint[i].position, Quaternion.identity);
                player[i].GetComponent<XXXCtrl>().PlayerNUM = i + 1;
                if (TeamMode)
                {
                    if (DebugCharacterData) player[i].GetComponent<XXXCtrl>().teamNum = dataCtrl.getTeamNum(i);
                    else player[i].GetComponent<XXXCtrl>().teamNum = DebugTeam[i];
                }
                else player[i].GetComponent<XXXCtrl>().teamNum = i + 1;

            }

            if (player[i] == null) { isDead[i] = true; }
            else { isDead[i] = false; }

        }

        //玩家產生後抓取隊伍生命資訊
        for (int i = 0; i < 4; i++)
        {
            //玩家生命數
            if (isInBattle[i])
            {
                if (DebugCharacterData) playerStuck[i] = dataCtrl.getStuckNum();
                if (TeamMode)
                {
                    teamStuck[player[i].GetComponent<XXXCtrl>().teamNum - 1] += playerStuck[i]; // 隊伍生命數
                }
            }
            else playerStuck[i] = 0;

            dataCtrl.setPlayerLifeTime(i + 1, 0.0f); // 重製玩家淘汰時間

        }

        //隊伍生命權 須等上方隊伍生命數完全計算完畢
        for (int i = 0; i < 4; i++)
        {
            if (teamStuck[i] > 0) teamAlive[i] = true;
            else teamAlive[i] = false;
        }


       
        //===DEbug========================
        #region

        if (!DebugCharacterData)
        {
            for (int i = 0; i < 4; i++)
            {
                //玩家生命數
                if (isInBattle[i])
                {
                    if (TeamMode)
                    {
                        teamStuck[player[i].GetComponent<XXXCtrl>().teamNum - 1] += playerStuck[i]; // 隊伍生命數
                    }
                }

            }
            //隊伍生命權 須等上方隊伍生命數完全計算完畢
            for (int i = 0; i < 4; i++)
            {
                if (teamStuck[i] > 0) teamAlive[i] = true;
                else teamAlive[i] = false;
            }
        }
        //===DEbug========================
        #endregion
    }

    //最後計分
    public void pointResult(){

		for (int i = 0; i <= 3; i++) {
			if(isInBattle[i]){
				dataCtrl.setPlayerPoint(i + 1, playerPoint[i]);
			}
		}

		//============old===================
		int big = 0;
		int tie = 0;
		for (int i = 1; i < 4; i++) {
			if (isInBattle [i]) {

				if (playerPoint [big] == playerPoint [i]) {
					tie++;
				}
				if (playerPoint [big] < playerPoint [i]) {
					big = i;
					tie = 0;
				}
			}
		}

		/*
		if(tie == 0)WinText.text = "Player " + (big + 1) + " Win".ToString();
		else{
			WinText.text = "Tie".ToString();
		}
		*/
	}

	//結束到畫面變暗
	void overFadeOutToExit(){
		TimeUpToFadeOutCTime += Time.deltaTime / SlowModeTimeScale;
		if (TimeUpToFadeOutCTime >= TimeUpToFadeOutTime) {
			//canFadeOut = true;
		}
        if (TimeUpToFadeOutCTime >= TimeUpToFadeOutTime + FadeOutToExitTime)
        {
            for(int i = 0; i< 4; i++)
            {
                GamePad.SetVibration((PlayerIndex)i, 0, 0);
            }
           

            Time.timeScale = 1;
            SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
           
        }
	}
	


	//=====重生做法=====================================

	IEnumerator reborn(float codeTime, int playerNUM){
		float time = 0;
		while (codeTime > time) {  
			time += Time.deltaTime;
			yield return 0;
		}
		player[playerNUM] = Instantiate(PlayerList[SelectedCharacterIndex[playerNUM]],new Vector3(BirthPoint[playerNUM].position.x,BirthPoint[playerNUM].position.y, 0.0f), Quaternion.identity) as GameObject;
        Instantiate(PlayerBornEffect, BirthPoint[playerNUM].position, Quaternion.identity);
        player[playerNUM].GetComponent<XXXCtrl>().PlayerNUM = playerNUM+1;
        if (TeamMode)
        {
            if(DebugCharacterData)player[playerNUM].GetComponent<XXXCtrl>().teamNum = dataCtrl.getTeamNum(playerNUM);
            else player[playerNUM].GetComponent<XXXCtrl>().teamNum = DebugTeam[playerNUM];
        }
        else player[playerNUM].GetComponent<XXXCtrl>().teamNum = playerNUM + 1;
        player [playerNUM].GetComponent<XXXCtrl>().actionInvincible(3.0f);
		isDead [playerNUM] = false;
	}

    //==========搖桿震動===============================
    public void PadVibration(int playerindex, float dur, float VValue)
    {
        PVDur[playerindex] = dur;
        PVValue[playerindex] = VValue;
        GamePad.SetVibration((PlayerIndex)playerindex, VValue, VValue);
    }

    //=============================外部呼叫用===============================
    public void SetPause(bool b)
    {
        isPaused = b;
    }

    public void SetTimeScale(float t)
    {
        Time.timeScale = t;
    }

    public void SetCanStartBorn(bool b)
    {
        canStartBorn = b;
    }

    public void SetCountDownComplete(bool b)
    {
        CountDownComplete = b;
    }

    public void ExecuteStartBornEffect()
    {
        for (int i = 0; i < 4; i++)
        {
            if (isInBattle[i])
            {
                Instantiate(StartBornEffect, BirthPoint[i].position, Quaternion.identity);
            }
        }
    }
}
