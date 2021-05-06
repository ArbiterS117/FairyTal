using UnityEngine;
using System.Collections;

public class healBarDamagedHP : MonoBehaviour {

	public float healthBarLen = 1.0f;
	float preHealthBarLen = 1.0f;

	float OriHealthBarLen = 1.0f;

	float coldTime = 1.0f;
	float Ctime = 0.0f;

	public float reduceTime = 0.5f;
	float rCtime = 0.0f;

	public float rebornIncreaseTime = 0.5f;
	float riCtime = 0.0f;

	bool damageSwitch = false;
	bool rebornSwitch = false;
	
	void Update () {
		//======普通少血=================
		if (healthBarLen != preHealthBarLen && preHealthBarLen != 0)damageSwitch = true;
		if (damageSwitch) {
			Ctime += Time.deltaTime;
			if(healthBarLen != preHealthBarLen)Ctime = 0.0f;
			if(Ctime >= coldTime){
				rCtime += Time.deltaTime;
				if (rCtime > reduceTime)
				{
					damageSwitch = false;
					Ctime = 0.0f;
					rCtime = 0.0f;
				}
				GetComponentInChildren<RectTransform> ().localScale = new Vector3 (OriHealthBarLen - ((OriHealthBarLen - healthBarLen)*rCtime) / reduceTime, 1,1);
				if(rCtime == 0)GetComponentInChildren<RectTransform> ().localScale = new Vector3 (healthBarLen, 1,1);


			}
		}

		//======重生血量效果================
		if (preHealthBarLen == 0 && healthBarLen == 1) {
			rebornSwitch = true;
		}
		if (rebornSwitch) {
			riCtime += Time.deltaTime;
			if (riCtime > rebornIncreaseTime)
			{
				rebornSwitch = false;
				riCtime = 0.0f;
			}
			GetComponentInChildren<RectTransform> ().localScale = new Vector3 (0 - ((0 - healthBarLen)*riCtime) / rebornIncreaseTime, 1,1);
			if(riCtime == 0)GetComponentInChildren<RectTransform> ().localScale = new Vector3 (healthBarLen, 1,1);
			
		}


		//===處理變化=========

		//最終確保
		if(!damageSwitch && !rebornSwitch)GetComponentInChildren<RectTransform> ().localScale = new Vector3 (healthBarLen, 1,1);

		if (!damageSwitch)OriHealthBarLen = healthBarLen;
		preHealthBarLen = healthBarLen;


	}
}
