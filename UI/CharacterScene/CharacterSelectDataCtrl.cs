using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

/// <summary>
///  選角畫面參數控制
/// </summary>

public class CharacterSelectDataCtrl : MonoBehaviour {

	DataCtrl dataCtrl;

    public Flowchart FunctionFlowchart;
    public Flowchart SettingFlowchart;

    //選擇資料
    public bool[] isInCtrl                    = new bool[4]; // 玩家控制中
	public bool[] inChoosenAI                 = new bool[4]; // 幾號AI正在被選擇 防止多名玩家同時選擇同一號AI

	public int[] SelectedCharacterIndex       = new int[4];   
    
    public int[] PlayerTeam                   = new int[4];  // 玩家隊伍

	//控制參數
	public bool[] isSelected                  = new bool[4];

	//bool  switchScene      = false;
	//float switchSceneTime  = 1.0f;
	//float switchSceneCTime = 0.0f;

	public bool canStart = false;

    //保存物件
    public GameObject P1ChangeTeamBtn;
    public GameObject P2ChangeTeamBtn;
    public GameObject P3ChangeTeamBtn;
    public GameObject P4ChangeTeamBtn;


    void Awake(){
		dataCtrl = GetComponent<DataCtrl> ();
    }

	void Start () {
		for (int i = 0; i<=3; i++) {
			isInCtrl[i] = false;
			SelectedCharacterIndex[i] = 0;
			isSelected[i] = false;

            PlayerTeam[i] = dataCtrl.getTeamNum(i);

            FunctionFlowchart.SetIntegerVariable("P" + (i+1).ToString() + "TeamNum", dataCtrl.getTeamNum(i));
        }
	}
	
	
	void Update () {

        if (dataCtrl.getTeamMode())
        {
            if (isSelected[0]) FunctionFlowchart.SendFungusMessage("P1TeambtnShow"); else FunctionFlowchart.SendFungusMessage("P1TeambtnVanish"); 
            if (isSelected[1]) FunctionFlowchart.SendFungusMessage("P2TeambtnShow"); else FunctionFlowchart.SendFungusMessage("P2TeambtnVanish");
            if (isSelected[2]) FunctionFlowchart.SendFungusMessage("P3TeambtnShow"); else FunctionFlowchart.SendFungusMessage("P3TeambtnVanish");
            if (isSelected[3]) FunctionFlowchart.SendFungusMessage("P4TeambtnShow"); else FunctionFlowchart.SendFungusMessage("P4TeambtnVanish");
        }
        else
        {
            FunctionFlowchart.SendFungusMessage("P1TeambtnVanish");
            FunctionFlowchart.SendFungusMessage("P2TeambtnVanish");
            FunctionFlowchart.SendFungusMessage("P3TeambtnVanish");
            FunctionFlowchart.SendFungusMessage("P4TeambtnVanish");
        }


        //if (switchScene)SwitchScene();

		int selectedPlayerAmount = 0;
		for (int i = 0; i<=3; i++) {
			if(isSelected[i])selectedPlayerAmount++;
			if(selectedPlayerAmount >= 2)canStart = true;
			else canStart = false;
		}

        if (dataCtrl.getTeamMode())
        {
            bool[] teamNum = new bool[3];
            teamNum[0] = false;
            teamNum[1] = false;
            teamNum[2] = false;
            for (int i = 0; i <= 3; i++)
            {
                if (isSelected[i])
                {
                    if (dataCtrl.getTeamNum(i) == 1) teamNum[0] = true;
                    else if (dataCtrl.getTeamNum(i) == 2) teamNum[1] = true;
                    else if (dataCtrl.getTeamNum(i) == 3) teamNum[2] = true;
                }
            }

            int differentTeamNum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (teamNum[i]) differentTeamNum++;
            }

            if (differentTeamNum >= 2 && selectedPlayerAmount >= 2) canStart = true;
            else canStart = false;

        }

            //設定隊伍
            if (dataCtrl.getTeamMode())
        {
            for (int i = 0; i < 4; i++)
            {
                dataCtrl.setTeamNum(i + 1,PlayerTeam[i]);
            }
        }


    }


    //=================自創==========================

    // 自創功能
    #region
    public void setSelectedCharacterIndex(int playerNUM, int character){
		SelectedCharacterIndex [playerNUM - 1] = character;
		isSelected [playerNUM - 1] = true;
	}

	public void GameStart(){
		for(int playerNUM=1; playerNUM<=4; playerNUM++){
			if(isSelected[playerNUM - 1]){
				dataCtrl.setIsInBattle(playerNUM,true);
				dataCtrl.setCharacterIndex(playerNUM,SelectedCharacterIndex[playerNUM-1]);
			}
			else{
				dataCtrl.setIsInBattle(playerNUM,false);
				dataCtrl.setCharacterIndex(playerNUM,SelectedCharacterIndex[playerNUM-1]);
			}
		}
		//transform.Find("BlackScreen").GetComponent<BlackFadeOutCtrl>().GameStart = true;
		//switchScene = true;
        FunctionFlowchart.SendFungusMessage("ChangeToMap");

    }

    /*
	void SwitchScene(){
		switchSceneCTime += Time.deltaTime;
		if(switchSceneCTime >= switchSceneTime)SceneManager.LoadScene("MapSelectScene",LoadSceneMode.Single);
	}
    */

    public void ChangeTeam(int playerNum)
    {
        PlayerTeam[playerNum - 1]++;
        if (PlayerTeam[playerNum - 1] > 3) PlayerTeam[playerNum - 1] = 1;
        FunctionFlowchart.SetIntegerVariable("P" + playerNum + "TeamNum", PlayerTeam[playerNum - 1]);
        FunctionFlowchart.SendFungusMessage("P" + playerNum + "ChangeTeam");
    }
    #endregion

    //=======================按鈕功能========================
    #region

    public void SettingbtnPressed()
    {
        SettingFlowchart.SendFungusMessage("Enter Setting");
    }


    #endregion

}
