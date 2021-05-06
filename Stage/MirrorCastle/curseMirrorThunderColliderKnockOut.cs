using UnityEngine;
using System.Collections;

public class curseMirrorThunderColliderKnockOut : MonoBehaviour {
	
	AudioSource audioCtrl;
	[System.NonSerialized] public Transform owner;


    StageObjectCtrlMirrorCastle stageCtrl = null;

    //內部變數
    GameObject effectObject;

    bool sideType = false;
    int Damage = 3;
    float knockOutTime = 1.0f;
    float knockOutSpeedX = 7.0f;
    float hitForceY = 300.0f;

    float dir = 1;

    AudioClip hittedSE;
    float hittedSEPitch;
    float knockOutGravity = 2.5f;
    float knockOutDecressSpeed = 0.0f;

    void Awake(){
        stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlMirrorCastle>();
        audioCtrl = transform.GetComponent<AudioSource>();

        effectObject = stageCtrl.ThunderHiteffectObject;
        sideType = stageCtrl.ThunderSideType;
        Damage = stageCtrl.ThunderDamage;
        knockOutTime = stageCtrl.ThunderKnockOutTime;
        knockOutSpeedX = stageCtrl.ThunderKnockOutSpeedX;
        hitForceY = stageCtrl.ThunderHitForceY;
        hittedSE = stageCtrl.ThunderHittedSE;
        hittedSEPitch = stageCtrl.ThunderHittedSEPitch;
        knockOutGravity = stageCtrl.ThunderKnockOutGravity;
        knockOutDecressSpeed = stageCtrl.ThunderKnockOutDecressSpeed;
    }

	void Start(){
		dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;

	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(owner.GetComponent<XXXCtrl>().isFront == enemyCtrl.isFront){
				enemyCtrl.actionKnockOuted(sideType ,Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
				GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;
				
				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
	}
}
