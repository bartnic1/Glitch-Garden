using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script stores various player preferences as key-value pairs. Various methods are defined that allow the player to set and get certain
//parameters, such as the volume, difficulty, or current level.

public class PlayerPrefsManager : MonoBehaviour {

    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string LEVEL_KEY = "level_unlocked_";

    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            //Note that LogError is different from Log in that a message will be printed to the console as a red error, rather than
            //as a simple message.
            Debug.LogError("Master volume out of range");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void UnlockLevel (int level)
    {
        if (level >= 0 && level <= (SceneManager.sceneCountInBuildSettings - 1))
        {
            //Use 1 for true (don't have bools)
            PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1);
        }
        else
        {
            Debug.LogError("Trying to unlock level not in build order");
        }
    }

    public static bool IsLevelUnlocked (int level)
    {
        if (level >= 0 && level <= (SceneManager.sceneCountInBuildSettings - 1))
        {
            if (PlayerPrefs.GetInt(LEVEL_KEY + level.ToString()) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("Level queried is not in build order");
            return false;
        }
    }

    public static void SetDifficulty(float difficultyLevel)
    {
        if (difficultyLevel <= 3 && 1 <= difficultyLevel)
        {
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficultyLevel);
        }
        else
        {
            Debug.LogError("Attempted to set difficulty outside of acceptable limits");
        }
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }


}
