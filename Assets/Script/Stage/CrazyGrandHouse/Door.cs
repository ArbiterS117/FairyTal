using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[System.NonSerialized] public Animator animator;
	[System.NonSerialized] public StageObjectCtrlCrazyGrandHouse stageCtrl;

	float CTime = 0.0f;

	void Awake(){
		animator = GetComponent<Animator>();
		stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlCrazyGrandHouse>();
	}

	void Start () {
		
	}

	void Update () {
		CTime += Time.deltaTime;
		if (!stageCtrl.CrazyOldManIsLive && CTime > stageCtrl.DoorToCrazyOldManTime + 1.0f) { // 1.0預防程式執行過快出錯
			animator.SetTrigger("DoorClose");
		}
	}


	//=========自創功能========

	public void DstroyObj(){
		stageCtrl.DoorIsLive = false;

		Destroy(this.gameObject);
	}

}
