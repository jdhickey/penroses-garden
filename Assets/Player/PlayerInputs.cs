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

    bool InitializePosition(PenroseTile tile, int[] ignore){
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
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        for (int i = 0; i < adjacentTiles.Count; i++){
            PenroseTile adjacentTile = adjacentTiles[i];
            if (adjacentTile != null && adjacentTile != tile){
                float xOff = Normalize(x - adjacentTile.transform.position.x);
                float yOff = Normalize(y - adjacentTile.transform.position.y);
                //Debug.Log("Old offset: " + xOff + ", " + yOff);
                for(int j = 0; j < 4; j++){
                    int connection = tile.CanConnectWith(adjacentTile, ignore);
                    if (connection != 4){
                        float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                        tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                        Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                        Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                        tile.transform.position = newPosition;
                        //Debug.Log("New offset: " + Normalize(tile.transform.position.x - adjacentTile.transform.position.x) + ", " + Normalize(tile.transform.position.y - adjacentTile.transform.position.y));
                        if(Approx(xOff, Normalize(tile.transform.position.x - adjacentTile.transform.position.x), yOff, Normalize(tile.transform.position.y - adjacentTile.transform.position.y))){
                            j = adjacentTiles.Count;
                            Debug.Log("Correct Position, connection = " + connection);
                            return true;
                        }
                        else{
                            ignore[j] = j;

                        }
                    }
                    else{
                        return false;
                    }
                }
            }
        }   
        return false;
    }
    
    public bool Approx(float x1, float x2, float y1, float y2){
        if(x1 == x2 && y1 == y2){
            return true;
        }
        else if(y1 == 1 && x1 == 1 && x2 == 0 && y2 == 1){
            return true;
        }
        else if(y1 == -1 && x1 == -1 && x2 == 0 && y2 == -1){
            return true;
        }
        return false;
    }
    
    public float Normalize(float n){
        if(n > 0){
            n = 1;
        }
        else if(n < 0){
            n = -1;
        }
        else{
            n = 0;
        }
        return n;
    }

    private bool IsValidPlacement(PenroseTile tile, PenroseTile adjacentTile, int connection){
        if (tile.IsTouchingTile()){
            Debug.Log("Overlap detected");
            return true;
        }
        if ((connection <= 1 && tile.freeSide[(-1) * (connection - 1)] == null) || (connection >= 1 && tile.freeSide[(connection - 3) * (-1) + 2] == null)){
            Debug.Log("Valid connection");
            return true;
        }
        Debug.Log("Invalid connection");
        return false;
    }


    void OrientTile(PenroseTile tile, int[] ignore){
        if (Array.IndexOf(ignore, 4) == -1 || !InitializePosition(tile, ignore)){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength * 0.35f);
        Debug.Log("Colliders: " + colliders.Length);
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null && temp != tile){
                    adjacentTiles.Add(temp);
                }
            }
        }
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        float r = tile.transform.rotation.eulerAngles.z;
        bool validPlacement = true;
        List<int> connections = new List<int>();
        Debug.Log("Adjacent Tiles: " + adjacentTiles.Count);
        for (int i = 0; i < adjacentTiles.Count; i++){
            if(adjacentTiles[i] != null){
                if(adjacentTiles[i]){
                    int connection = tile.CanConnectWith(adjacentTiles[i], ignore);
                    if (connection == 4 || !IsValidPlacement(tile, adjacentTiles[i], connection)){
                        Debug.Log("Not valid placement");
                        validPlacement = false;
                    }
                    else{
                        Debug.Log("Ignoring:" + connection);
                        connections.Add(connection);
                        ignore[i] = connection;
                    }
                }
            }
        }
        if(validPlacement){
            for (int i = 0; i < adjacentTiles.Count; i++){
                adjacentTiles[i].freeSide[connections[i]] = tile;
                if(connections[i] <= 1){
                    tile.freeSide[(-1) * (connections[i] - 1)] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((-1) * (connections[i] - 1)));
                }
                else{
                    tile.freeSide[(connections[i] - 3) * (-1) + 2] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((connections[i] - 3) * (-1) + 2));
                }
            }
        }
        else{
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
    }
}
