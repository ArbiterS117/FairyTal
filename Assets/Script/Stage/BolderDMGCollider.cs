using UnityEngine;
using System.Collections;

public class BolderDMGCollider : MonoBehaviour {
	
	public GameObject effectObjectSmash1;
	AudioSource audioCtrl;
	
	public AudioClip hittedSE;
	public float hittedSEPitch;
	
	//外部變數
	
	public bool  sideType       = false;
	public bool  HitToLeft      = false;
	public int   Damage         = 3;
	public float knockOutTime   = 1.0f;
	public float knockOutSpeedX = 7.0f;
	public float hitForceY      = 300.0f;
    public float deathHitForceY = 2000.0f;
    public float knockOutGravity = 2.5f;
    public float knockOutDecressSpeed = 0.0f;
	
	//內部變數
	
	float dir = 1;
	
	void Awake(){
		audioCtrl = transform.GetComponent<AudioSource>();
	}
	
	
	void Start(){
		if(!sideType)dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;
		else if(HitToLeft)dir = -1.0f;
	}

	void Update(){
		if(!sideType)dir = (Random.Range(-1.0f, 1.0f) > 0)? 1.0f : -1.0f;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerECollider") {
			XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl> ();
			if (!enemyCtrl.isDead) {
				enemyCtrl.hakki = false;
				enemyCtrl.actionKnockOuted (sideType, Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
				Instantiate (effectObjectSmash1, new Vector3 (other.transform.position.x + Random.Range (-1.0f, 1.0f), other.transform.position.y + Random.Range (-1.0f, 1.0f), other.transform.position.z), Quaternion.identity);

				audioCtrl.pitch = hittedSEPitch + Random.Range (-0.05f, 0.05f);
				audioCtrl.PlayOneShot (hittedSE);

			}
            else
            {
                enemyCtrl.hakki = false;
                enemyCtrl.actionKnockOuted(sideType, Damage, knockOutTime, dir, knockOutSpeedX, deathHitForceY, 0, knockOutGravity, knockOutDecressSpeed);
                Instantiate(effectObjectSmash1, new Vector3(other.transform.position.x + Random.Range(-1.0f, 1.0f), other.transform.position.y + Random.Range(-1.0f, 1.0f), other.transform.position.z), Quaternion.identity);

                audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f, 0.05f);
                audioCtrl.PlayOneShot(hittedSE);
            }
		}
	}
}
