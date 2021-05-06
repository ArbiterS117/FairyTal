using UnityEngine;
using System.Collections;

public class StageObjectCtrlCrazyGrandHouse : MonoBehaviour {

	protected GameCtrl gameCtrl;
	
	//欲產生的東西預製物與資訊
	public GameObject DoorPrefab;
	public GameObject CrazyOldManPrefab;
	public GameObject GiftPrefab;

	public Transform  DoorPosition;
	public Transform  GiftBoxPosition;

	//生成物件

	public GameObject CrazyOldMan;
	public GameObject Gift;

	//遊戲物件生成控制參數
	public float StartColdBornTime      = 10.0f;
	       float StartColdBornCTime     =  0.0f;

	                              bool  BornSwitch             = false;
    [System.NonSerialized] public bool  DoorIsLive             = false;
    [System.NonSerialized] public bool  CrazyOldManIsLive      = false;//OldMan是否存在於場上
    [System.NonSerialized] public bool  GiftIsLive             = false;

    public float CrazyOldManBornRate    = 80.0f;//重生機率
    public float CrazyOldManBornTime    = 20.0f;//重生時間
           float CrazyOldManBornCTime   =  0.0f;
    public float DoorToCrazyOldManTime  =  1.0f;//門生成後到OldMan出現的時間
           float DoorToCrazyOldManCTime =  0.0f;

  	public float GiftBormRate           = 80.0f;
	public float GiftBornTime           = 10.0f;
	       float GiftBornCTime          =  0.0f;


	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
	}

	void Start () {
		BornSwitch        = false;
		CrazyOldManIsLive = false;
		DoorIsLive        = false;
		GiftIsLive        = false;
	}
	
	//=======Update==================================
	void Update () {

        float _deltaTime = Time.deltaTime;
		if (StartColdBornCTime < StartColdBornTime)
			StartColdBornCTime += _deltaTime; 
		else{
			//======生成CrazyOldMan(Door)====
			if(!CrazyOldManIsLive && !DoorIsLive){
				CrazyOldManBornCTime += _deltaTime;
				if(CrazyOldManBornCTime >= CrazyOldManBornTime){
					if(Random.Range (0.0f, 100.0f) < CrazyOldManBornRate){
						BornSwitch = true;
						Instantiate (DoorPrefab, DoorPosition.position, Quaternion.identity);
						DoorIsLive = true;
					}
					CrazyOldManBornCTime = 0.0f;
				}
			}

			if(DoorIsLive && !CrazyOldManIsLive && BornSwitch){
				DoorToCrazyOldManCTime += _deltaTime;
				if(DoorToCrazyOldManCTime >= DoorToCrazyOldManTime){
					if(CrazyOldMan == null){
						CrazyOldMan = Instantiate (CrazyOldManPrefab, new Vector3(DoorPosition.position.x, DoorPosition.position.y,0.0f), Quaternion.identity)as GameObject;
						CrazyOldManIsLive = true;
						BornSwitch = false;
						DoorToCrazyOldManCTime = 0.0f;
					}
				}
			}
			

			
			//======生成探望禮物箱==========

			if(Gift == null)GiftIsLive = false;

			if(!GiftIsLive){
				GiftBornCTime += _deltaTime;
				if(GiftBornCTime >= GiftBornTime){
					GiftIsLive = true;
					GiftBornCTime = 0.0f;
					
					if(Random.Range(0.0f,2.0f) >= 1.0f){//左
						Gift = Instantiate (GiftPrefab, new Vector3(Random.Range(-6.0f, -12.0f), Random.Range(-2.0f, 3.0f), 0), Quaternion.identity)as GameObject;
					}
					else{//右
						Gift = Instantiate (GiftPrefab, new Vector3(Random.Range(6.0f, 12.0f), Random.Range(-2.0f, 3.0f), 0), Quaternion.identity)as GameObject;
					}
				}
			}


		}
	}
	//===============================================

}
