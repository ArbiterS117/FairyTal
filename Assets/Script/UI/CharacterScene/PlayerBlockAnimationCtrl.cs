using UnityEngine;
using System.Collections;

public class PlayerBlockAnimationCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;
	Animator animator;

	int playerNUM = 0;

	void Awake () {
		sceneCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
		animator = GetComponent<Animator> ();
	}

	void Start () {
		if(this.transform.tag == ("Player1")){playerNUM = 1;}
		else if(this.transform.tag == ("Player2")){playerNUM = 2;}
		else if(this.transform.tag == ("Player3")){playerNUM = 3;}
		else if(this.transform.tag == ("Player4")){playerNUM = 4;}
	}
	

	void Update () {
		//腳色滑入部分
		if(sceneCtrl.playerImageStartTrigger[playerNUM-1] && !sceneCtrl.isSelected [playerNUM - 1]){
			animator.SetBool("Idle",false);
		         if(sceneCtrl.PlayerImageStart [playerNUM-1] == 0)animator.SetBool("OnRED",true);
			else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 1)animator.SetBool("OnALICE",true);
			else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 2)animator.SetBool("OnMOMOTARO",true);
			else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 3)animator.SetBool("OnSNOWWHITE",true);
			else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 4)animator.SetBool("OnRAPUNZEL",true);
            else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 5)animator.SetBool("OnALADDIN",true);
			else if(sceneCtrl.PlayerImageStart [playerNUM-1] == 6)animator.SetBool("OnRANDOM",true);
		}
		else{
			animator.SetBool("Idle",true);
			animator.SetBool("OnRED",false);
			animator.SetBool("OnALICE",false);
			animator.SetBool("OnMOMOTARO",false);
			animator.SetBool("OnSNOWWHITE",false);
			animator.SetBool("OnRAPUNZEL",false);
            animator.SetBool("OnALADDIN", false);
            animator.SetBool("OnRANDOM",false);
		}

		//選定腳色部分
		if (sceneCtrl.isSelected [playerNUM - 1]) {
			animator.SetBool("Idle",false);
			animator.SetBool("OnRED",false);
			animator.SetBool("OnALICE",false);
			animator.SetBool("OnMOMOTARO",false);
			animator.SetBool("OnSNOWWHITE",false);
			animator.SetBool("OnRAPUNZEL",false);
            animator.SetBool("OnALADDIN", false);
            animator.SetBool("OnRANDOM",false);

			     if(sceneCtrl.SelectedCharacterIndex[playerNUM - 1] == 0)animator.SetBool("SelectRED",true);
			else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 1)animator.SetBool("SelectALICE",true);
			else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 2)animator.SetBool("SelectMOMOTARO",true);
			else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 3)animator.SetBool("SelectSNOWWHITE",true);
			else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 4)animator.SetBool("SelectRAPUNZEL",true);
            else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 5)animator.SetBool("SelectALADDIN",true);
			else if(sceneCtrl.SelectedCharacterIndex [playerNUM-1] == 6)animator.SetBool("SelectRANDOM",true);
		}
		else{
			animator.SetBool("SelectRED",false);
			animator.SetBool("SelectALICE",false);
			animator.SetBool("SelectMOMOTARO",false);
			animator.SetBool("SelectSNOWWHITE",false);
			animator.SetBool("SelectRAPUNZEL",false);
            animator.SetBool("SelectALADDIN", false);
            animator.SetBool("SelectRANDOM",false);
		}
		if (sceneCtrl.CancelSelected[playerNUM - 1] == true) {
			animator.SetTrigger("CancelSelected");
			sceneCtrl.CancelSelected[playerNUM - 1] = false;
		}


	}

	//======自創===============



}
