using UnityEngine;
using Fungus;

public class MapPointerCtrl : MonoBehaviour {

    public Flowchart FunctionFlowchart;

    MapDataCtrl DataCtrl;
	public float moveSpeed = 1.5f;

	//判斷參數
	public bool moveEnable = false;

	void Awake(){
		DataCtrl = GameObject.Find("MapGameCtrl").GetComponent<MapDataCtrl> ();
	}

	void Start () {
		moveEnable = true;
	}
	

	void Update () {

		//控制指標移動
		if (moveEnable) {
			pointerMove ();
			checkPointerBlock ();
		}

		if (DataCtrl.switchScene) {
			moveEnable = false;
		}

        //返回選角色
        if (Input.GetButtonDown("P1Dash") || Input.GetButtonDown("P2Dash") ||
               Input.GetButtonDown("P3Dash") || Input.GetButtonDown("P4Dash"))
        {
            FunctionFlowchart.SendFungusMessage("ChangeToCharacter");
        }

    }

	
	//=================自創==========================
	
	
	void pointerMove(){
		
		float moveX = Input.GetAxis ("P1MenuHorizontal") * moveSpeed + Input.GetAxis ("P2MenuHorizontal") * moveSpeed +
			          Input.GetAxis ("P3MenuHorizontal") * moveSpeed + Input.GetAxis ("P4MenuHorizontal") * moveSpeed + 
					  Input.GetAxis ("P1HorizontalKey")  * moveSpeed + Input.GetAxis ("P2HorizontalKey")  * moveSpeed +
					  Input.GetAxis ("P3HorizontalKey")  * moveSpeed + Input.GetAxis ("P4HorizontalKey")  * moveSpeed;

		float moveY = Input.GetAxis ("P1MenuVertical") * moveSpeed + Input.GetAxis ("P2MenuVertical") * moveSpeed +
			          Input.GetAxis ("P3MenuVertical") * moveSpeed + Input.GetAxis ("P4MenuVertical") * moveSpeed + 
					  Input.GetAxis ("P1VerticalKey")  * moveSpeed + Input.GetAxis ("P2VerticalKey")  * moveSpeed +
					  Input.GetAxis ("P3VerticalKey")  * moveSpeed + Input.GetAxis ("P4VerticalKey")  * moveSpeed;

		if(moveX >=  moveSpeed)moveX =  moveSpeed;
		if(moveY >=  moveSpeed)moveY =  moveSpeed;
		if(moveX <= -moveSpeed)moveX = -moveSpeed;
		if(moveY <= -moveSpeed)moveY = -moveSpeed;
		
		this.transform.position += new Vector3 (moveX, moveY, this.transform.position.z);
	}

	void checkPointerBlock(){
		if (this.transform.localPosition.y >= 4.9f) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, 4.9f, this.transform.localPosition.z);
		}
		if (this.transform.localPosition.x >= 9.1f) {
			this.transform.localPosition = new Vector3(9.1f, this.transform.localPosition.y, this.transform.localPosition.z);
		}
		if (this.transform.localPosition.y <= -4.9f) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, -4.9f, this.transform.localPosition.z);
		}
		if (this.transform.localPosition.x <= -9.1f) {
			this.transform.localPosition = new Vector3(-9.1f, this.transform.localPosition.y, this.transform.localPosition.z);
		}
	}


}
