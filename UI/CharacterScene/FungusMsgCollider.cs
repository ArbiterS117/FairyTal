using UnityEngine;
using Fungus;

public class FungusMsgCollider : MonoBehaviour {

    public Flowchart FunctionFlowChart;

    public int isOnButton = 0;

    bool hasSend = false;
    public string SIn = null;
    public string Sout = null;


    void Update()
    {
        if (isOnButton >= 1)
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

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        isOnButton += 1;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        isOnButton -= 1;
    }

    //===============================================
}
