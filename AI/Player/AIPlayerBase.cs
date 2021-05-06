using System.Collections;
using UnityEngine;

public class AIPlayerBase : MonoBehaviour {

    protected GameCtrl gameCtrl;
    public XXXCtrl playerCtrl;

    public Transform TargetTransform;

    protected float _deltaTime;

    //狀態參數
    public enum AIstate
    {
        ready,
        follow,
        attack,
        hide,
        back, // 返回地圖中央
        move  // 移動到地圖某點
    }
    public AIstate aiState;

    public bool isTargeted = false;
    public int targetNUM = 0;

    public bool IsOutMapLeft = true;

    //控制參數
    public float ChangeTargetTime = 15.0f;
    float ChangeTargetCTime = 0.0f;

    public float followDistance = 2.0f;

    public float followColdTime = 0.7f;
    protected float followCTime = 0.0f;

    public float attackColdTime = 0.5f;
    protected float attackCTime = 0.0f;

    public float FollowAttackColdTime = 3.0f;
    protected float FollowAttackColdCTime = 0.0f;

    public float attackContinueColdTime = 0.2f; // 避免短時間內連續出招
    protected float attackContinueCTime = 0.0f;

    public float FollowAvoidColdTime = 0.4f;
    protected float FollowAvoidColdCTime = 0.0f;

    protected float FollowAvoidRandomTime = 0.0f;

    public float FollowBackDamageColdTime = 0.2f;
    protected float FollowBackDamageColdCTime = 0.0f;

    public float BackAirJumpColdTime = 0.2f;
    protected float BackAirJumpColdCTime = 0.0f;


    public float BacktoMapMidDuration = 1.0f;
    public float BacktoMapMidCDuration = 0.0f;



    //============================INIT==========================
    public virtual void Awake()
    {
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        playerCtrl = GetComponent<XXXCtrl>();
    }

    public virtual void Start()
    {
        
        if (playerCtrl.PlayerNUM == 1)
        {
            tag = "Player1";
        }
        else if (playerCtrl.PlayerNUM == 2)
        {
            tag = "Player2";
        }
        else if (playerCtrl.PlayerNUM == 3)
        {
            tag = "Player3";
        }
        else if (playerCtrl.PlayerNUM == 4)
        {
            tag = "Player4";
        }

        gameCtrl.isAI[playerCtrl.PlayerNUM - 1] = true;

        FollowAvoidRandomTime = Random.Range(0.0f, 0.5f);
    }


    //=========================update==============================
    public virtual void Update()
    {
        if (playerCtrl.isDead) return;
        _deltaTime = Time.deltaTime;

      
        //============準備狀態==================

        if (aiState == AIstate.ready)
        {
            ReadyToFollow();
            ReadyToAttack();
        }

        //============抓取目標==================
        if (gameCtrl.DebugAITrace)
        {//debug

            if (!isTargeted)
            {
                if (FindTarget()) isTargeted = true;
            }

            if (isTargeted)
            {
                if (gameCtrl.isDead[targetNUM])
                {
                    TargetTransform = null;
                    isTargeted = false;
                }

                //切換目標
                ChangeTargetCTime += _deltaTime;
                if(ChangeTargetCTime >= ChangeTargetTime)
                {
                    if (aiState == AIstate.ready)
                    {
                        TargetTransform = null;
                        isTargeted = false;
                        ChangeTargetCTime = 0.0f;
                    }
                }
                

            }

        }//debug

        //================跟隨===================
        if (aiState == AIstate.follow)
        {
            followTarget();
            FollowPokeAttatk();
        }

        //================攻擊====================
        if (aiState == AIstate.attack)
        {
            AIAttatk();
            ReadyToFollow();
        }

        
        //============返回地圖中央===================
        if(aiState == AIstate.back)
        {
            BacktoMapMid();
        }


    }

    //===========================自訂功能======================

    //==========跟隨目標前待機
    #region
    void ReadyToFollow()
    {

        playerCtrl.actionMove(0);

        if (TargetTransform != null)
        {
            if (this.transform.position.x - TargetTransform.position.x <= -followDistance
                || this.transform.position.x - TargetTransform.position.x >= followDistance)
            {
                if (aiState != AIstate.follow)
                {
                    
                    followCTime += _deltaTime;
                    if (followCTime >= followColdTime + Random.Range(0.0f, 1.0f))
                    {
                        aiState = AIstate.follow;
                        followCTime = 0.0f;
                    }
                }
            }
        }
    }
    #endregion
    //==========抓取目標
    #region
    bool FindTarget()
    {
        int randomNUM = 0;
        randomNUM = Mathf.FloorToInt(Random.Range(0.0f, 4.0f));
        if (randomNUM == 4) randomNUM = 3; // 怕數值剛好取到4.0
        targetNUM = randomNUM;

        if (gameCtrl.isDead[targetNUM] || playerCtrl.PlayerNUM - 1 == targetNUM )
        {
            return false;
        }
        else
        {

            //隊伍
            if (gameCtrl.TeamMode == true)
            {
                if(playerCtrl.teamNum == gameCtrl.player[targetNUM].GetComponent<XXXCtrl>().teamNum) return false;
            }
            //

            //MirrorCastle
            //if((gameCtrl.player[targetNUM].GetComponent<XXXCtrl>().isFront != playerCtrl.isFront)) return false;
            //

            if (gameCtrl.player[targetNUM] != null) TargetTransform = gameCtrl.player[targetNUM].transform;
            return true;
        }

    }
    #endregion
    //==========跟隨目標
    #region
    void followTarget()
    {
        if (TargetTransform != null)
        {
            FollowAvoidColdCTime += _deltaTime;

            //============跟隨隨機動作(閃招)
            if(FollowAvoidColdCTime >= FollowAvoidColdTime +  FollowAvoidRandomTime)
            {
                float randomNUM = Random.Range(0.0f, 5.0f);
                if (randomNUM <= 1.0f)
                {
                    playerCtrl.actionJump(); // 跳
                }
                else if(randomNUM <= 2.0f)
                {
                    playerCtrl.actionDash(); //衝刺
                }
                else if(randomNUM <= 3.0f)
                {
                    playerCtrl.actionJump();
                    StartCoroutine(DashDelay(0.4f)); //跳 -> 衝刺
                }
                FollowAvoidColdCTime = 0.0f;
                FollowAvoidRandomTime = Random.Range(-0.1f, 0.3f);
            }

            //============受身
            if (playerCtrl.isKnockOuted || playerCtrl.isKnockDown)
            {
                FollowBackDamageColdCTime += _deltaTime;
                if (FollowBackDamageColdCTime >= FollowBackDamageColdTime)
                {
                    if (Random.Range(0.0f, 2.0f) < 1.0f) playerCtrl.actionBackDamage();
                    FollowBackDamageColdCTime = 0.0f;
                }
            }
            else
            {
                FollowBackDamageColdCTime = 0.0f;
            }

            //============移動 & 切換
            if (this.transform.position.x - TargetTransform.position.x <= -followDistance)
            {
                playerCtrl.actionMove(1);
                if (playerCtrl.dir < 0)
                    playerCtrl.actionFlip();
                aiState = AIstate.follow;

            }
            else if (this.transform.position.x - TargetTransform.position.x >= followDistance)
            {
                playerCtrl.actionMove(-1);
                if (playerCtrl.dir > 0)
                    playerCtrl.actionFlip();
                aiState = AIstate.follow;
            }
            else
            {
                playerCtrl.isDashing = false;
                StopAllCoroutines();
                //StopCoroutine(DashDelay(0.4f));
                playerCtrl.actionMove(0);
                FollowAvoidColdCTime = 0.0f;
                aiState = AIstate.ready;
                if (!playerCtrl.grounded) playerCtrl.actionAttack();
            }

        }
    }

    IEnumerator DashDelay(float time)
    {
        yield return new WaitForSeconds(time);

        FollowAvoidColdCTime = 0.0f;
        playerCtrl.actionDash();
    }
    #endregion
    //==========跟隨目標時遠距離攻擊
    #region
    public virtual void FollowPokeAttatk()
    {
        FollowAttackColdCTime += _deltaTime;
        if(FollowAttackColdCTime >= FollowAttackColdTime)
        {
            FollowAttackColdCTime = 0.0f;
            float randomNUM = Random.Range(0.0f, 2.0f);
            if(randomNUM < 1.0f)
            {
                //玩家遠距離招式
            }
        }
    }
    #endregion
    //==========準備狀態待機
    #region
    void ReadyToAttack()
    {
        if (TargetTransform != null)
        {
            if (this.transform.position.x - TargetTransform.position.x >= -followDistance
                && this.transform.position.x - TargetTransform.position.x <= followDistance)
            {
                attackCTime += _deltaTime;
                if (attackCTime >= attackColdTime)
                {
                    aiState = AIstate.attack;
                    attackCTime = 0.0f;
                }
            }
        }
    }
    #endregion
    //==========攻擊
    #region
    public virtual void AIAttatk()
    {
        if (TargetTransform != null)
        {
            if (this.transform.position.x - TargetTransform.position.x >= -followDistance
                  && this.transform.position.x - TargetTransform.position.x <= followDistance)
            {

                //判斷敵方位於前or後
                if (this.transform.position.x > TargetTransform.position.x && playerCtrl.dir > 0 ||
                   this.transform.position.x < TargetTransform.position.x && playerCtrl.dir < 0)
                {
                    playerCtrl.actionFlip();
                }
                attackContinueCTime += _deltaTime;
                if (attackContinueCTime >= attackContinueColdTime)
                {
                    float randomNUM = Random.Range(-2.0f, 6.0f);
                    if (randomNUM <= 1.0f)
                    {
                        playerCtrl.actionAttack();
                    }
                    if (randomNUM <= 2.0f)
                    {
                        playerCtrl.actionUpAttack();
                    }
                    if (randomNUM <= 3.0f)
                    {
                        playerCtrl.actionDownAttack();
                    }
                    else if (randomNUM <= 4.0f)
                    {
                        //playerCtrl.actionSPAttack1();
                    }
                    else if (randomNUM <= 5.0f)
                    {
                        //playerCtrl.actionSPAttack2();
                    }
                    else if (randomNUM <= 5.5f)
                    {
                        if (playerCtrl.grounded) playerCtrl.animator.SetTrigger("DashAttack");
                    }
                    else if (randomNUM > 5.5f && playerCtrl.HP <= playerCtrl.HPValueToRecoverMP)
                    {
                        //playerCtrl.actionULT1();
                    }
                    attackContinueCTime = 0.0f;
                }

                //====受身
                if (playerCtrl.isKnockOuted || playerCtrl.isKnockDown)
                {
                    FollowBackDamageColdCTime += _deltaTime;
                    if (FollowBackDamageColdCTime >= FollowBackDamageColdTime)
                    {
                        if (Random.Range(0.0f, 2.0f) < 1.0f) playerCtrl.actionBackDamage();
                        FollowBackDamageColdCTime = 0.0f;
                    }
                }
                else
                {
                    FollowBackDamageColdCTime = 0.0f;
                }
            }
        }
        else
        {
            aiState = AIstate.ready;
        }
    }
    #endregion
    //==========返回地圖中間
    #region
    public virtual void BacktoMapMid()
    {
        BacktoMapMidCDuration += _deltaTime;
        if(BacktoMapMidCDuration >= BacktoMapMidDuration)
        {
            BacktoMapMidCDuration = 0.0f;
            BackAirJumpColdCTime = 0.0f;
            aiState = AIstate.ready;
            if (playerCtrl.grounded)
            {
                playerCtrl.actionMove(0);
                playerCtrl.isDashing = false;
            }
            
        }

        if (IsOutMapLeft)
        {
            if (playerCtrl.dir < 0) playerCtrl.actionFlip();
            playerCtrl.actionMove(1);
        }
        else
        {
            if (playerCtrl.dir > 0) playerCtrl.actionFlip();
            playerCtrl.actionMove(-1);
        }

        BackAirJumpColdCTime += _deltaTime;
        if (BackAirJumpColdCTime >= BackAirJumpColdTime)
        {
            playerCtrl.actionBackDamage();
            if (playerCtrl.AirJumpEnabled) playerCtrl.actionJump();
            else if (playerCtrl.DashEnabled && !playerCtrl.grounded) playerCtrl.actionDash();
            //else   玩家上招式 需要OverRide
            BackAirJumpColdCTime = 0.0f;
        }
    }
    #endregion


    //==========================================================
}
