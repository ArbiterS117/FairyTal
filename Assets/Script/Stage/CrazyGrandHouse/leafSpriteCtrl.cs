using UnityEngine;
using System.Collections;

public class leafSpriteCtrl : MonoBehaviour {

	GameCtrl gameCtrl;
	SpriteRenderer leafSprite;

	public float alpha = 0.5f;

	public int playerInLeaf = 0;

	public float fadeInTime = 0.5f;
	       float fadeInCTime = 0.0f;

	public bool isFadeIn = false;

	public bool[] isInTree = new bool[4];

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl>();
		leafSprite = GetComponentInParent<SpriteRenderer>();
	}

	void Start () {
		playerInLeaf = 0;
	}

	void Update(){

		if (playerInLeaf > 0) {
			if(!isFadeIn){
				fadeInCTime += Time.deltaTime;
				leafSprite.color = new Color(1,1,1,1 - ( (1-alpha) * (fadeInCTime/fadeInTime)));
				if(fadeInCTime >= fadeInTime){
					isFadeIn = true;
					fadeInCTime = 0.0f;
				}
			}
			else{
				leafSprite.color = new Color(1,1,1,alpha);
			}
		}
		else{
			fadeInCTime = 0.0f;
			isFadeIn = false;
			leafSprite.color = new Color(1,1,1,1);
		}

		for (int i = 0; i<=3; i++) {
			if(isInTree[i] && gameCtrl.isDead[i]){
				isInTree[i] = false;
				playerInLeaf -= 1;
			}
		}

	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerECollider") {
			isInTree[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = true;
			playerInLeaf += 1;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "PlayerECollider") {
			isInTree[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = false;
			playerInLeaf -= 1;
		}
	}



}
