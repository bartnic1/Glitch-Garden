using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Note that you don't actually need to pass an instance of the gameManager into your start/options/quit buttons - you can also load the prefab in directly,
    //and delete the gameManager game object from your hierarchy! The downside to this is that you can't specify any public options, but in this case, since
    //all you need are the functions, they aren't needed. In general, the instructor recommends using prefabs for this purpose.

    public delegate void gameState();
    public static event gameState endEvent;
    public static event gameState startEvent;
    public int startGameDelay;

    MusicManager musicManager;
    public GameObject victoryText;
    GameObject introText;

    private void Start()
    {
        //You can't find objects that are disabled when the scene loads; thus the victory text game object must be
        //defined in the game manager's inspector. Fortunately you don't need to do this for the intro text, which starts enabled:

        introText = GameObject.Find("Introductory Text");

        //Only run this if you are in the Start scene. Otherwise there is no point resetting the musicManager variable or the volume.
        //This ensures the music volume is equal to the volume slider setting in the options scene.

        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
                musicManager.SetVolume(PlayerPrefsManager.GetMasterVolume());
                break;
            case 3:
                Invoke("StartGame", startGameDelay);
                break;
            case 4:
                Invoke("StartGame", startGameDelay);
                break;
            case 5:
                Invoke("StartGame", startGameDelay);
                break;
            default:
                break;
        }
    }

    void StartGame()
    {
        introText.SetActive(false);
        startEvent();
    }

    public void LoadLevel(string name)
    {
        //Works with strings as well as build index integers!
        SceneManager.LoadScene(name);
    }

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShutDown()
    {
        print("Quit Requested");
        Application.Quit();
    }

    public void Victory()
    {
        //An audio source is attached to the victory text, which should play on activation.
        victoryText.SetActive(true);
        //It would be even better if you could flip the animations and have them run away, but for now freezing them will do.
        endEvent();
        Invoke("LoadNext", 5f);
    }
}
