using UnityEngine;
using System.Collections;

public class CrazyOldManReceiveGiftCollider : MonoBehaviour {

	CrazyOldMan crazyOldMan;

	void Awake () {
		crazyOldMan = GetComponentInParent<CrazyOldMan>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerECollider") {
			if(other.GetComponentInParent<XXXCtrl>().isCatchingGift)
			crazyOldMan.isGettingGift = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "PlayerECollider") {
			if(other.GetComponentInParent<XXXCtrl>().isCatchingGift)
			crazyOldMan.isGettingGift = false;
		}
	}


}
