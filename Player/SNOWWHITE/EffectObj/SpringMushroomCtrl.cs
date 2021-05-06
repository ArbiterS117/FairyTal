using UnityEngine;
using System.Collections;

public class SpringMushroomCtrl : DirectionEffectCtrl {

	Animator animator;
	public SpriteRenderer sprite;

	
	public float existTime;
	float existCTime = 0.0f;
	public float fadeTime;

	//快消失時閃爍
	float a = 0.4f;
	float fadeColdTime = 0.07f;
	float fadeColdCTime = 0;

	//anim
	[System.NonSerialized] public bool Stand = false;

    public override void Awake () {
        base.Awake();
		animator = GetComponent<Animator>();
	}

	public override void FixedUpdate(){
        base.FixedUpdate();
	}
	

	void Update () {

		existCTime += Time.deltaTime;

		if (existCTime >= existTime) {
			Destroy(this.gameObject);
		}

		if (existTime - existCTime <= fadeTime) {
			fadeColdCTime += Time.deltaTime;
			if(fadeColdCTime < fadeColdTime)sprite.color = new Color (1,1,1,a);
			if(fadeColdCTime > fadeColdTime)sprite.color = new Color (1,1,1,1);
			if(fadeColdCTime > fadeColdTime*2) fadeColdCTime =0;
		}


	}

	//====================anim=======================

	public void startStand(){
		if (Stand == false) {
			Stand = true;
			animator.SetTrigger ("stand");
		}
	}

	public void endStand(){
		Stand = false;
	}

}
