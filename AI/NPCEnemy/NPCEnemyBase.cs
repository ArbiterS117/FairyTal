using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NPCEnemyBase : MonoBehaviour {

	//狀態參數
	[System.NonSerialized] public float speedX       = 0.0f;
	[System.NonSerialized] public float dir          = 1.0f;

	[System.NonSerialized] public bool  toSetVelocityX          = false;
	[System.NonSerialized] public float setVelocityX            = 0.0f;
	[System.NonSerialized] public bool  toSetVelocityY          = false;
	[System.NonSerialized] public float setVelocityY            = 0.0f;
	[System.NonSerialized] public bool  toSetUpForce = false;
	[System.NonSerialized] public float setUpForce   = 0.0f;

    Rigidbody2D _rigidBody = null;


	public float HP = 100.0f;

    public bool[] hittedPlayer = new bool[4];

    //攻擊碰撞器專用
    [System.Serializable]
    public struct ATKColliderData
    {
        public int ATK;
        public GameObject effectObject;
        public float knockOutTime;
        public float knockBackSpeedX;//擊飛速度 包含KnockBack KnockDamagedDown KnockOut
        public float hitForceY;
        public bool canPauseAnim;

        public bool sideType;
        public bool twoSide;

        public AudioClip hittedSE;
        public float hittedSEPitch;

        public float knockOutDecressSpeed;
        public float knockOutGravity;
    }
    protected ATKColliderData ATKData;//腳色目前持有的攻擊的資訊
    public Dictionary<string, ATKColliderData> AtkDataDic = new Dictionary<string, ATKColliderData>();

    protected virtual void Awake(){
        if (GetComponent<Rigidbody2D>() != null) _rigidBody = GetComponentInParent<Rigidbody2D>();
        else _rigidBody = null;

    }

	protected virtual void Start () {
	
	}
	
	protected virtual void FixedUpdate(){
        if (_rigidBody != null)
        {
            //移動計算
            _rigidBody.velocity = new Vector2(speedX, _rigidBody.velocity.y);

            //跳躍計算
            if (toSetUpForce)
            {
                _rigidBody.velocity = new Vector2(0.0f, 0.0f);
                _rigidBody.AddForce(new Vector2(0.0f, setUpForce));
                toSetUpForce = false;
            }

            //強制變更速度
            if (toSetVelocityX)
            {
                toSetVelocityX = false;
                _rigidBody.velocity = new Vector2(setVelocityX, _rigidBody.velocity.y);
            }
            if (toSetVelocityY)
            {
                toSetVelocityY = false;
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, setVelocityY);
            }
        }
	}

	protected virtual void Update () {

	}

    public void actionTakeDMG(int DMG)
    {
        HP -= DMG;
        //if(!unityCtrl)閃白光被打中效果
    }

    public void reDetectDMG()
    {
        hittedPlayer[0] = false;
        hittedPlayer[1] = false;
        hittedPlayer[2] = false;
        hittedPlayer[3] = false;
    }

    //====設置攻擊資訊==============
    public virtual void setATKData(string s)
    {
        ATKData = AtkDataDic[s];
    }

    public ATKColliderData getATKData()
    {
        return ATKData;
    }

    public ATKColliderData getATKDataInDic(string s)
    {
        return AtkDataDic[s];
    }

}
