using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public PenroseTile ThinRhombusPrefab;

    private InventoryManager inventoryManagementScript;
    private PlayerTilePlacement playerTilePlacementScript;

    void Start()
    {
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        playerTilePlacementScript = this.gameObject.GetComponent<PlayerTilePlacement>();
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
        inventoryManagementScript.PlayerScroll(intVal);
    }

    private void OnExit()
    {
        print("Exit!");
    }

    private void OnPlace()
    {
        PenroseTile tilePlayed = inventoryManagementScript.GetActiveTile();
        if (tilePlayed != inventoryManagementScript.emptyTile) // Replace with reference to the empty tile.
        {
            Vector3 currPos = transform.position;
            tilePlayed = ThinRhombusPrefab; // Remove eventually.
            bool result = playerTilePlacementScript.PlaceTile(currPos, tilePlayed); // result = PlaceTile(currPos, tilePlayed). tilePlayed will need to be an argument for PlaceTile and to return a boolean whether successful or not.
            if (result)
            {
                inventoryManagementScript.ActiveDestroy();
            }
        }
    }
}
