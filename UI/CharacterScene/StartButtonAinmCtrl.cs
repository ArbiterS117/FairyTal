using UnityEngine;
using System.Collections;

public class StartButtonAinmCtrl : MonoBehaviour {

	CharacterSelectDataCtrl  characterData;
	Animator animator;

	void Awake(){
		animator = GetComponent<Animator>();
		characterData = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectDataCtrl> ();
	}

	void Start () {
	
	}
	

	void Update () {
		if(characterData.canStart)animator.SetBool("CanStart", true);
		else animator.SetBool("CanStart",false);
	}

}
