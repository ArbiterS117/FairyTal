using UnityEngine;
using System.Collections;

public class AIMarkCtrl : MonoBehaviour {

	CharacterSelectDataCtrl  characterData;
	
	//儲存玩家資訊
	int playerNUM = 0;
	
	
	void Awake(){
		characterData = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectDataCtrl>();

	}
	
	void Start () {
		if(this.transform.parent.tag == ("AIPlayer1Block")){playerNUM = 1;}
		else if(this.transform.parent.tag == ("AIPlayer2Block")){playerNUM = 2;}
		else if(this.transform.parent.tag == ("AIPlayer3Block")){playerNUM = 3;}
		else if(this.transform.parent.tag == ("AIPlayer4Block")){playerNUM = 4;}
	}
	
	
	void Update () {
		
		if (!characterData.isInCtrl [playerNUM - 1] && (characterData.isSelected[playerNUM - 1] || characterData.inChoosenAI [playerNUM - 1])){
			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		}
		else {
			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}
		
	}
	
}
