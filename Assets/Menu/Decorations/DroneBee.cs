using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBee : MonoBehaviour
{
    private Vector3 movement;
    private Vector3 original;
    private bool flip = true;
    private SpriteRenderer _renderer;

    private Vector3 previous_pos;

    // Start is called before the first frame update
    void Start()
    {
        original = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        previous_pos = transform.position;
        MoveCalc(Time.unscaledTime);
        // Updates the new position, relative to the original placement
        transform.position = original + movement;

        // Updating a variable for the animations.
        if (transform.position.x - previous_pos.x > 0 && !flip) {
            flip = true;
        } else if (transform.position.x - previous_pos.x < 0 && flip) {
            flip = false;
        }

        _renderer.flipX = flip;
    }

    void MoveCalc(float dt) {
        // To see this curve, plot a point with the position
        // (2cos(0.5x), Sin(x)) over time
        float xStep = 2 * Mathf.Cos(0.5f * dt);
        float yStep = Mathf.Sin(dt);
        movement = new Vector3(xStep, yStep, 0) * 0.5f;
    }
}
