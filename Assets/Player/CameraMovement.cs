using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform player;
    public GameObject canvas;

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
        canvas.transform.position = transform.position;
    }
}
