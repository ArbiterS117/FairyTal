using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrazyGrandHouseCameraCtrl : TRyCamera {

	StageObjectCtrlCrazyGrandHouse stageCtrl;
	
	//===========自訂========================
	
	public bool addCrazyOldManSwitch = true;
	public Transform TempCrazyOldManTransform;

	public bool addGiftSwitch = true;
	public Transform TempGiftTransform;
	
	
	public override void Awake () 
	{
        base.Awake();
		stageCtrl = GameObject.FindGameObjectWithTag ("StageCtrl").GetComponent<StageObjectCtrlCrazyGrandHouse> ();
	}

    public override void Start(){
        base.Start();
		addCrazyOldManSwitch = true;
		addGiftSwitch = true;
	}

    public override void FixedUpdate(){
		//玩家物件
		for (int i =0; i < 4; i++) {
			if (gameCtrl.isDead [i] && !addPlayerSwitch [i]) {
				targets.Remove (TempPlayerTransform [i]);
				addPlayerSwitch [i] = true;
			}
			
			if (!gameCtrl.isDead [i] && addPlayerSwitch [i]) {
				if(gameCtrl.player[i] != null){
					targets.Add (gameCtrl.player [i].transform);
					TempPlayerTransform [i] = gameCtrl.player[i].transform;
					addPlayerSwitch [i] = false;
				}
			}
		}
		
		//場景特定物件
		
		//crazyOldMan
		if (!addCrazyOldManSwitch && !stageCtrl.CrazyOldManIsLive) {
			targets.Remove (TempCrazyOldManTransform);
			addCrazyOldManSwitch = true;
		}
		
		if (addCrazyOldManSwitch && stageCtrl.CrazyOldManIsLive && stageCtrl.CrazyOldMan != null) {
			addCrazyOldManSwitch = false;
			TempCrazyOldManTransform = stageCtrl.CrazyOldMan.transform;
			targets.Add (stageCtrl.CrazyOldMan.transform);
		}
		
		//探望禮物藍
		
		/*
		if (!addGiftSwitch && !stageCtrl.GiftIsLive) {
			targets.Remove (TempCrazyOldManTransform);
			addGiftSwitch = true;
		}
		
		if (addGiftSwitch && stageCtrl.GiftIsLive && stageCtrl.Gift != null) {
			addGiftSwitch = false;
			TempGiftTransform = stageCtrl.Gift.transform;
			targets.Add (stageCtrl.Gift.transform);
		}
		*/
		
		
		Rect boundingBox = CalculateTargetsBoundingBox();
		
		float posX = Mathf.SmoothDamp (transform.position.x, CalculateCameraPosition(boundingBox).x, ref velocity.x,smoothTimeX );
		float posY = Mathf.SmoothDamp (transform.position.y, CalculateCameraPosition(boundingBox).y, ref velocity.y,smoothTimeY );
		transform.position = new Vector3(posX, posY, CalculateCameraPosition(boundingBox).z);
		
		if (!isOrthographic) {
			cameraObj.fieldOfView = CalculateOrthographicSize (boundingBox) * 8.8f + 9.0f;
		}
		cameraObj.orthographicSize = CalculateOrthographicSize (boundingBox);

	}
	

}
