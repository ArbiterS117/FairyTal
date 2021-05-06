using UnityEngine;
using System.Collections;

public class LampDMGCollider : MonoBehaviour {

    StageObjectCtrlMirrorCastle stageCtrl = null;
    AudioSource audioCtrl;

    //內部變數
    GameObject effectObject;
    AudioClip hittedSE;
    float hittedSEPitch;
    
    bool sideType = false;
    int Damage = 3;
    float knockOutTime = 1.0f;
    float knockOutSpeedX = 7.0f;
    float hitForceY = 300.0f;
    float knockOutGravity = 2.5f;
    float knockOutDecressSpeed = 0.0f;



    float dir = 1;
	
	void Awake(){
        stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlMirrorCastle>();
        audioCtrl = transform.GetComponent<AudioSource>();
	}


	void Start(){
		dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;
        effectObject = stageCtrl.LampDMGEffectObject;
        hittedSE = stageCtrl.LampDMGHittedSE;
        hittedSEPitch = stageCtrl.LampDMGHittedSEPitch;
        sideType = stageCtrl.LampDMGSideType;
        Damage = stageCtrl.LampDMGDamage;
        knockOutTime = stageCtrl.LampDMGKnockOutTime;
        knockOutSpeedX = stageCtrl.LampDMGKnockOutSpeedX;
        hitForceY = stageCtrl.LampDMGHitForceY;
        knockOutGravity = stageCtrl.LampKnockOutGravity;
        knockOutDecressSpeed = stageCtrl.LampKnockOutDecressSpeed;

    }
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(!enemyCtrl.isFront){
				enemyCtrl.actionKnockOuted(sideType ,Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
				Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z - 4.5f), Quaternion.identity);

				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
	}
}
