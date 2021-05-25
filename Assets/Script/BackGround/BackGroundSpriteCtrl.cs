using UnityEngine;
using System.Collections;

public class BackGroundSpriteCtrl : MonoBehaviour {

	GameCtrl gameCtrl;
	
	bool isBlack = false;
	float time = 0.0f;
	
	void Awake() {
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
	}

	void Update () {

        float _deltaTime = Time.deltaTime;

		//控制大絕使畫面變暗的效果
		if (gameCtrl.isUsingULT == 0 && !isBlack) {
			GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 0);
			time = 0;
		}

		else if (gameCtrl.isUsingULT > 0 && !isBlack) {
			time += _deltaTime;
			GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 0.5f * time*2);
			if (time >= 0.5f) {
				isBlack = true;
				time = 0;
			}
		}

		else if (gameCtrl.isUsingULT > 0 && isBlack) {
			GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 0.5f);
			time = 0;
		}

		else if (gameCtrl.isUsingULT == 0 && isBlack) {
			time += _deltaTime;
			GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 0.5f - 0.5f * time*2);
			if (time >= 0.5f) {
				isBlack = false;
				time = 0;
			}
		}


	}
	
}
