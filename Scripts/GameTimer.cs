using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    //Make level time an integer to keep things simple (in seconds).
    public int levelTime = 100;
    private float startTime;
    AudioSource audioSource;
    GameManager gameManager;
    Slider slider;

    // Use this for initialization
    private void OnEnable()
    {
        GameManager.startEvent += StartTimer;
    }

    private void OnDisable()
    {
        GameManager.startEvent -= StartTimer;
    }

    void StartTimer()
    {
        InvokeRepeating("AdvanceTimeSlider", 0.01f, 1f);
        startTime = Time.time;
    }

    void Start () {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        slider = GetComponent<Slider>();
        slider.maxValue = levelTime;
        slider.value = 0;
    }
	
    void AdvanceTimeSlider()
    {
        float elapsedTime = Time.time - startTime;
        if (elapsedTime <= levelTime)
        {
            slider.value = elapsedTime;
        }
        else
        {
            CancelInvoke("AdvanceTimeSlider");
            slider.value = levelTime;
            audioSource.Play();
            gameManager.Victory();
        }
    }
}
