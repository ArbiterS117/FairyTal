using UnityEngine;
using System.Collections;

public class HairWindCtrl : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	
	//控制變數
	public float WindSpeedX;
	public float WindFlyForce;
	
	float dir;

	void Awake() {

	}
	
	void Start () {
		if (!owner) {
			return;
		}
		if (owner != null) {
			this.transform.localScale = owner.lossyScale;
			if(owner.lossyScale.x >= 0)dir = 1;
			else dir = -1;
		}
		
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, WindFlyForce));
		GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
	}
	
	void FixedUpdate(){

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (WindSpeedX * dir, GetComponent<Rigidbody2D> ().velocity.y);

	}
	
}
