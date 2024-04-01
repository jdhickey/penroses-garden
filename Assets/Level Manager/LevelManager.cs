using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static int playerScore = 0;
    public static bool initialTile = true; // Should an initial tile be placed?
    public static bool pointPerTile = false; // Should the player earn 1 point per tile?
    public static int randomPreTile = 0; // How many pre-placed tile should there be?
    public static int winThreshold = 0;
    public static bool pointPerConnection = false;
    public static bool won = false;
    public static bool lost = false;
    public static float timerVal = 0.0f; // Time in seconds that the clock should be set to.
    public static bool[] levels = new bool[28];
    public static int currLevel = -1;
    public static int winBySurround = 0;
    public static bool firstTilePlaced = false;
}