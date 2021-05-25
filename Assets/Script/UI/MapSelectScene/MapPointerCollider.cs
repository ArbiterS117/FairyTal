using UnityEngine;
using System.Collections;

public class MapPointerCollider : MonoBehaviour {
	
	MapDataCtrl DataCtrl;
	MapSelectSceneCtrl SceneCtrl;

	public AudioSource audioCtrl;
	
	public AudioClip SlideInSE;
	public float SlideInSEPitch;
	public AudioClip ChooseSE;
	public float ChooseSEPitch;
	public AudioClip CancelSE;
	public float CancelSEPitch;
	public AudioClip StartSE;
	public float StartSEPitch;

    Collider2D OnWatCollider = null;
	
	void Awake(){
		DataCtrl = GameObject.Find ("MapGameCtrl").GetComponent<MapDataCtrl> ();
		SceneCtrl = GameObject.Find("MapGameCtrl").GetComponent<MapSelectSceneCtrl> (); 
		//audioCtrl = GetComponent<AudioSource>();
	}

	void Start () {
        OnWatCollider = null;
    }
	

	void Update () {
        if (!DataCtrl.switchScene && OnWatCollider != null)
        {
            if (Input.GetButtonDown("P1Jump") || Input.GetButtonDown("P2Jump") ||
                Input.GetButtonDown("P3Jump") || Input.GetButtonDown("P4Jump"))
            {
                selectMap(OnWatCollider);
            }
        }
    }

	void OnTriggerEnter2D(Collider2D other){
		OnMapBlock (other);
        OnWatCollider = other;
    }

	void OnTriggerExit2D(Collider2D other){
        OnWatCollider = null;
        OutMapBlock (other);
	}


	//=================自創==========================

	//判斷選擇地圖
	void selectMap(Collider2D other){
		if (other.GetComponentInParent<MapBlockData> ().MapName == "BattleField") {
			DataCtrl.EndedSelectMap (other.GetComponentInParent<MapBlockData> ().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "MirrorCastle"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "CrazyGrandHouse"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "SkyWorld"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "Slpbty"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "SevenDraftMine"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}
		else if(other.GetComponentInParent<MapBlockData>().MapName == "Trainning"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}

		//=======暫時=================
		else if(other.GetComponentInParent<MapBlockData>().MapName == "UnityChanBattle"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}



		else if(other.GetComponentInParent<MapBlockData>().MapName == "DreamRush_CrazyGrandHouse"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}

		else if(other.GetComponentInParent<MapBlockData>().MapName == "MarioWorld"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}

		else if(other.GetComponentInParent<MapBlockData>().MapName == "Midgar"){
			DataCtrl.EndedSelectMap(other.GetComponentInParent<MapBlockData>().MapName);
			audioCtrl.pitch = StartSEPitch ;
			audioCtrl.PlayOneShot(StartSE);
		}




	}

	//判斷指標在於哪個地圖格上 用於顯示下方大圖
	void OnMapBlock(Collider2D other){
		if (other.GetComponentInParent<MapBlockData> ().MapName == "BattleField") {
			SceneCtrl.isOnBattleField = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "MirrorCastle"){
			SceneCtrl.isOnMirrorCastle = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "CrazyGrandHouse"){
			SceneCtrl.isOnCrazyGrandHouse = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "Trainning"){
			SceneCtrl.isOnTrainning = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "SkyWorld"){
			SceneCtrl.isOnSkyWorld = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "Slpbty"){
			SceneCtrl.isOnSlpbty = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "SevenDraftMine"){
			SceneCtrl.isOnSevenDraftMine = true;
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
		//==========暫時==============
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "UnityChanBattle"){
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}

		else if (other.GetComponentInParent<MapBlockData> ().MapName == "DreamRush_CrazyGrandHouse"){
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}

		else if (other.GetComponentInParent<MapBlockData> ().MapName == "MarioWorld"){
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}

		else if (other.GetComponentInParent<MapBlockData> ().MapName == "Midgar"){
			audioCtrl.pitch = SlideInSEPitch;
			audioCtrl.PlayOneShot(SlideInSE);
		}
	}

	void OutMapBlock(Collider2D other){
		if (other.GetComponentInParent<MapBlockData> ().MapName == "BattleField")
			SceneCtrl.isOnBattleField = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "MirrorCastle")
			SceneCtrl.isOnMirrorCastle = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "CrazyGrandHouse")
			SceneCtrl.isOnCrazyGrandHouse = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "SkyWorld")
			SceneCtrl.isOnSkyWorld = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "Slpbty")
			SceneCtrl.isOnSlpbty = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "SevenDraftMine")
			SceneCtrl.isOnSevenDraftMine = false;
		else if (other.GetComponentInParent<MapBlockData> ().MapName == "Trainning")
			SceneCtrl.isOnTrainning = false;
	}

}
