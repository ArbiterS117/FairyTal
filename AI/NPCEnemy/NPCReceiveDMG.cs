using UnityEngine;
using System.Collections;

public class NPCReceiveDMG : MonoBehaviour {

	NPCEnemyBase NPCData;

	//特殊效果
	//UnityChanCtrl unityCtrl;
	
	void Awake(){
		NPCData = GetComponentInParent<NPCEnemyBase>();
		//if(GetComponentInParent<UnityChanCtrl>()!=null)unityCtrl = GetComponentInParent<UnityChanCtrl>() ;
	}

	public void actionTakeDMG(int DMG){
		NPCData.HP -= DMG;
		//if(!unityCtrl)閃白光被打中效果
	}
	


}
