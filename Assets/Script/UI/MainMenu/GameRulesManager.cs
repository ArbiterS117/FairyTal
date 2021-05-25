using UnityEngine;
using Fungus;
using TMPro;

public class GameRulesManager : MonoBehaviour {

    DataCtrl dataCtrl;

    public Flowchart RulesFlowchart = null;
    public Flowchart MenuFlowchart = null;

    public TextMeshProUGUI GameTimeText = null;
    public TextMeshProUGUI SurvivalTimeText = null;
    public TextMeshProUGUI StuckText = null;

    //初始值設定用
    public GameObject TimeText = null;
    public GameObject SurvivalText = null;
    public GameObject FreeForAllText = null;
    public GameObject TeamBattleText = null;
    public GameObject RegularText = null;
    public GameObject UnlimitText = null;
    public GameObject MPOffText = null;
    public GameObject PauseOnText = null;
    public GameObject PauseOffText = null;

    public GameObject GameTimeOBJ = null;
    public GameObject BigGameTimeText = null;
    public GameObject StuckOBJ = null;
    public GameObject BigStuckText = null;

    //遊戲變數
    public int GameTimeMin = 2;
    public int StuckNum = 3;

    public int SurvivalTimeMin = 5;

    public int MenuFungusState = 0;
    public bool CharacterSelectScene = false; // 腳色選擇時不用判斷目前MenuState

    //狀態
    public enum Btn
    {
        GameModebtn,
        GameTimebtn,
        Teambtn,
        SurvivalTimebtn,
        MPStylebtn,
        Pausebtn,
        Stuckbtn,
        Nobtn
    }
    public Btn CurrentbtnState;


    //輸入
    //string SubmitString = "Submit";
    //string CancelString = "Cancel";
    string HorizontalString = "Horizontal";
    //string VerticalString = "Vertical";

    private void Awake()
    {
        //FindTextObject();
    }

    void Start () {

        if(!CharacterSelectScene) dataCtrl = GetComponent<DataCtrl>();
        else dataCtrl = GameObject.Find("GameCtrl").GetComponent<DataCtrl>();

        CurrentbtnState = Btn.GameModebtn;
        GameTimeText.SetText(GameTimeMin.ToString() + ":00");
        SurvivalTimeText.SetText("UNLIMIT");

        GetDataFromDataCtrl(); // 預設值設置
        InitDataIcon();
    }
	
	void Update () {

        SetData();

        if(!CharacterSelectScene) MenuFungusState = MenuFlowchart.GetIntegerVariable("State");

        if (MenuFungusState == 5 || MenuFungusState == 6)
        {
            switch (CurrentbtnState)
            {
                case Btn.GameModebtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("GameModebtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("GameModebtnR");
                    break;

                case Btn.GameTimebtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("GameTimebtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("GameTimebtnR");
                    break;

                case Btn.Teambtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("TeambtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("TeambtnR");
                    break;

                case Btn.SurvivalTimebtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("SurvivalTimebtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("SurvivalTimebtnR");
                    break;

                case Btn.MPStylebtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("MPStylebtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("MPStylebtnR");
                    break;

                case Btn.Pausebtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("PausebtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("PausebtnR");
                    break;

                case Btn.Stuckbtn:
                    if (Input.GetAxis(HorizontalString) < -0.5f) RulesFlowchart.SendFungusMessage("StuckbtnL");
                    else if (Input.GetAxis(HorizontalString) > 0.5f) RulesFlowchart.SendFungusMessage("StuckbtnR");
                    break;

            }

        }

       
    }
    
    public void SetbtnState(int btn)
    {
        if (btn == 0) CurrentbtnState = Btn.GameModebtn;
        else if (btn == 1) CurrentbtnState = Btn.GameTimebtn;
        else if (btn == 2) CurrentbtnState = Btn.Teambtn;
        else if (btn == 3) CurrentbtnState = Btn.SurvivalTimebtn;
        else if (btn == 5) CurrentbtnState = Btn.MPStylebtn;
        else if (btn == 6) CurrentbtnState = Btn.Pausebtn;
        else if (btn == 7) CurrentbtnState = Btn.Stuckbtn;
        else CurrentbtnState = Btn.Nobtn;
    }

    public void SetStuckText(int i)
    {
        StuckNum += i;
        if (StuckNum > 3) StuckNum = 1;
        else if (StuckNum < 1) StuckNum = 3;
        StuckText.SetText(StuckNum.ToString());
    }
   
    public void SetGameTimeText(int i)
    {
        GameTimeMin += i;
        if (GameTimeMin > 5) GameTimeMin = 1;
        else if (GameTimeMin < 1) GameTimeMin = 5;
        GameTimeText.SetText(GameTimeMin.ToString() + ":00");
    }

    public void SetSurvivalTimeText(int i)
    {
        SurvivalTimeMin += i;
        if (SurvivalTimeMin > 7) SurvivalTimeMin = 0;
        else if (SurvivalTimeMin < 0) SurvivalTimeMin = 7;

        if (SurvivalTimeMin > 0)
        {
            SurvivalTimeText.SetText(SurvivalTimeMin.ToString() + ":00");
        }

        else
        {
            SurvivalTimeText.SetText("UNLIMIT");
        }
    }

    void GetDataFromDataCtrl()
    {
        RulesFlowchart.SetBooleanVariable("GameModeTime", !dataCtrl.getSurvivalMode());
        RulesFlowchart.SetBooleanVariable("GameModeSurvival", dataCtrl.getSurvivalMode());
        RulesFlowchart.SetBooleanVariable("TeamF4A", !dataCtrl.getTeamMode());
        RulesFlowchart.SetBooleanVariable("TeamTB", dataCtrl.getTeamMode());

        if(dataCtrl.getMPEnable() && !dataCtrl.getUnlimitMP()) RulesFlowchart.SetBooleanVariable("MPStyleR", dataCtrl.getMPEnable());
        else RulesFlowchart.SetBooleanVariable("MPStyleR", !dataCtrl.getMPEnable());

        RulesFlowchart.SetBooleanVariable("MPStyleU", dataCtrl.getUnlimitMP());
        RulesFlowchart.SetBooleanVariable("MPStyleN", !dataCtrl.getMPEnable());
        RulesFlowchart.SetBooleanVariable("PauseN", !dataCtrl.getCanPause());
        RulesFlowchart.SetBooleanVariable("PauseY", dataCtrl.getCanPause());

        GameTimeMin = (int)dataCtrl.getGameTime();
        SurvivalTimeMin = (int)dataCtrl.getSurvivalTime();
        StuckNum = dataCtrl.getStuckNum();
    }

    void InitDataIcon()
    {
        if (!dataCtrl.getSurvivalMode()) { TimeText.SetActive(true); SurvivalText.SetActive(false); GameTimeOBJ.SetActive(true); StuckOBJ.SetActive(false); BigGameTimeText.SetActive(true); BigStuckText.SetActive(false); }
        else { TimeText.SetActive(false); SurvivalText.SetActive(true); GameTimeOBJ.SetActive(false); StuckOBJ.SetActive(true); BigGameTimeText.SetActive(false); BigStuckText.SetActive(true); }

        if (dataCtrl.getTeamMode()) { FreeForAllText.SetActive(false); TeamBattleText.SetActive(true); }
        else { FreeForAllText.SetActive(true); TeamBattleText.SetActive(false); }

        if(dataCtrl.getMPEnable() && !dataCtrl.getUnlimitMP()) { RegularText.SetActive(true); UnlimitText.SetActive(false); MPOffText.SetActive(false); }
        if(dataCtrl.getUnlimitMP()) { RegularText.SetActive(false); UnlimitText.SetActive(true); MPOffText.SetActive(false); }
        if(!dataCtrl.getMPEnable()) { RegularText.SetActive(false); UnlimitText.SetActive(false); MPOffText.SetActive(true); }

        if (dataCtrl.getCanPause()) { PauseOnText.SetActive(true); PauseOffText.SetActive(false); }
        else { PauseOnText.SetActive(false); PauseOffText.SetActive(true); }

        GameTimeText.SetText(GameTimeMin.ToString() + ":00");
        if (SurvivalTimeMin > 0)
        {
            SurvivalTimeText.SetText(SurvivalTimeMin.ToString() + ":00");
        }

        else
        {
            SurvivalTimeText.SetText("UNLIMIT");
        }
        StuckText.SetText(StuckNum.ToString());
    }

    /*  一開始SetActive為off 抓不到
    void FindTextObject()
    {
        TimeText = GameObject.Find("Time Text");
        SurvivalText = GameObject.Find("Survival Text");
        FreeForAllText = GameObject.Find("Free For All Text");
        TeamBattleText = GameObject.Find("Team Battle Text");
        RegularText = GameObject.Find("Regular Text");
        UnlimitText = GameObject.Find("Unlimit Text");
        MPOffText = GameObject.Find("MP Off Text");
        PauseOffText = GameObject.Find("Pause Off Text");
        PauseOnText = GameObject.Find("Pause On Text");
    }
    */

    void SetData()
    {
        dataCtrl.setGameTime((float)GameTimeMin);
        dataCtrl.setSurvivalTime((float)SurvivalTimeMin);
        dataCtrl.setSurvivalMode(RulesFlowchart.GetBooleanVariable("GameModeSurvival"));
        dataCtrl.setStuckNum(StuckNum);
        dataCtrl.setTeamMode(RulesFlowchart.GetBooleanVariable("TeamTB"));
        dataCtrl.setMPEnable(!RulesFlowchart.GetBooleanVariable("MPStyleN"));
        dataCtrl.setUnlimitMP(RulesFlowchart.GetBooleanVariable("MPStyleU"));
        dataCtrl.setCanPause(RulesFlowchart.GetBooleanVariable("PauseY"));
    }

    //選擇腳色Scene專用
    public void SetMenuFungusState(int i) // 在Setting時設定成5 or 6
    {
        MenuFungusState = i;
    }

}
