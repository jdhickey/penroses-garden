using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    Vector3 currPos = new Vector3(0, 0, 0);

    public ThinRhombusTile ThinRhombusPrefab;

    InventoryManager inventoryManagementScript;
    int intVal;
    bool result = false;
    PenroseTile tilePlayed;

    void Start()
    {
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
    }

    public void OnShuffle()
    {
        inventoryManagementScript.PlayerShuffle();
    }

    public void OnInventorySelect(InputValue value)
    {
        if (value.isPressed)
        {
            intVal = (int) value.Get<float>();
            inventoryManagementScript.PlayerSelect(intVal);
        }
    }

    public void OnInventoryScroll(InputValue value)
    {
        intVal = (int) value.Get<float>();
        inventoryManagementScript.PlayerScroll(intVal);
    }

    public void OnExit()
    {
        print("Exit!");
    }

    public void OnPlace()
    {
        currPos.x = transform.position.x;
        currPos.y = transform.position.y;
        tilePlayed = inventoryManagementScript.ActiveTile();
        if (tilePlayed != null) // Replace with reference to the empty tile.
        {
            PlaceTile(currPos); // result = PlaceTile(currPos, tilePlayed). tilePlayed will need to be an argument for PlaceTile and to return a boolean whether successful or not.
            if (result)
            {
                inventoryManagementScript.ActiveDestroy();
            }
        }
    }

    void PlaceTile(Vector2 position){
        ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
        newTile.InitializeTile();
        int[] ignore = {4,4,4,4};
        OrientTile(newTile, ignore);
    }

    void OrientTile(PenroseTile tile, int[] ignore){
        if (Array.IndexOf(ignore, 4) == -1){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.2f);
        foreach (Collider2D collider in colliders){
            PenroseTile adjacentTile = collider.GetComponent<PenroseTile>();
            if (adjacentTile != null && adjacentTile != tile){
                int connection = tile.CanConnectWith(adjacentTile, ignore);
                if (connection != 4){
                    float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                    Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                    Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                    tile.transform.position = newPosition;

                    Collider2D[] collisionCheck = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength*0.6f);
                    if (collisionCheck.Length != 0){
                        for (int i = 0; i <= 4; i++){
                            if (ignore[i] == 4){
                                ignore[i] = connection;
                                break;
                            }
                        }
                        OrientTile(tile, ignore);
                    }
                    else
                    {
                        adjacentTile.freeSide[connection] = tile;
                        int i = 0;
                        if (connection < 2)
                        {
                            i = (-1) * (connection - 1);
                        }
                        else
                        {
                            i = (connection - 3) * (-1) + 2;
                        }
                        tile.freeSide[i] = adjacentTile;
                    }
                    Debug.Log("Tile Placed");
                    return;
                }
            }
        }
    }
}
