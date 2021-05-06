using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SDMStageEventCtrl : MonoBehaviour {

	Animator Anim;

	bool isIdle;
	public float IdleTime  = 10.0f;
	       float IdleCTime = 0.0f;
	public float EventRate = 80.0f;
	public List<string> eventHappened = new List<string>();

	public int eventSize = 0;

	void Awake(){
		Anim = GetComponent<Animator> ();
	}

	void Start () {
		for (int i = 1; i <= eventSize; i ++) {
			eventHappened.Add("Event" + i.ToString());
		}
	}

	void Update () {
		if (isIdle) {
			IdleCTime += Time.deltaTime;
			if(IdleCTime >= IdleTime){
				IdleCTime = 0.0f;
				isIdle = false;
				pickEvent();

			}
		}
	}

	//========Function=========

	void pickEvent(){
		if (eventHappened.Count > 0) {
			int r = (int)Mathf.Ceil (Random.Range (0.01f, eventHappened.Count - 0.01f)); 
			Anim.SetTrigger (eventHappened [r - 1]);
		}
	}

	//========Anim=============

	public void isInIdle(){
		isIdle = true;
	}

}
