using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TipText : MonoBehaviour {

    public Text tipT;

    float TipsNum = 8.0f;
    string tip1 = "衝刺取消 : 在普攻時可以透過衝刺來取消目前動作 !";
    string tip2 = "衝刺跳躍 : 如果在衝刺中按下跳躍，可以跳得更遠 !";
    string tip3 = "連段攻擊 : 在普攻的時候試著加入方向鍵或是招式 !";
    string tip4 = "終結技 : 在HP條閃爍時按下終結技指令P即可使用 !";
    string tip5 = "二段跳 : 在滯空時再次按下跳躍鍵即可二段跳 !";
    string tip6 = "受身 : 在倒地或是擊飛狀態時按下跳躍鍵 能更快地起身 !";
    string tip7 = "招式方向 : 部分腳色使出招式時，可透過搖桿改變招式方向 !";
    string tip8 = "招式 : 搖桿上下狀態的不同，能使出的招式也會不同 !";


    void Start () {
        float randomNUM = Random.Range(0.0f, TipsNum);
        if (randomNUM <= 1.0f)
        {
            tipT.text = tip1;
        }
        else if (randomNUM <= 2.0f)
        {
            tipT.text = tip2;
        }
        else if (randomNUM <= 3.0f)
        {
            tipT.text = tip3;
        }
        else if (randomNUM <= 4.0f)
        {
            tipT.text = tip4;
        }
        else if (randomNUM <= 5.0f)
        {
            tipT.text = tip5;
        }
        else if (randomNUM <= 6.0f)
        {
            tipT.text = tip6;
        }
        else if (randomNUM <= 7.0f)
        {
            tipT.text = tip7;
        }
        else if (randomNUM <= 8.0f)
        {
            tipT.text = tip8;
        }
    }
	
}
