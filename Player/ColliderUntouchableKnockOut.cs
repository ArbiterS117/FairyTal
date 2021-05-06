using UnityEngine;
using System.Collections;

public class ColliderUntouchableKnockOut : MonoBehaviour {

	XXXCtrl playerCtrl;
	public GameObject effectObjectSmash1;

	AudioSource audioCtrl;
	
	public AudioClip hittedSE;
	public float hittedSEPitch;

	//外部變數
	
	public bool  sideType       = false;
	public float knockOutTime   = 1.0f;
	public float knockOutSpeedX = 7.0f;
	public float hitForceY      = 300.0f;
    public float knockOutGravity = 2.5f;
    public float knockOutDecressSpeed = 0.0f;


    //內部變數

    float dir = 1;

	void Awake(){
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();
		audioCtrl = transform.GetComponent<AudioSource>();
	}

	void Update(){
		if (playerCtrl.isUntouchable) {
			dir = (Random.Range (-1.0f, 1.0f) > 0) ? 1.0f : -1.0f;
			this.transform.GetComponent<BoxCollider2D> ().enabled = true;
		}
		else 
		{
			this.transform.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerDMG") {
			XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl> ();
			if (playerCtrl.tag != enemyCtrl.tag && playerCtrl.isFront == enemyCtrl.isFront && !playerCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] && playerCtrl.teamNum != enemyCtrl.teamNum) {
				enemyCtrl.actionKnockOuted (sideType,  playerCtrl.getATKData().ATK, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
				GameObject effect = Instantiate (effectObjectSmash1, new Vector3 (other.transform.position.x + Random.Range (-1.0f, 1.0f), other.transform.position.y + Random.Range (-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
				effect.GetComponent<DirectionEffectCtrl> ().owner = playerCtrl.transform;
				playerCtrl.hittedPlayer[enemyCtrl.PlayerNUM - 1] = true;
				
				audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f);
				audioCtrl.PlayOneShot(hittedSE);
			}
		}
		//========================NPCEnemy===========================
		
		if (other.tag == "NPCReceiveDMG") {
            NPCEnemyBase NPCCtrl = other.GetComponentInParent<NPCEnemyBase>();
            NPCCtrl.actionTakeDMG( playerCtrl.getATKData().ATK);
			
			GameObject effect = Instantiate(effectObjectSmash1, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-1.0f,1.0f),other.transform.position.z), Quaternion.identity) as GameObject;
			effect.GetComponent<DirectionEffectCtrl>().owner = playerCtrl.transform;
			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
			
		}
	}
}
