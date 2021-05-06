using UnityEngine;

public class AutoMoveCamera : TRyCamera
{
    public Animator animator;
    bool isStop = false;
    int level = 0;

    float currentTime = 0.0f;

    //回歸動畫位置用
    bool BackToAnimPos = false;
    Vector3 startPos;
    Vector3 TempAnimPos;
    float backTime = 1.0f;
    float backCTime = 0.0f;

    //相機資訊
    [System.Serializable]
    public struct CameraImformation
    {
        public Transform StopPos;
        public float ElapsedTime;
        public float StopCameraMoveLimitX;
        public float StopCameraMoveLimitY;
        public float MaxOrthographicSize;
        public float MinimumOrthographicSize;

        

    }
    public CameraImformation[] cameraImformations;

    [System.Serializable]
    public struct AnimatioanImformation
    {
        public Animator animator;
        public string triggerName;
    }
    public AnimatioanImformation[] animatioanImformation;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponentInParent<Animator>();
    }

    public override void FixedUpdate()
    {

        //玩家物件
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

        //是否停下
        if (isStop)
        {

            float _deltaTime = Time.fixedDeltaTime;

            if (currentTime <= cameraImformations[level].ElapsedTime)
            {
                currentTime += _deltaTime;
                CaculateCamera();

            }
            else
            {
                currentTime = 0.0f;
                level++;
                if (level >= cameraImformations.Length) level = 0;
                isStop = false;
                startPos = transform.position;
                BackToAnimPos = true;
                //animator.enabled = true;
            }


        }

        //結束回歸動畫位置
        if (BackToAnimPos)
        {
            float _deltaTime = Time.fixedDeltaTime;
            if (backCTime <= backTime)
            {
                backCTime += _deltaTime;
                float t = backCTime / backTime;
                transform.position = Vector3.Lerp(startPos, TempAnimPos, t);
            }
            else
            {
                backCTime = 0.0f;
                BackToAnimPos = false;
                transform.position = TempAnimPos;
                animator.enabled = true;
            }
        }


    }

    public override void Update()
    {
        if (isMainCamera)
        {
            //相機晃動功能
            if (shakeTime >= 0)
            {
                ShakePos = Random.insideUnitCircle * shakePower;
                transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
                shakeTime -= Time.deltaTime;
            }
            else
            {
                if(!isStop)transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3 (0,0,0), 0.05f);
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
        else if (boundingBoxCenter.x < cameraImformations[level].StopPos.position.x - cameraImformations[level].StopCameraMoveLimitX)
            boundingBoxCenter.x = cameraImformations[level].StopPos.position.x - cameraImformations[level].StopCameraMoveLimitX;

        if (boundingBoxCenter.y >= cameraImformations[level].StopPos.position.y + cameraImformations[level].StopCameraMoveLimitY)
            boundingBoxCenter.y = cameraImformations[level].StopPos.position.y + cameraImformations[level].StopCameraMoveLimitY;
        else if (boundingBoxCenter.y < cameraImformations[level].StopPos.position.y - cameraImformations[level].StopCameraMoveLimitY)
            boundingBoxCenter.y = cameraImformations[level].StopPos.position.y - cameraImformations[level].StopCameraMoveLimitY;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y + CameraMoveUPDefault, GetComponent<Camera>().transform.position.z);
    }

    //=============相機動畫呼叫用===========================

    public void StopPosTrigger()
    {
        if (isStop) isStop = false;
        else isStop = true;
        animator.enabled = false ;
        TempAnimPos = cameraImformations[level].StopPos.position;
    }

    public void CallAnimation(int id)
    {
        animatioanImformation[id].animator.SetTrigger(animatioanImformation[id].triggerName);
    }

    //=============自訂===============================

    public void CaculateCamera()
    {

       
        if (targets.Count > 0)
        {
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
