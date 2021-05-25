using UnityEngine;
using System.Collections;

public class AppleMineCtrl : DirectionEffectCtrl {


	Animator animator;

    //控制變數
    public float AppleSpeedX;
	public float FlyForce;

	public float rollTime;
	float rollCTime = 0.0f;
	public float explodeReadyTime;
	float explodeReadyCTime = 0.0f;

    float spriteBlinkCTime = 0.0f;
    SpriteRenderer appleSprite = null;
    float blinkTime = 0.08f;

	//狀態變數
	public bool isHit = false;
	bool isExploded = false;

    [System.NonSerialized]public float AnimAppleRollSpeed = 1.0f;

    public override void Awake() {
        base.Awake();
		animator = GetComponent<Animator>();
    }


    public override void Start () {
        base.Start();
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, FlyForce));
        appleSprite = GetComponentInChildren<SpriteRenderer>();

    }

	public override void FixedUpdate(){
		if (!isHit) {
            speedX = AppleSpeedX * dir;
		}
		else{
            speedX = 0.0f;
            //animator.SetBool("isHit",true);
            
		}

        base.FixedUpdate();

        if (isExploded) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0,0);
		}


    }

	void Update(){
        float _deltaTime = Time.deltaTime;

		if (!isHit) {
            animator.speed = AnimAppleRollSpeed;
            rollCTime += _deltaTime;
			if(rollCTime >= rollTime){
				isHit = true;
				//animator.SetBool("isHit",true);
			}

        }

		else{

            if (!isExploded)
            {
                //animator.SetBool("isHit",true);
                explodeReadyCTime += _deltaTime;
                animator.speed = 0.0f;

                if (explodeReadyCTime >= explodeReadyTime)
                {
                    appleSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    animator.SetBool("explode", true);
                    isExploded = true;
                    animator.speed = 1.0f;
                }
              

            }
        }

    }

    private void LateUpdate() // 在動畫更新之後更新 - 可控制被動畫重設的屬性
    {
        if (isHit && !isExploded)
        {
            spriteBlinkCTime += Time.deltaTime;
            if (spriteBlinkCTime < blinkTime)
            {
                float t = 1.0f - ((spriteBlinkCTime / blinkTime) * 0.4f);
                appleSprite.color = new Color(t, t, t, 1.0f);
            }
            else if (spriteBlinkCTime >= blinkTime && spriteBlinkCTime < blinkTime * 2.0f)
            {
                float t = 0.6f + (((spriteBlinkCTime / blinkTime) - 1.0f) * 0.4f);
                if (t > 1.0f) t = 1.0f;
                appleSprite.color = new Color(t, t, t, 1.0f);
            }
            else if (spriteBlinkCTime >= blinkTime * 2.0f)
            {
                spriteBlinkCTime = 0.0f;
            }

        }

    }
    //================FUNCTION====================


    //================動畫用程式===================
    public void AOpenisHit(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0,0);
		isHit = true;
		animator.SetBool("isHit",true);
	}

}
