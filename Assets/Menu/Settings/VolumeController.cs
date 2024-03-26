using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate(){
        if (PlayerPreferences.volume != audioSource.volume){
            audioSource.volume = PlayerPreferences.volume;
        }
    }
}
