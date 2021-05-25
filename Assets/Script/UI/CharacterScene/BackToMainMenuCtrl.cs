using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class BackToMainMenuCtrl : MonoBehaviour {

    
    Flowchart FunctionFlowchart;

    public Animator animator;

    public bool isBacking = false;
    public int isCtrlNum = 0; // 有多少人在控制

    float _Time;

    //============參照==========
    public GameObject ArrowSprite1;
    public GameObject ArrowSprite2;

    public float BlinkTime = 0.3f;
           float BlinkCTime = 0.0f;
    void Awake () {
        
        FunctionFlowchart = GameObject.Find("Function Flowchart").GetComponent<Flowchart>();
        _Time = 0.05f; // init
    }
	
	void Update () {
        float _deltaTime = Time.deltaTime;

        // 防止動畫播超過一圈(-0.05) 動畫設定2秒 所以此處設定2秒
        if (_Time >= 1.95f)   //跳離
        {
            _Time = 1.95f;
            
            FunctionFlowchart.SendFungusMessage("BackToMainMenu");
            //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else if (_Time <= 0.05f) _Time = 0.05f;

        if (isBacking)
        {
            _Time += _deltaTime;
            animator.Play("Back",-1, _Time * 0.5f);
        }
        else
        {
            _Time -= _deltaTime;
        }
        animator.Play("Back", -1, _Time * 0.5f);


        

        //===========圖片閃爍================
        if (isBacking)
        {
            BlinkCTime += _deltaTime;
            if(BlinkCTime >= BlinkTime * 0.5f)
            {
                ArrowSprite1.SetActive(true);
                ArrowSprite2.SetActive(true);
            }
            if (BlinkCTime >= BlinkTime)
            {
                BlinkCTime = 0.0f;
                ArrowSprite1.SetActive(false);
                ArrowSprite2.SetActive(false);
            }
        }
        else
        {
            BlinkCTime = 0.0f;
            ArrowSprite1.SetActive(false);
            ArrowSprite2.SetActive(false);
        }
	}

    //==================================================

    public void SetIsBacking(bool b)
    {
        if (b) isCtrlNum++;
        else isCtrlNum--;

        if (isCtrlNum > 0) isBacking = true;
        else
        {
            isCtrlNum = 0;
            isBacking = false;
        }
    }

    public void ForceStopBaking() // 按入設定時
    {
        isCtrlNum = 0;
        isBacking = false;
    }

}
