using System.Collections;
using UnityEngine;
using TMPro;

public class WinnerSpriteCtrl : MonoBehaviour {

    public VictorySceneDataCtrl gameCtrl;

    public TextMeshProUGUI WinnerText;

    int winnerPlayerNum = 0;

    private void Awake()
    {
        
    }

    void Start () {
      
    }
	
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            if (gameCtrl.isInBattle[i])
            {
                if (gameCtrl.PlayerRank[i] == 1) winnerPlayerNum = i + 1;
            }
        }
        if (winnerPlayerNum != 0)
        {
            int characterIndex = gameCtrl.dataCtrl.returnCharacterIndex(winnerPlayerNum - 1);
            if (characterIndex == 0 || characterIndex == 1)
            {
                transform.Find("Mask").Find("RED").gameObject.SetActive(true);
                WinnerText.SetText("RED");
            }
            else if (characterIndex == 2 || characterIndex == 3)
            {
                transform.Find("Mask").Find("ALICE").gameObject.SetActive(true);
                WinnerText.SetText("ALICE");
            }
            else if (characterIndex == 4 || characterIndex == 5)
            {
                transform.Find("Mask").Find("MOMOTARO").gameObject.SetActive(true);
                WinnerText.SetText("MOMOTARO");
            }
            else if (characterIndex == 6 || characterIndex == 7)
            {
                transform.Find("Mask").Find("SNOWWHITE").gameObject.SetActive(true);
                WinnerText.SetText("SNOWWHITE");
            }
            else if (characterIndex == 8 || characterIndex == 9)
            {
                transform.Find("Mask").Find("RAPUNZEL").gameObject.SetActive(true);
                WinnerText.SetText("RAPUNZEL");
            }
            else if (characterIndex == 10 || characterIndex == 11)
            {
                transform.Find("Mask").Find("ALADDIN").gameObject.SetActive(true);
                WinnerText.SetText("ALADDIN");
            }

            if (gameCtrl.dataCtrl.getTeamMode())
            {
                int WinTeam = gameCtrl.dataCtrl.getTeamNum(winnerPlayerNum - 1);
                if (WinTeam == 1)
                {
                    WinnerText.SetText("");
                    transform.Find("Mask").Find("WinnerRT").gameObject.SetActive(true);
                }
                else if (WinTeam == 2)
                {
                    WinnerText.SetText("");
                    transform.Find("Mask").Find("WinnerBT").gameObject.SetActive(true);
                }
                else if (WinTeam == 3)
                {
                    WinnerText.SetText("");
                    transform.Find("Mask").Find("WinnerGT").gameObject.SetActive(true);
                }
            }

        }

    }
}
