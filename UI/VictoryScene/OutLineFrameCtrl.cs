using UnityEngine;

public class OutLineFrameCtrl : MonoBehaviour {

    VictorySceneDataCtrl gameCtrl;

	public int playerNum = 0;

	void Awake(){
		gameCtrl = GetComponentInParent<VictorySceneDataCtrl>();
	}

	void Start () {
		
	}
	

	void Update () {

        //===============================不同隊
        if (!gameCtrl.dataCtrl.getTeamMode())
        {
            if (gameCtrl.isInBattle[playerNum - 1])
            {
                if (gameCtrl.PlayerRank[playerNum - 1] == 1)
                {
                    transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = true;
                    transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                    transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else
            {
                transform.Find("frameSprite").gameObject.SetActive(false);
                transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //================================同隊
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (gameCtrl.PlayerRank[i] == 1)
                {
                    // 紅隊伍
                    if (gameCtrl.dataCtrl.getTeamNum(i) == 1) 
                    {
                        if(playerNum == 2)
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = true;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
                        }
                    } 
                    //籃隊
                    else if (gameCtrl.dataCtrl.getTeamNum(i) == 2)
                    {
                        if (playerNum == 1)
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = true;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    //綠隊
                    else if (gameCtrl.dataCtrl.getTeamNum(i) == 3)
                    {
                        if (playerNum == 3)
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = true;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                            transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    //關掉紫色
                    if(playerNum == 4)
                    {
                        transform.Find("frameSprite").gameObject.SetActive(false);
                        transform.Find("frameSprite").GetComponent<SpriteRenderer>().enabled = false;
                        transform.Find("winnerSprite").GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }

        }

	}
}
