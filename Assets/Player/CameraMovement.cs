using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform player;

    void Awake(){
        gameObject.GetComponent<Camera>().orthographicSize = PlayerPreferences.FOV;
        Debug.Log(PlayerPreferences.FOV);
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
