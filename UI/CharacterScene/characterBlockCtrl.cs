using UnityEngine;

public class characterBlockCtrl : MonoBehaviour {

	//CharacterSelectDataCtrl characterData;
	SpriteRenderer BlockSSprite;
	SpriteRenderer CharSSprite;

	AudioSource audioCtrl;

	public AudioClip SlideInSE;
	public float SlideInSEPitch;

	public int onTriggerNUM = 0;

	void Awake(){
		//characterData = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
		BlockSSprite = this.transform.parent.Find ("BlockSSprite").GetComponent<SpriteRenderer>();
		CharSSprite = this.transform.parent.Find ("CharSSprite").GetComponent<SpriteRenderer>();
		audioCtrl = GetComponent<AudioSource>();
	}

	void Start () {
		
	}
	

	void Update () {

		if (onTriggerNUM > 0) {
			BlockSSprite.enabled = true;
			CharSSprite.enabled = true;
		}
		else {
			BlockSSprite.enabled = false;
			CharSSprite.enabled = false;
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		onTriggerNUM += 1;

		audioCtrl.pitch = SlideInSEPitch;
		audioCtrl.PlayOneShot(SlideInSE);

	}

	void OnTriggerExit2D(Collider2D other) {
		onTriggerNUM -= 1;
	}

}
