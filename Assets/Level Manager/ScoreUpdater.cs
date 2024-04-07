using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI scoreCounter;
    public GameObject PlayerScoreUpdateText;
    private TextMeshProUGUI scoreUpdater;
    public GameObject HighScoreObject;
    private TextMeshProUGUI highScoreText;
    private Vector3 originPos;
    private bool move = false;

    void Start(){
        scoreCounter = GetComponent<TextMeshProUGUI>();
        if (PlayerScoreUpdateText != null){
            scoreUpdater = PlayerScoreUpdateText.GetComponent<TextMeshProUGUI>();
            PlayerScoreUpdateText.SetActive(false);
        }
        if (HighScoreObject != null){
            highScoreText = HighScoreObject.GetComponent<TextMeshProUGUI>();
        }
    }

    public void UpdateScore(int newPoints){
        LevelManager.playerScore += newPoints;
        if (LevelManager.highscore < LevelManager.playerScore){
            LevelManager.highscore = LevelManager.playerScore;
            if (highScoreText != null){
                highScoreText.SetText(LevelManager.highscore.ToString());
            }
        }
        scoreCounter.SetText(LevelManager.playerScore.ToString());
        if (PlayerScoreUpdateText != null){
            originPos = PlayerScoreUpdateText.transform.position;
            PlayerScoreUpdateText.SetActive(true);
            scoreUpdater.SetText("+" + newPoints.ToString());
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
