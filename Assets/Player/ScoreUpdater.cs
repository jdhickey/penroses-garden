using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    TextMeshProUGUI scoreCounter;

    void Start(){
        scoreCounter = GetComponent<TextMeshProUGUI>();
    }
    
    void FixedUpdate(){
        scoreCounter.SetText(LevelManager.playerScore.ToString());
    }
}
