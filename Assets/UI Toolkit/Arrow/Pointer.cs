using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Center of the world (0, 0, 0)
    public Vector3 centerOfWorld = Vector3.zero;

    Renderer arrowRenderer;

    void Start()
    {
        arrowRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        // Update arrow's position to be relative to the camera
        transform.position = Camera.main.transform.position;

        // Check if the center of the world is visible on the screen
        Vector3 centerPosition = Camera.main.WorldToViewportPoint(centerOfWorld);
        if (centerPosition.x > 0 && centerPosition.x < 1 && centerPosition.y > 0 && centerPosition.y < 1 && centerPosition.z > 0)
        {
            // Center of the world is on screen, hide the arrow
            arrowRenderer.enabled = false;
        }
        else
        {
            // Center of the world is not on screen, show the arrow and point it towards the center
            arrowRenderer.enabled = true;
            Vector3 dir = centerOfWorld - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}   
