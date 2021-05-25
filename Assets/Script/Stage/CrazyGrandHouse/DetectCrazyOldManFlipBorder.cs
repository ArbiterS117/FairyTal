using UnityEngine;

public class DetectCrazyOldManFlipBorder : MonoBehaviour {

	CrazyOldMan crazyOldMan;

	void Awake(){
		crazyOldMan = GetComponentInParent<CrazyOldMan>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "CrazyOldManFlip") {
			crazyOldMan.actionFlip();
			crazyOldMan.GetMadFlipCTime = 0.0f;
		}

	}

}
