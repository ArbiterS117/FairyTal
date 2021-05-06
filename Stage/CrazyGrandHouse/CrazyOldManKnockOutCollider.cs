using UnityEngine;
using System.Collections;

public class CrazyOldManKnockOutCollider : MonoBehaviour {

    CrazyOldMan ownerCtrl;

	AudioSource audioCtrl;
	
	AudioClip hittedSE  = null;
	float hittedSEPitch = 1.0f;

	
	//內部變數
	
	float dir = 1;

	void Awake(){
        ownerCtrl = transform.parent.GetComponent<CrazyOldMan>();
        audioCtrl = transform.GetComponent<AudioSource>();
	}

	void Start(){
		dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;
        hittedSE = ownerCtrl.hittedSE;
        hittedSEPitch = ownerCtrl.hittedSEPitch;
    }

	void Update(){
		dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			enemyCtrl.actionKnockOuted(ownerCtrl.sideType, ownerCtrl.Damage, ownerCtrl.knockOutTime, dir, ownerCtrl.knockOutSpeedX, ownerCtrl.hitForceY, 0, ownerCtrl.knockOutGravity, ownerCtrl.knockOutDecressSpeed);
			Instantiate(ownerCtrl.effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity);

			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
		}
	}
}

