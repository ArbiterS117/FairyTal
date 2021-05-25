using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healBarHP : MonoBehaviour {


	public float healthBarLen = 1.0f;
	float preHealthBarLen = 1.0f;

	public float rebornIncreaseTime = 0.5f;
	float riCtime = 0.0f;

	bool rebornSwitch = false;

	public float playerHP = 50.0f;
	public Color OriColor;
	public Color DangerColor;
	public float TrasformTime;
	float TrasformCTime = 0.0f;

	void Start () {
		OriColor = GetComponent<Image> ().color;
	}

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

		//瀕血時閃爍
		if(playerHP <= 20.0f){
			colorTransform();
		}
		else{
			GetComponent<Image> ().color = OriColor;
		}


	}

	//======自訂========
	void colorTransform(){
		TrasformCTime += Time.deltaTime;
		if (TrasformCTime <= TrasformTime) {
			GetComponent<Image> ().color = new Color (OriColor.r + ((DangerColor.r - OriColor.r) * TrasformCTime / TrasformTime),
			                                          OriColor.g + ((DangerColor.g - OriColor.g) * TrasformCTime / TrasformTime),
			                                          OriColor.b + ((DangerColor.b - OriColor.r) * TrasformCTime / TrasformTime),
			                                          DangerColor.a);
		} 
		else if (TrasformCTime <= (TrasformTime * 2)) {
			GetComponent<Image> ().color = new Color (DangerColor.r + ((OriColor.r - DangerColor.r) * (TrasformCTime - TrasformTime) / TrasformTime),
			                                          DangerColor.g + ((OriColor.g - DangerColor.g) * (TrasformCTime - TrasformTime) / TrasformTime),
			                                          DangerColor.b + ((OriColor.b - DangerColor.r) * (TrasformCTime - TrasformTime) / TrasformTime),
			                                          OriColor.a);
		}
		else{
			TrasformCTime = 0.0f;
		}
	}
	
}
