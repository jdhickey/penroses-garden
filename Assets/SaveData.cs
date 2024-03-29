using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < 28; i++){
            if (PlayerPrefs.GetInt((i+1).ToString()) == 1){
                LevelManager.levels[i] = true;
            }
        }
        PlayerPreferences.FOV = PlayerPrefs.GetFloat("FOV");
        PlayerPreferences.volume = PlayerPrefs.GetFloat("Volume");
    }

    void OnDisable(){
        for (int i = 0; i < 28; i++){
            if (LevelManager.levels[i]){
                PlayerPrefs.SetInt((i+1).ToString(), 1);
            }
            else{
                PlayerPrefs.SetInt((i+1).ToString(), 0);
            }
        }
        PlayerPrefs.SetFloat("FOV", PlayerPreferences.FOV);
        PlayerPrefs.SetFloat("Volume", PlayerPreferences.volume);
        PlayerPrefs.Save();
    }
}
