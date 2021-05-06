using UnityEngine;
using System.Collections;

public class BaseCharCtrl : MonoBehaviour {
    //外部變數
    public int PlayerNUM = 1;

    public float   speedX      = 0.0f;
    public float   speedY      = 0.0f;
	public Vector2 velocityMin = new Vector2 (-100.0f, -100.0f);
	public Vector2 velocityMax = new Vector2 (+100.0f, +50.0f);

	public float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	public LayerMask whatIsPenetrateGround;

	[System.NonSerialized] public bool  activeSts               = false;
	[System.NonSerialized] public bool  grounded                = false;
	[System.NonSerialized] public bool 	groundedPrev            = false;
	[System.NonSerialized] public bool  groundedDetect          = false;
	[System.NonSerialized] public bool  PenetrategroundedDetect = false;
	[System.NonSerialized] public bool  toSetVelocityX          = false;
	[System.NonSerialized] public bool  toSetVelocityY          = false;
	[System.NonSerialized] public bool  toSetUpForce            = false;
    [System.NonSerialized] public bool  toSetSideForce          = false;
	[System.NonSerialized] public float initGravity             = 2.0f;
	[System.NonSerialized] public float dir                     = 1.0f;
	[System.NonSerialized] public float setVelocityX            = 0.0f;
    [System.NonSerialized] public float setVelocityY            = 0.0f;
    [System.NonSerialized] public float setUpForce              = 0.0f;
    [System.NonSerialized] public float setSideForce            = 0.0f;

    //關卡使用
    [System.NonSerialized] public bool  isOnMovingPlatform = false; // 移動平台專用
    [System.NonSerialized] public bool  toSetMPVelocityY   = false; // 移動平台專用
    [System.NonSerialized] public float setMPVelocityY     =  0.0f; // 移動平台專用
    [System.NonSerialized] public bool  isFront            =  true; // 魔鏡城專用 判斷前後排地板
    [System.NonSerialized] public bool  isCursed           = false; // 魔鏡城專用 判斷是否被詛咒中

    [System.NonSerialized] public bool  isCatchingGift     = false; // 瘋狂奶奶家專用 判斷是否撿起蘋果

	//快取
	[System.NonSerialized] public Animator animator;
	protected Transform groundCheckL;
	protected Transform groundCheckC;
	protected Transform groundCheckR;
    protected Rigidbody2D _rigidbody = null;

	//內部變數
	protected GameCtrl gameCtrl;
	
	protected float speedAddPower = 0.0f;
	protected float gravityScale  = 7.0f;
	

	protected void BaseAwake() {
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        groundCheckL 		= transform.Find("GroundCheckL");
		groundCheckC 		= transform.Find("GroundCheckC");
		groundCheckR 		= transform.Find("GroundCheckR");
		dir = (transform.lossyScale.x > 0.0f) ? 1.0f : -1.0f;
		activeSts = true;
		initGravity = GetComponent<Rigidbody2D>().gravityScale;
	}


	protected void BaseFixedUpdate() {
       
        //判斷是否在地面
        groundedPrev = grounded;
		grounded = false;

        if (isOnMovingPlatform == false)
        {
            groundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsGround) ||
                         Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsGround) ||
                         Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsGround))
                        && _rigidbody.velocity.y <= 0.1); //0.1是為了防止稍微卡牆有往上的一點小力就判斷為沒接觸地面

            PenetrategroundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsPenetrateGround))
                        && _rigidbody.velocity.y <= 0.1);
        }
        if (isOnMovingPlatform)
        {
            groundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsGround) ||
                     Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsGround) ||
                     Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsGround)));

            PenetrategroundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsPenetrateGround)));

        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 2f, whatIsGround); //斜坡
        if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
        {
            
            groundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsGround) ||
                    Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsGround) ||
                    Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsGround)));

            PenetrategroundedDetect = ((Physics2D.OverlapCircle(groundCheckC.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckL.position, groundRadius, whatIsPenetrateGround) ||
                         Physics2D.OverlapCircle(groundCheckR.position, groundRadius, whatIsPenetrateGround)));

        }

        if (isFront){
			grounded = groundedDetect;
		}
			else{
			grounded = PenetrategroundedDetect;
		}

		animator.SetBool("Grounded", grounded);


        //移動計算

        if (isOnMovingPlatform) { transform.Translate(speedX * 0.02f, 0.0f, 0.0f); _rigidbody.velocity = new Vector2(0.0f, _rigidbody.velocity.y); } //移動平台上面卡頓問題
        else _rigidbody.velocity = new Vector2(speedX, _rigidbody.velocity.y);

        //限制速度
        float vx = Mathf.Clamp(_rigidbody.velocity.x, velocityMin.x, velocityMax.x);
		float vy = Mathf.Clamp(_rigidbody.velocity.y, velocityMin.y, velocityMax.y);
		_rigidbody.velocity = new Vector2(vx, vy);


        //強制變更速度
        if (toSetVelocityX) {
			toSetVelocityX = false;
			_rigidbody.velocity = new Vector2(setVelocityX, _rigidbody.velocity.y);
		}
		if (toSetVelocityY) {
			toSetVelocityY = false;
			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, setVelocityY);
		}
        if (toSetMPVelocityY) {
			toSetMPVelocityY = false;
			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, setMPVelocityY);
		}

		//向上力量運算

		if (toSetUpForce) {
			toSetUpForce = false;
			_rigidbody.velocity = new Vector2 (0.0f, 0.0f);
			_rigidbody.AddForce (new Vector2 (0.0f, setUpForce));

            if(setUpForce > 0.0f) isOnMovingPlatform = false;

        }

		animator.SetFloat ("SpeedY", _rigidbody.velocity.y);

        //側向力運算
        if (toSetSideForce)
        {
            toSetSideForce = false;
            _rigidbody.AddForce(new Vector2(setSideForce, 0.0f));
        }

        //===================================DEBUG================================================
        //if(this.tag == "Player1")
        //Debug.Log (_rigidbody.velocity);
        
    }
    
}

