using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public Rigidbody2D rb;
    public Animator animator;
    public InputController playerActions;

    Vector2 movementRaw;
    Vector3 movement = new Vector3(0,0,0);

    void Update()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    void OnMove(InputValue value)
    {
        movementRaw = value.Get<Vector2>();
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
