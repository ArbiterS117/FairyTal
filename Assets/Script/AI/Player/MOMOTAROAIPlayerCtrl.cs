using UnityEngine;

public class MOMOTAROAIPlayerCtrl : AIPlayerBase {

    MOMOTAROSpecialAction playerSpecial;

    public override void Awake()
    {
        base.Awake();
        playerSpecial = GetComponent<MOMOTAROSpecialAction>();
    }

    //==========攻擊
    #region
    public override void AIAttatk()
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
                    else if (randomNUM <= 1.5f)
                    {
                        playerCtrl.actionUpAttack();
                    }
                    else if (randomNUM <= 2.0f)
                    {
                        playerCtrl.actionDownAttack();
                    }
                    else if (randomNUM <= 3.0f)
                    {
                        playerSpecial.actionSPAttack1();
                    }
                    else if (randomNUM <= 4.0f)
                    {
                        playerSpecial.actionSPAttack2();
                    }
                    else if (randomNUM <= 5.0f)
                    {
                        playerSpecial.actionSPAttack4();
                    }
                    else if (randomNUM <= 5.5f)
                    {
                        if (playerCtrl.grounded) playerCtrl.animator.SetTrigger("DashAttack");
                    }
                    else if (randomNUM > 5.5f && playerCtrl.HP <= playerCtrl.HPValueToRecoverMP)
                    {
                        playerSpecial.actionULT1();
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
    public override void BacktoMapMid()
    {
        BacktoMapMidCDuration += _deltaTime;
        if (BacktoMapMidCDuration >= BacktoMapMidDuration)
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
            else playerSpecial.actionSPAttack1();
            BackAirJumpColdCTime = 0.0f;
        }
    }
    #endregion
    //==========跟隨目標時遠距離攻擊
    #region
    public override void FollowPokeAttatk()
    {
        FollowAttackColdCTime += _deltaTime;
        if (FollowAttackColdCTime >= FollowAttackColdTime)
        {
            FollowAttackColdCTime = 0.0f;
            float randomNUM = Random.Range(0.0f, 2.0f);
            if (randomNUM < 1.0f)
            {
                if (playerCtrl.MP >= playerSpecial.DogStrikeMP)
                {
                    if (playerCtrl.grounded) playerSpecial.actionSPAttack2();
                    else playerSpecial.actionSPAttack3();
                }
            }
        }
    }
    #endregion


}
