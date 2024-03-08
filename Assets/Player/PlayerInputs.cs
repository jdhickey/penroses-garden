using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public SquareTile ThinRhombusPrefab;

    private InventoryManager inventoryManagementScript;
    private PlayerTilePlacement playerTilePlacementScript;
    
    public GameObject canvas;

    void Start()
    {
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        playerTilePlacementScript = this.gameObject.GetComponent<PlayerTilePlacement>();
        canvas.SetActive(false);
    }

    private void OnShuffle()
    {
        inventoryManagementScript.PlayerShuffle();
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
        int intVal = (int) value.Get<float>();
        if (intVal != 0){
            if (intVal != 1 || intVal != -1){
                intVal /= Mathf.Abs(intVal);
                inventoryManagementScript.PlayerScroll(intVal);
            }
        }
    }

    private void OnExit()
    {
        if (canvas.activeSelf){
            canvas.SetActive(false);
        }
        else {
            canvas.SetActive(true);
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
        currPos.y -= 0.5f; // Offset to place on shadow.
        float currPosX = Mathf.Floor(currPos.x);
        float currPosY = Mathf.Floor(currPos.y);
        if (currPos.x - currPosX > 0.5){
            currPosX += 1;
        }
        if (currPos.y - currPosY > 0.5){
            currPosY += 1;
        }
        Vector3 gridPos = new Vector3(currPosX, currPosY, 0);
        return gridPos;
    }

    private void OnPlace()
    {
        SquareTile tilePlayed = inventoryManagementScript.GetActiveTile();
        if (tilePlayed != inventoryManagementScript.emptyTile) // Replace with reference to the empty tile.
        {
            bool result = false;
            Vector3 gridPos = GetGridPos();
            // result = PlaceTile(gridPos, tilePlayed);
            /*
            Vector3 currPos = transform.position;
            currPos.y -= 0.5f; // Offset to place on shadow.
            tilePlayed = ThinRhombusPrefab; // Remove when tile placement works with the thick rhombus.
            bool result = false;
            //bool result = playerTilePlacementScript.PlaceTile(currPos, tilePlayed);
            */
            if (result)
            {
                inventoryManagementScript.ActiveDestroy();
            }
        }
    }
}
