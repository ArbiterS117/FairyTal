using UnityEngine;

public class RAPUNZELAIPlayerCtrl : AIPlayerBase {
    RAPUNZELSpecialAction playerSpecial;

    public override void Awake()
    {
        base.Awake();
        playerSpecial = GetComponent<RAPUNZELSpecialAction>();
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
                    float randomNUM = Random.Range(-1.0f, 8.0f);
                    if (randomNUM <= 2.0f)
                    {
                        playerCtrl.actionAttack();
                    }
                    else if (randomNUM <= 3.0f)
                    {
                        playerCtrl.actionUpAttack();
                    }
                    else if (randomNUM <= 4.0f)
                    {
                        playerCtrl.actionDownAttack();
                    }
                    else if (randomNUM <= 4.5f)
                    {
                        if(playerCtrl.grounded)playerCtrl.animator.SetTrigger("DashAttack");
                    }
                    else if (randomNUM <= 5.5f)
                    {
                        playerSpecial.actionSPAttack1();
                    }
                    else if (randomNUM <= 6.5f)
                    {
                        playerSpecial.actionSPAttack3();
                    }
                    else if (randomNUM <= 7.5f)
                    {
                        playerSpecial.actionSPAttack4();
                    }
                    else if (randomNUM > 7.5f && playerCtrl.HP <= playerCtrl.HPValueToRecoverMP)
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
            else playerSpecial.actionSPAttack3();
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
                if (playerCtrl.MP >= playerSpecial.HairWindMP) playerSpecial.actionSPAttack4();
            }
        }
    }
    #endregion


}
