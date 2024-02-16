using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public Animator animator;
    public InputController playerActions;
    private SpriteRenderer _renderer;
    private bool flip;
    Vector3 movement = new Vector3(0,0,0);
    Vector3 previousMove = new Vector3(0, 0, 0);

    void Start() 
    {
        _renderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        // Updating a variable for the animations.
        if (movement.x > 0 && !flip) {
            flip = true;
        } else if (movement.x < 0 && flip) {
            flip = false;
        }

        _renderer.flipX = flip;
    }

    void OnMove(InputValue value)
    {
        // Update Movement when there is input to process.
        Vector2 movementRaw = value.Get<Vector2>();
        movement.x = movementRaw.x;
        movement.y = movementRaw.y;
    }

    void FixedUpdate()
    {
        // If there is movement and it's not fast. Move faster.
        if (moveSpeed < 10f && movement.magnitude != 0)
        {
            moveSpeed *= 1.1f;
        }
        Vector3 moveVal = movement * moveSpeed * Time.fixedDeltaTime; // Initial moveVal, only changed if deacceleration is engaged.

        // Deaccelearation.
        if (movement.magnitude == 0)
        {
            if (moveSpeed > 1)
            {
                moveSpeed /= 1.2f;
                moveVal = previousMove * moveSpeed * Time.fixedDeltaTime;
            } 
        }

        // If bee should move, move.
        if (moveVal.magnitude != 0)
        {
            transform.Translate(moveVal);
        }

        // If there is actual input then update previousMove. It's only going activating on every 0.1 second because it was causing issues.
        if (movement.magnitude != 0 && Time.fixedDeltaTime % 0.1f == 0f)
        {
            previousMove = movement;
        }
    }
}
