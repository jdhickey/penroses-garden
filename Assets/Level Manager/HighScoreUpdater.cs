using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI highScoreCounter;

    void Start()
    {
        highScoreCounter = GetComponent<TextMeshProUGUI>();
        highScoreCounter.SetText(LevelManager.highscore.ToString());
    }
}
