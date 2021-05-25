using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TRyCamera : MonoBehaviour {

	protected GameCtrl gameCtrl;

	//===========自訂========================

	public bool isOrthographic = true;

    public bool isMainCamera = true;

	public bool[] addPlayerSwitch = new bool[4];
	public Transform[] TempPlayerTransform = new Transform[4];

    public Transform[] TempPlayerDeathTransform = new Transform[4];
    public Transform[] PlayerDeathTransform = new Transform[4];
    public bool[] addPlayerDeathSwitch = new bool[4];

    public float CameraMoveUPDefault = 2.0f;

	public float smoothTimeX = 0.5f;
	public float smoothTimeY = 0.5f;
	public Vector2 velocity;

	
	public float disMaxX = -20.0f;
	public float disMinX = 20.0f;
	public float disMaxY = -20.0f;
	public float disMinY = 20.0f;

	public float OriCameraPosX;
	public float OriCameraPosY;

	public float limitCameraMoveDisX = 10.0f;
	public float limitCameraMoveDisY = 10.0f;
    public float TlimitCameraMoveDisX = 0.0f;

    //相機晃動功能
    public float shakeTime;
	public float shakePower;
	public float MaxShakePower;
	public float MaxShakeTime;
    [System.NonSerialized] public TRyCamera MainCamera = null;
    [System.NonSerialized] public Vector2 ShakePos;
    [System.NonSerialized] public float OriShakePower;


	//=================參考===================


	[SerializeField] 
	public List<Transform> targets = new List<Transform>();

	//[SerializeField] 
	//Transform[] targets;
	
	[SerializeField] 
	 public float boundingBoxPadding = 4.0f;
	
	[SerializeField]
	public float minimumOrthographicSize = 4.0f;
	public float maxOrthographicSize = 10.0f;
	
	[SerializeField]
	public float zoomOutSpeed = 2.0f;
    public float zoomInSpeed = 2.0f;
    float preOrthographicSize = 0.0f; //上一偵的相機尺寸


    public Camera cameraObj;
	
	public virtual void Awake () 
	{
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		cameraObj = GetComponent<Camera>();
		cameraObj.orthographic = isOrthographic;
        if(!isMainCamera) MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TRyCamera>();

    }

    public virtual void Start(){
		for (int i = 0; i < 4; i++) {
			addPlayerSwitch[i] = true;
            addPlayerDeathSwitch[i] = true;
        }
		OriCameraPosX = transform.position.x;
		OriCameraPosY = transform.position.y;
	}


    public virtual void FixedUpdate(){
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

            //死亡特效物件
            if (PlayerDeathTransform[i] == null && !addPlayerDeathSwitch[i])
            {
                targets.Remove(TempPlayerDeathTransform[i]);
                addPlayerDeathSwitch[i] = true;
            }

            if (PlayerDeathTransform[i] != null && addPlayerDeathSwitch[i])
            {
                TempPlayerDeathTransform[i] = PlayerDeathTransform[i];
                targets.Add(TempPlayerDeathTransform[i]);
                addPlayerDeathSwitch[i] = false;
            }

        }

        if (targets.Count > 0)
        {

            Rect boundingBox = CalculateTargetsBoundingBox();

            float posX = Mathf.SmoothDamp(transform.position.x, CalculateCameraPosition(boundingBox).x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, CalculateCameraPosition(boundingBox).y, ref velocity.y, smoothTimeY);
            transform.position = new Vector3(posX, posY, CalculateCameraPosition(boundingBox).z);
           
            cameraObj.orthographicSize = CalculateOrthographicSize(boundingBox);
            if (!isOrthographic) transform.position = new Vector3(posX, posY, cameraObj.orthographicSize * -1.7f + 1.0f);
        }
        preOrthographicSize = cameraObj.orthographicSize;
    }

    public virtual void Update(){
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

    public virtual void LateUpdate()
	{
		//Debug.Log (targets.Count);

	}
	
	/// <summary>
	/// Calculates a bounding box that contains all the targets.
	/// </summary>
	/// <returns>A Rect containing all the targets.</returns>
	public virtual Rect CalculateTargetsBoundingBox()
	{
		float minX = disMinX;
		float maxX = disMaxX;
		float minY = disMinY;
		float maxY = disMaxY;

		
		foreach (Transform target in targets) {
			if(target != null){
				Vector3 position = target.position;
				
				minX = Mathf.Min(minX, position.x);
				minY = Mathf.Min(minY, position.y);
				maxX = Mathf.Max(maxX, position.x);
				maxY = Mathf.Max(maxY, position.y);
			}
		}
		
		return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
	}
	
	/// <summary>
	/// Calculates a camera position given the a bounding box containing all the targets.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A Vector3 in the center of the bounding box.</returns>
	public virtual Vector3 CalculateCameraPosition(Rect boundingBox)
	{
		Vector2 boundingBoxCenter = boundingBox.center;


        float DNUM = 0;
        TlimitCameraMoveDisX = limitCameraMoveDisX;
        if (maxOrthographicSize != minimumOrthographicSize) TlimitCameraMoveDisX = Mathf.Lerp(DNUM , TlimitCameraMoveDisX, (maxOrthographicSize - cameraObj.orthographicSize) / (maxOrthographicSize - minimumOrthographicSize));


        if (boundingBoxCenter.x >= OriCameraPosX + TlimitCameraMoveDisX)
			boundingBoxCenter.x = OriCameraPosX + TlimitCameraMoveDisX;
		else if(boundingBoxCenter.x < OriCameraPosX - TlimitCameraMoveDisX)
			boundingBoxCenter.x = OriCameraPosX - TlimitCameraMoveDisX;

		if (boundingBoxCenter.y >= OriCameraPosY + limitCameraMoveDisY)
			boundingBoxCenter.y = OriCameraPosY + limitCameraMoveDisY;
		else if(boundingBoxCenter.y < OriCameraPosY - limitCameraMoveDisY)
			boundingBoxCenter.y = OriCameraPosY - limitCameraMoveDisY;

		return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y + CameraMoveUPDefault, GetComponent<Camera>().transform.position.z);
	}
	
	/// <summary>
	/// Calculates a new orthographic size for the camera based on the target bounding box.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A float for the orthographic size.</returns>
	public virtual float CalculateOrthographicSize(Rect boundingBox)
	{
		float orthographicSize = cameraObj.orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = cameraObj.WorldToViewportPoint(topRight);
		
		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = Mathf.Abs(boundingBox.width) / cameraObj.aspect / 1.5f;//(1.5f)表示X軸縮放容易度
		else
			orthographicSize = Mathf.Abs(boundingBox.height) / 1.5f;//(1.5f)表示Y軸縮放容易度

        float zoomSpeed = 0.0f;
        if (orthographicSize > preOrthographicSize) zoomSpeed = zoomOutSpeed;
        else zoomSpeed = zoomInSpeed;

        return Mathf.Clamp(Mathf.Lerp(cameraObj.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, maxOrthographicSize);
    }
    //Mathf.SmoothDamp(transform.position.x, CalculateCameraPosition(boundingBox).x, ref velocity.x, smoothTimeX);

	//==============相機晃動功能方法=================

	public virtual void ShakeCamera(float shakePwr, float shakeDur){
		if ((shakePwr > OriShakePower && shakeTime > 0.0f) || shakeTime <= 0.0f) {
			shakePower = shakePwr;
			shakeTime = shakeDur;
			if(shakePwr > MaxShakePower)shakePwr = MaxShakePower;
			if(shakeDur > MaxShakeTime)shakeTime = MaxShakeTime;

			OriShakePower = shakePwr;
		}
	}


}
