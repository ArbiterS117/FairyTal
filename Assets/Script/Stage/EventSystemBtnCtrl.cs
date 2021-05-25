using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;

public class EventSystemBtnCtrl : MonoBehaviour {

    EventSystem _eventsystem;

    public GameObject CurrentSelectedbtn;

    void Awake () {
        _eventsystem = GetComponent<EventSystem>();

    }
	
	void Update () {
        if (_eventsystem.IsPointerOverGameObject())
        {
            _eventsystem.SetSelectedGameObject(CurrentSelectedbtn);
        }

        if (_eventsystem.currentSelectedGameObject == null)
        {
            _eventsystem.SetSelectedGameObject(CurrentSelectedbtn);
        }
    }

    //=====================================

    public void SetSelecedbtn(GameObject _button)
    {
        CurrentSelectedbtn = _button;
    }
}
