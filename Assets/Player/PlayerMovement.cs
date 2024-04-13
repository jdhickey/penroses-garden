using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float topSpeed = 1.0f;
    public float acc = 1.05f;
    [SerializeField]
    private float moveSpeed = 1.0f;

    public Animator animator;
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

        try {
            GameObject hat = gameObject.transform.Find("Hat").gameObject;
            hat.GetComponent<SpriteRenderer>().flipX = flip;

            if (hat.transform.localPosition.x > 0 && flip || hat.transform.localPosition.x < 0 && !flip) {
                hat.transform.localPosition = new Vector3(hat.transform.localPosition.x * -1, hat.transform.localPosition.y, 0);
            }
        } catch {}

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
        if (moveSpeed < topSpeed && movement.magnitude != 0)
        {
            moveSpeed = Mathf.Clamp(moveSpeed * acc, 0, topSpeed);
        }
        Vector3 moveVal = movement * moveSpeed * Time.fixedDeltaTime; // Initial moveVal, only changed if deacceleration is engaged.

        // Deaccelearation.
        if (movement.magnitude == 0)
        {
            if (moveSpeed > 1)
            {
                moveSpeed /= acc;
                moveVal = previousMove * moveSpeed * Time.fixedDeltaTime;
            } 
        }

        // If bee should move, move.
        if (moveVal.magnitude != 0)
        {
            transform.Translate(moveVal);
            // If there is actual input then update previousMove. It's only going activating on every 0.1 second because it was causing issues.
            if (Time.fixedDeltaTime % 0.1f == 0f){
                previousMove = movement;
            }
        }
    }
}
