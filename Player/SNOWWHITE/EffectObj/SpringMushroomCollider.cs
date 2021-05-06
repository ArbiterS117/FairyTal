using UnityEngine;
using System.Collections;

public class SpringMushroomCollider : MonoBehaviour {

	public float springJumpForce;

    XXXCtrl playerCtrl = null;


    void OnTriggerEnter2D(Collider2D other) {
        //======玩家=========
        if (other.CompareTag("PlayerECollider")) {
            playerCtrl = other.GetComponentInParent<XXXCtrl>();

            if (!playerCtrl.isAirAttacking)
            {
                if (playerCtrl.isFront == GetComponentInParent<DirectionEffectCtrl>().isFront &&
                    other.GetComponentInParent<Rigidbody2D>().velocity.y <= 0.0f &&
                    !playerCtrl.isDead)
                {
                    playerCtrl.isDashing = false;
                    playerCtrl.toSetUpForce = true;
                    playerCtrl.setUpForce = springJumpForce;
                    playerCtrl.backDMGSwitch = true;
                    playerCtrl.actionBackDamage();

                    //anim
                    GetComponentInParent<SpringMushroomCtrl>().startStand();

                }
            }
		}

        //====道具===============
        if (other.CompareTag("Item"))
        {
            if(other.GetComponentInParent<AppleMineCtrl>() != null)
            {
                other.transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, springJumpForce * 0.8f));
                other.GetComponentInParent<AppleMineCtrl>().rollTime *= 1.5f;

                GetComponentInParent<SpringMushroomCtrl>().startStand();
            }
        }
	}

}
