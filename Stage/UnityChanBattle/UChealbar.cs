using UnityEngine;
using System.Collections;

public class UChealbar : MonoBehaviour {

	public NPCEnemyBase NPCdata;

    public healBarHP _healBarHP; // 程式快取
    public healBarDamagedHP _healBarDamagedHP;

    public float maxhealth = 100.0f;
	public float curhealth = 50.0f;
	public float healthBarLen;

	public bool isTargeted = false;

	void Awake(){
		NPCdata = GameObject.FindGameObjectWithTag("NPC").GetComponent<NPCEnemyBase>();
        _healBarHP = GetComponentInChildren<healBarHP>();
        _healBarDamagedHP = GetComponentInChildren<healBarDamagedHP>();

    }

	void Start(){

	}

	void Update()
	{
		//抓取目標
		if (!isTargeted) {
			maxhealth = NPCdata.HP;
			curhealth = maxhealth; 
			healthBarLen = (curhealth / maxhealth);

			isTargeted = true;
		}
		curhealth = NPCdata.HP;

		//==============計算 顯示=====================
		AddjustCurrentHealth(0);
        _healBarHP.healthBarLen = healthBarLen;
        _healBarDamagedHP.healthBarLen = healthBarLen;
	}
	
	//========================================自訂=========================================
	
	//======計算血量比例========
	public void AddjustCurrentHealth(float adj){
		curhealth += adj;
		if (curhealth < 0)
			curhealth = 0;
		if (curhealth > maxhealth)
			curhealth = maxhealth;
		if (maxhealth < 1)
			maxhealth = 1;
		healthBarLen = (curhealth / maxhealth);
	}

}
