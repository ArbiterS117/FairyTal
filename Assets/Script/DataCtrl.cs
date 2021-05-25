using UnityEngine;

public class DataCtrl : MonoBehaviour {

    //選擇腳色相關
	static bool[] isInBattle = new bool[4];

	static int[] SelectedCharacterIndex = new int[4];

    static int[] TeamNum = new int[4]; //隊伍變數 總共只有1 2 3  如果不是隊伍模式 就是1 2 3 4 在選角色時設置

    //遊戲設置相關
    static bool hasInit; // 最開始初始

    static float gameTime;
    static float SurvivalTime;

    static bool SurvivalMode;
    static int  StuckNum;
    static bool TeamMode;
    static bool MPEnable;
    static bool UnlimitMP;

    static bool CanPause;

    public float _gameTime;
    public bool _survivalMode;
    public bool _teamMode;


	/*======================
	0:RED
	1:ALICE
	2:MOMOTARO
	3:RED AI
	4:ALICE: AI
	5:MOMOTARO AI
	========================*/

    //遊戲結算用
	static int[] PlayerPoint = new int[4];
    static float[] playerLifeTime = new float[4]; // 存活時間

    private void Awake()
    {
        if (!hasInit)
        {
            hasInit = true;

            gameTime = 2.0f;
            SurvivalMode = false;
            SurvivalTime = 5.0f;
            StuckNum = 3;
            TeamMode = false;
            MPEnable = true;
            UnlimitMP = false;
            CanPause = true;

            TeamNum[0] = 1;
            TeamNum[1] = 1;
            TeamNum[2] = 1;
            TeamNum[3] = 1;
        }
    }

    void Update () {
        _gameTime = gameTime;
        _survivalMode = SurvivalMode;
        _teamMode = TeamMode;

        if (Input.GetKeyDown((KeyCode.Escape))) ExitGame();
    }

    // GET & SET
    #region
    public void setIsInBattle(int PlayerNUM, bool i){
		isInBattle [PlayerNUM - 1] = i;
	}

	public void setCharacterIndex(int PlayerNUM, int i){
		SelectedCharacterIndex [PlayerNUM - 1] = i;
	}

	public void setPlayerPoint(int PlayerNUM, int i){
		PlayerPoint [PlayerNUM - 1] = i;
	}

	public bool returnIsBattle(int i){
		return isInBattle [i];
	}

	public int returnCharacterIndex(int i){
		return SelectedCharacterIndex[i];
	}

	public int returnPlayerPoint(int i){
		return PlayerPoint[i];
	}

    public void setSurvivalTime(float f)
    {
        SurvivalTime = f;
    }

    public float getSurvivalTime()
    {
        return SurvivalTime;
    }

    public void setTeamNum(int playerNum, int teamNum)
    {
        TeamNum[playerNum - 1] = teamNum;
    }

    public int getTeamNum(int i)
    {
        return TeamNum[i];
    }

    public void setGameTime(float f)
    {
        gameTime = f;
    }

    public float getGameTime()
    {
        return gameTime;
    }

    public void setSurvivalMode(bool b)
    {
        SurvivalMode = b;
    }

    public bool getSurvivalMode()
    {
        return SurvivalMode;
    }

    public void setStuckNum(int i)
    {
        StuckNum = i;
    }

    public int getStuckNum()
    {
        return StuckNum;
    }

    public void setTeamMode(bool b)
    {
        TeamMode = b;
    }

    public bool getTeamMode()
    {
        return TeamMode;
    }

    public void setMPEnable(bool b)
    {
        MPEnable = b;
    }

    public bool getMPEnable()
    {
        return MPEnable;
    }

    public void setUnlimitMP(bool b)
    {
        UnlimitMP = b;
    }

    public bool getUnlimitMP()
    {
        return UnlimitMP;
    }

    public void setCanPause(bool b)
    {
        CanPause = b;
    }

    public bool getCanPause()
    {
        return CanPause;
    }

    public void setPlayerLifeTime(int PlayerNum, float _playerlifeTime)
    {
        playerLifeTime[PlayerNum - 1] = _playerlifeTime;
    }

    public float getPlayerLifeTime(int PlayerNum)
    {
        return playerLifeTime[PlayerNum - 1];
    }
#endregion

    // Exit
    public void ExitGame()
    {
        Application.Quit();
    }

}
