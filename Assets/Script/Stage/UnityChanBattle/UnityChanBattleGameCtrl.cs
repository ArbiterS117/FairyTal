using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UnityChanBattleGameCtrl : MonoBehaviour {

	GameCtrl gameCtrl;
	public UnityChanCtrl unityChan;

	public float DeadToExitTime = 3.0f;
	float DeadToExitCTime = 0.0f;

	float TimeUpToFadeOutCTime = 0.0f;
	bool timeSlow = false;

	//控制變數
	public float playerNUMBossHPBuff = 20.0f;

	void Awake(){
		gameCtrl = GetComponent<GameCtrl>();
		unityChan = GameObject.FindGameObjectWithTag("NPC").GetComponent<UnityChanCtrl>();
	}

	void Start () {
		int playernum = 0;
		int playerStuck = 0;
		for (int i = 0; i<=3; i++) {
			if(gameCtrl.isInBattle[i])playernum++;
		}
		     if(playernum == 1)playerStuck = 7;
		else if(playernum == 2)playerStuck = 6;
		else if(playernum == 3)playerStuck = 5;
		else if(playernum == 4)playerStuck = 4;

		for (int i = 0; i<=3; i++) {
			gameCtrl.playerStuck[i] = playerStuck;
		}

		unityChan.HP += (playernum * playernum * playerNUMBossHPBuff);

	}
	

	void Update () {

		for(int i = 0; i < 4; i ++){
			if(gameCtrl.isInBattle[i] && !gameCtrl.isDead[i]){
				gameCtrl.player[i].GetComponent<XXXCtrl>().tag = "TeamA";
			}
		}

		//==========結束遊戲
		if (unityChan.isDead) {
			DeadToExitCTime += Time.deltaTime;
			if(DeadToExitCTime >= DeadToExitTime){
				gameCtrl.GameOver = true;
			}
		}

		else if(playerStuckOver()){
			if(!timeSlow){
				Time.timeScale = gameCtrl.SlowModeTimeScale; 
				timeSlow = true;
			}
			gameCtrl.TimeUP.SetActive (true);

			TimeUpToFadeOutCTime += Time.deltaTime / gameCtrl.SlowModeTimeScale;
			if (TimeUpToFadeOutCTime >= gameCtrl.TimeUpToFadeOutTime) {
				gameCtrl.canFadeOut = true;
			}
			if (TimeUpToFadeOutCTime >= gameCtrl.TimeUpToFadeOutTime + gameCtrl.FadeOutToExitTime) {
				Time.timeScale = 1;
				SceneManager.LoadScene("StartScene",LoadSceneMode.Single);
			}
		}



	}

	//==============自創=========================

	bool playerStuckOver(){

		if (
		((gameCtrl.isInBattle [0] && gameCtrl.playerStuck [0] == 0) || !gameCtrl.isInBattle [0]) &&
			((gameCtrl.isInBattle [1] && gameCtrl.playerStuck [1] == 0) || !gameCtrl.isInBattle [1]) &&
			((gameCtrl.isInBattle [2] && gameCtrl.playerStuck [2] == 0) || !gameCtrl.isInBattle [2]) &&
			((gameCtrl.isInBattle [3] && gameCtrl.playerStuck [3] == 0) || !gameCtrl.isInBattle [3]) 
		  )
			return true;
		else
			return false;
	}
		

}
