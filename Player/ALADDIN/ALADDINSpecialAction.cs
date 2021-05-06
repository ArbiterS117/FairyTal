using UnityEngine;
using System.Collections;

public class ALADDINSpecialAction : XXXCtrl
{

    //這裡就負責放每隻腳色的特性
    //例:衝刺時無敵、三段跳之類的
    //可利用動畫或狀態來判斷
    //可用動畫控制器裡面的新增功能

    //====控制參數============

    public ATKColliderData DoubleKnifePierceATK;
    public ATKColliderData MagicCarpetFlyATK;
    public ATKColliderData MagicCarpetHitATK;
    public ATKColliderData MagicCarpetJumpATK;
    public ATKColliderData MagicCarpetJumpDownATK;
    public ATKColliderData MagicCarpetJumpSlashATK;

    public float ULT1MP = 30.0f;
    public float DoubleKnifePierceMP = 4.0f;
    public float MagicCarpetFlyMP = 5.0f;
    public float MagicCarpetJumpMP = 4.0f;

    public float MagicCarpetJumpForce = 500.0f;
    public float MagicCarpetJumpDashSpeed = 4.0f;
    public float MagicCarpetJumpGravity = 1.0f;

    public float MagicCarpetJumpDownForce = 500.0f;
    public float MagicCarpetJumpDownDashSpeed = 4.0f;
    public float MagicCarpetJumpDownGravity = 1.0f;

    public float MagicCarpetFlyForce = 500.0f;
    public float MagicCarpetFlyDashSpeed = 4.0f;
    public float MagicCarpetFlyGravity = 1.0f;

    public float DoubleKnifePierceDashSpeed = 8.0f;
    public float DoubleKnifePierceATKDashSpeed = 7.0f;

    public bool isStorgedPower  = false;
    public bool canStorgedPower = false; // 一般攻擊後面接此招 會有動畫沒播完瞬間發動的問題  加此變數來用動畫控制
    public bool closeStorgedPower = false; 
    public float DNPStorgeTimeMin = 0.5f;
    public float DNPStorgeTimeMax = 2.0f;
    public float DNPDashTime = 0.5f;
           float DNPStorgeCTime = 0.0f;
    public bool ULTBtnPressed = false;
    //====音效參數============
    public AudioClip normalATK1SE;
    public float normalATK1SEPitch;
    public AudioClip normalATK2SE;
    public float normalATK2SEPitch;
    public AudioClip MagicCarpetHitSE;
    public float MagicCarpetHitSEPitch;
    public AudioClip MagicCarpetFlySE;
    public float MagicCarpetFlySEPitch;
    public AudioClip DoubleKnifePierceSE;
    public float DoubleKnifePierceSEPitch;
    public AudioClip MagicCarpetJumpSE;
    public float MagicCarpetJumpSEPitch;
    public AudioClip MagicCarpetJumpDownSE;
    public float MagicCarpetJumpDownSEPitch;
    public AudioClip GeniePunchSE;
    public float GeniePunchSEPitch;

    //====開關============
    bool MagicCarpetFlyEnable = false;
    bool MCFCombpEnable = true;  // MCF防止重複Combo

    //bool bMCFtoDNPCombo = false; // MagicCarpetFly to DoubleKnifePierce
    

    volatile bool SPatkInputEnabled = false;
    volatile bool SPatkInputNow = false;
    protected string SPNextATKName;

    protected override void Awake()
    {
        base.Awake();
        audioCtrl = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        setStartATKData();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        //測試用
        AtkDataDic.Clear();
        setStartATKData();

        
        if (!isAttacking && !isAirAttacking)
        {
            SPatkInputEnabled = false;
            SPatkInputNow = false;
            
            canStorgedPower = false;
            closeStorgedPower = false;
            animator.SetBool("DNPDash", false);
            animator.SetBool("DNPPierce", false);
            animator.SetBool("MagicCarpetJumpSlash", false);
        }
        
        if (grounded && !isAttacking) {
            MagicCarpetFlyEnable = true;
            //bMCFtoDNPCombo = true;
        }

        if (!MoveEnable)
        {
            isStorgedPower = false;
            canStorgedPower = false;
            closeStorgedPower = false;
            animator.SetBool("DNPStorge", false);
            DNPStorgeCTime = 0.0f;
        }

        if (isDashing)
        {
            isStorgedPower = false;
            canStorgedPower = false;
            animator.SetBool("DNPStorge", false);
            DNPStorgeCTime = 0.0f;
        }

        

        if (isStorgedPower)
        {
            DNPStorgeCTime += deltaTime;
            if (closeStorgedPower)
            {
                isStorgedPower = false;
                closeStorgedPower = false;
            }
            if(DNPStorgeCTime >= DNPStorgeTimeMax && ULTBtnPressed)
            {
                //animator.SetTrigger("DNPDash"); //衝出去
                animator.SetBool("DNPDash",true); //衝出去
                animator.SetBool("DNPStorge", false);
                DoubleKnifePierceDash(DNPStorgeCTime);
                DNPStorgeCTime = 0.0f;
                isStorgedPower = false;
                canStorgedPower = false;
            }

            if(stateInfo.shortNameHash == ANISTS_Idle ||
               stateInfo.shortNameHash == ANISTS_OnAir ||
               stateInfo.shortNameHash == ANISTS_Move)
            {
                isStorgedPower = false;
                DNPStorgeCTime = 0.0f;
            }
            SetPadVibration(0.05f, 0.3f);
        }
        if((stateInfo.shortNameHash == ANISTS_SPAttack1 ||
            stateInfo.shortNameHash == ANISTS_SPAttack2) &&
            !ULTBtnPressed)
        {
            DNPStorgeCTime += deltaTime;
            if (DNPStorgeCTime >= DNPStorgeTimeMin)
            {
                //animator.SetTrigger("DNPDash"); //衝出去
                animator.SetBool("DNPDash", true); //衝出去
                animator.SetBool("DNPStorge", false);
                DoubleKnifePierceDash(DNPStorgeCTime);
                DNPStorgeCTime = 0.0f;
            }
        }
    }
    //====動畫用程式=======
    public void SPEnableAttackInput()
    {
        SPatkInputEnabled = true;
    }

    public void SPSetNextAttack()
    {
        if (SPatkInputNow == true)
        {
            SPatkInputNow = false;
            animator.Play(SPNextATKName, -1, 0f);
        }

    }

    //==攻擊 ====================================================
    public override void actionAttack()
    {
        if (MoveEnable)
        {
            if (grounded && !isDashing)
            {
                //地面攻擊程式 動畫
                if (!isAttacking)
                {
                    isAttacking = true;
                    isInCombo = true;
                    animator.SetTrigger("NormalAttack");

                }
                else
                {
                    if (isInCombo && !CNormal2ATK && atkInputEnabled)
                    {
                        nextATKName = "XXX_Attack2";
                        CNormal2ATK = true;
                    }

                    else if (isInCombo && CNormal2ATK && atkInputEnabled)
                    {
                        nextATKName = "XXX_Attack1";
                        CNormal2ATK = false;
                    }

                    if (isInCombo && atkInputEnabled)
                    {
                        atkInputEnabled = false;
                        atkInputNow = true;
                    }
                }
            }
            else if (!grounded && !isAttacking && !isAirAttacking)
            {
                //空中攻擊程式 動畫
                isAirAttacking = true;
                isDashing = false;
                animator.SetTrigger("AirAttack");
            }
            else if (grounded && isDashing)
            {
                //衝刺攻擊程式 動畫
                isAttacking = true;
                isDashing = false;
                animator.SetTrigger("DashAttack");
            }
        }
    }

    public override void actionUpAttack()
    {
        if (MoveEnable)
        {
            if (grounded)
            {
                if (isInCombo && atkInputEnabled && !CUpATK)
                {
                    CUpATK = true;
                    nextATKName = "XXX_UpAttack";
                    if (atkInputEnabled)
                    {
                        atkInputEnabled = false;
                        atkInputNow = true;
                    }
                }
               
                else if (grounded && !isInCombo && !isAttacking && !isDashing && !isDashJump)
                {
                    animator.SetTrigger("UpAttack");
                    isInCombo = true;
                    CUpATK = true;
                }

            }
            else
            {
                isAirAttacking = true;
                isDashing = false;
                animator.SetTrigger("AirAttack");
            }
        }
    }

    public override void actionDownAttack()
    {
        if (MoveEnable)
        {
            if (grounded)
            {
                if (isInCombo && atkInputEnabled && !CDownATK)
                {
                    CDownATK = true;
                    nextATKName = "XXX_DownAttack";
                    if (atkInputEnabled)
                    {
                        atkInputEnabled = false;
                        atkInputNow = true;
                    }
                }
                
                else if (grounded && !isInCombo && !isAttacking && !isDashing && !isDashJump)
                {
                    animator.SetTrigger("DownAttack");
                    isInCombo = true;
                    CDownATK = true;
                }

            }
            else
            {
                isAirAttacking = true;
                isDashing = false;
                animator.SetTrigger("AirAttack");
            }
        }
    }

    //===招式1&2&3&4&5  DoubleKnifePierce====================================

    public void actionSPAttack1()
    {
        if (MoveEnable && !isAttacking && !isDamaged &&
            !isDashing && !isDashJump && !isAirAttacking)
        {

            if (MP >= DoubleKnifePierceMP)
            {
                animator.SetBool("DNPStorge", true);
                MPCalculation(DoubleKnifePierceMP);
                isStorgedPower = true;
                canAttackMove = true;
            }

        }
        else if (isInCombo && atkInputEnabled)
        {   //普攻接技可接招式

            if (MP >= DoubleKnifePierceMP)
            {
                canStorgedPower = true;
                /*
                MPCalculation(DoubleKnifePierceMP);
                nextATKName = "XXX_SPAttack1";
                canAttackMove = true;
                atkInputEnabled = false;
                atkInputNow = true;
                */
            }
        }
        else if (MoveEnable && isAirAttacking && atkInputEnabled)
        {   //空中接技
            if (MP >= DoubleKnifePierceMP)
            {
                canStorgedPower = true;
                /*
                MPCalculation(DoubleKnifePierceMP);
                nextATKName = "XXX_SPAttack2";
                canAttackMove = true;
                atkInputEnabled = false;
                atkInputNow = true;
                */
            }
        }
        /*
        else if (MoveEnable && SPatkInputEnabled && !MagicCarpetFlyEnable && bMCFtoDNPCombo)
        {
            if (MP >= MagicCarpetFlyMP)
            {
                MPCalculation(DoubleKnifePierceMP);
                bMCFtoDNPCombo = false;
                isStorgedPower = true;
                nextATKName = "XXX_SPAttack2";
                canAttackMove = true;
                SPatkInputEnabled = false;
                SPatkInputNow = true;
            }
        }
        */
    }

    void DoubleKnifePierceDash(float boost)
    {
        if (stopDash) stopDash = false;
        StartCoroutine(DoubleKnifePierceDashIE(boost));
    }

    public void DoubleKnifePierceATKDash()
    {
        if (stopDash) stopDash = false;
        StartCoroutine(DoubleKnifePierceATKIE());
    }

    public void ADoubleKnifePierceTriggerG()
    {
        if (canStorgedPower)
        {
            nextATKName = "XXX_SPAttack1";
            MPCalculation(DoubleKnifePierceMP);
            canAttackMove = true;
            atkInputEnabled = false;
            atkInputNow = true;
            isStorgedPower = true;
            canStorgedPower = false;
        }
    }

    public void ADoubleKnifePierceTriggerA()
    {
        if (canStorgedPower)
        {
            nextATKName = "XXX_SPAttack2";
            MPCalculation(DoubleKnifePierceMP);
            canAttackMove = true;
            atkInputEnabled = false;
            atkInputNow = true;
            isStorgedPower = true;
            canStorgedPower = false;
        }
    }

    //===招式6&7&8&9&10  MagicCarpetJump=====================================

    public void actionSPAttack6()
    {
        if (MoveEnable && !isAttacking && !isDamaged &&
            !isDashing && !isDashJump && !isAirAttacking)
        {
            if (MP >= MagicCarpetJumpMP)
            {
                MPCalculation(MagicCarpetJumpMP);
                animator.SetTrigger("MagicCarpetJump");            
            }
        }
        else if (isInCombo && atkInputEnabled)
        {
            if (MP >= MagicCarpetJumpMP)
            {
                MPCalculation(MagicCarpetJumpMP);
                if (grounded) nextATKName = "XXX_SPAttack6";
                else nextATKName = "XXX_SPAttack7";
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
        else if (MoveEnable && isAirAttacking && atkInputEnabled)
        {
            if (MP >= MagicCarpetJumpMP)
            {
                MPCalculation(MagicCarpetJumpMP);
                if (grounded) nextATKName = "XXX_SPAttack6";
                else nextATKName = "XXX_SPAttack7";
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
        else if (MoveEnable && SPatkInputEnabled)
        {
            if (MP >= MagicCarpetJumpMP)
            {
                MPCalculation(MagicCarpetJumpMP);
                if (grounded) SPNextATKName = "XXX_SPAttack6";
                else SPNextATKName = "XXX_SPAttack7";
                SPatkInputEnabled = false;
                SPatkInputNow = true;
            }
        }

    }

    public void MagicCarpetJump()
    {
        SetPadVibration(0.2f, 0.4f);
        AirJumpEnabled = false;
        toSetUpForce = true;
        if (stopDash) stopDash = false;
        if (grounded)
        {
            setUpForce = MagicCarpetJumpForce;
            StartCoroutine(MagicCarpetJumpIE(MagicCarpetJumpGravity, MagicCarpetJumpDashSpeed));
        }
        else
        {
            setUpForce = MagicCarpetJumpDownForce;
            StartCoroutine(MagicCarpetJumpIE(MagicCarpetJumpDownGravity, MagicCarpetJumpDownDashSpeed));
        }
    }

    public void MCJAirReady()
    {
        StartCoroutine(MCJAirReadyIE());
    }

    //===招式11  MajicCarpetFly====================================

        public void actionSPAttack11()
    {
        if (MoveEnable && !isAttacking && !isDamaged &&
            !isDashing && !isDashJump && !isAirAttacking)
        {
            if (MP >= MagicCarpetJumpMP && MagicCarpetFlyEnable)
            {
                MPCalculation(MagicCarpetFlyMP);
                canAttackMove = true;
                animator.SetTrigger("MagicCarpetFly");
                MCFCombpEnable = false;
            }
        }
        
        else if (isInCombo && atkInputEnabled)
        {
            if (MP >= MagicCarpetFlyMP && MagicCarpetFlyEnable)
            {
                MPCalculation(MagicCarpetFlyMP);
                canAttackMove = true;
                nextATKName = "XXX_SPAttack11";
                MCFCombpEnable = false;
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
        else if (MoveEnable && isAirAttacking && atkInputEnabled)
        {
            if (MP >= MagicCarpetFlyMP && MagicCarpetFlyEnable)
            {
                MPCalculation(MagicCarpetFlyMP);
                canAttackMove = true;
                nextATKName = "XXX_SPAttack11";
                MCFCombpEnable = false;
                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
        else if (MoveEnable && SPatkInputEnabled)
        {
            if (MP >= MagicCarpetFlyMP && MagicCarpetFlyEnable && MCFCombpEnable)
            {
                MPCalculation(MagicCarpetFlyMP);
                canAttackMove = true;
                SPNextATKName = "XXX_SPAttack11";
                MCFCombpEnable = false;
                SPatkInputEnabled = false;
                SPatkInputNow = true;
            }
        }
    }

    public void MagicCarpetFly()
    {
        AirJumpEnabled = false;
        MagicCarpetFlyEnable = false;
        toSetUpForce = true;
        if (stopDash) stopDash = false;
        setUpForce = MagicCarpetFlyForce;
        StartCoroutine(MagicCarpetFlyIE());
    }
    
    //===ULT=======================================

    public void actionULT1()
    {
        if (MoveEnable && !isAttacking &&
            !isDamaged && !isDashing && !isDashJump)
        {
            if (MP >= ULT1MP)
            {
                AirJumpEnabled = false;
                animator.SetTrigger("GeniePunch");
                MPCalculation(ULT1MP);
            }
        }
    }

    public void actionHakki()
    {
        hakki = true;
    }

    public void actionStopHakki()
    {
        hakki = false;
    }

    //===實作=========================================

    //DoubleKnifePierce
    IEnumerator DoubleKnifePierceDashIE(float boost)
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        float time = 0.0f;

        while (!stopDash && (DNPDashTime * (1.0f + boost) > time))
        {
            if (isDamaged || isKnockOuted || isDead) break;
            time += deltaTime;
            speedX = DoubleKnifePierceDashSpeed * dir * (1.0f + boost);
            toSetVelocityY = true;
            setVelocityY = 0.0f;
            yield return null;
        }
        if (!stopDash && DNPDashTime * (1.0f + boost) < time)//前方碰撞器會觸動stopDash
        {
            //animator.SetTrigger("DNPPierce");
            animator.SetBool ("DNPPierce",true);
        }
        stopDash = false;
        GetComponent<Rigidbody2D>().gravityScale = initGravity;
    }

    IEnumerator DoubleKnifePierceATKIE()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        while (!stopDash)
        {
            if (isDamaged || isKnockOuted || isDead) break;
            speedX = DoubleKnifePierceATKDashSpeed * dir;
            toSetVelocityY = true;
            setVelocityY = 0.0f;
            yield return null;
        }
        stopDash = false;
        GetComponent<Rigidbody2D>().gravityScale = initGravity;
    }


    //MagicCarpetJump
    IEnumerator MagicCarpetJumpIE(float gravity, float dashSpeedx)
    {
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        float time = 0.0f;
        while (!stopDash)
        {
            if (isDamaged || isKnockOuted || isDead) break;
            time += deltaTime;
            if(time >= 0.1f) // 防止馬上偵測到grounded
            {
                if (grounded || isTouchingWall) break;
            }
            speedX = dashSpeedx * dir;
            yield return null;
        }
        if (grounded && !isDamaged && !isKnockOuted && !isKnockDown && !stopDash && !isDead) animator.SetBool("MagicCarpetJumpSlash", true);
        else if(isTouchingWall) animator.SetBool("MagicCarpetJumpSlash", true);
        stopDash = false;
        GetComponent<Rigidbody2D>().gravityScale = initGravity;
    }

    IEnumerator MCJAirReadyIE()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        while (!stopDash)
        {
            if (isDamaged || isKnockOuted || isDead || grounded) break;
            speedX = 0.0f;
            toSetVelocityY = true;
            setVelocityY = 0.0f;
            yield return null;
        }
        stopDash = false;
        GetComponent<Rigidbody2D>().gravityScale = initGravity;
    }

    //MagicCarpetFly
    IEnumerator MagicCarpetFlyIE()
    {
        while (!stopDash)
        {
            if (isDamaged || isKnockOuted || isDead) break;
            GetComponent<Rigidbody2D>().gravityScale = MagicCarpetFlyGravity;
            speedX = MagicCarpetFlyDashSpeed * dir;
            yield return null;
        }
        MagicCarpetFlyEnable = false;
        stopDash = false;
        MCFCombpEnable = true;
        GetComponent<Rigidbody2D>().gravityScale = initGravity;
    }




    //===音效=========================================

    public void PlaySENormalATK1SE()
    {
        audioCtrl.pitch = normalATK1SEPitch + Random.Range(-0.05f, 0.05f);
        audioCtrl.PlayOneShot(normalATK1SE);
    }

    public void PlaySENormalATK2SE()
    {
        audioCtrl.pitch = normalATK2SEPitch + Random.Range(-0.05f, 0.05f);
        audioCtrl.PlayOneShot(normalATK2SE);
    }

    public void PlaySEMagicCarpetHitSE()
    {
        audioCtrl.pitch = MagicCarpetHitSEPitch;
        audioCtrl.PlayOneShot(MagicCarpetHitSE);
    }

    public void PlaySEMagicCarpetFlySE()
    {
        audioCtrl.pitch = MagicCarpetFlySEPitch;
        audioCtrl.PlayOneShot(MagicCarpetFlySE);
    }

    public void PlaySEMagicCarpetJumpSE()
    {
        audioCtrl.pitch = MagicCarpetJumpSEPitch;
        audioCtrl.PlayOneShot(MagicCarpetJumpSE);
    }

    public void PlaySEMagicCarpetJumpDownSE()
    {
        audioCtrl.pitch = MagicCarpetJumpDownSEPitch;
        audioCtrl.PlayOneShot(MagicCarpetJumpDownSE);
    }

    public void PlaySEDoubleKnifePierceSE()
    {
        audioCtrl.pitch = DoubleKnifePierceSEPitch;
        audioCtrl.PlayOneShot(DoubleKnifePierceSE);
    }

    public void PlaySEGeniePunchSE()
    {
        audioCtrl.pitch = GeniePunchSEPitch;
        audioCtrl.PlayOneShot(GeniePunchSE);
    }

    //===設置攻擊力============================
    public override void setATKData(string s)
    {
        ATKData = AtkDataDic[s];
    }

    //設置初始攻擊資料
    private void setStartATKData()
    {
        AtkDataDic.Add("Attack1ATK", Attack1ATK);
        AtkDataDic.Add("Attack2ATK", Attack2ATK);
        AtkDataDic.Add("DownAttackATK", DownAttackATK);
        AtkDataDic.Add("UpAttackATK", UpAttackATK);
        AtkDataDic.Add("AirAttackATK", AirAttackATK);
        AtkDataDic.Add("DashAttackATK", DashAttackATK);
        AtkDataDic.Add("DoubleKnifePierceATK", DoubleKnifePierceATK);
        AtkDataDic.Add("MagicCarpetFlyATK", MagicCarpetFlyATK);
        AtkDataDic.Add("MagicCarpetHitATK", MagicCarpetHitATK);
        AtkDataDic.Add("MagicCarpetJumpDownATK", MagicCarpetJumpDownATK);
        AtkDataDic.Add("MagicCarpetJumpATK", MagicCarpetJumpATK);
        AtkDataDic.Add("MagicCarpetJumpSlashATK", MagicCarpetJumpSlashATK);
        AtkDataDic.Add("ULT1ATK", ULT1ATK);
    }

    

}
