using System;
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
    
    public GameObject canvas;

    void Start()
    {
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        squareTilePlacementScript = this.gameObject.GetComponent<SquareTilePlacement>();
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
        Vector3 gridPos = new Vector3(Mathf.Round(currPos.x), Mathf.Round(currPos.y), 0);
        return gridPos;
    }

    private void OnPlace()
    {
        SquareTile tilePlayed = inventoryManagementScript.GetActiveTile();
        if (tilePlayed != inventoryManagementScript.emptyTile) // Replace with reference to the empty tile.
        {
            Vector3 gridPos = GetGridPos();
            bool result = squareTilePlacementScript.PlaceTile(gridPos, tilePlayed);

            if (result)
            {
                inventoryManagementScript.ActiveDestroy();
            }
        }
    }
}
