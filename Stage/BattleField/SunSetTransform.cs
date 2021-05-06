using UnityEngine;
using System.Collections;

public class SunSetTransform : MonoBehaviour {

    public float SunSetTransformTime;
	float transformCTime;

	
	void Update () {

		transformCTime += Time.deltaTime;

		if(transformCTime < SunSetTransformTime)
        {
			this.transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,transformCTime/ SunSetTransformTime);
		}
		else{
			this.transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
		}
	}
	

}
