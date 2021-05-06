using UnityEngine;
using System.Collections;

public class PlayerNUMMarkCtrl : MonoBehaviour {

	public GameCtrl gameCtrl;
	XXXCtrl playerCtrl;

	public string MarkString;

	int playerNum;
	string playerNumString;

    Vector3 oriScale;


	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag ("GameCtrl").GetComponent<GameCtrl>();
		playerCtrl = this.transform.parent.GetComponentInParent<XXXCtrl> ();


        oriScale = this.transform.localScale;


    }

	void Start () {

        //隊伍
        if (gameCtrl.TeamMode)
        {
            if (MarkString == "REDTeam" || MarkString == "BlueTeam" || MarkString == "GreenTeam")
            {
                if (playerCtrl.teamNum == 1)
                {
                    if (MarkString == "REDTeam") { this.transform.GetComponent<SpriteRenderer>().enabled = true; return; }
                }
                else if (playerCtrl.teamNum == 2)
                {
                    if (MarkString == "BlueTeam") { this.transform.GetComponent<SpriteRenderer>().enabled = true; return; }
                }
                else if (playerCtrl.teamNum == 3)
                {
                    if (MarkString == "GreenTeam") { this.transform.GetComponent<SpriteRenderer>().enabled = true; return; }
                }
                GameObject.Destroy(this.gameObject);
                return;
            }


        }
        else if(MarkString == "REDTeam" || MarkString == "BlueTeam" || MarkString == "GreenTeam") GameObject.Destroy(this.gameObject);


        //一般
        playerNum = playerCtrl.PlayerNUM;
		if(playerNum == 1) playerNumString = "1";
		else if(playerNum == 2) playerNumString = "2";
		else if(playerNum == 3) playerNumString = "3";
		else if(playerNum == 4) playerNumString = "4";

		if (!gameCtrl.isAI [playerNum - 1]) {
			if (MarkString != "P" + playerNumString)
				GameObject.Destroy (this.gameObject);
		}
		else{
			if(MarkString != "CP")
				GameObject.Destroy (this.gameObject);
		}

		this.transform.GetComponent<SpriteRenderer> ().enabled = true;


	}
	

	void Update () {
		if(playerCtrl.dir == 1)this.transform.localScale = new Vector3(oriScale.x, oriScale.y, oriScale.z);
		else this.transform.localScale = new Vector3(-oriScale.x, oriScale.y, oriScale.z);
	}

}
