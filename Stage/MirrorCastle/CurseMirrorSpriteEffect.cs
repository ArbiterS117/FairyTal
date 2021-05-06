using UnityEngine;
using System.Collections;

public class CurseMirrorSpriteEffect : MonoBehaviour {

	CurseMirror Mirror;
	
	public float a = 0.4f;
	public float coldTime = 0.07f;

	float time  = 0.0f;
	
	void Awake() {
		Mirror = transform.parent.GetComponent<CurseMirror>();
	}
	
	
	void Update () {
		if (Mirror.curseReady) {
			time += Time.deltaTime;
			if (time < coldTime)
				GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, a);
			if (time > coldTime)
				GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.78f);
			if (time > coldTime * 2)
				time = 0;
		}
		else {
			GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.78f);
		}
	}
}
