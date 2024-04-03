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
        if (winThreshold > 0){
            goalsStr += "* earn " + winThreshold + " points";
        }
        if (winThreshold > 0 && winBySurround > 0){
            goalsStr += "\n";
        }
        if (winBySurround > 0){
            goalsStr += "* surround " + winBySurround + " tiles";
        }
        if (LevelManager.winThreshold > 0 || LevelManager.winBySurround > 0){
            goals.SetText(goalsStr);
        }
    }

    public void UpdateWin(){
        if (goals != null){
            goals.SetText("* 'esc' to main menu");
        }
    }


}
