using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InventoryManager inventoryManagementScript;
    private SquareTilePlacement squareTilePlacementScript;
    private float lastScroll = 0f;
    [Range(0.0f, 1.0f)]
    public float scrollInterval = 1f;
    private PlayerInput input;
    public float shuffleRadius = 1.5f;
    
    public GameObject canvas;
    public GameObject winCondition;
    public GameObject loseCondition;
    public GameObject inventoryUI;
    
    private AudioSource audio;
    [SerializeField]
    private AudioClip placeSound;
    [SerializeField]
    private AudioClip failSound;
    [SerializeField]
    private AudioClip shuffleSound;


    void Start()
    {
        input = GetComponent<PlayerInput>();
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        inventoryUI = FindObjectOfType<RuntimeInventoryUI>().gameObject;
        squareTilePlacementScript = this.gameObject.GetComponent<SquareTilePlacement>();
        canvas.SetActive(false);
        audio = GetComponent<AudioSource>();
    }

    private void OnShuffle()
    {
        Collider2D[] near = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), shuffleRadius);

        bool found = false;

        foreach (Collider2D obj in near) {
            SquareTile tile = obj.gameObject.GetComponent<SquareTile>();

            if (tile != null && tile.isHive) {
                inventoryManagementScript.PlayerShuffle();
                audio.PlayOneShot(shuffleSound);
                found = true;
                break;
            }
        }

        if (!found){
            audio.PlayOneShot(failSound);
        }
        inventoryManagementScript.GetActiveTile().gameObject.SetActive(true);
    }

    private void OnInventorySelect(InputValue value)
    {
        if (value.isPressed)
        {
            int intVal = (int) value.Get<float>();
            inventoryManagementScript.PlayerSelect(intVal);
        }
    }

    private void OnInventoryScroll(InputValue value)
    {
        // The time check manages scrolling going too fast.
        int intVal = (int) value.Get<float>();
        if (intVal != 0 && Time.time - lastScroll > scrollInterval){
            if (intVal != 1 || intVal != -1){
                lastScroll = Time.time;
                intVal /= Mathf.Abs(intVal);
                inventoryManagementScript.PlayerScroll(intVal);
            }
        } 
    }

    private void OnExit()
    {
        if (canvas.activeSelf || (winCondition != null && winCondition.activeSelf) || (loseCondition != null &&loseCondition.activeSelf)){
            inventoryUI.SetActive(true);
            canvas.SetActive(false);
            winCondition.SetActive(false);
            if (loseCondition != null){
                loseCondition.SetActive(false);
            }
            input.actions.FindAction("Move").Enable();
            input.actions.FindAction("Place").Enable();
            input.actions.FindAction("Inventory Select").Enable();
            input.actions.FindAction("Inventory Scroll").Enable();
            input.actions.FindAction("Inventory Rotate").Enable();
            input.actions.FindAction("Shuffle").Enable();
        }
        else {
            canvas.SetActive(true);
            input.actions.FindAction("Move").Disable();
            input.actions.FindAction("Place").Disable();
            input.actions.FindAction("Inventory Select").Disable();
            input.actions.FindAction("Inventory Scroll").Disable();
            input.actions.FindAction("Inventory Rotate").Disable();
            input.actions.FindAction("Shuffle").Disable();
        }
    }

    private void OnInventoryRotate(InputValue value){
        int intVal = (int) value.Get<float>();
        if (intVal != 0){
            if (intVal != 1 || intVal != -1){
                intVal /= Mathf.Abs(intVal);
            }
        }
        inventoryManagementScript.RotateCurrent(intVal);
    }

    public Vector3 GetGridPos(){
        Vector3 currPos = transform.position;
        Vector3 gridPos = new Vector3(Mathf.Round(currPos.x), Mathf.Round(currPos.y), 0);
        return gridPos;
    }

    private void OnPlace()
    {
        SquareTile tilePlayed = inventoryManagementScript.GetActiveTile();
        if (tilePlayed.sides[0] != -1) // Replace with reference to the empty tile.
        {
            Vector3 gridPos = GetGridPos();
            bool result = squareTilePlacementScript.PlaceTile(gridPos, tilePlayed);

            if (result)
            {
                audio.PlayOneShot(placeSound);
                if (LevelManager.pointPerTile){
                    LevelManager.playerScore++;
                }
                if (LevelManager.pointPerConnection){
                    foreach (SquareTile side in tilePlayed.neigh){
                        if (side != null){
                            LevelManager.playerScore++;
                        }
                    }
                }
                inventoryManagementScript.ActiveDestroy();
            }
            else{
                audio.PlayOneShot(failSound);
            }
        }
    }

    void FixedUpdate(){
        inventoryManagementScript.GetActiveTile().transform.position = GetGridPos();
    }
}
