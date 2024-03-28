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
        LevelManager.pointPerTile = false;
        LevelManager.randomPreTile = 0;
        LevelManager.winThreshold = 0;
        LevelManager.won = false;
        LevelManager.lost = false;
    }
}
