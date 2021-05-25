using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Fungus;

public class MainMenuManager : MonoBehaviour {

    public bool InputEnabled = true;

    public int FungusState;
    public string FunGusBackMessage;

    public Flowchart flowchart = null;

    //string SubmitString = "Submit";
    string CancelString = "Cancel";
    //string HorizontalString = "Horizontal";
    //string VerticalString = "Vertical";


    EventSystem _eventsystem;

    //記憶切換為滑鼠控制前的選項
    public GameObject CurrentSelectedbtn;

    //記憶離開頁面時的選項
    static int FungusStateMemory = 0;
    public bool isInMainMenu = true;

    //Intro
    public GameObject IntroList;

    public GameObject SMASHbtn;

    void Start () {
        if (isInMainMenu)
        {
            _eventsystem = EventSystem.current;
            FungusState = FungusStateMemory;

            flowchart.SetIntegerVariable("State", FungusState);

            if (FungusState == 1) { FunGusBackMessage = "VS Back"; _eventsystem.firstSelectedGameObject = SMASHbtn; }
            else if (FungusState == 2) FunGusBackMessage = "Solo Back";
            else if (FungusState == 3) FunGusBackMessage = "Setting Back";
            else if (FungusState == 4) FunGusBackMessage = "Data Back";
            else if (FungusState == 5) FunGusBackMessage = "Custom Rules Back";
            else if (FungusState == 6) FunGusBackMessage = "Extra Rules Back";

        }

    }
	
	void Update () {

        if (isInMainMenu)
        {

            FungusState = flowchart.GetIntegerVariable("State");

            if (FungusState != 0) // 在Main Menu
            {
                if (Input.GetButtonDown(CancelString))
                {
                    flowchart.SendFungusMessage(FunGusBackMessage);
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

    //============Fungus Fuction===============================

   
    public void ReceiveFMessage(string s)
    {
        FunGusBackMessage = s;
    }

    public void SetSelecedbtn(GameObject _button)
    {
        CurrentSelectedbtn = _button;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public int GetFungusStateMemory()
    {
        return FungusStateMemory;
    }

    public void SetFungusStateMemory(int i)
    {
        FungusStateMemory = i;
    }

    public void TriggerFungusMSG(string s)
    {
        flowchart.SendFungusMessage(s);
    }

    public void SetIntro(GameObject intro)
    {
        //IntroList.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < IntroList.transform.childCount; i++) {
             IntroList.transform.GetChild(i).gameObject.SetActive(false);
        }

        if(intro != null) intro.SetActive(true);
    }

}
