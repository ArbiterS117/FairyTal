using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIntroLoop : MonoBehaviour {

    AudioSource audioCtrl;

    public AudioClip IntroBGM;
    public AudioClip LoopBGM;

    public float playStartTimeIntro;

    bool isLooping = false;


    void Awake()
    {
        audioCtrl = GetComponent<AudioSource>();
        audioCtrl.loop = false;
        audioCtrl.clip = IntroBGM;
        audioCtrl.Play();
        audioCtrl.time = playStartTimeIntro;
    }
    void Update()
    {
        if(!audioCtrl.isPlaying && isLooping == false)
        {
            isLooping = true;
            audioCtrl.loop = true;
            audioCtrl.clip = LoopBGM;
            audioCtrl.Play();
        }




    }

}