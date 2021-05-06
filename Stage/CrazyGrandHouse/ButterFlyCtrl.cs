using UnityEngine;
using System.Collections;

public class ButterFlyCtrl : MonoBehaviour {

	[System.NonSerialized] public Animator animator;

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	void Start () {
		AsetAnimTrigger ();
	}
	

	void AsetAnimTrigger(){
		float RandomNum = Random.Range (0.0f, 3.0f);
		if (RandomNum >= 0.0f && RandomNum < 1.0f) animator.SetTrigger ("Idle1");
		else if (RandomNum >= 1.0f && RandomNum < 2.0f) animator.SetTrigger ("Idle2");
		else if (RandomNum >= 2.0f) animator.SetTrigger ("Idle3");
		animator.speed = RandomNum / 6.0f + 0.65f;
	}

}
