using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    public void UpdateFOV(System.Single fov){
        PlayerPreferences.FOV = fov;
    }

    public void UpdateVolume(System.Single vol){
        PlayerPreferences.volume = vol;
    }
}
