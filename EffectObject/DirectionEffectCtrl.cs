using UnityEngine;
using System.Collections;

public class DirectionEffectCtrl : MonoBehaviour {

	[System.NonSerialized] public Transform owner;


    public Vector2 velocityMin = new Vector2(-30.0f, -30.0f);
    public Vector2 velocityMax = new Vector2(+30.0f, +30.0f);
    [System.NonSerialized] public bool toSetVelocityX = false;
    [System.NonSerialized] public float setVelocityX = 0.0f;
    [System.NonSerialized] public bool toSetMPVelocityY = false; // 移動平台專用
    [System.NonSerialized] public float setMPVelocityY = 0.0f; // 移動平台專用

    [System.NonSerialized] public float speedX = 0.0f;
    [System.NonSerialized] public float dir;

    AudioSource audioCtrl;
	public AudioClip objectSE;
	public float objectSEPitch;
	public AudioClip objectSE2;
	public float objectSE2Pitch;
	public AudioClip objectSE3;
	public float objectSE3Pitch;
	public AudioClip objectSE4;
	public float objectSE4Pitch;


    [System.NonSerialized] public bool isFront = true;

    [System.NonSerialized] public bool hasRigidBody = false;

    public virtual void Awake(){
		audioCtrl = GetComponent<AudioSource>();
        hasRigidBody = GetComponent<Rigidbody2D>();

    }

    public virtual void Start() {
		if (!owner) {
			return;
		}
        if (owner != null)
        {
            if (owner.lossyScale.x < 0.0f) {
                this.transform.localScale = new Vector3(transform.lossyScale.x * -1.0f, transform.lossyScale.y, transform.lossyScale.z);
                    }
            if (owner.lossyScale.x >= 0) dir = 1;
            else dir = -1;
        }
        speedX = 0.0f;

    }

    public virtual void FixedUpdate()
    {
        if (hasRigidBody)
        {
            //移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, GetComponent<Rigidbody2D>().velocity.y);

            //限制速度
            float vx = Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, velocityMin.x, velocityMax.x);
            float vy = Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, velocityMin.y, velocityMax.y);
            GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);

            //強制變更速度
            if (toSetVelocityX)
            {
                toSetVelocityX = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(setVelocityX, GetComponent<Rigidbody2D>().velocity.y);
            }

            if (toSetMPVelocityY)
            {
                toSetMPVelocityY = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, setMPVelocityY);
            }

        }

    }

    public void DstroyObj(){
		Destroy(this.gameObject);
	}

	public void playSEObjectSE(){
		if (audioCtrl) {
			audioCtrl.pitch = objectSEPitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(objectSE);
		}
	}

	public void playSEObjectSE2(){
		if (audioCtrl) {
			audioCtrl.pitch = objectSE2Pitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(objectSE2);
		}
	}

	public void playSEObjectSE3(){
		if (audioCtrl) {
			audioCtrl.pitch = objectSE3Pitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(objectSE3);
		}
	}


	public void playSEObjectSE4(){
		if (audioCtrl) {
			audioCtrl.pitch = objectSE4Pitch + Random.Range(-0.05f,0.05f) ;
			audioCtrl.PlayOneShot(objectSE4);
		}
	}

    public void ASetNextStepATKData(string ATKDataName)//動畫編輯器用程式 需搭配子物件的FireObjColliderKnockOut
    {
        GetComponentInChildren<FireObjColliderKnockOut>().ASetNextStepATKData(ATKDataName);
    }


}
