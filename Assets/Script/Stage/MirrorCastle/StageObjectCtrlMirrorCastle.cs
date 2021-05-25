using UnityEngine;
using System.Collections;

public class StageObjectCtrlMirrorCastle : MonoBehaviour {

	//外部變數

	public GameObject CureseNoteText;

	//欲產生的東西預製物與資訊
	public GameObject TPMirror;
	public GameObject BehindTPMirror;
	public GameObject CursedMirror;
    public GameObject Thunder;
	public Transform [] MirrorBirthPoint         = new Transform [3];
	public Transform [] BehindMirrorBirthPoint   = new Transform [2];

	//允許生成開關

	//生成物件
	public GameObject[] Mirror                   = new GameObject[4];
	public GameObject[] BehindMirror             = new GameObject[2];
	public GameObject[] CursedMirrorObj          = new GameObject[1];

	//遊戲物件生成控制參數
	public float StartColdBornTime      = 10.0f;
	public float TPMirrorBornTime       =  5.0f;
	public float TPMirrorBornRate       = 90.0f;
    public float TPMirrorlifeTime       = 13.0f;
	public float TPBehindMirrorBornTime =  8.0f;
	public float TPBehindMirrorBornRate = 80.0f;
	public float CursedMirrorBornTime   = 20.0f;
	public float CursedMirrorBornRate   = 75.0f;

    public float CursedMirrorStartColdTime = 1.0f;
    public float CursedMirrorCurseTime = 10.0f;
    public float CursedMirrorDestroyTime = 2.0f;

    public GameObject ThunderHiteffectObject;

    public AudioClip ThunderHittedSE;
    public float ThunderHittedSEPitch = 1.2f;

    public bool ThunderSideType = false;
    public int ThunderDamage = 20;
    public float ThunderKnockOutTime = 1.0f;
    public float ThunderKnockOutSpeedX = 3.0f;
    public float ThunderHitForceY = 900.0f;
    public float ThunderKnockOutGravity = 2.5f;
    public float ThunderKnockOutDecressSpeed = 0.0f;

    public GameObject LampHittedEffectObject;//玩家打中吊燈
    public AudioClip LamphittedSE;
    public float LamphittedSEPitch = 1.0f;
    public float LampHP = 10.0f;
    public float UpColdTime = 10.0f;

    public GameObject LampDMGEffectObject;//吊燈打中玩家
    public AudioClip LampDMGHittedSE;
    public float LampDMGHittedSEPitch;

    public bool LampDMGSideType = false;
    public int LampDMGDamage = 15;
    public float LampDMGKnockOutTime = 1.0f;
    public float LampDMGKnockOutSpeedX = 2.0f;
    public float LampDMGHitForceY = 1000.0f;
    public float LampKnockOutGravity = 2.5f;
    public float LampKnockOutDecressSpeed = 0.0f;

    //內部變數
    protected GameCtrl gameCtrl;


	float TPMirrorTime       = 0.0f;
	float TPBehindMirrorTime = 0.0f;
	float CursedMirrorTime   = 0.0f;

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
	}

	void Start () {
		
	}


	void Update () {
        float _deltaTime = Time.deltaTime;
		if (gameCtrl.GameTime >= StartColdBornTime) {

			//===========傳送魔鏡===================
			TPMirrorTime += _deltaTime;
			if (TPMirrorTime >= TPMirrorBornTime) {
				if (Random.Range (0.0f, 100.0f) < TPMirrorBornRate) {
					int i = Mathf.FloorToInt (Random.Range (0.0f, 2.99f));
                    if (Mirror[i] == null)
                    {
                        Mirror[i] = Instantiate(TPMirror, MirrorBirthPoint[i].position, Quaternion.identity) as GameObject;
                        Mirror[i].GetComponentInChildren<MirrorTeleportSpriteEffect>().lifeTime = TPMirrorlifeTime;
                        Destroy(Mirror[i], TPMirrorlifeTime);
                    }
				}
				TPMirrorTime = 0.0f;
			}

			TPBehindMirrorTime += _deltaTime;
			if (TPBehindMirrorTime >= TPBehindMirrorBornTime) {
				if (Random.Range (0.0f, 100.0f) < TPBehindMirrorBornRate) {
					int i = Mathf.FloorToInt (Random.Range (0.0f, 3.99f));
					if (BehindMirror [i] == null) {
						BehindMirror [i] = Instantiate (BehindTPMirror, BehindMirrorBirthPoint [i].position, Quaternion.identity) as GameObject;
                        BehindMirror[i].GetComponentInChildren<MirrorTeleportSpriteEffect>().lifeTime = TPMirrorlifeTime;
                        Destroy(BehindMirror[i], TPMirrorlifeTime);
                    }
				}
				TPBehindMirrorTime = 0.0f;
			}

			//=========詛咒魔鏡=========================
			if (CursedMirrorObj [0] == null) {
				CursedMirrorTime += _deltaTime;
				if (CursedMirrorTime >= CursedMirrorBornTime) {
					if (Random.Range (0.0f, 100.0f) < CursedMirrorBornRate) {
						CursedMirrorObj [0] = Instantiate (CursedMirror, new Vector3 (0.0f, 8.0f, 1.0f), Quaternion.identity)as GameObject;
					}
					CursedMirrorTime = 0.0f;
				}
				CureseNoteText.SetActive(false);//==詛咒警告文字
			}


			else{
				CureseNoteText.SetActive(true);//==詛咒警告文字
			}

		}
	}
}
