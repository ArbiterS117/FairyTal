using UnityEngine;
using System.Collections;

public class lightCtrl : MonoBehaviour {

	SpriteRenderer sprite;

	public float fadeInTime = 1.0f;
	float fadeInCTime = 0.0f;
	bool fadeIn = false;
	
	public float fadeOutTime = 1.0f;
	float fadeOutCTime = 0.0f;
	bool fadeOut = false;

	void Awake(){
		sprite = GetComponent<SpriteRenderer>();
	}

	void Start () {
		//fadeIn = true;
		fadeInTime = Random.Range (fadeInTime, fadeInTime + 2.0f);
		fadeOutTime = Random.Range (fadeOutTime, fadeOutTime + 2.0f);
	}
	

	void Update () {
	
		if (fadeIn) {
			fadeInCTime += Time.deltaTime;
			sprite.color = new Color(1,1,1,(1-fadeInCTime/fadeInTime) * 100/256);
			if(fadeInCTime >= fadeInTime){
				sprite.color = new Color(1,1,1,0);
				fadeIn = false;
				fadeOut = true;
				fadeInCTime = 0.0f;
				fadeOutTime = Random.Range (fadeOutTime, fadeOutTime + 2.0f);
			}
		}

		if (fadeOut) {
			fadeOutCTime += Time.deltaTime;
			sprite.color = new Color(1,1,1,(fadeOutCTime/fadeOutTime) * 100/256);
			if(fadeOutCTime >= fadeOutTime){
				//sprite.color = new Color(1,1,1,(100/256));
				fadeOut = false;
				fadeIn = true;
				fadeOutCTime = 0.0f;
				fadeInTime = Random.Range (fadeInTime, fadeInTime + 2.0f);
			}
		}

	}

}
