using UnityEngine;
using System.Collections;

public class ReceiveEventCollider : MonoBehaviour {

	//GameCtrl gameCtrl;
	XXXCtrl playerCtrl;

	//傳染標記
	public bool  canPass      = false;
	public float passColdTime = 0.5f;

	float passCtime = 0.0f;


	void Awake(){
		//gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();

	}


    void Update()
    {
        if (!canPass && playerCtrl.isCursed)
        {
            passCtime += Time.deltaTime;
            if (passCtime >= passColdTime)
            {
                canPass = true;
                passCtime = 0.0f;
            }
        }

    }
	void OnTriggerEnter2D(Collider2D other) {

            if (playerCtrl.isCursed && canPass && !playerCtrl.isDead) {
                if (other.tag == "PlayerECollider") {
                    XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl>();
                    if (playerCtrl.isFront == enemyCtrl.isFront) {
                        enemyCtrl.isCursed = true;
                        playerCtrl.isCursed = false;
                        canPass = false;
                    }
                }
            }

        }
        
}
