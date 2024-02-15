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

    void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (movement.x > 0) {
            flip = true;
        } else if (movement.x < 0) {
            flip = false;
        }

        _renderer.flipX = flip;
    }

    void OnMove(InputValue value)
    {
        Vector2 movementRaw = value.Get<Vector2>();
        movement.x = movementRaw.x;
        movement.y = movementRaw.y;
    }

    void FixedUpdate()
    {
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
        if (movement.magnitude == 0 && moveSpeed > 1)
        {
            moveSpeed /= 1.1f;
        }
        else if (moveSpeed < 15f)
        {
            moveSpeed *= 1.1f;
        }
    }
}
