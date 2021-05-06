using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreCtrl : MonoBehaviour {
	
	VictorySceneDataCtrl gameCtrl;
	
	public int playerNum = 0;
	public int playerCharacterIndex = 0;

	public Text PointText;

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<VictorySceneDataCtrl>();
	}

	void Start () {
		//取得資料
		playerCharacterIndex = gameCtrl.playerCharacterIndex [playerNum - 1];


		if(!gameCtrl.isInBattle[playerNum - 1])Destroy(this.gameObject);

		//關閉預設顯示
		transform.Find("1st").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("1stMark").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("2nd").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("2ndMark").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("3rd").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("3rdMark").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("4th").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("4thMark").GetComponent<SpriteRenderer>().enabled = false;
		transform.Find("Mask").transform.Find("RED").gameObject.SetActive(false);
		transform.Find("Mask").transform.Find("ALICE").gameObject.SetActive(false);
		transform.Find("Mask").transform.Find("MOMOTARO").gameObject.SetActive(false);
		transform.Find("Mask").transform.Find("SNOWWHITE").gameObject.SetActive(false);
		transform.Find("Mask").transform.Find("RAPUNZEL").gameObject.SetActive(false);
        transform.Find("Mask").transform.Find("ALADDIN").gameObject.SetActive(false); 
        transform.Find("TeamMark").transform.Find("Red Team").gameObject.SetActive(false);
        transform.Find("TeamMark").transform.Find("Blue Team").gameObject.SetActive(false);
        transform.Find("TeamMark").transform.Find("Green Team").gameObject.SetActive(false);
        //顯示外框
        if (gameCtrl.PlayerRank [playerNum - 1] == 1) {
			transform.Find("1st").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("1stMark").GetComponent<SpriteRenderer>().enabled = true;
		}
		else if (gameCtrl.PlayerRank [playerNum - 1] == 2) {
			transform.Find("2nd").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("2ndMark").GetComponent<SpriteRenderer>().enabled = true;
		}
		else if (gameCtrl.PlayerRank [playerNum - 1] == 3) {
			transform.Find("3rd").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("3rdMark").GetComponent<SpriteRenderer>().enabled = true;
		}
		else if (gameCtrl.PlayerRank [playerNum - 1] == 4) {
			transform.Find("4th").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("4thMark").GetComponent<SpriteRenderer>().enabled = true;
		}

		//顯示腳色
		if (playerCharacterIndex == 0 || playerCharacterIndex == 1) {
			transform.Find("Mask").transform.Find("RED").gameObject.SetActive(true);
		}
		else if (playerCharacterIndex == 2 || playerCharacterIndex == 3) {
			transform.Find("Mask").transform.Find("ALICE").gameObject.SetActive(true);
		}
		else if (playerCharacterIndex == 4 || playerCharacterIndex == 5) {
			transform.Find("Mask").transform.Find("MOMOTARO").gameObject.SetActive(true);
		}
		else if (playerCharacterIndex == 6 || playerCharacterIndex == 7) {
			transform.Find("Mask").transform.Find("SNOWWHITE").gameObject.SetActive(true);
		}
		else if (playerCharacterIndex == 8 || playerCharacterIndex == 9) {
			transform.Find("Mask").transform.Find("RAPUNZEL").gameObject.SetActive(true);
		}
        else if (playerCharacterIndex == 10 || playerCharacterIndex == 11)
        {
            transform.Find("Mask").transform.Find("ALADDIN").gameObject.SetActive(true);
        }


        //顯示文字

        PointText.text = "Point: " + gameCtrl.playerPoint [playerNum - 1].ToString();

		//顯示玩家或CP標記

		if (playerCharacterIndex == 1 || playerCharacterIndex == 3 || playerCharacterIndex == 5 ||
            playerCharacterIndex == 7 || playerCharacterIndex == 9 || playerCharacterIndex == 11) {
			transform.Find("CPMark").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("PlayerMark").GetComponent<SpriteRenderer>().enabled = false;
		}

		else{
			transform.Find("CPMark").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("PlayerMark").GetComponent<SpriteRenderer>().enabled = true;
		}

        //顯示隊伍
        if (gameCtrl.dataCtrl.getTeamMode())
        {
            int teamNum = gameCtrl.dataCtrl.getTeamNum(playerNum - 1);
            if(teamNum == 1) transform.Find("TeamMark").transform.Find("Red Team").gameObject.SetActive(true);
            else if (teamNum == 2) transform.Find("TeamMark").transform.Find("Blue Team").gameObject.SetActive(true);
            else if (teamNum == 3) transform.Find("TeamMark").transform.Find("Green Team").gameObject.SetActive(true);
        }

	}
	

	
}
