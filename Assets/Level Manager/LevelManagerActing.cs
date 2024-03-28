using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManagerActing : MonoBehaviour
{

    private InventoryManager inventoryManagementScript;
    private PlayerInput input;
    private SquareTilePlacement squareTilePlacementScript;
    public GameObject winLabel;

    // Start is called before the first frame update
    void Start()
    {
        winLabel.SetActive(false);
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        bool result = false;
        squareTilePlacementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SquareTilePlacement>();
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();

        // Places an initial tile.
        if (LevelManager.initialTile){
            SquareTile tilePlayed = inventoryManagementScript.randomTile();
            result = squareTilePlacementScript.PlaceTile(new Vector3(0, 0, 0), tilePlayed);
        }    

        // Pre-Places Tiles
        if (LevelManager.randomPreTile > 0){
            for (int i = 0; i < LevelManager.randomPreTile; i++){
                SquareTile tilePlayed = inventoryManagementScript.randomTile();
                do {
                    if (squareTilePlacementScript.initialTile){
                        squareTilePlacementScript.initialTile = false;
                    }
                    result = squareTilePlacementScript.PlaceTile(new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), tilePlayed);
                } while (!result);
            }
        }  
    }

    void FixedUpdate(){
        if (!(LevelManager.won) && LevelManager.playerScore >= LevelManager.winThreshold){
            winLabel.SetActive(true);
            input.actions.FindAction("Move").Disable();
            input.actions.FindAction("Place").Disable();
            input.actions.FindAction("Inventory Select").Disable();
            input.actions.FindAction("Inventory Scroll").Disable();
            input.actions.FindAction("Inventory Rotate").Disable();
            input.actions.FindAction("Shuffle").Disable();
            LevelManager.won = true;
        }
    }
}
