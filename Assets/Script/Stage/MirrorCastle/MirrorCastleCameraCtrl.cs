using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MirrorCastleCameraCtrl : TRyCamera {
	
	public StageObjectCtrlMirrorCastle stageObject;

    //===========自訂========================
    //public float maxFieldOfView = 70.0f;
    public float maxCameraDis = -4.0f;

    public bool[] addMirrorSwitch = new bool[4];
	public Transform[] TempMirrorTransform = new Transform[4];
	public bool[] addBehindMirrorSwitch = new bool[2];
	public Transform[] TempBehindMirrorTransform = new Transform[2];
	public bool addCurseMirrorSwitch = false;
	public Transform TempCurseMirrorTransform ;
	

	public override void Start(){
        base.Start();
        for (int i = 0; i < 4; i++)
        {
            addBehindMirrorSwitch[i] = true;
            if(i < 3) addMirrorSwitch[i] = true;
            if (i < 1) addCurseMirrorSwitch = false;
        }
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
					TempPlayerTransform [i] = gameCtrl.player [i].transform;
					addPlayerSwitch [i] = false;
				}
			}
		}
		
		
		//場景特定物件
		
		for (int i =0; i < 3; i++) {
			if (stageObject.Mirror [i] == null && !addMirrorSwitch [i]) {
				targets.Remove (TempMirrorTransform [i]);
				addMirrorSwitch [i] = true;
			}
			
			if (stageObject.Mirror [i] != null && addMirrorSwitch [i]) {
				targets.Add (stageObject.Mirror [i].transform);
				TempMirrorTransform [i] = stageObject.Mirror [i].transform;
				addMirrorSwitch [i] = false;
			}
		}
		
		for (int i =0; i < 4; i++) {
			if (stageObject.BehindMirror[i] == null && !addBehindMirrorSwitch [i]) {
				targets.Remove (TempBehindMirrorTransform [i]);
				addBehindMirrorSwitch [i] = true;
			}
			
			if (stageObject.BehindMirror [i] != null && addBehindMirrorSwitch [i]) {
				targets.Add (stageObject.BehindMirror [i].transform);
				TempBehindMirrorTransform [i] = stageObject.BehindMirror [i].transform;
				addBehindMirrorSwitch [i] = false;
			}
		}
		
		if (stageObject.CursedMirrorObj[0] == null && !addCurseMirrorSwitch) {
			targets.Remove (TempCurseMirrorTransform);
			addCurseMirrorSwitch = true;
		}
		
		if (stageObject.CursedMirrorObj[0] != null && addCurseMirrorSwitch) {
			targets.Add (stageObject.CursedMirrorObj[0].transform);
			TempCurseMirrorTransform = stageObject.CursedMirrorObj[0].transform;
			addCurseMirrorSwitch = false;
		}
		
		
		Rect boundingBox = CalculateTargetsBoundingBox();
		
		float posX = Mathf.SmoothDamp (transform.position.x, CalculateCameraPosition(boundingBox).x, ref velocity.x,smoothTimeX );
		float posY = Mathf.SmoothDamp (transform.position.y, CalculateCameraPosition(boundingBox).y, ref velocity.y,smoothTimeY );
		transform.position = new Vector3(posX, posY, CalculateCameraPosition(boundingBox).z);

        /*
		if (!isOrthographic) {
			if(CalculateOrthographicSize (boundingBox) * 8.8f + 9.0f >= maxFieldOfView)cameraObj.fieldOfView = maxFieldOfView;
			else{
				cameraObj.fieldOfView = CalculateOrthographicSize (boundingBox) * 8.8f + 9.0f;
			}
		}
		cameraObj.orthographicSize = CalculateOrthographicSize(boundingBox);
        */
        if (!isOrthographic)
        {
            if (CalculateOrthographicSize(boundingBox) * -1.7f + 1.0f >= maxCameraDis) transform.position =  new Vector3(posX, posY, maxCameraDis);
            else
            {
                transform.position = new Vector3(posX, posY, CalculateOrthographicSize(boundingBox) * -1.7f + 1.0f);
            }
        }
        cameraObj.orthographicSize = CalculateOrthographicSize(boundingBox);
    }
	
	
}
