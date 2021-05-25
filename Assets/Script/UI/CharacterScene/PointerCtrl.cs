using UnityEngine;
using System.Collections;

public class PointerCtrl : MonoBehaviour {

	CharacterSelectSceneCtrl sceneCtrl;
	CharacterSelectDataCtrl  characterData;

    public BackToMainMenuCtrl backToMainMenuCtrl;

    //控制參數
    public float startFadeTime  = 0.5f;
	       float startFadeCTime = 0.0f;

	public float MoveSpeed      = 0.1f;

    public bool isPressedDown = false; // 新按鈕方法 有無按下
    public bool isPressedUp = false;
    public bool isPressed = false;

    public bool isPressedInbtn = false; //邊按下邊進入按鈕

    //判斷參數
    public bool moveEnable = false;
    public bool SettingStopMove = false;
    public bool SettingMap = false;

	//儲存玩家資訊
	public int playerNUM = 0;
    string playerVerticalString;
    string playerVerticalKeyString;
    string playerMenuVerticalString;
    string playerHorizontalString;
    string playerHorizontalKeyString;
    string playerMenuHorizontalString;
    string playerJumpString;

    SpriteRenderer _sprite;
    CircleCollider2D _collider;

    Vector2 oriPos;



	void Awake(){
		characterData = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectDataCtrl>();
		sceneCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<CharacterSelectSceneCtrl>();
        backToMainMenuCtrl = GameObject.Find("Back").GetComponent<BackToMainMenuCtrl>();
    }

	void Start () {
        _sprite = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _collider = this.transform.Find("Collider").GetComponent<CircleCollider2D>();

        _sprite.color = new Color(1,1,1,0);
        _collider.enabled = false;
        startStringCheck();

        oriPos = transform.position;
	}
	

	void Update () {

        if (!SettingStopMove && !SettingMap)
        {
            //加入遊戲
            if (sceneCtrl.startOver == true && characterData.isInCtrl[playerNUM - 1] == false && (
               Input.GetButtonDown(playerJumpString) ||
               Mathf.Abs(Input.GetAxis(playerHorizontalString)) > 0.0f ||
               Mathf.Abs(Input.GetAxis(playerVerticalString)) > 0.0f ||
               Mathf.Abs(Input.GetAxis(playerHorizontalKeyString)) > 0.0f ||
               Mathf.Abs(Input.GetAxis(playerVerticalKeyString)) > 0.0f)
               )
            {
                characterData.isInCtrl[playerNUM - 1] = true;
                backToMainMenuCtrl.SetIsBacking(false);
                if (characterData.isSelected[playerNUM - 1])
                {
                    characterData.isSelected[playerNUM - 1] = false;
                    sceneCtrl.setSceneSelectedFalse(playerNUM);
                    sceneCtrl.setPlayerImageStartIdle(playerNUM);
                }
            }


            //圖片顯示、是否可控制
            if (!moveEnable && sceneCtrl.startOver == true && characterData.isInCtrl[playerNUM - 1])
            {
                startFadeCTime += Time.deltaTime;
                _sprite.color = new Color(1, 1, 1, startFadeCTime / startFadeTime);
                if (startFadeCTime >= startFadeTime)
                {
                    _sprite.color = new Color(1, 1, 1, 1);
                    _collider.enabled = true;
                    moveEnable = true;
                }
            }

            //控制指標移動
            if (moveEnable)
            {
                pointerMove();
                checkPointerBlock();
            }

        }
	}

	//=================自創==========================


	void pointerMove(){

		float moveX = 0.0f;
		float moveY = 0.0f;


		moveX += Input.GetAxis(playerMenuHorizontalString);
		moveX += Input.GetAxis(playerHorizontalKeyString);
		moveY += Input.GetAxis(playerMenuVerticalString);
		moveY += Input.GetAxis(playerVerticalKeyString);

		if(moveX >= 1.0f)moveX = 1.0f;
		else if(moveX <= -1.0f)moveX = -1.0f;
		if(moveY >= 1.0f)moveY = 1.0f;
		else if(moveY <= -1.0f)moveY = -1.0f;

		moveX = moveX * MoveSpeed;
		moveY = moveY * MoveSpeed;

		this.transform.position += new Vector3(moveX, moveY, this.transform.position.z);

	}

	void checkPointerBlock(){
		if (this.transform.position.y >= 4.9f) {
			this.transform.position = new Vector3(this.transform.position.x, 4.9f, this.transform.position.z);
		}
		if (this.transform.position.x >= 9.1f) {
			this.transform.position = new Vector3(9.1f, this.transform.position.y, this.transform.position.z);
		}
		if (this.transform.position.y <= -4.9f) {
			this.transform.position = new Vector3(this.transform.position.x, -4.9f, this.transform.position.z);
		}
		if (this.transform.position.x <= -9.1f) {
			this.transform.position = new Vector3(-9.1f, this.transform.position.y, this.transform.position.z);
		}
	}

	public void eject(){
        _sprite.color = new Color(1,1,1,0);
        _collider.enabled = false;
		characterData.isInCtrl[playerNUM - 1] = false;
		moveEnable = false;
		this.transform.position = oriPos;
		startFadeCTime = 0.0f;
		
		
	}

    void startStringCheck()
    {
        if (this.transform.CompareTag("Player1"))
        {
            playerNUM = 1;
            playerJumpString = "P1Jump";
            playerVerticalString = "P1Vertical";
            playerVerticalKeyString = "P1VerticalKey";
            playerMenuVerticalString = "P1MenuVertical";
            playerHorizontalString = "P1Horizontal";
            playerHorizontalKeyString = "P1HorizontalKey";
            playerMenuHorizontalString = "P1MenuHorizontal";
        }
        else if (this.transform.CompareTag("Player2"))
        {
            playerNUM = 2;
            playerJumpString = "P2Jump";
            playerVerticalString = "P2Vertical";
            playerVerticalKeyString = "P2VerticalKey";
            playerMenuVerticalString = "P2MenuVertical";
            playerHorizontalString = "P2Horizontal";
            playerHorizontalKeyString = "P2HorizontalKey";
            playerMenuHorizontalString = "P2MenuHorizontal";
        }
        else if (this.transform.CompareTag("Player3"))
        {
            playerNUM = 3;
            playerJumpString = "P3Jump";
            playerVerticalString = "P3Vertical";
            playerVerticalKeyString = "P3VerticalKey";
            playerMenuVerticalString = "P3MenuVertical";
            playerHorizontalString = "P3Horizontal";
            playerHorizontalKeyString = "P3HorizontalKey";
            playerMenuHorizontalString = "P3MenuHorizontal";
        }
        else if (this.transform.CompareTag("Player4"))
        {
            playerNUM = 4;
            playerJumpString = "P4Jump";
            playerVerticalString = "P4Vertical";
            playerVerticalKeyString = "P4VerticalKey";
            playerMenuVerticalString = "P4MenuVertical";
            playerHorizontalString = "P4Horizontal";
            playerHorizontalKeyString = "P4HorizontalKey";
            playerMenuHorizontalString = "P4MenuHorizontal";
        }
    }

    public void SetSettingStopMove(bool b)
    {
        SettingStopMove = b;
    }

    public void SetMoveEnable(bool b)
    {
        moveEnable = b;
    }

    public void SetSettingMap(bool b)
    {
        SettingMap = b;
    }

}
