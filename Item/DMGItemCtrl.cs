using UnityEngine;
using System.Collections;

public class DMGItemCtrl : MonoBehaviour {

	//控制變數
	public float StoneSpeedX;
	public float StoneFlyForce;
	public float StoneGravity;

	//狀態變數
	public bool isHit = false;
	
	float dir = 1;


	void Start () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, StoneFlyForce));
		GetComponent<Rigidbody2D> ().gravityScale = StoneGravity;
	}

	void FixedUpdate(){
		if (!isHit) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (StoneSpeedX * dir, GetComponent<Rigidbody2D>().velocity.y);
			
		}
		else{
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0,0);
			Destroy(this.gameObject);
		}
	}

	void Update () {
	
	}

}
