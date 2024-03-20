using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtains : MonoBehaviour
{

    public Transition prefab;
    [Range(0f, 1f)]
    public float place;

    // Start is called before the first frame update
    void Start()
    {
        float aspect = Screen.width / Screen.height;
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * aspect;

        // This spawns in four virtual "transitions" in each cardinal direction
        for (int i = 0; i < 360; i += 90) {
            Vector3 direction = new Vector3(0, 0, 0);
            float angle = i * Mathf.PI / 180;

            // Sets the direction of the transition curtains relative to the centre of the screen.
            if (i % 180 == 0) {
                direction.y = -Mathf.Cos(angle) * screenWidth * place;
            } else {
                direction.x = Mathf.Sin(angle) * screenHeight * place;
            }

            // Instantiates the "curtain" and sets it's rotation.
            Transition curtain = (Transition) Instantiate(prefab, direction, Quaternion.Euler(0, 0, i));
            curtain.updateTransform(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
