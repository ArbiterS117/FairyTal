using UnityEngine;
using System.Collections;

public class BorderCtrl : MonoBehaviour {

	Camera cameraObj;

	public bool  SideMode;
	public float MoveRate;
	public float smoothTimeX = 0.5f;
	public float smoothTimeY = 0.5f;
	public Vector2 velocity;
	float oriLocalPosX;
	float oriLocalPosY;

	void Awake(){
		cameraObj = GetComponentInParent<Camera>();
	}

	void Start(){
		oriLocalPosX = transform.localPosition.x;
		oriLocalPosY = transform.localPosition.y;
	}
	
	void Update () {

		float posX = Mathf.SmoothDamp (transform.localPosition.x, oriLocalPosX + MoveRate * (5.4f - cameraObj.orthographicSize), ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.localPosition.y, oriLocalPosY + MoveRate * (5.4f - cameraObj.orthographicSize), ref velocity.y, smoothTimeY);
		if(SideMode)transform.localPosition = new Vector3 (posX, transform.localPosition.y, transform.localPosition.z);
		else        transform.localPosition = new Vector3 (transform.localPosition.x, posY, transform.localPosition.z);

	}

}
