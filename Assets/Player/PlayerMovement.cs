using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 12.5f;

    public Rigidbody2D rb;
    public Animator animator;
    public InputController playerActions;

    Vector2 movementRaw;
    Vector3 movement = new Vector3(0,0,0);

    // Update is called once per frame
    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

    }

    void FixedUpdate()
    {
        movementRaw = (Vector2) playerActions.Player.Move.ReadValue<Vector2>();
        movement = new Vector3(movementRaw.x, movementRaw.y, 0);
        // Movement
        transform.position = transform.position + movement * moveSpeed * Time.fixedDeltaTime;
    }

    void Awake()
    {
        playerActions = new InputController();
    }

    private void OnEnable()
    {
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }
}
