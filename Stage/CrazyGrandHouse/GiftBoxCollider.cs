using UnityEngine;
using System.Collections;

public class GiftBoxCollider : MonoBehaviour {

	GiftBox giftBox;
	
	void Awake(){
		giftBox = GetComponentInParent<GiftBox>();
	}
	
	
	void OnTriggerEnter2D(Collider2D other){
		if (!giftBox.isCatched && giftBox.canCatch) {
			if (other.tag == "PlayerECollider") {
				if(!other.transform.parent.GetComponent<XXXCtrl>().isKnockDown  &&
				   !other.transform.parent.GetComponent<XXXCtrl>().isKnockOuted &&
				   !other.transform.parent.GetComponent<XXXCtrl>().isDead){
				giftBox.playerTransform = other.transform.parent.transform;
				giftBox.isCatched = true;
				}
			}
		}
	}

}
