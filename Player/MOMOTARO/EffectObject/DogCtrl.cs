using UnityEngine;
using System.Collections;

public class DogCtrl : MonoBehaviour {

	[System.NonSerialized] public Transform owner;

	//控制變數
	public float DogSpeedX;
	public float DogFlyForce;
	public float DogGravity;

	float dir;

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
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, DogFlyForce));
		GetComponent<Rigidbody2D> ().gravityScale = DogGravity;

	}
	
	void FixedUpdate(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (DogSpeedX * dir, GetComponent<Rigidbody2D>().velocity.y);
	}


}
