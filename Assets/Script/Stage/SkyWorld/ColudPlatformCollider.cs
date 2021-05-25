using UnityEngine;
using System.Collections;

public class ColudPlatformCollider : MonoBehaviour {
   
    GameCtrl gameCtrl;
    public bool canDisappear = false;
	public float speedX;
    public float speedY = 0.0f;

    public float ReturnTime  = 3.0f;
	       float ReturnCTime = 0.0f;
	public float ReturnRate = 10.0f;
	        bool b  = false; //開關
            bool bY = false;
	       float TempReturnSpeed = 0.0f;
           float TempReturnSpeedY = 0.0f;

    public float CloudDisappearTime;
		   float CloudDisappearCTime = 0.0f;

    //慢慢退色專用
    public float fadeInTime = 0.5f;
    float fadeInCTime = 0.0f;

    public bool isFadeIn = false;
    public float alpha = 0.85f;
    public bool[] isInCloud = new bool[4];

    SpriteRenderer CloudSprite;

	bool inTrigger = false;

	void Awake(){
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        CloudSprite = GetComponentInChildren<SpriteRenderer>();
		TempReturnSpeed = speedX;
        TempReturnSpeedY = speedY;
        b = true;
        bY = true;
		speedX *= -1.0f;
        speedY *= -1.0f;
	}

	void FixedUpdate(){
        //GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
        transform.Translate(speedX*0.02f, speedY*0.02f,0.0f);
    }

	void Update(){
        float _deltaTime = Time.deltaTime;
        //來回移動
        ReturnCTime += _deltaTime;

        if (ReturnCTime >= ReturnTime) {
			TempReturnSpeed = speedX * -1.0f;
            TempReturnSpeedY = speedY * -1.0f;
            b = true;
            bY = true;
			ReturnCTime = 0.0f;
		}
		if (b) {
			ReturnSpeed();
		}
        if (bY)
        {
            ReturnSpeedY();
        }

        //偵測玩家是否採在雲上
        if (canDisappear)
        {
            if (inTrigger)
            {
                CloudDisappearCTime += _deltaTime;

                CloudSprite.color = new Color(1, 1, 1, (CloudDisappearTime - CloudDisappearCTime) / CloudDisappearTime + 0.25f);
                if (CloudDisappearCTime >= CloudDisappearTime)
                {
                    disappear();
                }
            }
        }
        else // 不能消失的雲 當玩家踩上 會淡化圖片
        {
            if (isInCloud[0] || isInCloud[1] || isInCloud[2] || isInCloud[3])
            {
                if (!isFadeIn)
                {
                    fadeInCTime += Time.deltaTime;
                    CloudSprite.color = new Color(1, 1, 1, 1 - ((1 - alpha) * (fadeInCTime / fadeInTime)));
                    if (fadeInCTime >= fadeInTime)
                    {
                        isFadeIn = true;
                        fadeInCTime = 0.0f;
                    }
                }
                else
                {
                    CloudSprite.color = new Color(1, 1, 1, alpha);
                }
            }
            else
            {
                fadeInCTime = 0.0f;
                isFadeIn = false;
                CloudSprite.color = new Color(1, 1, 1, 1);
            }

            for (int i = 0; i <= 3; i++)
            {
                if (isInCloud[i] && gameCtrl.isDead[i])
                {
                    isInCloud[i] = false;
                }
            }
        }

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("BottomPenetrateDetect") && other.GetComponentInParent<Rigidbody2D>().velocity.y <= 0.0f && other.transform.parent.CompareTag("Player")){
			inTrigger = true;
            if (!canDisappear) {
                isInCloud[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = true;
            }
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BottomPenetrateDetect") && other.transform.parent.CompareTag("Player"))
        {
            isInCloud[other.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = false;
        }
    }

    //-----自創-------

    void disappear(){
		Destroy (transform.gameObject);
	}

	void ReturnSpeed(){

		speedX += TempReturnSpeed / ReturnRate;
		if (Mathf.Abs(speedX) >= Mathf.Abs (TempReturnSpeed)) {
			speedX = TempReturnSpeed;
			b = false;
		}
	}
    void ReturnSpeedY()
    {

        speedY += TempReturnSpeedY / ReturnRate;
        if (Mathf.Abs(speedY) >= Mathf.Abs(TempReturnSpeedY))
        {
            speedY = TempReturnSpeedY;
            bY = false;
        }
    }


}
