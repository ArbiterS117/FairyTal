using UnityEngine;
using System.Collections;

public class StageAutoMoveCamera : TRyCamera {
		
	//控制變數

	public int level;

	public float limitCameraMoveDis = 1.0f;

	//float AutoMoveStopZoomCTime = 0.0f;

    //相機資訊
    [System.Serializable]
    public struct CameraImformation
    {
        public Transform StopPos;
        public float ElapsedTime;
        public float OrthographicSize;
        public float ZoomSpeed;

        public  bool StopType;
        public float StopCameraMoveLimitX;
        public float StopCameraMoveLimitY;
        public float MaxOrthographicSize;
        public float MinimumOrthographicSize;

        public  bool SetAnimTriggerInEnd;
        public Animator animator;
        public string triggerName;
    }
    public CameraImformation[] cameraImformations;

    Vector3 startPos;
    float currentTime = 0.0f;

  
    public override void Start()
    {
        base.Start();       
        startPos = transform.position;
    }

	public override void FixedUpdate(){
        float _deltaTime = Time.fixedDeltaTime;

        //偵測是否停下
       
        if (currentTime <= cameraImformations[level].ElapsedTime)
        {
            if (!cameraImformations[level].StopType)
            {
                float t = currentTime / cameraImformations[level].ElapsedTime;
                transform.position = Vector3.Lerp(startPos, cameraImformations[level].StopPos.position, t);

                cameraObj.orthographicSize = Mathf.Lerp(cameraObj.orthographicSize, cameraImformations[level].OrthographicSize, _deltaTime * cameraImformations[level].ZoomSpeed);
                if (!isOrthographic) transform.position = new Vector3(transform.position.x, transform.position.y, cameraObj.orthographicSize * -1.7f + 1.0f);
            }
            currentTime += _deltaTime;
        }
        else
        {
            if (cameraImformations[level].SetAnimTriggerInEnd)
            {
                cameraImformations[level].animator.SetTrigger(cameraImformations[level].triggerName);
            }
            currentTime = 0.0f;
            level ++;
            if (level >= cameraImformations.Length) level = 0;
            startPos = transform.position;
        }

        //==================停下點======================
        //玩家物件

        if (cameraImformations[level].StopType)
        {
            for (int i = 0; i < 4; i++)
            {
                if (gameCtrl.isDead[i] && !addPlayerSwitch[i])
                {
                    targets.Remove(TempPlayerTransform[i]);
                    addPlayerSwitch[i] = true;
                }

                if (!gameCtrl.isDead[i] && addPlayerSwitch[i])
                {
                    if (gameCtrl.player[i] != null)
                    {
                        targets.Add(gameCtrl.player[i].transform);
                        TempPlayerTransform[i] = gameCtrl.player[i].transform;
                        addPlayerSwitch[i] = false;
                    }
                }
            }

            if (targets.Count > 0)
            {
                zoomOutSpeed = cameraImformations[level].ZoomSpeed;
                zoomInSpeed = cameraImformations[level].ZoomSpeed;
                maxOrthographicSize = cameraImformations[level].MaxOrthographicSize;
                minimumOrthographicSize = cameraImformations[level].MinimumOrthographicSize;

                Rect boundingBox = CalculateTargetsBoundingBox();

                float posX = Mathf.SmoothDamp(transform.position.x, CalculateCameraPosition(boundingBox).x, ref velocity.x, smoothTimeX);
                float posY = Mathf.SmoothDamp(transform.position.y, CalculateCameraPosition(boundingBox).y, ref velocity.y, smoothTimeY);
                transform.position = new Vector3(posX, posY, CalculateCameraPosition(boundingBox).z);

                cameraObj.orthographicSize = CalculateOrthographicSize(boundingBox);
                if (!isOrthographic) transform.position = new Vector3(posX, posY, cameraObj.orthographicSize * -1.7f + 1.0f);
            }

        }

    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (isMainCamera)
        {
            //相機晃動功能
            if (shakeTime >= 0)
            {
                ShakePos = Random.insideUnitCircle * shakePower;
                transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
                shakeTime -= Time.deltaTime;
            }
        }
        else
        {
            if (MainCamera.shakeTime >= 0)
            {
                ShakePos = MainCamera.ShakePos;
                transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
            }
        }

    }

    public override Vector3 CalculateCameraPosition(Rect boundingBox)
	{
		Vector2 boundingBoxCenter = boundingBox.center;

		if (boundingBoxCenter.x >= cameraImformations[level].StopPos.position.x + cameraImformations[level].StopCameraMoveLimitX)
			boundingBoxCenter.x = cameraImformations[level].StopPos.position.x + cameraImformations[level].StopCameraMoveLimitX;
		else if(boundingBoxCenter.x < cameraImformations[level].StopPos.position.x - cameraImformations[level].StopCameraMoveLimitX)
			boundingBoxCenter.x = cameraImformations[level].StopPos.position.x - cameraImformations[level].StopCameraMoveLimitX;

        if (boundingBoxCenter.y >= cameraImformations[level].StopPos.position.y + cameraImformations[level].StopCameraMoveLimitY)
            boundingBoxCenter.y = cameraImformations[level].StopPos.position.y + cameraImformations[level].StopCameraMoveLimitY;
        else if (boundingBoxCenter.y < cameraImformations[level].StopPos.position.y - cameraImformations[level].StopCameraMoveLimitY)
            boundingBoxCenter.y = cameraImformations[level].StopPos.position.y - cameraImformations[level].StopCameraMoveLimitY;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y + CameraMoveUPDefault, GetComponent<Camera>().transform.position.z);
	}
	

   
}
