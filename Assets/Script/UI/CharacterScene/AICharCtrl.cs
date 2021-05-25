using UnityEngine;
using System.Collections;

public class AICharCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;
	CharacterSelectDataCtrl  characterData;
	
	//儲存玩家資訊
	int playerNUM = 0;

	public float blinkTime = 0.5f;
	float blinkCTime = 0.0f;
	
	
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

		if (sceneCtrl.startOver == true) {

			//玩家是否操作中
			if (!characterData.isInCtrl [playerNUM - 1] && !characterData.inChoosenAI [playerNUM - 1] && !characterData.isSelected [playerNUM - 1]) {
				blinkCTime += Time.deltaTime;
				if (blinkCTime < blinkTime)
					GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);

				if (blinkCTime > blinkTime)
					GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
	
				if (blinkCTime > 2 * blinkTime)
					blinkCTime = 0.0f;
			} else {
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			}

		} 
		else {
			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}


	}
	
}
