using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  選角畫面動畫控制
/// </summary>
/// 
public class CharacterSelectSceneCtrl : MonoBehaviour {

    //控制參數

    MainMenuManager menuManager;


    //判斷參數

    //腳色滑入部分
    public int[]  PlayerImageStart = new int[4];
	public bool[] playerImageStartTrigger = new bool[4];

	//選定腳色部分
	public int[]  SelectedCharacterIndex = new int[4];
	public bool[] isSelected = new bool[4];
	public bool[] CancelSelected = new bool[4];


	public bool startOver;

    public void Awake()
    {
        menuManager = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<MainMenuManager>();

    }

    //=================自創==========================
    //腳色滑入部分
    public void setPlayerImageStart(int playerNUM, int character){
		PlayerImageStart [playerNUM - 1] = character;
		playerImageStartTrigger [playerNUM - 1] = true;

	}

	public void setPlayerImageStartIdle(int playerNUM){
		playerImageStartTrigger [playerNUM - 1] = false;
		
	}

	//選定腳色部分
	public void setSceneSelectedCharacterIndex(int playerNUM, int character){
		SelectedCharacterIndex [playerNUM - 1] = character;
		isSelected [playerNUM - 1] = true;
	}

	public void setSceneSelectedFalse(int playerNUM){
		if(isSelected [playerNUM - 1])CancelSelected[playerNUM - 1] = true;
		isSelected [playerNUM - 1] = false;

	}

    //切換場景部分
    public void ChangeSceneMainMenu()
    {
        menuManager.SetFungusStateMemory(1);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

}
