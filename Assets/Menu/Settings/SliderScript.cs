using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;

    void OnEnable(){
        if (gameObject.name == "FOVSlider"){
            slider.value = PlayerPreferences.FOV;
        }
        else if (gameObject.name == "VolumeSlider"){
            slider.value = PlayerPreferences.volume;
        }
    }

    public void UpdateFOV(System.Single fov){
        PlayerPreferences.FOV = fov;
    }

    public void UpdateVolume(System.Single vol){
        PlayerPreferences.volume = vol;
    }
}
