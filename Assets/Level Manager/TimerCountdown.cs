using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TimerCountdown : MonoBehaviour
{
    private TextMeshProUGUI textVal;
    private PlayerInput input;
    public GameObject loseCondition;
    public GameObject PauseMenu;

    void Start(){
        textVal = GetComponent<TextMeshProUGUI>();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    void FixedUpdate(){
        if (PauseMenu == null || !PauseMenu.activeSelf){
            if (LevelManager.timerVal > 0 && !(LevelManager.won || LevelManager.lost)){
                LevelManager.timerVal -= Time.deltaTime;
                int timeMin = ((int) LevelManager.timerVal) / 60;
                int timeSec = ((int) LevelManager.timerVal) % 60;
                textVal.SetText(timeMin.ToString() + ":" + timeSec.ToString("00"));
            }
            if (!LevelManager.lost && LevelManager.timerVal < 0)
            {
                loseCondition.SetActive(true);
                LevelManager.lost = true;
                input.actions.FindAction("Move").Disable();
                input.actions.FindAction("Place").Disable();
                input.actions.FindAction("Inventory Select").Disable();
                input.actions.FindAction("Inventory Scroll").Disable();
                input.actions.FindAction("Inventory Rotate").Disable();
                input.actions.FindAction("Shuffle").Disable();
            }
        }
    }
}
