﻿using UnityEngine;
using System.Collections;

public class StageBalckFadeCtrl : MonoBehaviour {

	GameCtrl gameCtrl;

	public float fadeInTime = 1.0f;
	float fadeInCTime = 0.0f;
	bool fadeIn = false;

	public float fadeOutTime = 1.0f;
	float fadeOutCTime = 0.0f;

	void Awake(){

		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl>();

	}

	void Start () {
		fadeIn = true;
	}
	

	void Update () {

		//開始淡入
		if (fadeIn) {
			fadeInCTime += Time.deltaTime;
			this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1-fadeInCTime/fadeInTime);
			if(fadeInCTime >= fadeInTime){
				this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
				fadeIn = false;
			}
		}

		//開始淡出
		if (gameCtrl.canFadeOut) { // 用動畫代替
			fadeOutCTime += Time.deltaTime / gameCtrl.SlowModeTimeScale;
			this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,fadeOutCTime/fadeOutTime);
			if(fadeOutCTime >= fadeOutTime){
				this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
			}
		}



	}

}
