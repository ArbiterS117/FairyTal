using UnityEngine;
using System.Collections;

public class FireObjColliderKnockOut : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	AudioSource audioCtrl;

	
    //內部變數

    GameObject effectObject;
    bool sideType = false;
    bool twoSide = false;
    float knockOutTime = 1.0f;
    float knockOutSpeedX = 7.0f;
    float hitForceY = 300.0f;
    float dir = 1;
    int damage;//飛行道具 腳色會切換狀態
    float knockOutGravity = 2.5f;
    float knockOutDecressSpeed = 0.0f;

    string ownerTag = null;
    int ownerPlayerNum = 0;
    int ownerTeamNum = 0;
    bool[] ownerHittedPlayer = new bool[4];

    AudioClip hittedSE;
    float hittedSEPitch;

    XXXCtrl ownerCtrl = null;

    void Awake(){
		audioCtrl = transform.GetComponent<AudioSource>();
	}

	void Start(){
        if (owner != null)
        {
            ownerCtrl = owner.GetComponent<XXXCtrl>();
            dir = (owner.lossyScale.x > 0) ? 1.0f : -1.0f;
            damage =ownerCtrl.getATKData().ATK;
            effectObject =ownerCtrl.getATKData().effectObject;
            sideType =ownerCtrl.getATKData().sideType;
            twoSide =ownerCtrl.getATKData().twoSide;
            knockOutTime =ownerCtrl.getATKData().knockOutTime;
            knockOutSpeedX =ownerCtrl.getATKData().knockBackSpeedX;
            hitForceY =ownerCtrl.getATKData().hitForceY;
            hittedSE =ownerCtrl.getATKData().hittedSE;
            hittedSEPitch =ownerCtrl.getATKData().hittedSEPitch;
            knockOutGravity = ownerCtrl.getATKData().knockOutGravity;
            knockOutDecressSpeed = ownerCtrl.getATKData().knockOutDecressSpeed;

            ownerTag = owner.tag;
            ownerPlayerNum = ownerCtrl.PlayerNUM;
            ownerTeamNum = ownerCtrl.teamNum;
            ownerHittedPlayer = ownerCtrl.hittedPlayer;
        }
        else ownerCtrl = null;

    }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("PlayerDMG")) {
			XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl> ();
			if (ownerTag != enemyCtrl.tag && GetComponentInParent<DirectionEffectCtrl> ().isFront == enemyCtrl.isFront && !ownerHittedPlayer[enemyCtrl.PlayerNUM - 1] && ownerTeamNum != enemyCtrl.teamNum) {
                if (!twoSide) enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, dir, knockOutSpeedX, hitForceY, ownerPlayerNum, knockOutGravity, knockOutDecressSpeed);
                else
                {
                    if (transform.position.x >= enemyCtrl.transform.position.x) enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, -1.0f, knockOutSpeedX, hitForceY, ownerPlayerNum, knockOutGravity, knockOutDecressSpeed);
                    else enemyCtrl.actionKnockOuted(sideType, damage, knockOutTime, 1.0f, knockOutSpeedX, hitForceY, ownerPlayerNum, knockOutGravity, knockOutDecressSpeed);
                }
                
				GameObject effect = Instantiate (effectObject, new Vector3 (other.transform.position.x + Random.Range (-1.0f, 1.0f), other.transform.position.y + Random.Range (-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
				if(owner != null)effect.GetComponent<DirectionEffectCtrl> ().owner = owner.transform;
				if(!owner)ownerCtrl.hittedPlayer [enemyCtrl.PlayerNUM - 1] = true;

				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
		//========================NPCEnemy===========================
		
		if (other.CompareTag("NPCReceiveDMG")) {
            NPCEnemyBase NPCCtrl = other.GetComponentInParent<NPCEnemyBase>();
			NPCCtrl.actionTakeDMG(damage);
			
			GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = owner.transform;
			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
			
		}
	}

    //傷害儲存器  用於放出系攻擊之二或多段傷害之資料儲存 必須搭配多碰撞器使用
    public void ASetNextStepATKData(string ATKDataName)
    {
        dir = (owner.lossyScale.x > 0) ? 1.0f : -1.0f;
        damage = ownerCtrl.getATKDataInDic(ATKDataName).ATK;
        effectObject = ownerCtrl.getATKDataInDic(ATKDataName).effectObject;
        sideType = ownerCtrl.getATKDataInDic(ATKDataName).sideType;
        twoSide = ownerCtrl.getATKDataInDic(ATKDataName).twoSide;
        knockOutTime = ownerCtrl.getATKDataInDic(ATKDataName).knockOutTime;
        knockOutSpeedX = ownerCtrl.getATKDataInDic(ATKDataName).knockBackSpeedX;
        hitForceY = ownerCtrl.getATKDataInDic(ATKDataName).hitForceY;
        hittedSE = ownerCtrl.getATKDataInDic(ATKDataName).hittedSE;
        hittedSEPitch = ownerCtrl.getATKDataInDic(ATKDataName).hittedSEPitch;
    }

}
