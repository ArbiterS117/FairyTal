using UnityEngine;
using System.Collections;

public class BehindGroundDetect : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "BottomPenetrateDetect") {
            if (other.transform.parent.tag == "Player")
            {

                if (!other.transform.GetComponentInParent<XXXCtrl>().isFront)
                {
                    Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
                }
                else
                {
                    Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
                }
            }

            else if (other.transform.parent.tag == "Item")
            {
                if (!other.transform.GetComponentInParent<DirectionEffectCtrl>().isFront)
                {
                    if (other.transform.parent.GetComponent<BoxCollider2D>() != null)  //球型物件或是方形物件
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>(), false);
                    }
                }
                else
                {
                    if (other.transform.parent.GetComponent<BoxCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>(), true);
                    }
                }
            }
         }
		
		if (other.tag == "PenetrateDetect") {
            if (other.transform.parent.tag == "Player")
            {
                if (other.transform.parent.parent.GetComponent<XXXCtrl>().isFront)
                {
                    Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
                }
            }

            else if (other.transform.parent.tag == "Item")
            {
                if (other.transform.GetComponentInParent<DirectionEffectCtrl>().isFront)
                {
                    if (other.transform.parent.GetComponent<BoxCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(other.transform.parent.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>(), true);
                    }
                }
            }

        }
	}
	
}
