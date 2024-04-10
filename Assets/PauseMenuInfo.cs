using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuInfo : MonoBehaviour
{
    public TextMeshProUGUI levelInfo;
    // Start is called before the first frame update
    void Start()
    {
        levelInfo = GetComponent<TextMeshProUGUI>();
        string levelInformation = "";
        if (LevelManager.currLevel == -1){
            levelInformation += "endless\n";
        }
        else if (LevelManager.currLevel == 1){
            levelInformation += "tutorial\n- learn your controls";
        }
        else{
            levelInformation += "Level " + LevelManager.currLevel.ToString() + "\n";
        }
        if (LevelManager.timerVal > 0){
            levelInformation += "- " + (((int) LevelManager.timerVal)/60+1).ToString() + " mins. on the timer\n";
        }
        if (LevelManager.pointPerTile){
            levelInformation += "- point per tile\n";
        }
        if (LevelManager.pointPerConnection){
            levelInformation += "- point per connection\n";
        }
        if (LevelManager.winThreshold > 0){
            levelInformation += "- " + LevelManager.winThreshold.ToString() + " points to win\n";
        }
        if (LevelManager.initialTile){
            levelInformation += "- initial tile\n";
        }
        if (LevelManager.randomPreTile > 0){
            levelInformation += "- " + LevelManager.randomPreTile + " pre-placed tiles\n";
        }
        if (LevelManager.winBySurround > 0){
            levelInformation += "- " + LevelManager.winBySurround + " tiles to surround\n";
        }
        if (LevelManager.connectHive > 0){
            levelInformation += "- " + LevelManager.connectHive + " tiles to connect\n";
        }
        levelInformation = levelInformation.Substring(0, levelInformation.Length-1);
        levelInfo.SetText(levelInformation);
    }
}
