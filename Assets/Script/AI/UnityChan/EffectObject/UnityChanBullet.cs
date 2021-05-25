using UnityEngine;
using System.Collections;

public class UnityChanBullet : MonoBehaviour {

	[System.NonSerialized] public Transform owner;
	[System.NonSerialized] public Animator animator;
	

	//控制變數

	public float angle           = 0.0f;
	float bulletSpeedX    = 4.0f; 
	float lifeTime        = 2.0f;
	float lifeCTime              = 0.0f;
	
	
	//狀態變數
	public bool isHit = false;

	public float dir;
	
	void Awake(){
		animator = GetComponent<Animator>();
	}
	
	
	void Start() {
		if (!owner) {
			return;
		}
		if (owner != null) {
			this.transform.localScale = owner.localScale;
			if(owner.localScale.x == 1)dir = 1;
			else dir = -1;
		}

		SpriteRenderer sprite = transform.Find("effectSprite").GetComponent<SpriteRenderer>();
		sprite.transform.Rotate (new Vector3 (0,0,angle));

        bulletSpeedX = owner.GetComponent<UnityChanCtrl>().bulletSpeed;
        lifeTime = owner.GetComponent<UnityChanCtrl>().bulletLifeTime;
    }

	void FixedUpdate(){
		if (!isHit) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (bulletSpeedX * dir * Mathf.Cos(dir * angle * Mathf.Deg2Rad), bulletSpeedX * dir * Mathf.Sin(dir * angle * Mathf.Deg2Rad));
			
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			animator.SetBool ("Over", true);
		}
	}

	void Update() {

		lifeCTime += Time.deltaTime;
		if (lifeCTime >= lifeTime) {
			Destroy(this.gameObject);
		}

	}

}
