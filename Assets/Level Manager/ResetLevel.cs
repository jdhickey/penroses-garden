using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.playerScore = 0;
        LevelManager.initialTile = false;
        LevelManager.pointPerTile = true;
        LevelManager.randomPreTile = 0;
        LevelManager.winThreshold = 1000;
        LevelManager.won = false;
        LevelManager.lost = false;
        LevelManager.timerVal = 0;
        LevelManager.pointPerConnection = false;
        LevelManager.currLevel = -1;
        LevelManager.winBySurround = 0;
        LevelManager.firstTilePlaced = false;
        LevelManager.winByConnect = 0;
    }
}
