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
        if (LevelManager.winThreshold > 0){
            goalsStr += "* earn " + LevelManager.winThreshold + " points";
        }
        if (LevelManager.winThreshold > 0){
            goals.SetText(goalsStr);
        }
    }


}
