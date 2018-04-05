using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The music manager class creates a persistent music manager that maintains a presence in every scene after loading in the initial splash window.
//It uses the built-in sceneLoaded event from the SceneManager class to subscribe a function that, when called, changes the music depending on which
//scene has loaded.

//Also contains a setvolume function that can be used by other scripts to change the music volume.

public class MusicManager : MonoBehaviour {

    //Make sure your music files are .mp3, as .ogg files don't work on iOS!

    public AudioSource managerAudioSource;
    public AudioClip[] levelMusicChangeArray;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        //print("Don't destroy on load: " + gameObject.name);
    }
    /*
    The OnEnable() and OnDisable() functions describe when the objects are activated and deactivated, specifically, when a scene loads or when you leave a scene.
    sceneLoaded is a built-in event from the SceneManagement part of UnityEngine (imported at the top).
    
    Events are a special class of delegates, which in turn are like variables except that they can store functions. You can use the += operator
    to subscribe functions to existing delegates or events. That way, when the event occurs, the function is automatically called.

    Here, the SceneManager.sceneLoaded event occurs whenever the scene is loaded. You can then assign a function to that event, 
    though it must use the same parameters as the event's class (as shown in the onSceneLoaded function).

    Since you don't actually use scene or loadscenemode  parameters, both are ignored. In general it is always recommended to unsubscribe functions when disabled.
    This is important, because otherwise it could lead to errors and memory leaks. For this, the built-in onEnable and onDisable functions are useful.
    */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        //The level should both have music available to play, and it should not be the same as what is already playing, for the clip to change.
        if (levelMusicChangeArray[level] != null && levelMusicChangeArray[level] != managerAudioSource.clip)
        {
            managerAudioSource.clip = levelMusicChangeArray[level];
            managerAudioSource.Play();
        }

    }

    public void SetVolume(float optionsVolume)
    {
        managerAudioSource.volume = optionsVolume;
    }

    /*
    private void OnLevelWasLoaded(int level)
    {
        if(levelMusicChangeArray[level] != null && !GetComponent<AudioSource>().isPlaying)
        {
            managerAudioSource.clip = levelMusicChangeArray[level];
            managerAudioSource.Play();
        }
    }


    The instructor used the OnLevelWasLoaded(){} function. As with OnCollisionEnter2D(){}, this is a predefined function that allows the
    scene to do something once loaded. Note that "level" returns the same index as "SceneManager.GetActiveScene().buildIndex",  you don't
    need to specify anything! Unfortunately, this function is deprecated and will no longer be used in future unity updates.
     */


}
