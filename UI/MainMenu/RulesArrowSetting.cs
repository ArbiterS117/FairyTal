using System.Collections;
using UnityEngine;

public class RulesArrowSetting : MonoBehaviour {

    public GameObject text;
    public GameObject arrow;

	void Start () {
		
	}
	
	void Update () {
        if (text.gameObject.activeSelf) arrow.SetActive(true);
        else arrow.SetActive(false);

    }
}
