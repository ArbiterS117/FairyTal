using UnityEngine;
using System.Collections;

public class SlpbtyDragonCtrl : NPCEnemyBase
{
	
	GameCtrl gameCtrl;
	Animator anim;
    public SlpbtyCameraCtrl CameraCtrl;

	public bool isAttacking = false;
	public float AttackColdTime;
	float AttackColdCTime = 0.0f;

	public float AttackRate = 80.0f;

    public bool randomDir = true;
    public ATKColliderData FlyDownATK_ATK;
    public ATKColliderData FlyATK_ATK;
    public ATKColliderData FireATK_ATK;

    public ParticleSystem fog1;
	public ParticleSystem fog2;

	public Color fog1OriColor;
	public Color fog2OriColor;
	public Color fog1DangerColor;
	public Color fog2DangerColor;
	public float TrasformTime;
	       float TrasformCTime = 0.0f;
	bool transformComplete = true;

    float _deltaTime = 0.0f;


    protected override void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		anim = GetComponent<Animator>();

    }

    protected override void Start () {
		fog1OriColor = fog1.main.startColor.color;
		fog2OriColor = fog2.main.startColor.color;
		transformComplete = true;
        setStartATKData();
}
	

	protected override void Update () {
        _deltaTime = Time.deltaTime;

        if (!isAttacking) {
            CameraCtrl.isAttatk = false;
			anim.SetBool("isAttacking",false);
			AttackColdCTime += _deltaTime;
			colorTransformBack();
			if(AttackColdCTime >= AttackColdTime){
				if(Random.Range(0.0f,100.0f) <= AttackRate){

					isAttacking = true;
					transformComplete = false;

					if(Random.Range(0.0f,2.0f)>= 1.0f)this.transform.localScale = 
						new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y);//left or right

					float n = Random.Range(0.0f,3.0f);//選擇攻擊
                    if (n >= 0.0f && n < 1.0f)
                    {
                        anim.SetTrigger("fire");
                        setATKData("FireATK_ATK");
                        randomDir = false;
                    }
                    else if (n >= 1.0f && n < 2.0f)
                    {
                        anim.SetTrigger("Down");
                        setATKData("FlyDownATK_ATK");
                        randomDir = true;
                    }
                    else
                    {
                        anim.SetTrigger("fly");
                        setATKData("FlyATK_ATK");
                        randomDir = true;
                    }
				}
				AttackColdCTime = 0.0f;
			}
		}
		
		else{
            CameraCtrl.isAttatk = true;
            anim.SetBool("isAttacking",true);
			colorTransform();
		}
	}

	void colorTransform(){
		if (!transformComplete && TrasformCTime <= TrasformTime) {
			TrasformCTime += _deltaTime;
            ParticleSystem.MainModule main = fog1.main;
            main.startColor = new Color (fog1OriColor.r + ((fog1DangerColor.r - fog1OriColor.r) * TrasformCTime / TrasformTime),
		                            fog1OriColor.g + ((fog1DangerColor.g - fog1OriColor.g) * TrasformCTime / TrasformTime),
		                            fog1OriColor.b + ((fog1DangerColor.b - fog1OriColor.r) * TrasformCTime / TrasformTime),
			                        fog1OriColor.a);
            main = fog2.main;
            main.startColor = new Color (fog2OriColor.r + ((fog2DangerColor.r - fog2OriColor.r) * TrasformCTime / TrasformTime),
		                            fog2OriColor.g + ((fog2DangerColor.g - fog2OriColor.g) * TrasformCTime / TrasformTime),
		                            fog2OriColor.b + ((fog2DangerColor.b - fog2OriColor.r) * TrasformCTime / TrasformTime),
			                        fog2OriColor.a);

			if (TrasformCTime >= TrasformTime) {
				transformComplete = true;
				TrasformCTime = 0.0f;
			}
		}
		else {
            ParticleSystem.MainModule main = fog1.main;
            main.startColor = fog1DangerColor;
            main = fog2.main;
			main.startColor = fog2DangerColor;
		}

	}

	void colorTransformBack(){
		if (!transformComplete && TrasformCTime <= TrasformTime) {
			TrasformCTime += _deltaTime;
            ParticleSystem.MainModule main = fog1.main;
            main.startColor = new Color (fog1DangerColor.r + ((fog1OriColor.r - fog1DangerColor.r) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.g + ((fog1OriColor.g - fog1DangerColor.g) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.b + ((fog1OriColor.b - fog1DangerColor.r) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.a);
            main = fog2.main;
			main.startColor = new Color (fog1DangerColor.r + ((fog1OriColor.r - fog1DangerColor.r) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.g + ((fog1OriColor.g - fog1DangerColor.g) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.b + ((fog1OriColor.b - fog1DangerColor.r) * TrasformCTime / TrasformTime),
			                             fog1DangerColor.a);
			
			if (TrasformCTime >= TrasformTime) {
				transformComplete = true;
				TrasformCTime = 0.0f;
			}
		}
		else {
            ParticleSystem.MainModule main = fog1.main;
            main.startColor = fog1OriColor;
            main = fog2.main;
			main.startColor = fog2OriColor;
		}
		
	}

	//============動畫用程式

	public void AnimDragonScream(float time){
		gameCtrl.MainCamera.ShakeCamera(3.0f/70.0f,time);
	}

	public void StopAttack(){
		isAttacking = false;
		transformComplete = false;
	}

    //===設置攻擊力============================
    public override void setATKData(string s)
    {
        ATKData = AtkDataDic[s];
    }

    //設置初始攻擊資料
    private void setStartATKData()
    {
        AtkDataDic.Add("FireATK_ATK", FireATK_ATK);
        AtkDataDic.Add("FlyDownATK_ATK", FlyDownATK_ATK);
        AtkDataDic.Add("FlyATK_ATK", FlyATK_ATK);
    }

}
