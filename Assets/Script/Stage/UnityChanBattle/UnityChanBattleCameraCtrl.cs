using UnityEngine;

public class UnityChanBattleCameraCtrl : TRyCamera {

	
	public Transform UnityChanTransform;

   

    public override void Awake () 
	{
        base.Awake();
		UnityChanTransform = GameObject.FindGameObjectWithTag("NPC").transform;
	}

    public override void Start(){
		for (int i = 0; i < 4; i++) {
			addPlayerSwitch[i] = true;
		}
		targets.Add (UnityChanTransform);
	}

	
}
