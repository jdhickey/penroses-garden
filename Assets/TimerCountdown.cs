using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    private TextMeshProUGUI textVal;

    void Start(){
        textVal = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate(){
        if (LevelManager.timerVal > 0){
            LevelManager.timerVal -= Time.deltaTime;
            textVal.SetText(LevelManager.timerVal.ToString("0.00"));
        }
        if (LevelManager.timerVal < 0)
        {
            // Call a "Lose" function.
        }

    }
}
