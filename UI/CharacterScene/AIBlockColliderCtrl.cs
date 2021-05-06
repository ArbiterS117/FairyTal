using System.Collections;
using UnityEngine;

public class AIBlockColliderCtrl : MonoBehaviour {

    public int playerNum = 0;

    CharacterSelectDataCtrl characterData;

    void Start () {
        characterData = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
    }
	
	void Update () {
		if(characterData.isInCtrl[playerNum - 1] || characterData.isSelected[playerNum - 1] || characterData.inChoosenAI[playerNum - 1])
        {
            if(this.GetComponent<BoxCollider2D>()) this.GetComponent<BoxCollider2D>().enabled = false;
            if (this.GetComponent<CircleCollider2D>()) this.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            if (this.GetComponent<BoxCollider2D>()) this.GetComponent<BoxCollider2D>().enabled = true;
            if (this.GetComponent<CircleCollider2D>()) this.GetComponent<CircleCollider2D>().enabled = false;
        }
	}
}
