using UnityEngine;
using System.Collections;

public class KnockDamagedDown : MonoBehaviour {
	
	XXXCtrl playerCtrl;
	AudioSource audioCtrl;

	void Awake(){
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();
		audioCtrl = transform.GetComponent<AudioSource>();
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("PlayerDMG")) {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(playerCtrl.tag != enemyCtrl.tag && playerCtrl.isFront == enemyCtrl.isFront && !playerCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] && playerCtrl.teamNum != enemyCtrl.teamNum)
            {

                if (!playerCtrl.getATKData().twoSide) enemyCtrl.actionKnockDamagedDown(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, playerCtrl.dir, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                else
                {
                    if (playerCtrl.transform.position.x >= enemyCtrl.transform.position.x) enemyCtrl.actionKnockDamagedDown(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, -1.0f, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                    else enemyCtrl.actionKnockDamagedDown(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, 1.0f, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                }
                //playerCtrl.SetPadVibration(Mathf.Max(Mathf.Abs(playerCtrl.getATKData().knockBackSpeedX / 1500.0f), Mathf.Abs(playerCtrl.getATKData().hitForceY / 1500.0f)), playerCtrl.getATKData().ATK / 5.0f);

                GameObject effect = Instantiate(playerCtrl.getATKData().effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
				playerCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] = true;

				if(playerCtrl.grounded && playerCtrl.getATKData().canPauseAnim)playerCtrl.animPause = true;
				audioCtrl.pitch = playerCtrl.getATKData().hittedSEPitch + Random.Range(-0.05f,0.05f) ;
				audioCtrl.PlayOneShot(playerCtrl.getATKData().hittedSE);
			}
		}
		//========================NPCEnemy===========================
		
		if (other.CompareTag("NPCReceiveDMG")) {
            NPCEnemyBase NPCCtrl = other.GetComponentInParent<NPCEnemyBase>();
            NPCCtrl.actionTakeDMG( playerCtrl.getATKData().ATK);

			GameObject effect = Instantiate(playerCtrl.getATKData().effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
			audioCtrl.pitch = playerCtrl.getATKData().hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(playerCtrl.getATKData().hittedSE);
			
		}
	}
}

