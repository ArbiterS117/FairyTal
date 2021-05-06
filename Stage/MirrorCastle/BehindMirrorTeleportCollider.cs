using UnityEngine;
using System.Collections;

public class BehindMirrorTeleportCollider : MonoBehaviour {

	XXXCtrl playerCtrl;
	public GameObject effectObjectBlink;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerECollider") {
			playerCtrl = other.transform.parent.GetComponent<XXXCtrl>();
			if(!playerCtrl.isFront){
				playerCtrl.isFront = true;
				playerCtrl.toSetUpForce = true;
				playerCtrl.setUpForce = 0.0f;
				float blinkPosZ = other.transform.position.z;

                float RandomPos = 0;
                if (Random.Range(0.0f, 2.0f) >= 1.0f) RandomPos = Random.Range(-5.0f, -7.0f);
                else RandomPos = Random.Range(5.0f, 7.0f);
                other.transform.parent.position = new Vector3 (RandomPos, 6.0f, 0.0f);

				GameObject effect = Instantiate(effectObjectBlink, new Vector3(this.transform.position.x +Random.Range(-0.5f,0.5f) ,this.transform.position.y +Random.Range(-0.5f,0.5f),blinkPosZ), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
			}
		}
	}
}
