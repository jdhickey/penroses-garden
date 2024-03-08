using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferencesReader : MonoBehaviour
{
    private ColorBlindFilter colorBlindFilterScript;

    void OnEnable(){
        colorBlindFilterScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ColorBlindFilter>();
        ColorBlindMode mode = PlayerPreferences.colourBlindMode;
        colorBlindFilterScript.mode = mode;
    }
}
