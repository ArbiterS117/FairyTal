using UnityEngine;
using System.Collections;

public class MirrorTeleportCollider : MonoBehaviour {

	XXXCtrl playerCtrl;
	public GameObject effectObjectBlink;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerECollider") {
			playerCtrl = other.transform.parent.GetComponent<XXXCtrl>();
			if(playerCtrl.isFront){
				playerCtrl.isFront = false;
				playerCtrl.toSetUpForce = true;
				playerCtrl.setUpForce = 0.0f;
				float blinkPosZ = other.transform.position.z;
				other.transform.parent.position = new Vector3 (Random.Range(-4.0f , 4.0f) , 8.0f, 8.0f);

				GameObject effect = Instantiate(effectObjectBlink, new Vector3(this.transform.position.x +Random.Range(-0.5f,0.5f) ,this.transform.position.y +Random.Range(-0.5f,0.5f),blinkPosZ), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
			}
		}
	}
}
