using UnityEngine;
using System.Collections;

public class WallCollider : MonoBehaviour {

	XXXCtrl character;

	void Awake() {
		character = transform.parent.GetComponent<XXXCtrl>();
	}

	//判斷是否貼牆
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wall") {
			character.isTouchingWall = true;
			character.isDashing = false;
			character.isDashJump = false;
			character.toSetVelocityX = true;  
			character.setVelocityX   = 0.0f;  
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Wall") {
			character.isTouchingWall = false;
		}
	}
}
