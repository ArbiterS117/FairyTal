using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class MapDataCtrl : MonoBehaviour {

    public Flowchart FunctionFlowchart;

    string _SelectMapName;

	public bool  switchScene      = false;



	void Update () {
		
	}

	//==========自創================
	public void EndedSelectMap(string selectMapName){
		switchScene = true;
		//transform.Find("BlackScreen").GetComponent<MapBlackFadeCtrl>().GameStart = true;
		_SelectMapName = selectMapName;
        FunctionFlowchart.SendFungusMessage("EndSelectMap");



    }

	public void SwitchScene(){
		SceneManager.LoadScene(_SelectMapName, LoadSceneMode.Single);
	}
	



}
