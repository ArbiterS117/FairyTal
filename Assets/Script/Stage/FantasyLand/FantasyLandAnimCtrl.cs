using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasyLandAnimCtrl : MonoBehaviour {

	public Animator[] stageAnimators = new Animator[0];
    int animQuantity = 0;

    void Start () {
        animQuantity = stageAnimators.Length;

        for (int i = 0; i < animQuantity; i++)
        {
            stageAnimators[i].speed = Random.Range(0.7f, 1.5f);
        }
    }


    void Update () {

        

    }
}
