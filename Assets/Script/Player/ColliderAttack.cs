using UnityEngine;
using System.Collections;

public class ColliderAttack : MonoBehaviour {

	XXXCtrl playerCtrl;
	AudioSource audioCtrl;


	void Awake(){
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();
		audioCtrl = transform.GetComponent<AudioSource>();
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("PlayerDMG")) {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(playerCtrl.tag != enemyCtrl.tag && playerCtrl.isFront == enemyCtrl.isFront && !playerCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] && playerCtrl.teamNum != enemyCtrl.teamNum){
                if (!playerCtrl.getATKData().twoSide)
                {
                    enemyCtrl.actionTakeDMG(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, playerCtrl.dir, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                    if (enemyCtrl.isTriggeredWall)
                    {
                        playerCtrl.toSetSideForce = true;
                        playerCtrl.setSideForce = playerCtrl.WallATKBackForce * -playerCtrl.dir;
                    }
                }
                else
                {
                    if (playerCtrl.transform.position.x >= enemyCtrl.transform.position.x)
                    {
                        if (enemyCtrl.isTriggeredWall)
                        {
                            playerCtrl.toSetSideForce = true;
                            playerCtrl.setSideForce = playerCtrl.WallATKBackForce * -playerCtrl.dir;
                        }
                        enemyCtrl.actionTakeDMG(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, -1.0f, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                    }
                    else
                    {
                        if (enemyCtrl.isTriggeredWall)
                        {
                            playerCtrl.toSetSideForce = true;
                            playerCtrl.setSideForce = playerCtrl.WallATKBackForce * -playerCtrl.dir;
                        }
                        enemyCtrl.actionTakeDMG(playerCtrl.getATKData().ATK, playerCtrl.getATKData().knockOutTime, 1.0f, playerCtrl.getATKData().knockBackSpeedX, playerCtrl.getATKData().hitForceY, playerCtrl.PlayerNUM, playerCtrl.getATKData().knockOutGravity, playerCtrl.getATKData().knockOutDecressSpeed);
                    }
                }
                
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
            NPCCtrl.actionTakeDMG(playerCtrl.getATKData().ATK);

			GameObject effect = Instantiate(playerCtrl.getATKData().effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
			audioCtrl.pitch = playerCtrl.getATKData().hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(playerCtrl.getATKData().hittedSE);
			
		}

	}
}
