using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySceneBGMCtrl : MonoBehaviour {


    AudioSource audioCtrl;
    public VictorySceneDataCtrl gameCtrl;


    public AudioClip LoopBGM;

    public AudioClip REDIntro;
    public AudioClip ALICEIntro;
    public AudioClip MOMOTAROIntro;
    public AudioClip SNOWWHITEIntro;
    public AudioClip RAPUNZELIntro;
    public AudioClip ALADDINIntro;

    bool isLooping = false;
    int winnerPlayerNum = 0;

    bool isPlayedIntro = false;


    void Awake()
    {
        audioCtrl = GetComponent<AudioSource>();
       
    }

    private void Start()
    {
        audioCtrl.loop = false;
        //audioCtrl.clip = IntroBGM;
        //audioCtrl.Play();
        //audioCtrl.time = playStartTimeIntro;
    }

    void Update()
    {

        if(isPlayedIntro == false)
        {
            setCharacterBGM();
        }


        if (!audioCtrl.isPlaying && isLooping == false && isPlayedIntro)
        {
            isLooping = true;
            audioCtrl.loop = true;
            audioCtrl.clip = LoopBGM;
            audioCtrl.Play();
        }




    }

    void setCharacterBGM()
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameCtrl.PlayerRank[i] == 1) winnerPlayerNum = i + 1;
        }
        if (winnerPlayerNum != 0)
        {
            int characterIndex = gameCtrl.dataCtrl.returnCharacterIndex(winnerPlayerNum - 1);
            if (characterIndex == 0 || characterIndex == 1)
            {
                audioCtrl.clip = REDIntro;
            }
            else if (characterIndex == 2 || characterIndex == 3)
            {
                audioCtrl.clip = ALICEIntro;
            }
            else if (characterIndex == 4 || characterIndex == 5)
            {
                audioCtrl.clip = MOMOTAROIntro;
            }
            else if (characterIndex == 6 || characterIndex == 7)
            {
                audioCtrl.clip = SNOWWHITEIntro;
            }
            else if (characterIndex == 8 || characterIndex == 9)
            {
                audioCtrl.clip = RAPUNZELIntro;
            }
            else if (characterIndex == 10 || characterIndex == 11)
            {
                audioCtrl.clip = ALADDINIntro;
            }
            isPlayedIntro = true;
            audioCtrl.Play();
        }
    }
}
