using UnityEngine;
using System.Collections;

public class ColliderDetectPlayerAction : MonoBehaviour {

	CrazyOldMan crazyOldMan;

	void Awake(){
		crazyOldMan = GetComponentInParent<CrazyOldMan>();
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (!crazyOldMan.isGetMad && !crazyOldMan.isGetBack) {
			if (other.tag == "Attack") {
				crazyOldMan.getAngry();
			}
		}
	}
	
}
