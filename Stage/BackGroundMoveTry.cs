using UnityEngine;
using System.Collections;

public class BackGroundMoveTry : MonoBehaviour {

	public Transform cameraPos; 

	//左右視差效果
	public bool TypeRL;
	public float RLrate;
	

	//上下視差效果
	public bool TypeUD;
	public float UDrate;

	//自動移動
	public bool autoMove;
	public float speedX;

	//歸位
	public float bounderX;
	public float rePosX;

	Vector3 oriPos;
	Vector3 tempPos;


	void Awake(){
		cameraPos = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	void Start () {
		oriPos = this.transform.position;
		tempPos = oriPos;
	}


	void Update () {
	
		if (TypeRL) {
			this.transform.position = new Vector3 (oriPos.x + cameraPos.position.x * RLrate, oriPos.y, oriPos.z);
			tempPos = this.transform.position;
		}

		if (TypeUD) {
			this.transform.position = new Vector3 (tempPos.x, oriPos.y + cameraPos.position.y * UDrate,oriPos.z);
			tempPos = this.transform.position;
		}

		if (autoMove) {
			oriPos = new Vector3(oriPos.x + speedX * Time.deltaTime,oriPos.y,oriPos.z);
			if(this.transform.position.x > bounderX){
				oriPos = new Vector3(rePosX,oriPos.y,oriPos.z);
			}
		}

	}

}
