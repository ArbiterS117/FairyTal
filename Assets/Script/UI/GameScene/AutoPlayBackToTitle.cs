using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoPlayBackToTitle : MonoBehaviour {
	
	void Update () {
		//=======重置=============================
		
		if (Input.GetKeyDown (KeyCode.Return) || 
		    Input.GetButtonDown ("Reset") ||
		    Input.GetButtonDown ("Start")) {  
			Time.timeScale = 1;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }  

	}

}
