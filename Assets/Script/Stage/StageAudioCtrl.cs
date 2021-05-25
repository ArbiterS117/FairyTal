using UnityEngine;
using System.Collections;

public class StageAudioCtrl : MonoBehaviour {

	//GameCtrl gameCtrl;
	AudioSource audioCtrl ;

	public AudioClip StageBGM;
	public AudioClip OutOfTimeBGM;

	public float playStartTime;
	public float playBackTime ;
	public float reTime;

	public float OutOfTimeBGMStartTime;
	public float OutOfTimeBGMBackTime ;
	public float OutOfTimeBGMReTime;

	bool isOutOfTimePlayed;

	void Awake()
	{
		//gameCtrl  = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl>();
		audioCtrl = GetComponent<AudioSource> ();

	}

	void Start () {

		audioCtrl.clip = StageBGM;
		audioCtrl.Play();
		audioCtrl.time = playStartTime;
		isOutOfTimePlayed = false;

	}
	

	void Update () {
	
		//重複撥放
		if (!isOutOfTimePlayed) {
			if (audioCtrl.time >= reTime) {
				audioCtrl.Play();
				audioCtrl.time = playBackTime;
			}
		}
		else{
			if (audioCtrl.time >= OutOfTimeBGMReTime) {
				audioCtrl.Play();
				audioCtrl.time = OutOfTimeBGMBackTime;
			}
		}

		//Out of Time
		/*
		if (gameCtrl.finishTime - gameCtrl.GameTime < gameCtrl.outOfTime && !isOutOfTimePlayed) {
			audioCtrl.Stop();
			audioCtrl.clip = OutOfTimeBGM;
			audioCtrl.Play();
			audioCtrl.time = OutOfTimeBGMStartTime;
			isOutOfTimePlayed = true;
		}
		*/
	

	}

}
