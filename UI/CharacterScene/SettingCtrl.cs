using UnityEngine;
using Fungus;
using UnityEngine.EventSystems;



public class SettingCtrl : MonoBehaviour {

    public Flowchart SettingFlowchart = null;
    EventSystem _eventsystem;

    //記憶切換為滑鼠控制前的選項
    public GameObject CurrentSelectedbtn;

    bool isSetting = false;
    bool isExtraSetting = false;
    string CancelString = "Cancel";

    void Start () {
        _eventsystem = EventSystem.current;
        SettingFlowchart = GameObject.Find("Rules Flowchart").GetComponent<Flowchart>();
    }
	
	void Update () {

        if (isSetting)
        {
            if (isExtraSetting)
            {
                if (Input.GetButtonDown(CancelString))
                {
                    SettingFlowchart.SendFungusMessage("Exit Extra Rules");
                }
            }
            else
            {
                if (Input.GetButtonDown(CancelString))
                {
                    SettingFlowchart.SendFungusMessage("Exit Setting");
                }
            }
            
            if (_eventsystem.IsPointerOverGameObject())
            {
                _eventsystem.SetSelectedGameObject(CurrentSelectedbtn);
            }

            if (_eventsystem.currentSelectedGameObject == null)
            {
                _eventsystem.SetSelectedGameObject(CurrentSelectedbtn);
            }

        }

	}

    //=======================================Custom
    
    public void SetSetting(bool b)
    {
        isSetting = b;
    }

    public void SetExtraSetting(bool b)
    {
        isExtraSetting = b;
    }

    public void SetSelecedbtn(GameObject _button)
    {
        CurrentSelectedbtn = _button;
    }

    public void SetSelecedbtnNULL()
    {
        CurrentSelectedbtn = null;
        _eventsystem.SetSelectedGameObject(null);
    }

}
