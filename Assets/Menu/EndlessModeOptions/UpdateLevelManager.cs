using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateLevelManager : MonoBehaviour
{
    public void UpdatePrePlacedTiles(){
        string text = GetComponent<TMP_InputField>().text;
        try{
            LevelManager.randomPreTile = int.Parse(text); 
        }
        catch (Exception){
            Debug.Log("Converting to integer failed. Value of randomPreTile was not updated.");
        }
    }

    public void UpdateWinThreshold(){
        string text = GetComponent<TMP_InputField>().text;
        try{
            LevelManager.winThreshold = int.Parse(text); 
        }
        catch (Exception){
            Debug.Log("Converting to integer failed. Value of winThreshold was not updated.");
        }
    }

    public void UpdateInitialTile(){
        Debug.Log("The new value of initialTile is: " + GetComponent<Toggle>().isOn);
        LevelManager.initialTile = GetComponent<Toggle>().isOn;
    }

    public void UpdatePointsPerTile(){
        LevelManager.pointPerTile = GetComponent<Toggle>().isOn;
    }


    public void UpdatePointsPerConnection(){
        LevelManager.pointPerConnection = GetComponent<Toggle>().isOn;
    }

    public void UpdateTimer(){
        string minVal = GetComponent<TMP_InputField>().text;
        try{
            LevelManager.timerVal = int.Parse(minVal) * 60.0f;
        }
        catch (Exception){
            Debug.Log("Converting to int failed. Value of timerVal was not updated.");
        }
    }
}
