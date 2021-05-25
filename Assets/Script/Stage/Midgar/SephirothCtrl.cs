using UnityEngine;
using System.Collections;

public class SephirothCtrl : MonoBehaviour {

	GameCtrl gameCtrl;
	Animator anim;

	public float AttackColdTime;
	float AttackColdCTime = 0.0f;

	int AttackNum = 0;
	public int MaxAttackNum;
	public int MinAttackNum;

	public bool isAttacking = false;

	void Awake(){
		anim = GetComponent<Animator> ();
	}

	void Start () {
		
	}
	
	void Update () {
		if (!isAttacking) {
			anim.SetBool("isAttacking",false);
			AttackColdCTime += Time.deltaTime;
			if(AttackColdCTime >= AttackColdTime){
				isAttacking = true;
				AttackNum = Random.Range(MinAttackNum,MaxAttackNum);
				AttackColdCTime = 0.0f;
			}
		}

		else{
			anim.SetBool("isAttacking",true);
		}

	}


	//==================動畫用程式=====================

	public void ASlash(){
		AttackNum -= 1;
		if (AttackNum >= 0) {
			int a;
			a = Random.Range(0,4);
			     if(a < 1)anim.Play("vertical Slash");
			else if(a < 2)anim.Play("vertical Slash L");
			else if(a < 3)anim.Play("vertical Slash R");
			else          anim.Play("horizontal Slash");
		}
		else{
			anim.Play("vanish");
			isAttacking = false;
		}
	}

}
