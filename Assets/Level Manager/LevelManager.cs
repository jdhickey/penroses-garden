using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static int playerScore = 0;
    public static bool initialTile = false; // Should an initial tile be placed?
    public static bool pointPerTile = false; // Should the player earn 1 point per tile?
    public static int randomPreTile = 0; // How many pre-placed tile should there be?
    public static int winThreshold = 1000;
    public static bool won = false;
    public static bool lost = false;
}
