using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAudio : MonoBehaviour {

    AudioSource audioCtrl;
    public AudioClip[] objectSE = new AudioClip[4];
    public float[] objectSEPitch = new float[4];

    public int audionum = 1;
   


    void Start () {
        audioCtrl = GetComponent<AudioSource>();

        if (audioCtrl)
        {
            for (int i = 0; i < audionum; i++)
            {
                audioCtrl.pitch = objectSEPitch[i] + Random.Range(-0.05f, 0.05f);
                audioCtrl.PlayOneShot(objectSE[i]);
            }
        }

    }
	
	void Update () {
		
	}
}
