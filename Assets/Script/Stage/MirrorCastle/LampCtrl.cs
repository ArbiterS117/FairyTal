using UnityEngine;
using System.Collections;

public class LampCtrl : MonoBehaviour {

    StageObjectCtrlMirrorCastle stageCtrl = null;
	Animator animator;

	[System.NonSerialized] public float LampHP;
	float oriLampHP; 
	float UpColdTime = 10.0f;
	float UpColdCTime = 0.0f;

	bool isFall = false;

	void Awake(){
        stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlMirrorCastle>();
        animator = GetComponent<Animator> ();

        LampHP = stageCtrl.LampHP;
        UpColdTime = stageCtrl.UpColdTime;
    }

	void Start () {
		oriLampHP = LampHP;
	}
	

	void Update () {

		if (LampHP <= 0) {
			animator.SetTrigger("FALL");
			isFall = true;
			LampHP = oriLampHP;
		}

		if (isFall) {
			UpColdCTime += Time.deltaTime;
			if(UpColdCTime >= UpColdTime){
				animator.SetTrigger("UP");
				isFall = false;
				UpColdCTime = 0.0f;
			}
		}

	}

}
