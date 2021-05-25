using UnityEngine;
using System.Collections;

public class InvincibleSpriteCtrl : MonoBehaviour {

	XXXCtrl playerCtrl;
	public GameObject effectObjectblink;

	public float a = 0.4f;
	public float coldTime = 0.07f;
	float time = 0;


	float blinkCTime = 0.0f;

	void Awake() {
		playerCtrl = transform.parent.GetComponent<XXXCtrl>();
	}
	
	void Update () {
        float _deltaTime = Time.deltaTime;

        //=================無敵=============================
		if (playerCtrl.isInvincible || playerCtrl.isUntouchable) {
			time += _deltaTime;
			if(time < coldTime)GetComponent<SpriteRenderer>().color = new Color (1,1,1,a);
			if(time > coldTime)GetComponent<SpriteRenderer>().color = new Color (1,1,1,1);
			if(time > coldTime*2) time =0;
		}
		else if(!playerCtrl.isInvincible && !playerCtrl.isUntouchable){
			GetComponent<SpriteRenderer>().color = new Color (1,1,1,1);
			time =0.0f;
			blinkCTime = 0.0f;
		}

		if (playerCtrl.isUntouchable) {
			blinkCTime += _deltaTime;
			if (blinkCTime >= 0.2f) {
				Instantiate (effectObjectblink, new Vector3 (transform.parent.position.x + Random.Range (-1.0f, 1.0f), transform.parent.position.y + Random.Range (-1.0f, 1.0f), transform.parent.position.z), Quaternion.identity);
				blinkCTime = 0.0f;
			}
		}
	}
	
}
