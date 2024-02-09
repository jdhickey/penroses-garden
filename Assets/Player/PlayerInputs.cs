using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public GameObject gameManager;

    public Vector3 currPos = new Vector3(0, 0, 0);

    public ThinRhombusTile ThinRhombusPrefab;

    public bool temporaryPlacedTile = false;

    void OnShuffle()
    {
        print("Shuffle!");
    }

    void OnInventorySelect()
    {
        print("Inventory Select!");
    }

    void OnInventoryScroll()
    {
        print("Inventory Scroll!");
    }

    void OnExit()
    {
        print("Exit!");
    }

    void OnPlace()
    {
        currPos.x = transform.position.x;
        currPos.y = transform.position.y;
        PlaceTile(currPos);
    }

    void PlaceTile(Vector2 position){
        if(temporaryPlacedTile == true){
            ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
            newTile.InitializeTile();
            int[] ignore = {4,4,4,4};
            OrientTile(newTile, ignore);
        }
        else{
            ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
            newTile.InitializeTile();
            temporaryPlacedTile = true;
        }
    }

    bool InitializePosition(PenroseTile tile){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.01f);
        float n = 0.01f;
        while(colliders.Length == 1){
            colliders = Physics2D.OverlapCircleAll(tile.transform.position, n);
            n += 0.01f;
            if (n >= tile.sideLength*2){
                return false; 
            }
        }
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null && temp != tile){
                    adjacentTiles.Add(temp);
                }
            }
        }
        PenroseTile adjacentTile = adjacentTiles[0];
        if (adjacentTile != null && adjacentTile != tile){
            int connection = tile.CanConnectWith(adjacentTile);
            if (connection != 4){
                float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                tile.transform.position = newPosition;
            }
            else{
                return false;
            }
        }
        return true;
    }

    void OrientTile(PenroseTile tile, int[] ignore){
        if (Array.IndexOf(ignore, 4) == -1 || !InitializePosition(tile)){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength * 0.6f);
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null && temp != tile){
                    Debug.Log("tiles arent the same");
                    adjacentTiles.Add(temp);
                }
            }
        }
        float[,] tilesInfo = new float[adjacentTiles.Count, 4];
        for (int i = 0; i < adjacentTiles.Count; i++){
            if(adjacentTiles[i] != null){
                tilesInfo[i, 0] = adjacentTiles[i].transform.position.x;
                tilesInfo[i, 1] = adjacentTiles[i].transform.position.y;
                tilesInfo[i, 2] = adjacentTiles[i].transform.rotation.eulerAngles.z;
            }
        }
            /*
            PenroseTile adjacentTile = collider.GetComponent<PenroseTile>();
            if (adjacentTile != null && adjacentTile != tile){
                int connection = tile.CanConnectWith(adjacentTile, ignore);
                if (connection != 4){
                    float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                    Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                    Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                    tile.transform.position = newPosition;
                    
                    PenroseTile otherTile = tile.collision.gameObject.GetComponent<PenroseTile>();
                    if (otherTile == null){
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
            */
        
    }
}
