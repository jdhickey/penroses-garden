using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    void Start()
    {
        // Level(bool initialTile, bool pointPerTile, int randomPreTile, int winThreshold, bool pointPerConnection, int timerVal, int winBySurround)
        LevelInformation.levels[2] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[3] = new Level(true, true, 2, 25, false, 0, 0);
        LevelInformation.levels[4] = new Level(true, true, 3, 25, false, 600, 0);
        LevelInformation.levels[5] = new Level(true, true, 5, 25, false, 300, 0);
        LevelInformation.levels[6] = new Level(false, true, 0, 10, false, 150, 0);
        LevelInformation.levels[7] = new Level(true, true, 5, 50, false, 0, 0);
        LevelInformation.levels[8] = new Level(true, false, 0, 0, false, 0, 1);
        LevelInformation.levels[9] = new Level(true, false, 0, 0, false, 300, 1);
        LevelInformation.levels[10] = new Level(false, true, 0, 50, false, 300, 2);
        LevelInformation.levels[11] = new Level(true, true, 3, 100, false, 0, 1);
        LevelInformation.levels[12] = new Level(true, false, 0, 65, true, 300, 2);
        LevelInformation.levels[13] = new Level(false, false, 2, 100, true, 0, 0);
        LevelInformation.levels[14] = new Level(true, false, 2, 0, false, 300, 2);
        LevelInformation.levels[15] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[16] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[17] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[18] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[19] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[20] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[21] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[22] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[23] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[24] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[25] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[26] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[27] = new Level(true, true, 0, 10, false, 0, 0);
        LevelInformation.levels[28] = new Level(true, true, 0, 10, false, 0, 0);
    }
}
