using UnityEngine;
using System.Collections;

public class StartBlockCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;
	//Animator animator;

	void Awake(){
		sceneCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
		//animator = GetComponent<Animator>();
	}

	void Start () {
	
	}
	

	void Update () {
	
	}

	//=========自創==================
	public void startOver(){
		sceneCtrl.startOver = true;
	}


}
