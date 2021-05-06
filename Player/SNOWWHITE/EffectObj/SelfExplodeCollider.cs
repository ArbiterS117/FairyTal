using UnityEngine;
using System.Collections;

public class SelfExplodeCollider : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	
	GameObject effectObject;
	
	AudioSource audioCtrl;
	
	AudioClip hittedSE;
	float hittedSEPitch;

    //外部變數
    public string ATKDataName = "SelfExplodeATK";//自爆資料特別用


    //內部變數
    bool sideType = false;
    bool twoSide = false;
    int damage = 3;
    float knockOutTime = 1.0f;
    float knockOutSpeedX = 7.0f;
    float hitForceY = 300.0f;
    float knockOutGravity = 2.5f;
    float knockOutDecressSpeed = 0.0f;


    float dir = 1;
	
	void Awake(){
		audioCtrl = transform.GetComponent<AudioSource>();
	}
	
	void Start(){
        if (owner != null)
        {
            XXXCtrl ownerxxxCtrl = owner.GetComponent<XXXCtrl>();
            dir = (owner.localScale.x > 0) ? 1.0f : -1.0f;
            damage = ownerxxxCtrl.getATKDataInDic(ATKDataName).ATK;
            effectObject = ownerxxxCtrl.getATKDataInDic(ATKDataName).effectObject;
            sideType = ownerxxxCtrl.getATKDataInDic(ATKDataName).sideType;
            twoSide = ownerxxxCtrl.getATKDataInDic(ATKDataName).twoSide;
            knockOutTime = ownerxxxCtrl.getATKDataInDic(ATKDataName).knockOutTime;
            knockOutSpeedX = ownerxxxCtrl.getATKDataInDic(ATKDataName).knockBackSpeedX;
            hitForceY = ownerxxxCtrl.getATKDataInDic(ATKDataName).hitForceY;
            hittedSE = ownerxxxCtrl.getATKDataInDic(ATKDataName).hittedSE;
            hittedSEPitch = ownerxxxCtrl.getATKDataInDic(ATKDataName).hittedSEPitch;
            knockOutGravity = ownerxxxCtrl.getATKDataInDic(ATKDataName).knockOutGravity;
            knockOutDecressSpeed = ownerxxxCtrl.getATKDataInDic(ATKDataName).knockOutDecressSpeed;
        }
        }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl> ();
			if (owner.GetComponent<XXXCtrl> ().isFront == enemyCtrl.isFront && !owner.GetComponent<XXXCtrl> ().hittedPlayer [enemyCtrl.PlayerNUM - 1]) {

                if (!twoSide) enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, dir, knockOutSpeedX, hitForceY, owner.GetComponent<XXXCtrl>().PlayerNUM , knockOutGravity, knockOutDecressSpeed);
                else
                {
                    if (transform.position.x >= enemyCtrl.transform.position.x) enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, -1.0f, knockOutSpeedX, hitForceY, owner.GetComponent<XXXCtrl>().PlayerNUM, knockOutGravity, knockOutDecressSpeed);
                    else enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, dir, knockOutSpeedX, hitForceY, owner.GetComponent<XXXCtrl>().PlayerNUM, knockOutGravity, knockOutDecressSpeed);
                }
                
				GameObject effect = Instantiate (effectObject, new Vector3 (other.transform.position.x + Random.Range (-1.0f, 1.0f), other.transform.position.y + Random.Range (-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl> ().owner = owner.transform;
				owner.GetComponent<XXXCtrl> ().hittedPlayer [enemyCtrl.PlayerNUM - 1] = true;
				
				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
		//========================NPCEnemy===========================
		
		if (other.tag == "NPCReceiveDMG") {
			NPCReceiveDMG NPCCtrl = other.GetComponent<NPCReceiveDMG>();
			NPCCtrl.actionTakeDMG(damage);
			
			GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;
			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
			
		}
	}
}
