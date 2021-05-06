using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneCtrl : MonoBehaviour {

	Animator animator;
	AudioSource audioCtrl;

	bool canPressStart = false;
	bool isPressStart  = false;

	public AudioClip StartSE;
	public float StartSEPitch;

	public float IdleTime = 30.0f;
	//float IdleCTime = 0.0f;

	void Awake(){
		animator = GetComponent<Animator>();
		audioCtrl = GetComponent<AudioSource>();
	}


	void Update () {
		
		if (canPressStart && !isPressStart) {
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown ("Start")){
				isPressStart = true;
				canPressStart = false;
				animator.SetTrigger ("End");
				audioCtrl.pitch = StartSEPitch ;
				audioCtrl.PlayOneShot(StartSE);
			}
		}

		//AutoPlay
        /*
		IdleCTime += Time.deltaTime;
		if (IdleCTime >= IdleTime) {
			canPressStart = false;
			animator.SetTrigger ("AutoPlay");
		}
        */
	}

	//====自創================

	public void PressStartAnimOver(){
		canPressStart = true;
	}

	public void EnterGame(){
        //Application.LoadLevel(1);
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);

	}

	public void EnterAutoPlay(){
		float f = Random.Range (0, 4);
		if(f <= 1.0f) SceneManager.LoadScene("Idle1", LoadSceneMode.Single);
		else if(f <= 2.0f) SceneManager.LoadScene("Idle2", LoadSceneMode.Single);
		else if(f <= 3.0f) SceneManager.LoadScene("Idle3", LoadSceneMode.Single);
		else SceneManager.LoadScene("Idle4", LoadSceneMode.Single);
	}

}
