using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	UnityChanBullet bulletCtrl;
	
	GameObject effectObject;
	AudioSource audioCtrl;
	
	AudioClip hittedSE;
	float hittedSEPitch;
	
	//外部變數
	
	bool  sideType       = false;
	int   Damage         = 0;
	float knockOutTime   = 0.0f;
	float knockOutSpeedX = 0.0f;
	float hitForceY      = 0.0f;
    float knockOutGravity = 2.5f;
    float knockOutDecressSpeed = 0.0f;

    //內部變數

    float dir = 1;
	
	void Awake(){
		
		bulletCtrl = GetComponentInParent<UnityChanBullet>();
		audioCtrl = transform.GetComponent<AudioSource>();
		
	}
	
	void Start(){
        UnityChanCtrl unityChanCtrl = owner.GetComponent<UnityChanCtrl>();
        dir = (owner.localScale.x > 0)? 1.0f : -1.0f;

        effectObject = unityChanCtrl.getATKData().effectObject;
        sideType = unityChanCtrl.getATKData().sideType;
        Damage = unityChanCtrl.getATKData().ATK;
        knockOutTime = unityChanCtrl.getATKData().knockOutTime;
        knockOutSpeedX = unityChanCtrl.getATKData().knockBackSpeedX;
        hitForceY = unityChanCtrl.getATKData().hitForceY;
        hittedSE = unityChanCtrl.getATKData().hittedSE;
        hittedSEPitch = unityChanCtrl.getATKData().hittedSEPitch;
        knockOutGravity = unityChanCtrl.getATKData().knockOutGravity;
        knockOutDecressSpeed = unityChanCtrl.getATKData().knockOutDecressSpeed;

    }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			enemyCtrl.actionKnockOuted(sideType ,Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
			bulletCtrl.isHit = true;
			GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;

			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f);
			audioCtrl.PlayOneShot(hittedSE);
			
		}

	}
}
