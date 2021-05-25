using UnityEngine;
using System.Collections;

public class ItemNoteSprite : MonoBehaviour {

	float time = 0.0f;

	void Update () {
		time += Time.deltaTime;

		if (time <= 0.2f) {
			GetComponent<SpriteRenderer> ().enabled = true;
		} 
		else if (time <= 0.4f && time >= 0.2f) {
			GetComponent<SpriteRenderer> ().enabled = false;
		}
		else if (time > 0.4f) {
			time = 0.0f;
		}

	}
}
