using UnityEngine;
using System.Collections;

public class BlockSpriteScale : MonoBehaviour {

    public Vector2 scaleValue = new Vector2(1.1f,1.1f); 

	void OnTriggerEnter2D(Collider2D other){
		transform.parent.Find ("Sprite").transform.localScale = scaleValue;
	}

	void OnTriggerExit2D(Collider2D other){
		transform.parent.Find ("Sprite").transform.localScale = scaleValue;
	}

}
