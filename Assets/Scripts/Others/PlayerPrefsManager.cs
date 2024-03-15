using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    public static string UserId
    {
        set
        {
            PlayerPrefs.SetString("userId", value);
            PlayerPrefs.Save();
        }
        get => PlayerPrefs.GetString("userId");
    }

    public static int Level
    {
        set
        {
            PlayerPrefs.SetInt("level", value);
            PlayerPrefs.Save();
        }
        get => PlayerPrefs.GetInt("level");
    }
}
