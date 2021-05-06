using UnityEngine;
using System.Collections;

public class StoneCollider : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	StoneCtrl stoneCtrl;

	GameObject effectObject;
	AudioSource audioCtrl;

	AudioClip hittedSE;
	float hittedSEPitch;
	
	//外部變數
	public bool canPenetratePlane = true;
	
	bool  sideType       = false;
	float knockOutTime   = 1.0f;
	float knockOutSpeedX = 7.0f;
	float hitForceY      = 300.0f;
    public float knockOutGravity = 2.5f;
    public float knockOutDecressSpeed = 0.0f;

    string ownerTag = null;
    int ownerPlayerNum = 0;
    int ownerTeamNum = 0;
    bool[] ownerHittedPlayer = new bool[4];

    //內部變數

    int Damage = 0;
	float dir = 1;

	void Awake(){

		stoneCtrl = GetComponentInParent<StoneCtrl>();
		audioCtrl = transform.GetComponent<AudioSource>();


	}
	
	void Start(){
        if (owner != null)
        {
            XXXCtrl ownerxxxCtrl = owner.GetComponent<XXXCtrl>();
            dir = (owner.localScale.x > 0) ? 1.0f : -1.0f;
            Damage = ownerxxxCtrl.getATKData().ATK;
            effectObject = ownerxxxCtrl.getATKData().effectObject;
            sideType = ownerxxxCtrl.getATKData().sideType;
            knockOutTime = ownerxxxCtrl.getATKData().knockOutTime;
            knockOutSpeedX = ownerxxxCtrl.getATKData().knockBackSpeedX;
            hitForceY = ownerxxxCtrl.getATKData().hitForceY;
            hittedSE = ownerxxxCtrl.getATKData().hittedSE;
            hittedSEPitch = ownerxxxCtrl.getATKData().hittedSEPitch;
            knockOutGravity = ownerxxxCtrl.getATKData().knockOutGravity;
            knockOutDecressSpeed = ownerxxxCtrl.getATKData().knockOutDecressSpeed;

            ownerTag = owner.tag;
            ownerPlayerNum = ownerxxxCtrl.PlayerNUM;
            ownerTeamNum = ownerxxxCtrl.teamNum;
            ownerHittedPlayer = ownerxxxCtrl.hittedPlayer;
        }
    }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(ownerTag != enemyCtrl.tag && GetComponentInParent<DirectionEffectCtrl>().isFront == enemyCtrl.isFront && !ownerHittedPlayer[enemyCtrl.PlayerNUM - 1] && ownerTeamNum != enemyCtrl.teamNum)
            {
				stoneCtrl.isHit = true;
				enemyCtrl.actionKnockOuted(sideType ,Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, ownerPlayerNum, knockOutGravity, knockOutDecressSpeed);
				GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
				if(owner != null)effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;

				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f);
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
		//========================NPCEnemy===========================
		
		if (other.tag == "NPCReceiveDMG") {
            NPCEnemyBase NPCCtrl = other.GetComponentInParent<NPCEnemyBase>();
            NPCCtrl.actionTakeDMG(Damage);
			stoneCtrl.isHit = true;
			GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;
			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
			
		}

		//==========================可穿透物件==========================

		if (!canPenetratePlane) {
			if(GetComponentInParent<DirectionEffectCtrl>().isFront){
				if (other.tag == "Platform" || other.tag == "PenetratePlatform" || other.tag == "Wall") {
					stoneCtrl.isHit = true;
				}
			}
			else{
				if (other.tag == "BehindPlatform" || other.tag == "BehindPenetratePlatform" || other.tag == "Wall") {
					stoneCtrl.isHit = true;
				}
			}
		}
	}
}
