using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is responsible for spawning attackers, and is attached separately to multiple empty game objects that act as spawn points for each lane.
//Note that the spawn period for each attacker is defined in the unity editor under the attacker component script.

public class AttackerSpawner : MonoBehaviour {

    public GameObject[] spawnerPrefabArray;
    public float[] alphaSpawnPeriods = new float[] { };
    public bool timeBased;

    private int sPALength;
    private float[] spawnTimer;
    private bool spawnOn = false;

    GameObject spawners;

    private void OnEnable()
    {
        GameManager.endEvent += Endspawn;
        GameManager.startEvent += Startspawn;
    }

    private void OnDisable()
    {
        GameManager.endEvent -= Endspawn;
        GameManager.startEvent -= Startspawn;
    }
    
    // Use this for initialization
    void Start () {
        sPALength = spawnerPrefabArray.Length;
        //Note that, when initializing the length of the float array, it also sets all the values to zero.
        spawnTimer = new float[sPALength];
        alphaSpawnPeriods = new float[sPALength];
        //Start by setting the spawnOn boolean to false. After some time has passed, an event is called in the game manager which will
        //allow spawning to begin.
        spawnOn = false;


        spawners = GameObject.Find("Spawners");
        if(spawners == null)
        {
            spawners = new GameObject("Spawners");
        }
        //Change spawn periods based on difficulty setting
        switch ((int)PlayerPrefsManager.GetDifficulty())
        {
            case 1:
                if(SceneManager.GetActiveScene().name == "02 Level 01")
                {
                    alphaSpawnPeriods[0] = 20f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 02")
                {
                    alphaSpawnPeriods[0] = 17f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 03")
                {
                    alphaSpawnPeriods[0] = 17f;
                    alphaSpawnPeriods[1] = 17f;
                }
                break;
            case 2:
                if (SceneManager.GetActiveScene().name == "02 Level 01")
                {
                    alphaSpawnPeriods[0] = 18f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 02")
                {
                    alphaSpawnPeriods[0] = 14f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 03")
                {
                    alphaSpawnPeriods[0] = 15f;
                    alphaSpawnPeriods[1] = 15f;
                }
                break;
            case 3:
                if (SceneManager.GetActiveScene().name == "02 Level 01")
                {
                    alphaSpawnPeriods[0] = 15f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 02")
                {
                    alphaSpawnPeriods[0] = 10f;
                }
                else if (SceneManager.GetActiveScene().name == "02 Level 03")
                {
                    alphaSpawnPeriods[0] = 10f;
                    alphaSpawnPeriods[1] = 10f;
                }
                break;
            default:
                break;
        }

        //Ensure spawn periods are never too small
        for(int i=0; i<sPALength; i++)
        {
            if (alphaSpawnPeriods[i] == 0)
            {
                alphaSpawnPeriods[i] = 20f;
            }
            if (alphaSpawnPeriods[i] < Time.deltaTime)
            {
                Debug.LogWarning(spawnerPrefabArray[i].name + "spawn period lower than frame rate cap! Resetting to one second");
                alphaSpawnPeriods[i] = 1.0f;
            }
        }
    }

    private void Update()
    {
        //Time-based, no randomness, no need to make framerate independent:
        if (spawnOn)
        {
            if (timeBased)
            {
                for (int i = 0; i < sPALength; i++)
                {
                    if (Time.time - spawnTimer[i] > alphaSpawnPeriods[i])
                    {
                        Spawn(spawnerPrefabArray[i]);
                        spawnTimer[i] = Time.time;
                    }
                }
            }

            //The instructor used Random.value so that if the value was below a threshold (which was spawnRate*Time.deltaTime), then spawning would occur.
            //This was calle every frame, in the update method. Note that you multiply by Time.deltaTime to get a frame indepenent spawn rate (otherwise,
            //higher framerate computers will call the update function more often, and hence you get a higher probability to spawn than slower computers).
            else
            {
                for (int i = 0; i < sPALength; i++)
                {
                    if (Random.value < Time.deltaTime / alphaSpawnPeriods[i])
                    {
                        Spawn(spawnerPrefabArray[i]);
                    }
                }
            }
        }
    }

    void Spawn(GameObject myGameObject)
    {
        GameObject newSpawn = Instantiate(myGameObject, transform.position, Quaternion.identity);
        newSpawn.transform.SetParent(transform);
    }
    
    void Endspawn()
    {
        spawnOn = false;
    }

    void Startspawn()
    {
        spawnOn = true;
    }
}
