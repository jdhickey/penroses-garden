using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerActing : MonoBehaviour
{

    private InventoryManager inventoryManagementScript;
    private SquareTilePlacement squareTilePlacementScript;

    // Start is called before the first frame update
    void Start()
    {
        bool result = false;
        squareTilePlacementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SquareTilePlacement>();
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        if (LevelManager.initialTile){
            SquareTile tilePlayed = inventoryManagementScript.randomTile();
            result = squareTilePlacementScript.PlaceTile(new Vector3(0, 0, 0), tilePlayed);
        }    
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

    }
}
