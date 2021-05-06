using System.Collections;
using UnityEngine;

public class DirectionEffectBackScale : MonoBehaviour {

    public bool hasParent = false;
	
	void Update () {
        if (!hasParent)
        {
            if (transform.localScale.x < 0.0f) transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.parent.localScale.x < 0.0f && transform.localScale.x > 0.0f) transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
        }
	}
}
