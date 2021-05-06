using UnityEngine;

public class KnockOutedWallCollider : MonoBehaviour {

	XXXCtrl playerCtrl;
	
	void Awake() {
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(playerCtrl.isKnockOuted && !playerCtrl.grounded && !playerCtrl.isDead){
			if (other.tag == "Wall") {
				playerCtrl.wallBackDamage = true;

            }
		}

        if (other.tag == "Wall") playerCtrl.isTriggeredWall = true;


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Wall") playerCtrl.isTriggeredWall = false;
    }
}
