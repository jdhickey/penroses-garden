using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform player;
    public GameObject pauseMenu;
    public GameObject ScoreUI;

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
        pauseMenu.transform.position = transform.position;
        ScoreUI.transform.position = transform.position;
    }
}
