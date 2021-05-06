using UnityEngine;
using System.Collections;

public class BackGroundLineMoveEffect : MonoBehaviour {

	//Vector3 oriPos;
	Vector3 tempPos;

	public bool SideType;

	public float speed;
	public float CirTime = 2.0f;
	float CirCTime = 0.0f;

	public bool isUp = true;

	void Start () {
		tempPos = this.transform.position;
	}
	
	void Update () {

		if (!SideType) {
			if (isUp) {
				CirCTime += Time.deltaTime;
				transform.position = new Vector3 (this.transform.position.x, tempPos.y + Mathf.Sin ((CirCTime / CirTime * 0.5f) * Mathf.PI) * CirTime * speed, tempPos.z);
				if (CirCTime >= CirTime) {
					transform.position = new Vector3 (this.transform.position.x, tempPos.y + CirTime * speed, tempPos.z);
					isUp = false;
					CirCTime = 0.0f;
					tempPos = transform.position;
				}
			} else {
				CirCTime += Time.deltaTime;
				transform.position = new Vector3 (this.transform.position.x, tempPos.y + Mathf.Sin ((CirCTime / CirTime * 0.5f) * Mathf.PI) * CirTime * -speed, tempPos.z);
				if (CirCTime >= CirTime) {
					transform.position = new Vector3 (this.transform.position.x, tempPos.y + CirTime * -speed, tempPos.z);
					isUp = true;
					CirCTime = 0.0f;
					tempPos = transform.position;
				}
			}
		}

		else {
			if (isUp) {
				CirCTime += Time.deltaTime;
				transform.position = new Vector3 (tempPos.x + Mathf.Sin ((CirCTime / CirTime * 0.5f) * Mathf.PI) * CirTime * speed,this.transform.position.y,  tempPos.z);
				if (CirCTime >= CirTime) {
					transform.position = new Vector3 (tempPos.x + CirTime * speed, this.transform.position.y, tempPos.z);
					isUp = false;
					CirCTime = 0.0f;
					tempPos = transform.position;
				}
			} else {
				CirCTime += Time.deltaTime;
				transform.position = new Vector3 (tempPos.x + Mathf.Sin ((CirCTime / CirTime * 0.5f) * Mathf.PI) * CirTime * -speed, this.transform.position.y, tempPos.z);
				if (CirCTime >= CirTime) {
					transform.position = new Vector3 (tempPos.x + CirTime * -speed, this.transform.position.y, tempPos.z);
					isUp = true;
					CirCTime = 0.0f;
					tempPos = transform.position;
				}
			}
		}


	}


}
