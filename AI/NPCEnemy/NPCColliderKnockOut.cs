using UnityEngine;
using System.Collections;

public class NPCColliderKnockOut : MonoBehaviour {

	NPCEnemyBase enemyBaseCtrl;
	
	AudioSource audioCtrl;
	
	void Awake(){
		audioCtrl = transform.GetComponent<AudioSource>();
		enemyBaseCtrl = transform.GetComponentInParent<NPCEnemyBase>();
	}

    void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl> ();
            UnityChanCtrl unityChanCtrl = enemyBaseCtrl.GetComponentInParent<UnityChanCtrl>();
			enemyCtrl.actionKnockOuted (unityChanCtrl.getATKData().sideType, unityChanCtrl.getATKData().ATK, unityChanCtrl.getATKData().knockOutTime, enemyBaseCtrl.dir, unityChanCtrl.getATKData().knockBackSpeedX, unityChanCtrl.getATKData().hitForceY, 0, unityChanCtrl.getATKData().knockOutGravity, unityChanCtrl.getATKData().knockOutDecressSpeed);
			GameObject effect = Instantiate(unityChanCtrl.getATKData().effectObject, new Vector3(other.transform.position.x + Random.Range (-1.0f, 1.0f), other.transform.position.y + Random.Range (-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = enemyBaseCtrl.transform;

            enemyBaseCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] = true;

            audioCtrl.pitch = unityChanCtrl.getATKData().hittedSEPitch + Random.Range(-0.05f, 0.05f);
			audioCtrl.PlayOneShot(unityChanCtrl.getATKData().hittedSE);

		}
	}

}
