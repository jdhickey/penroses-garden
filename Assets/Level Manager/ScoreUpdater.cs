using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI scoreCounter;
    public GameObject PlayerScoreUpdateText;
    private TextMeshProUGUI scoreUpdater;
    private Vector3 originPos;
    private bool move = false;

    void Start(){
        scoreCounter = GetComponent<TextMeshProUGUI>();
        if (PlayerScoreUpdateText != null){
            scoreUpdater = PlayerScoreUpdateText.GetComponent<TextMeshProUGUI>();
            originPos = PlayerScoreUpdateText.transform.position;
            PlayerScoreUpdateText.SetActive(false);
        }
    }

    public void UpdateScore(int newPoints){
        LevelManager.playerScore += newPoints;
        scoreCounter.SetText(LevelManager.playerScore.ToString());
        if (PlayerScoreUpdateText != null){
            PlayerScoreUpdateText.SetActive(true);
            scoreUpdater.SetText("+" + newPoints.ToString());
            float timeCounter = 0;
            move = true;
            Invoke("DisappearUpdater", 1);
        }
    }

    void DisappearUpdater(){
        move = false;
        PlayerScoreUpdateText.transform.position = originPos;
        PlayerScoreUpdateText.SetActive(false);
    }
    
    void FixedUpdate(){
        if (move){
            PlayerScoreUpdateText.transform.position = new Vector3(originPos.x, PlayerScoreUpdateText.transform.position.y + (Time.deltaTime) * 0.5f, 0);
        }
    }
}
