using UnityEngine;
using System.Collections;

public class BackGroundDimondCtrl : MonoBehaviour {

	CharacterSelectDataCtrl  characterData;
	Animator animator;

	public float RotationSpeed = 1.2f;
	
	void Awake(){
		animator = GetComponent<Animator>();
		characterData = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectDataCtrl> ();
	}
	
	void Start () {
		
	}
	
	
	void Update () {
		if(characterData.canStart)animator.speed = RotationSpeed;
		else animator.speed = 1.0f;
	}
}
