using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script loads the start screen after a predetermined period of time on the opening splash screen. 

public class SplashLoadLevel : MonoBehaviour {

    public float splashLoadTime;

    // Use this for initialization
    void Start() {
        Invoke("SplashLoad", splashLoadTime);
    }

    void SplashLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    


}
