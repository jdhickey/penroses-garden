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
    private SquareTile[] tilesToSurround;

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
            PreGenTiles(LevelManager.randomPreTile);
        }  

        if (LevelManager.winBySurround > 0){
            tilesToSurround = PreGenTiles(LevelManager.winBySurround);
            foreach (SquareTile tile in tilesToSurround){
                tile.connectable = false;
            }
        }
    }

    private SquareTile[] PreGenTiles(int numTiles){
        SquareTile[] genTiles = new SquareTile[numTiles];
        bool result;
        for (int i = 0; i < numTiles; i++){
            SquareTile newTile = inventoryManagementScript.randomTile();
            do {
                if (squareTilePlacementScript.initialTile){
                    squareTilePlacementScript.initialTile = false;
                }
                result = squareTilePlacementScript.PlaceTile(new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), newTile);
            } while (!result);
            genTiles[i] = newTile;
        }
        return genTiles;
    }

    void OnEnable(){
        winCondition.SetActive(false);
        if (loseCondition != null){
            loseCondition.SetActive(false);
        }
    }

    void Win(){
        winCondition.SetActive(true);
        input.actions.FindAction("Move").Disable();
        input.actions.FindAction("Place").Disable();
        input.actions.FindAction("Inventory Select").Disable();
        input.actions.FindAction("Inventory Scroll").Disable();
        input.actions.FindAction("Inventory Rotate").Disable();
        input.actions.FindAction("Shuffle").Disable();
        LevelManager.won = true;
        if (LevelManager.currLevel != -1){
            LevelManager.levels[LevelManager.currLevel-1] = true;
        }
    }

    void FixedUpdate(){
        if (!(LevelManager.won)){
            if (LevelManager.winThreshold > 0 && LevelManager.playerScore >= LevelManager.winThreshold){
                Win();
            }
            if (LevelManager.winBySurround > 0){
                bool valid = true;
                foreach (SquareTile tile in tilesToSurround){
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 3.0f);     
                    SquareTile[,] currNeighborhood = OrganizeNeighborhood3(colliders, tile.transform.position);

                    valid = valid && CheckSurrounded(currNeighborhood);       
                }
                if (valid){
                    Win();
                }
            }
        }
    }

    private bool CheckSurrounded(SquareTile[,] currNeighborhood){
        bool valid = (currNeighborhood[0, 0] != null) && (currNeighborhood[1, 0] != null) && (currNeighborhood[2, 0] != null) && (currNeighborhood[3, 0] != null) && (currNeighborhood[4, 0] != null) && (currNeighborhood[0, 1] != null) && (currNeighborhood[4, 1] != null) && (currNeighborhood[0, 2] != null) && (currNeighborhood[4, 2] != null) && (currNeighborhood[0, 3] != null) && (currNeighborhood[4, 3] != null) && (currNeighborhood[0, 4] != null) && (currNeighborhood[1, 4] != null) && (currNeighborhood[2, 4] != null) && (currNeighborhood[3, 4] != null) && (currNeighborhood[4, 4] != null);
        return valid;
    }

    private SquareTile[,] OrganizeNeighborhood3(Collider2D[] colliders, Vector3 gridPos){
        SquareTile[,] currNeighborhood = new SquareTile[5, 5];
        foreach (Collider2D collider in colliders){
            SquareTile tile = collider.gameObject.GetComponent<SquareTile>();
            Vector3 tilePos = collider.transform.position;
            Vector3 diff = tilePos - gridPos;

            if (diff.x == -2){
                if (diff.y == 2){ // Ex. -1, 1 -> 0, 0
                    currNeighborhood[0, 0] = tile;
                }
                else if (diff.y == 1){ // Ex. -1, 0 -> 1, 0
                    currNeighborhood[1, 0] = tile;
                }
                else if (diff.y == 0){ // Ex. -1, -1 -> 2, 0
                    currNeighborhood[2, 0] = tile;
                }
                else if (diff.y == -1){
                    currNeighborhood[3, 0] = tile;
                }
                else if (diff.y == -2){
                    currNeighborhood[4, 0] = tile;
                }
            }
            else if (diff.x == -1){
                if (diff.y == 2){ // Ex. 0, 1 -> 0, 1
                    currNeighborhood[0, 1] = tile;
                }
                else if (diff.y == -2){
                    currNeighborhood[4, 1] = tile;
                }
            }
            else if (diff.x == 0){
                if (diff.y == 2){ // Ex. 0, 1 -> 0, 1
                    currNeighborhood[0, 2] = tile;
                }
                else if (diff.y == -2){
                    currNeighborhood[4, 2] = tile;
                }
            }
            else if (diff.x == 1){
                if (diff.y == 2){ // Ex. 0, 1 -> 0, 1
                    currNeighborhood[0, 3] = tile;
                }
                else if (diff.y == -2){
                    currNeighborhood[4, 3] = tile;
                }
            }
            else if (diff.x == 2){
                if (diff.y == 2){ // Ex. -1, 1 -> 0, 0
                    currNeighborhood[0, 4] = tile;
                }
                else if (diff.y == 1){ // Ex. -1, 0 -> 1, 0
                    currNeighborhood[1, 4] = tile;
                }
                else if (diff.y == 0){ // Ex. -1, -1 -> 2, 0
                    currNeighborhood[2, 4] = tile;
                }
                else if (diff.y == -1){
                    currNeighborhood[3, 4] = tile;
                }
                else if (diff.y == -2){
                    currNeighborhood[4, 4] = tile;
                }
            }
        }
        return currNeighborhood;
    }
}
