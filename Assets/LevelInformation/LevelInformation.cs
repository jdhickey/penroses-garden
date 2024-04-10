using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelInformation
{
    public static Level[] levels = new Level[29];
}

public struct Level{
    public bool initialTile;
    public bool pointPerTile;
    public int randomPreTile;
    public int winThreshold;
    public bool pointPerConnection;
    public int timerVal;
    public int winBySurround;
    public int winByConnect;

    public Level(bool initialTile, bool pointPerTile, int randomPreTile, int winThreshold, bool pointPerConnection, int timerVal, int winBySurround, int winByConnect){
        this.initialTile = initialTile;
        this.pointPerTile = pointPerTile;
        this.randomPreTile = randomPreTile;
        this.winThreshold = winThreshold;
        this.pointPerConnection = pointPerConnection;
        this.timerVal = timerVal;
        this.winBySurround = winBySurround;
        this.winByConnect = winByConnect;
    }

    public void SetUpLevel(int levelNumber){
        LevelManager.initialTile = this.initialTile;
        LevelManager.pointPerTile = this.pointPerTile;
        LevelManager.randomPreTile = this.randomPreTile;
        LevelManager.winThreshold = this.winThreshold;
        LevelManager.pointPerConnection = this.pointPerConnection;
        LevelManager.timerVal = this.timerVal;
        LevelManager.winBySurround = this.winBySurround;
        LevelManager.winByConnect = this.winByConnect;
        LevelManager.currLevel = levelNumber;
    }
}