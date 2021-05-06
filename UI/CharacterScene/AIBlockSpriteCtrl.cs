using UnityEngine;
using System.Collections;

public class AIBlockSpriteCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;
	CharacterSelectDataCtrl  characterData;

	//儲存玩家資訊
	int playerNUM = 0;

	//判斷參數
	bool fadeComplete = false;

	//控制參數
	public float startFadeTime  = 0.5f;
	float startFadeCTime = 0.0f;


	void Awake(){
		characterData = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
		sceneCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
		
	}

	void Start () {
		if(this.transform.parent.tag == ("AIPlayer1Block")){playerNUM = 1;}
		else if(this.transform.parent.tag == ("AIPlayer2Block")){playerNUM = 2;}
		else if(this.transform.parent.tag == ("AIPlayer3Block")){playerNUM = 3;}
		else if(this.transform.parent.tag == ("AIPlayer4Block")){playerNUM = 4;}
	}
	

	void Update () {

		//淡出前
		if (sceneCtrl.startOver == false) {
			this.transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

		}

		//開始淡出
		if (!fadeComplete && sceneCtrl.startOver == true) {
			startFadeCTime += Time.deltaTime;
			this.transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,startFadeCTime/startFadeTime);

			if(startFadeCTime >= startFadeTime){
				this.transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
				fadeComplete = true;
			}
		}
		//運作
		if(fadeComplete){
			if (!characterData.isInCtrl [playerNUM - 1] && !characterData.inChoosenAI [playerNUM - 1] && !characterData.isSelected[playerNUM - 1]){
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			}
			else {
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			}

		}
	}

}
