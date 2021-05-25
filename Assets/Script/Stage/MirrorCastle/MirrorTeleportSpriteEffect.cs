using UnityEngine;
using System.Collections;

public class MirrorTeleportSpriteEffect : MonoBehaviour {

	public float a = 0.4f;
	public float coldTime = 0.07f;

    [System.NonSerialized] public float lifeTime = 10.0f;
	float CTime = 0.0f;
	float time  = 0.0f;


	void Update () {
        float _deltaTime = Time.deltaTime;
        CTime += _deltaTime;
        if (lifeTime - 3 <= CTime) {
			time += _deltaTime;
			if(time < coldTime)GetComponent<SpriteRenderer>().color = new Color (1,1,1,a);
			if(time > coldTime)GetComponent<SpriteRenderer>().color = new Color (1,1,1,1);
			if(time > coldTime*2) time =0;
		}

	}
}
