using UnityEngine;
using Fungus;

public class BtnManage : MonoBehaviour {

    public Flowchart FunctionFlowChart;
    public bool[] pointColliderIn = new bool[4]; // 是否有在
    public PointerCtrl[] pointerCtrl = new PointerCtrl[4]; // 註冊

    public int isOnButton = 0;

    bool hasSend = false;
    public string SIn = null;
    public string Sout = null;
    public string SPressedDown = null;
    public string SPressedUp = null;

    void Start () {

        FunctionFlowChart = GameObject.Find("Function Flowchart").GetComponent<Flowchart>();

		for(int i = 0; i < 4; i++)
        {
            pointerCtrl[i] = GameObject.Find("P" + (i + 1).ToString() + "Pointer").GetComponent<PointerCtrl>();
        }
	}
	
	
	void Update () {
		if(isOnButton >= 1)
        {
            if (!hasSend)
            {
                hasSend = true;
                FunctionFlowChart.SendFungusMessage(SIn);
            }
        }

        else
        {
            if (hasSend)
            {
                hasSend = false;
                FunctionFlowChart.SendFungusMessage(Sout);
            }
        }

        //按下
        for(int i = 0; i < 4; i++)
        {
            if (pointColliderIn[i])
            {
                if (pointerCtrl[i].isPressedDown) FunctionFlowChart.SendFungusMessage(SPressedDown);
                if (pointerCtrl[i].isPressedUp) FunctionFlowChart.SendFungusMessage(SPressedUp);
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int pointerNum = other.GetComponentInParent<PointerCtrl>().playerNUM;
            isOnButton += 1;

            pointColliderIn[pointerNum - 1] = true;

            if (pointerCtrl[pointerNum - 1].isPressed) pointerCtrl[pointerNum - 1].isPressedInbtn = true;

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isOnButton -= 1;
            pointColliderIn[other.GetComponentInParent<PointerCtrl>().playerNUM - 1] = false;

        }
    }

    //=======================按鈕功能在CharacterDataCtrl 防止多次宣告========================

    public void btnPointerReset()
    {
        for (int i = 0; i < 4; i++)
        {
            pointerCtrl[i].isPressed = false;
            pointerCtrl[i].isPressedDown = false;
            pointerCtrl[i].isPressedUp = false;
            pointerCtrl[i].isPressedInbtn = false;
        }
    }
    
   

}
