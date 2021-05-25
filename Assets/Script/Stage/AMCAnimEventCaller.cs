using System.Collections;
using UnityEngine;

public class AMCAnimEventCaller : MonoBehaviour {

    Animator anim;
    public Animator ShipAnim;
    public Animator castleAnim;
    AutoMoveCamera AMCamera;
    GameCtrl gameCtrl;

    //=============相機動畫呼叫用===========================

    public void Awake()
    {
        AMCamera = GetComponentInChildren<AutoMoveCamera>();
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        anim = GetComponent<Animator>();
       
    }

    public void Start()
    {
        
        float randomNUM = Random.Range(0.0f, 4.0f);
        if (randomNUM <= 1.0f) anim.SetInteger("Part", 0);
        else if (randomNUM <= 2.0f) anim.SetInteger("Part", 1);
        else if (randomNUM <= 3.0f) anim.SetInteger("Part", 2);
        else if (randomNUM <= 4.0f)
        {
            //飛船部分
            anim.SetInteger("Part", 3);
            ShipAnim.SetTrigger("Fly");
            castleAnim.SetTrigger("GameStart");
        }
        

        //測試
        /*
        anim.SetInteger("Part", 3);
        ShipAnim.SetTrigger("Fly");
        castleAnim.SetTrigger("GameStart");
        */
    }

    private void Update()
    {
        if (!gameCtrl.CountDownComplete)
        {
            anim.speed = 0.1f;
        }
        else anim.speed = 1;
    }

    public void StopPosTrigger()
    {
        AMCamera.StopPosTrigger();
    }

    public void CallAnimation(int id)
    {
        AMCamera.CallAnimation(id);
    }

}
