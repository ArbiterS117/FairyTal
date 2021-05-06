using UnityEngine;
using System.Collections;

public class CurseMirror : MonoBehaviour {
	
	//欲產生的東西
	public GameObject Thunder;

	GameCtrl gameCtrl;
	XXXCtrl  playerCtrl;
    StageObjectCtrlMirrorCastle stageCtrl;

	//內部變數
	float startColdTime = 5.0f;
	float curseTime     = 10.0f;
	float destroyTime   = 2.0f;

	float time    = 0.0f;
	bool  isCatched  = false;
	bool  isCursed  = false;

	public bool  curseReady = false;

	void Awake () {
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlMirrorCastle>();

    }

	void Start(){
        startColdTime = stageCtrl.CursedMirrorStartColdTime;
        curseTime = stageCtrl.CursedMirrorCurseTime;
        destroyTime = stageCtrl.CursedMirrorDestroyTime;

    }

	void Update () {

		time += Time.deltaTime;
		if (!isCatched) {
			if (time >= startColdTime) {
				float i = Random.Range (0.0f, 2.0f);         //抽選玩家
				if (i >= 0.0f && i < 1.0f) {
					if(!gameCtrl.isDead[0]){
						playerCtrl = GameObject.FindGameObjectWithTag ("Player1").GetComponent<XXXCtrl>();
					}
					else{
						Destroy(this.gameObject);
					}
				}
				else {
					if(!gameCtrl.isDead[1]){
						playerCtrl = GameObject.FindGameObjectWithTag ("Player2").GetComponent<XXXCtrl>();
					}
					else{
						Destroy(this.gameObject);
					}
				}
				if(playerCtrl != null){
					playerCtrl.isCursed = true;
					isCatched = true;
				}
				else{
					Destroy(this.gameObject);
				}

				time = 0.0f;
			}
		}

		//*****************************有風險******************************* 小機率會抓不到物件
		if (isCatched) {

			if(time >= curseTime - 3){
				curseReady = true;
			}

			if (!isCursed) {
				if (time >= curseTime) {
					if(!gameCtrl.isDead[0]){
						playerCtrl = GameObject.FindGameObjectWithTag ("Player1").GetComponent<XXXCtrl> ();
						if(playerCtrl != null){
							if (playerCtrl.isCursed) {
								this.transform.position = new Vector3(playerCtrl.transform.position.x, playerCtrl.transform.position.y +2.2f, playerCtrl.transform.position.z);
								GameObject effect = Instantiate (Thunder, playerCtrl.transform.position, Quaternion.identity)as GameObject;
								effect.GetComponentInChildren<curseMirrorThunderColliderKnockOut>().owner = playerCtrl.transform;
								effect.GetComponent<DirectionEffectCtrl>().owner = this.transform;
								playerCtrl.isCursed = false;
							}
						}
					}

					if(!gameCtrl.isDead[1]){
						playerCtrl = GameObject.FindGameObjectWithTag("Player2").GetComponent<XXXCtrl> ();
						if(playerCtrl != null){
							if (playerCtrl.isCursed) {
								this.transform.position = new Vector3(playerCtrl.transform.position.x, playerCtrl.transform.position.y +2.2f, playerCtrl.transform.position.z);
								GameObject effect = Instantiate (Thunder, playerCtrl.transform.position, Quaternion.identity)as GameObject;
								effect.GetComponentInChildren<curseMirrorThunderColliderKnockOut>().owner = playerCtrl.transform;
								effect.GetComponent<DirectionEffectCtrl>().owner = this.transform;
								playerCtrl.isCursed = false;
							}
						}
					}

					if(!gameCtrl.isDead[2]){
						playerCtrl = GameObject.FindGameObjectWithTag ("Player3").GetComponent<XXXCtrl> ();
						if(playerCtrl != null){
							if (playerCtrl.isCursed) {
								this.transform.position = new Vector3(playerCtrl.transform.position.x, playerCtrl.transform.position.y +2.2f, playerCtrl.transform.position.z);
								GameObject effect = Instantiate (Thunder, playerCtrl.transform.position, Quaternion.identity)as GameObject;
								effect.GetComponentInChildren<curseMirrorThunderColliderKnockOut>().owner = playerCtrl.transform;
								effect.GetComponent<DirectionEffectCtrl>().owner = this.transform;
								playerCtrl.isCursed = false;
							}
						}
					}

					if(!gameCtrl.isDead[3]){
						playerCtrl = GameObject.FindGameObjectWithTag ("Player4").GetComponent<XXXCtrl> ();
						if(playerCtrl != null){
							if (playerCtrl.isCursed) {
								this.transform.position = new Vector3(playerCtrl.transform.position.x, playerCtrl.transform.position.y +2.2f, playerCtrl.transform.position.z);
								GameObject effect = Instantiate (Thunder, playerCtrl.transform.position, Quaternion.identity)as GameObject;
								effect.GetComponentInChildren<curseMirrorThunderColliderKnockOut>().owner = playerCtrl.transform;
								effect.GetComponent<DirectionEffectCtrl>().owner = this.transform;
								playerCtrl.isCursed = false;
							}
						}
					}
					Destroy(this.gameObject , destroyTime);
					curseReady = false;
					isCursed = true;
					time = 0.0f;
				}
			}
		}
	}
}
