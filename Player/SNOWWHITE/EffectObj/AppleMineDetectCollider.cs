using UnityEngine;
using System.Collections;

public class AppleMineDetectCollider : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	AppleMineCtrl appleMineCtrl;

    string ownerTag = null;

	void Awake(){
		appleMineCtrl = GetComponentInParent<AppleMineCtrl>();
	}

    private void Start()
    {
        ownerTag = owner.tag;
    }

    void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl  = other.GetComponentInParent<XXXCtrl>();
			if(ownerTag != enemyCtrl.tag && GetComponentInParent<DirectionEffectCtrl>().isFront == enemyCtrl.isFront){
				appleMineCtrl.isHit = true;
			}
		}
        if (other.tag == "Wall")
        {
            appleMineCtrl.isHit = true;
        }
            //========================NPCEnemy===========================

            if (other.tag == "NPCReceiveDMG") {
			appleMineCtrl.isHit = true;

		}
	}
}
