using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManagerActing : MonoBehaviour
{

    private InventoryManager inventoryManagementScript;
    private PlayerInput input;
    private SquareTilePlacement squareTilePlacementScript;
    public GameObject winCondition;
    public GameObject loseCondition;
    public GameObject Timer;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        bool result = false;
        squareTilePlacementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SquareTilePlacement>();
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();

        if (LevelManager.timerVal > 0){
            
        }
        else{
            if (Timer != null){
                Timer.SetActive(false);
            }
        }

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

    void OnEnable(){
        winCondition.SetActive(false);
        if (loseCondition != null){
            loseCondition.SetActive(false);
        }
    }

    void FixedUpdate(){
        if (!(LevelManager.won) && LevelManager.playerScore >= LevelManager.winThreshold){
            winCondition.SetActive(true);
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
