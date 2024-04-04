using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;

    void OnEnable(){
        Debug.Log("I am the slider " + gameObject.name + ". FOV should be " + PlayerPreferences.FOV + " but is " + slider.value + " and volume should be " + PlayerPreferences.volume + ".");
        if (gameObject.name == "FOVSlider"){
            slider.value = PlayerPreferences.FOV;
        }
        else if (gameObject.name == "VolumeSlider"){
            slider.value = PlayerPreferences.volume;
            Debug.Log(PlayerPreferences.volume + " " + slider.value);
        }
    }

    public void UpdateFOV(System.Single fov){
        PlayerPreferences.FOV = fov;
    }

    public void UpdateVolume(System.Single vol){
        PlayerPreferences.volume = vol;
    }
}
