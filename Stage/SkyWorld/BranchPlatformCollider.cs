using UnityEngine;
using System.Collections;

public class BranchPlatformCollider : MonoBehaviour {

	public float springJumpForce;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerECollider") {
			if(other.GetComponentInParent<Rigidbody2D>().velocity.y <= 0.0f){
                XXXCtrl ownerCtrl = other.GetComponentInParent<XXXCtrl>();
                ownerCtrl.isDashing = false;
				ownerCtrl.toSetUpForce = true;
				ownerCtrl.setUpForce = springJumpForce;
				ownerCtrl.backDMGSwitch = true;
				ownerCtrl.actionBackDamage ();
			}
		}
	}

}
