using System.Collections;
using UnityEngine;

public class SupCamera : MonoBehaviour
{
    public TRyCamera MainCamera = null;
    public Camera cameraObj;

    public bool LandScapeMode = false; // 只移動背景

    public virtual void Awake()
    {
        cameraObj = GetComponent<Camera>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TRyCamera>();
    }

    void Start()
    {
        cameraObj.orthographic = MainCamera.isOrthographic;
        
    }


    public void FixedUpdate()
    {
        if (!LandScapeMode)
        {
            this.transform.position = MainCamera.transform.position;
            cameraObj.orthographic = MainCamera.isOrthographic;
        }

        else this.transform.position = new Vector3(this.transform.position.x, MainCamera.transform.position.y / 2.0f, this.transform.position.z);

    }
    public void Update()
    {
        if (!LandScapeMode) this.transform.position = MainCamera.transform.position;
    }
}
