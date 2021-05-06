using UnityEngine;
using System.Collections;

public class DisplayBigMapAnim : MonoBehaviour {

	Animator animator;
	MapSelectSceneCtrl SceneCtrl;

	void Awake(){
		SceneCtrl = GameObject.Find("MapGameCtrl").GetComponent<MapSelectSceneCtrl> (); 
		animator  = GetComponent<Animator>();

	}

	void Start () {
	
	}
	

	void Update () {
		if (SceneCtrl.isOnBattleField)
			animator.SetBool ("OnBattleField", true);
		else
			animator.SetBool ("OnBattleField", false);

		if (SceneCtrl.isOnMirrorCastle)
			animator.SetBool ("OnMirrorCastle", true);
		else
			animator.SetBool ("OnMirrorCastle", false);


		if (SceneCtrl.isOnCrazyGrandHouse)
			animator.SetBool ("OnCrazyGrandHouse", true);
		else
			animator.SetBool ("OnCrazyGrandHouse", false);

		if (SceneCtrl.isOnSkyWorld)
			animator.SetBool ("OnSkyWorld", true);
		else
			animator.SetBool ("OnSkyWorld", false);

		if (SceneCtrl.isOnSlpbty)
			animator.SetBool ("OnSlpbty", true);
		else
			animator.SetBool ("OnSlpbty", false);

		if (SceneCtrl.isOnSevenDraftMine)
			animator.SetBool ("OnSevenDraftMine", true);
		else
			animator.SetBool ("OnSevenDraftMine", false);


		if (SceneCtrl.isOnTrainning)
			animator.SetBool ("OnTrainning", true);
		else
			animator.SetBool ("OnTrainning", false);


	}

}
