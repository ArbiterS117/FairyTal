using UnityEngine;

public class MapBlockData : MonoBehaviour {

	public MapSelectSceneCtrl SceneCtrl;
	public string MapName;

	void Awake(){

		SceneCtrl = GameObject.Find("MapGameCtrl").GetComponent<MapSelectSceneCtrl>();

	}

	void Update(){

		//BattleField
		if (SceneCtrl.isOnBattleField) {
			if(MapName == "BattleField"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "BattleField"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		//MirrorCastle
		if (SceneCtrl.isOnMirrorCastle) {
			if(MapName == "MirrorCastle"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "MirrorCastle"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		//CrazyGrandHouse
		if (SceneCtrl.isOnCrazyGrandHouse) {
			if(MapName == "CrazyGrandHouse"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("PSText").transform.gameObject.SetActive(true);
			}
		}
		else{
			if(MapName == "CrazyGrandHouse"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("PSText").transform.gameObject.SetActive(false);
			}
		}

		//SkyWorld
		if (SceneCtrl.isOnSkyWorld) {
			if(MapName == "SkyWorld"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "SkyWorld"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		//Slpbty
		if (SceneCtrl.isOnSlpbty) {
			if(MapName == "Slpbty"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "Slpbty"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		//SevenDraftMine
		if (SceneCtrl.isOnSevenDraftMine) {
			if(MapName == "SevenDraftMine"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "SevenDraftMine"){
				transform.Find("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
				transform.Find("TextSprite").GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		/*
		//Trainning
		if (SceneCtrl.isOnTrainning) {
			if(MapName == "Trainning"){
				transform.FindChild("OutSideBlock").GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else{
			if(MapName == "Trainning"){
				transform.FindChild("OutSideBlock").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
		*/

	}


	
}
