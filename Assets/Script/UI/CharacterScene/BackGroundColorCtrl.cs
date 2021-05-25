using UnityEngine;
using System.Collections;

public class BackGroundColorCtrl : MonoBehaviour {

	CharacterSelectDataCtrl  characterData;
	Animator animator;

	Transform background1;
	Transform background2;

	public float fadeTime = 1.0f;
	float fadeCTime = 0.0f;
	bool isFade = false;
	float randomR;
	float randomG;
	float randomB;

	float TempR;
	float TempG;
	float TempB;
	
	void Awake(){

		characterData = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
		background1 = transform.Find ("background1").transform;
		background2 = transform.Find ("background2").transform;
	}
	
	void Start () {
		TempR = 1.0f;
		TempG = 1.0f;
		TempB = 1.0f;
	}
	
	
	void Update () {
		if (characterData.canStart) {

			if(isFade == false){
				randomR = Random.Range(0.1f,0.95f);
				randomG = Random.Range(0.1f,0.95f);
				randomB = Random.Range(0.1f,0.95f);
				isFade = true;
			}
			else{
				fadeCTime += Time.deltaTime;
				background1.GetComponent<SpriteRenderer>().color = new Color(TempR-(TempR - randomR)*fadeCTime/fadeTime,TempG-(TempG - randomG)*fadeCTime/fadeTime,TempB-(TempB - randomB)*fadeCTime/fadeTime);
				background2.GetComponent<SpriteRenderer>().color = new Color(TempR-(TempR - randomR)*fadeCTime/fadeTime,TempG-(TempG - randomG)*fadeCTime/fadeTime,TempB-(TempB - randomB)*fadeCTime/fadeTime);
				if(fadeCTime >= fadeTime){
					background1.GetComponent<SpriteRenderer>().color = new Color(randomR,randomG,randomB);
					background2.GetComponent<SpriteRenderer>().color = new Color(randomR,randomG,randomB);
					isFade = false;
					TempR = randomR;
					TempG = randomG;
					TempB = randomB;
					fadeCTime = 0.0f;
				}
			}

		}
		else{
			background1.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
			background2.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
			isFade = false;
			TempR = 1.0f;
			TempG = 1.0f;
			TempB = 1.0f;
			fadeCTime = 0.0f;
		}
		
	}
	
}

