using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Fungus;
using TMPro;

public class VictorySceneDataCtrl : MonoBehaviour {

	public DataCtrl dataCtrl;

	AudioSource audioCtrl;

    public Flowchart flowchart;

    public TextMeshProUGUI winnerNameText;

	//資料
	public bool[] isInBattle = new bool[4];
	public float[] playerPoint = new float[4];
	public int[] playerCharacterIndex = new int[4];
	float[] SortPlayerPoint = new float[4];
    float[] SortPlayerLifeTime = new float[4];

	public  int[] PlayerRank = new int[4];

	//控制參數
	public bool canExit;
	public float ExitTime = 3.0f;
	float ExitCTime = 0.0f;

	//音效參數
	public AudioClip ENDSE;
	public float ENDSEPitch;

    public int[] teamPoints = new int[3];
    public int[] OriteamPoint = new int[3];
    public int[] teamRank = new int[3];

    //物件參照
    public GameObject END;

    void Awake(){
		dataCtrl = GetComponent<DataCtrl> ();
		audioCtrl = GetComponent<AudioSource>();
	}

	void Start () {
		for (int i=0; i<=3; i++) {
			isInBattle[i] = dataCtrl.returnIsBattle(i);
			playerPoint[i] = dataCtrl.returnPlayerPoint(i);
			playerCharacterIndex[i] = dataCtrl.returnCharacterIndex(i);
		}

        //===========排序=============================

        if(!dataCtrl.getSurvivalMode() && !dataCtrl.getTeamMode())SortingTime();

        if (dataCtrl.getSurvivalMode() && !dataCtrl.getTeamMode()) SortingSurvival();

        if (!dataCtrl.getSurvivalMode() && dataCtrl.getTeamMode()) SortingTeamTime();

        if (dataCtrl.getSurvivalMode() && dataCtrl.getTeamMode()) SortingTeamSurvival();
        //==============================================


    }
	

	void Update () {

		if (!canExit) {
			if (Input.GetKeyDown (KeyCode.Return) || Input.GetButtonDown ("Start")) {  
				canExit = true;
				audioCtrl.pitch = ENDSEPitch ;
				audioCtrl.PlayOneShot(ENDSE);
			}  
		}

		if (canExit) {
            END.GetComponent<Animator>().SetTrigger("END");
			ExitCTime += Time.deltaTime;
			if(ExitCTime >= ExitTime){
				SceneManager.LoadScene("CharacterSelectScene",LoadSceneMode.Single);  

            }
		}

	}

    //==============自訂==========================

    //排序 時間
    void SortingTime()
    {
        for (int i = 0; i < 4; i++)
        {

            SortPlayerPoint[i] = playerPoint[i];

        }

        Array.Sort(SortPlayerPoint);

        int rank = 1;

        //取第一名
        for (int a = 0; a < 4; a++)
        {
            if (isInBattle[a])
            {
                if (playerPoint[a] == SortPlayerPoint[3])
                {
                    PlayerRank[a] = rank;
                }
            }
            else
            {
                PlayerRank[a] = -1;
            }
        }

        for (int f = 0; f <= 3; f++)
        {
            if (PlayerRank[f] == 1)
            {
                rank++;
                break;
            }
        }

        //取其他名次
        for (int i = 2; i >= 0; i--)
        {
            if (SortPlayerPoint[i] != SortPlayerPoint[i + 1])
            {
                for (int j = 0; j < 4; j++)
                {
                    if (isInBattle[j])
                    {
                        if (playerPoint[j] == SortPlayerPoint[i])
                        {
                            PlayerRank[j] = rank;
                        }

                    }
                }
                for (int f = 0; f <= 3; f++)
                {
                    if (PlayerRank[f] == rank)
                    {
                        rank++;
                        break;
                    }
                }

            }

        }

    }

    //排序 生命
    void SortingSurvival()
    {
        for(int i = 0; i < 4; i++)
        {
            SortPlayerLifeTime[i] = dataCtrl.getPlayerLifeTime(i + 1);
        }
        Array.Sort(SortPlayerLifeTime);
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 0; j < 4; j++)
            {
                if (dataCtrl.getPlayerLifeTime(j + 1) == SortPlayerLifeTime[i])
                {
                    if(PlayerRank[j] == 0)  PlayerRank[j] = i * -1 + 4;
                }
            }
        }
    }

    //排序 生命隊伍
    void SortingTeamSurvival()
    {
        int Rank1PlayerNum = 0;
        float[] OriPlayerLifeTime = new float[4];

        for (int i = 0; i < 4; i++)
        {
            SortPlayerLifeTime[i] = dataCtrl.getPlayerLifeTime(i + 1);
            OriPlayerLifeTime[i] = dataCtrl.getPlayerLifeTime(i + 1);
        }
        //OriPlayerLifeTime = SortPlayerLifeTime; 會複製參照 我猜
        Array.Sort(SortPlayerLifeTime);
       

        for (int j = 0; j < 4; j++)//找第一名
        {
            if (dataCtrl.getPlayerLifeTime(j + 1) == SortPlayerLifeTime[3])
            {
                PlayerRank[j] = 1;
                Rank1PlayerNum = j; // 紀錄第一名玩家號碼
            }
        }

        for (int i = 0; i < 4; i++)//第一名同隊
        {
            if (dataCtrl.getTeamNum(i) == dataCtrl.getTeamNum(Rank1PlayerNum) && PlayerRank[i] == 0) PlayerRank[i] = 1;
        }

        for (int i = 0; i < 4; i++)//第一名組的資料通通刪掉
        {
           if (PlayerRank[i] == 1) OriPlayerLifeTime[i] = 0;
        }

        for (int i = 0; i < 4; i++)
        {
            SortPlayerLifeTime[i] = OriPlayerLifeTime[i];
        }
        //SortPlayerLifeTime = OriPlayerLifeTime;
        Array.Sort(SortPlayerLifeTime);

        /////////////////////////////
        
        for (int j = 0; j < 4; j++)//找第二名
        {
            if (dataCtrl.getPlayerLifeTime(j + 1) == SortPlayerLifeTime[3] && PlayerRank[j] == 0)
            {
                PlayerRank[j] = 2;
                Rank1PlayerNum = j; // 紀錄第二名玩家號碼
            }
        }

        for (int i = 0; i < 4; i++)//第二名同隊
        {
            if (dataCtrl.getTeamNum(i) == dataCtrl.getTeamNum(Rank1PlayerNum) && PlayerRank[i] == 0) PlayerRank[i] = 2;
        }

        for (int i = 0; i < 4; i++)//第二名組的資料通通刪掉
        {
            if (PlayerRank[i] == 2) OriPlayerLifeTime[i] = 0;
        }

        /////////////////////////////
        //找第三名
        for (int i = 0; i < 4; i++)//第二名組的資料通通刪掉
        {
            if (PlayerRank[i] == 0) PlayerRank[i] = 3;
        }
        


    }

    //排序 時間隊伍
    void SortingTeamTime()
    {
        //int[] teamPoints = new int[3];
        //int[] OriteamPoint = new int[3];
        //int[] teamRank = new int[3];
        for(int i = 0; i < 4; i++)
        {
            teamPoints[dataCtrl.getTeamNum(i) - 1] += dataCtrl.returnPlayerPoint(i);
            OriteamPoint[dataCtrl.getTeamNum(i) - 1] += dataCtrl.returnPlayerPoint(i);
        }
        Array.Sort(teamPoints);

        if(teamPoints[2] == teamPoints[1])
        {
            teamPoints[1] = teamPoints[0];
            teamPoints[0] -= 1; // 減到最低位
        }

        //排隊伍名次
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 2; j >= 0; j--) {
                if (teamPoints[i] == OriteamPoint[j])
                {
                    if (teamRank[j] == 0)
                    {
                        teamRank[j] = -1 * i + 3;
                    }
                }
            }

        }

        //玩家名次設定

        for (int i = 0; i < 4; i++)
        {
            PlayerRank[i] = teamRank[dataCtrl.getTeamNum(i) - 1];
        }

    }

}
