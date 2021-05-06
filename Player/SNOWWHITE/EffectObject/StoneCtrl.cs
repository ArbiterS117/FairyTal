using UnityEngine;
using System.Collections;

public class StoneCtrl : MonoBehaviour {

	Animator animator;

	[System.NonSerialized] public Transform owner;

    //控制變數
    public float StoneSpeedX;
	public float StoneFlyForce;
	public float StoneGravity;
	

	//狀態變數
	public bool isHit = false;

	float dir;



	void Awake() {
		animator = GetComponent<Animator>();
	}

	void Start () {
		if (!owner) {
			return;
		}
		if (owner != null) {
			this.transform.localScale = owner.lossyScale;
			if(owner.lossyScale.x > 0)dir = 1;
			else dir = -1;
		}

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
			animator.SetBool("isHit",true);
		}
	}

	//=======================動畫用程式==============================

	public void AOpenisHit(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0,0);
		isHit = true;
		animator.SetBool("isHit",true);
	}



}
