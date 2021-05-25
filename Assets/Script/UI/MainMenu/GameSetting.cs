using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GameSetting : MonoBehaviour {

    public Resolution[] resolutions = new Resolution[4];

    void Start () {
		
	}
	
	
	void Update () {
		
	}

    //==========================================================
    public void SetBGMVolume(float volume)
    {

    }

    public void SetSEVolume(float volume)
    {
        
    }

    //qualityIndex : 0:Low 1:High 2:Fantastic
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool b)
    {
        Screen.fullScreen = b;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
