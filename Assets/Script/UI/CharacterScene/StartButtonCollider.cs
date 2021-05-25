using UnityEngine;
using System.Collections;

public class StartButtonCollider : MonoBehaviour {

	CharacterSelectDataCtrl  characterData;
	public int isOnButton = 0;
	
	void Awake(){
		characterData = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectDataCtrl> ();
	}

	void Update () {
		if(isOnButton == 0)this.transform.parent.localScale = new Vector3(1.0f,1.00f,1.0f);
		if(isOnButton >  0)this.transform.parent.localScale = new Vector3(1.0f,1.05f,1.0f);
		if(!characterData.canStart)isOnButton = 0;
	}

	public void OnTriggerEnter2D(Collider2D other){
		isOnButton += 1;
	}

	public void OnTriggerExit2D(Collider2D other){
		isOnButton -= 1;
	}


}
