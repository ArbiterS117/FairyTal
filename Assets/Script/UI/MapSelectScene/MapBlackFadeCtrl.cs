using UnityEngine;
using System.Collections;

public class MapBlackFadeCtrl : MonoBehaviour {
	
	Animator animator;

	public bool GameStart = false;

	void Awake(){

		animator = GetComponent<Animator>();

	}

	void Start () {
	
	}
	

	void Update () {
		if(GameStart)animator.SetBool("GameStart", true);
	}


}
