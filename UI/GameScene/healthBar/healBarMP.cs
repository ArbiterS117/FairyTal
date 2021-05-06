using UnityEngine;
using System.Collections;

public class healBarMP : MonoBehaviour {

	public float healthBarLen = 1.0f;
	float preHealthBarLen = 1.0f;
	
	public float rebornIncreaseTime = 0.5f;
	float riCtime = 0.0f;
	
	bool rebornSwitch = false;
	
	void Update () {
		GetComponentInChildren<RectTransform> ().localScale = new Vector3 (healthBarLen, 1,1);
		
		if (preHealthBarLen == 0 && healthBarLen == 1)rebornSwitch = true;
		if (rebornSwitch) {
			riCtime += Time.deltaTime;
			if (riCtime > rebornIncreaseTime)
			{
				rebornSwitch = false;
				riCtime = 0.0f;
			}
			GetComponentInChildren<RectTransform> ().localScale = new Vector3 (0 - ((0 - healthBarLen)*riCtime) / rebornIncreaseTime, 1,1);
			if(riCtime == 0 )GetComponentInChildren<RectTransform> ().localScale = new Vector3 (healthBarLen, 1,1);
		}
		
		preHealthBarLen = healthBarLen;
	}
}
