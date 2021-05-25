using UnityEngine;
using System.Collections;
using TMPro;

public class healbar : MonoBehaviour {

    DataCtrl dataCtrl;
	GameCtrl gameCtrl;
	Animator animator;

	public XXXCtrl playerCtrl;

    public GameObject PointPlus;

    TextMeshProUGUI livesTMP;
    string livesText = "Lives : ";

    float prePlayerPoint = 0;

	public float maxhealth = 100.0f;
	public float curhealth = 50.0f;
	public float maxMP     = 50.0f;
	public float curMP     = 50.0f;
	public float healthBarLen;
	public float MPBarLen;
	public int PlayerNUM = 0;

	public bool isTargeted = false;


	void Awake(){
        dataCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<DataCtrl>();
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		animator = this.transform.GetComponent<Animator>();
        livesTMP = transform.Find("Lives").GetComponent<TextMeshProUGUI>();

        prePlayerPoint = gameCtrl.playerPoint[PlayerNUM - 1];
    }

	void Start () {
		if (!gameCtrl.isInBattle [PlayerNUM-1]) {
			Destroy (this.gameObject);
            return;
		}
		else{
			     if(gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 0 ||
			        gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 1)animator.SetBool("SelectRED",true);

			else if(gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 2 ||
			        gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 3)animator.SetBool("SelectALICE",true);

			else if(gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 4 ||
			        gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 5)animator.SetBool("SelectMOMOTARO",true);

			else if(gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 6 ||
			        gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 7)animator.SetBool("SelectSNOWWHITE",true);

			else if(gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 8 ||
			        gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 9)animator.SetBool("SelectRAPUNZEL",true);

            else if (gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 10 ||
               gameCtrl.SelectedCharacterIndex[PlayerNUM - 1] == 11) animator.SetBool("SelectALADDIN", true);

        }

        //隊伍
        if (gameCtrl.TeamMode)
        {
            if (gameCtrl.DebugCharacterData)
            {
                if (dataCtrl.getTeamNum(PlayerNUM - 1) == 1) transform.Find("Team").Find("REDTeam").gameObject.SetActive(true);
                else if (dataCtrl.getTeamNum(PlayerNUM - 1) == 2) transform.Find("Team").Find("BlueTeam").gameObject.SetActive(true);
                else if (dataCtrl.getTeamNum(PlayerNUM - 1) == 3) transform.Find("Team").Find("GreenTeam").gameObject.SetActive(true);
            }
            else   //DEBUG模式開啟
            {
                if (gameCtrl.DebugTeam[PlayerNUM - 1] == 1) transform.Find("Team").Find("REDTeam").gameObject.SetActive(true);
                else if (gameCtrl.DebugTeam[PlayerNUM - 1] == 2) transform.Find("Team").Find("BlueTeam").gameObject.SetActive(true);
                else if (gameCtrl.DebugTeam[PlayerNUM - 1] == 3) transform.Find("Team").Find("GreenTeam").gameObject.SetActive(true);
            }
        }

	}

	//========================================Update=========================================
	void Update()
	{
        if (!gameCtrl.CountDownComplete) return;

        PlusPointEffect();
        ShowLivesNum();
        //===============抓取玩家資訊=============
        if (gameCtrl.isDead [PlayerNUM - 1]) {
			curhealth = 0;
			curMP = 0;
			isTargeted = false;
		}
		else{
			if(!isTargeted){
				playerCtrl = gameCtrl.player[PlayerNUM - 1].GetComponent<XXXCtrl>();

				maxhealth = playerCtrl.HP;
				curhealth = maxhealth; 
				healthBarLen = (curhealth / maxhealth);

				maxMP = playerCtrl.MP;
				curMP = maxMP;
				MPBarLen = (curMP / maxMP);

				isTargeted = true;
			}
		}

		if (playerCtrl != null) {
			curhealth = playerCtrl.HP;
			curMP = playerCtrl.MP;
		}
		//==============計算 顯示=====================
		AddjustCurrentHealth(0);
		AddjustCurrentMP(0);
		GetComponentInChildren<healBarHP> ().healthBarLen = healthBarLen;
		GetComponentInChildren<healBarDamagedHP> ().healthBarLen = healthBarLen;
		GetComponentInChildren<healBarMP> ().healthBarLen = MPBarLen;

		GetComponentInChildren<healBarHP> ().playerHP = curhealth;
	}

	//========================================自訂=========================================

	//======計算血量比例========
	public void AddjustCurrentHealth(float adj){
		curhealth += adj;
		if (curhealth < 0)
			curhealth = 0;
		if (curhealth > maxhealth)
			curhealth = maxhealth;
		if (maxhealth < 1)
			maxhealth = 1;
		healthBarLen = (curhealth / maxhealth);
	}

	//======計算MP比例========
	public void AddjustCurrentMP(float adj){
		curMP += adj;
		if (curMP < 0)
			curMP = 0;
		if (curMP > maxMP)
			curMP = maxMP;
		if (maxMP < 1)
			maxMP = 1;
		MPBarLen = (curMP / maxMP);
	}

    //=======得分顯示==========
    public void PlusPointEffect()
    {

        
        if(prePlayerPoint < gameCtrl.playerPoint[PlayerNUM - 1])
        {
            GameObject PP =  Instantiate(PointPlus, this.transform.localPosition, Quaternion.identity) as GameObject;
            PP.transform.SetParent(GameObject.Find("UI").transform.Find("Canvas"));
            PP.transform.localPosition = new Vector3(this.transform.localPosition.x + 124.0f, this.transform.localPosition.y, this.transform.localPosition.z);
            PP.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        prePlayerPoint = gameCtrl.playerPoint[PlayerNUM - 1];
    }

    //=======生命顯示===========
    public void ShowLivesNum()
    {
        if (gameCtrl.SurvivalMode)
        {
            livesTMP.enabled = true;
            livesTMP.SetText(livesText + gameCtrl.playerStuck[PlayerNUM - 1].ToString());
        }
        else
        {
            livesTMP.enabled = false;
        }
    }



}

