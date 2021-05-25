using UnityEngine;
using System.Collections;

public class LampReceiveDMGCollider : MonoBehaviour {

    //=====跳太高即使在前排還是可以打中吊燈

    StageObjectCtrlMirrorCastle stageCtrl = null;
    LampCtrl lampCtrl;
	AudioSource audioCtrl;

	GameObject effectObject;

	AudioClip hittedSE;
	float hittedSEPitch;

	void Awake(){
        stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlMirrorCastle>();
        lampCtrl = GetComponentInParent<LampCtrl>();
		audioCtrl = transform.GetComponent<AudioSource>();

    }

	void Start () {
        effectObject = stageCtrl.LampHittedEffectObject;
        hittedSE = stageCtrl.LamphittedSE;
        hittedSEPitch = stageCtrl.LamphittedSEPitch;

    }


    void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Attack") {
			Instantiate(effectObject, new Vector3(other.transform.position.x +Random.Range(-1.0f,1.0f) ,other.transform.position.y +Random.Range(-2.0f,1.0f),other.transform.position.z - 5.0f), Quaternion.identity) ;
			lampCtrl.LampHP -= 1;

			audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(hittedSE);
		}
	}



}
