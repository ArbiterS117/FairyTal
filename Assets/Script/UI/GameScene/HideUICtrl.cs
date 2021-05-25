using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideUICtrl : MonoBehaviour {

	GameCtrl gameCtrl;

	public float alpha = 0.5f;

	public float fadeInTime = 0.5f;
	float fadeInCTime = 0.0f;

	public bool isFadeIn = false;

	public bool[] isInZone = new bool[4];
	public int playerInZone = 0;

	public GameObject[] playerBarList;

	void Awake () {
		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl>();
	}

	void Start () {
		playerInZone = 0;
	}
	

	void Update () {
		if (playerInZone > 0) {
			if(!isFadeIn){
				fadeInCTime += Time.deltaTime;

				foreach(GameObject playerBar in playerBarList){
					if(playerBar != null){
						Image[] image;
						image = playerBar.GetComponentsInChildren<Image>();
						foreach(Image image1 in image){
							image1.color = new Color(1,1,1,1 - ( (1-alpha) * (fadeInCTime/fadeInTime)));
						}
					}
				}

				//leafSprite.color = new Color(1,1,1,1 - ( (1-alpha) * (fadeInCTime/fadeInTime)));

				if(fadeInCTime >= fadeInTime){
					isFadeIn = true;
					fadeInCTime = 0.0f;
				}
			}
			else{

				foreach(GameObject playerBar in playerBarList){
					if(playerBar != null){
						Image[] image;
						image = playerBar.GetComponentsInChildren<Image>();
						foreach(Image image1 in image){
							image1.color =  new Color(1,1,1,alpha);
						}
					}
				}
				//leafSprite.color = new Color(1,1,1,alpha);

			}
		}
		else{
			fadeInCTime = 0.0f;
			isFadeIn = false;

			foreach(GameObject playerBar in playerBarList){
				if(playerBar != null){
					Image[] image;
					image = playerBar.GetComponentsInChildren<Image>();
					foreach(Image image1 in image){
						image1.color =   new Color(1,1,1,1);
					}
				}
			}
			//leafSprite.color = new Color(1,1,1,1);

		}
		
		for (int i = 0; i<=3; i++) {
			if(isInZone[i] && gameCtrl.isDead[i]){
				isInZone[i] = false;
				playerInZone -= 1;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			isInZone[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = true;
			playerInZone += 1;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			isInZone[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = false;
			playerInZone -= 1;
		}
	}



}
