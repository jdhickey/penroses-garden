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
            currPos.y -= 0.5f; // Offset to place on shadow.
            tilePlayed = ThinRhombusPrefab; // Remove when tile placement works with the thick rhombus.
            bool result = playerTilePlacementScript.PlaceTile(currPos, tilePlayed);
            if (result)
            {
                inventoryManagementScript.ActiveDestroy();
            }
        }
    }
}
