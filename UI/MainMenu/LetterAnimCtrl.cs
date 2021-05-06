using UnityEngine;

public class LetterAnimCtrl : MonoBehaviour {

    Animator animator;

    public int AnimNum = 0;

    bool _CanPlayNew = true;

    int lastPlayed = -1;

    private void Awake()
    {
        animator = this.transform.GetComponent<Animator>();
    }

    void Start () {
		
	}
	
	void Update () {
        if (_CanPlayNew)
        {
            _CanPlayNew = false;

            int randomNUM = 0;
            
            randomNUM = Mathf.FloorToInt(Random.Range(0.0f, AnimNum));
            if (randomNUM == AnimNum) randomNUM = AnimNum - 1; // 怕數值剛好取到4.0

            while(lastPlayed == randomNUM) // 跟上次一樣 重抽
            {
                randomNUM = Mathf.FloorToInt(Random.Range(0.0f, AnimNum));
                if (randomNUM == AnimNum) randomNUM = AnimNum - 1; // 怕數值剛好取到4.0
            }

            animator.SetInteger("play",randomNUM);
            lastPlayed = randomNUM;
        }
	}

    //===================================

    public void AnimSetTrigger()
    {
        _CanPlayNew = true;
    }
}
