using UnityEngine;
using System.Collections;

public class SpriteCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;

	SpriteRenderer BotSprite;
	SpriteRenderer CharacterSprite;
	SpriteRenderer BlockSprite;
	SpriteRenderer CharSprite;

	//控制參數
	public bool SelectArea;

	public float startFadeTime  = 0.5f;
	float startFadeCTime = 0.0f;
	
	//判斷參數
	bool fadeComplete = false;
	
	
	void Awake(){
		sceneCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
		if (SelectArea == false) {
			BotSprite = this.transform.Find ("BotSprite").GetComponent<SpriteRenderer> ();
			CharacterSprite = this.transform.Find ("CharacterSprite").GetComponent<SpriteRenderer> ();
			BlockSprite = this.transform.Find ("BlockSprite").GetComponent<SpriteRenderer> ();
			CharSprite = this.transform.Find ("CharSprite").GetComponent<SpriteRenderer> ();
		}
	}
	
	void Start () {
		if (SelectArea == false) {
			BotSprite.color = new Color (1, 1, 1, 0);
			CharSprite.color = new Color (1, 1, 1, 0);
			CharacterSprite.color = new Color (1, 1, 1, 0);
			BlockSprite.color = new Color (1, 1, 1, 0);
		}
		//selectArea
		if(this.transform.Find("Sprite") != null)this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

	}
	
	
	void Update () {
		
		//圖片顯示、是否可控制
		if (!fadeComplete && sceneCtrl.startOver == true) {
			startFadeCTime += Time.deltaTime;
			if (SelectArea == false) {
				BotSprite.color = new Color(1,1,1,startFadeCTime/startFadeTime);
				CharSprite.color = new Color(1,1,1,startFadeCTime/startFadeTime);
				CharacterSprite.color = new Color(1,1,1,startFadeCTime/startFadeTime);
				BlockSprite.color = new Color(1,1,1,startFadeCTime/startFadeTime);
			}
			if(this.transform.Find("Sprite") != null)this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(1,1,1,startFadeCTime/startFadeTime);
			if(startFadeCTime >= startFadeTime){
				if (SelectArea == false) {
					BotSprite.color = new Color(1,1,1,1);
					CharSprite.color = new Color(1,1,1,1);
					CharacterSprite.color = new Color(1,1,1,1);
					BlockSprite.color = new Color(1,1,1,1);
				}
				if(this.transform.Find("Sprite") != null)this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
				fadeComplete = true;
			}
		}
	
	}

	//===========自創===================


}
