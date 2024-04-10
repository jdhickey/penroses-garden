using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateGoals : MonoBehaviour
{
    private TextMeshProUGUI goals;
    // Start is called before the first frame update
    void Start()
    {
        goals = GetComponent<TextMeshProUGUI>();
        string goalsStr = "";
        int winThreshold = LevelManager.winThreshold;
        int winBySurround = LevelManager.winBySurround;
        int winByConnect = LevelManager.winByConnect;
        if (winThreshold > 0){
            goalsStr += "* earn " + winThreshold + " point(s)\n";
        }
        if (winBySurround > 0){
            goalsStr += "* surround " + winBySurround + " tile(s)\n";
        }
        if (winByConnect > 0){
            goalsStr += "* connect to " + winByConnect + " tile(s)\n";
        }

        if (goalsStr != ""){
            goals.SetText(goalsStr.Substring(0, goalsStr.Length - 1));
        }
    }

    public void UpdateWin(){
        if (goals != null){
            goals.SetText("* 'esc' to main menu");
        }
    }


}
