using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=UnityEngine.Random;


public class PlayerTilePlacement : MonoBehaviour
{
    private bool initialTile = false;
    public PenroseTile[] tileOptions;

    // Returns a random tile from tileOptions.
    public PenroseTile RandomTile(){
        int randIndex = Random.Range(0, tileOptions.Length);
        return tileOptions[randIndex];
    }

    // Places a random tile from tileOptions
    public void PlaceRandomTile(Vector3 position){
        PlaceTile(position, RandomTile());
    }

    // Places given tile at given position.
    // This function calls all functions below this one.
    public bool PlaceTile(Vector3 position, PenroseTile tilePlayed){
        // Assumes placement is valid.
        bool ValidPlacement = true;

        // Instantiates tile with given tile, position and "Quaternion.identity" (no rotation).
        PenroseTile newTile = Instantiate(tilePlayed, position, Quaternion.identity);

        // Assigns the instantiated tile its small angle in [Thin/Thick]RhombusTile.cs/EmptyTile.cs
        newTile.InitializeTile();

        // If there is a tile already placed.
        if(initialTile == true){
            ValidPlacement = OrientTile(newTile);
        }
        else{
            initialTile = true;
        }

        // Returns a bool representing whether the tile was placed or not.
        return ValidPlacement;
    }

    // Orients tile.
    // Calls all functions below this one.
    private bool OrientTile(PenroseTile tile){
        int[] ignore = {4,4,4,4};

        // If tile couldn't actually be placed, get rid of it.
        if (!InitializePosition(tile, ignore)){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return false;
        }

        // Gather all colliders within height*0.62f of the placed tile.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength * 0.62f);
        Debug.Log("Colliders: " + colliders.Length);
        // Empty list of the adjacent tiles.
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();

        // Loop through the colliders we just got above.
        foreach (Collider2D collider in colliders) {
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                //Debug.Log("Yay"); Unnecessary? Should always work.
                if (temp != null && temp != tile){ // I think the null testing is not necessary.
                    Debug.Log("Yay 2");
                    adjacentTiles.Add(temp); // Adds neighbouring tiles to the neighbourhood. This happens somewhere else too. This is redundant.
                }
            }
        }

        // Current positioning of tile.
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        float r = tile.transform.rotation.eulerAngles.z;
        bool validPlacement = false; // To catch if final tile placement is successful.
        List<int> connections = new List<int>(); 
        Debug.Log("Adjacent Tiles: " + adjacentTiles.Count);

        // Loop through adjacentTiles.
        for (int i = 0; i < adjacentTiles.Count; i++){
            if(adjacentTiles[i]){ // Super redundant?
                // I think this is already done in another function.
                int connection = tile.CanConnectWith(adjacentTiles[i], ignore); // Gets the value of the first side it can connect with (adjacentTile's).
                if (connection != 4){
                    validPlacement = IsValidPlacement(tile, adjacentTiles[i], connection); // true if sides are available.

                    if (validPlacement){
                        Debug.Log("Ignoring:" + connection);
                        // Keep track on of the connections made.
                        connections.Add(connection);
                        // Update current tile's connections.
                        ignore[i] = connection;
                    }

                    else{
                        Debug.Log("Not valid placement.");
                    }
                }

                else {
                    Debug.Log("Not valid placement");
                }
            }
        }

        if (validPlacement) {
            // Loop through neighboring tiles.
            for (int i = 0; i < adjacentTiles.Count; i++){
                // Update adjacent tile neighbourhoods.
                adjacentTiles[i].freeSide[connections[i]] = tile;

                // Update current tile's neighbourhood. A little reundant with ignore?
                if(connections[i] < 2){
                    tile.freeSide[(-1) * (connections[i] - 1)] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((-1) * (connections[i] - 1)));
                }
                else{
                    tile.freeSide[(connections[i] - 3) * (-1) + 2] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((connections[i] - 3) * (-1) + 2));
                }
            }
        }
        
        // If bad placement, destroy tile.
        else {
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
        }

        return validPlacement;
    }

    // Takes in the tile being placed and ignore(?)
    // Returns true if the tile is being placed.
    // Called from OrientTile.
    private bool InitializePosition(PenroseTile tile, int[] ignore){
        // A list of all colliders within a circle around the insantiated tile at a radius of 0.01f.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.01f);
        float n = 0.01f;

        // While there is only one tile within 0.01f of the instantiatead tile. Probably itself?
        while(colliders.Length == 1){
            // Increments the radius of the sphere until at least one collider other than self is found.
            colliders = Physics2D.OverlapCircleAll(tile.transform.position, n);
            n += 0.01f;
            // Break out of function if instantiated tile is too far from the nearest tile. Too far is the side length of a tile multiplied by 2.
            if (n >= tile.sideLength*2){
                return false; 
            }
        }

        // Creates an empty list of type PenroseTile.
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();

        // Loop over every collider in the current neighbourhood.
        foreach (Collider2D collider in colliders){
            if (collider != null){
                // Gets the PenroseTile component of the neighbor tile.
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                // If this tile is not null and is not the instantiated tile.
                if (temp != null && temp != tile){
                    // Adds the neighbouring tile to the previously created list.
                    adjacentTiles.Add(temp);
                }
            }
        }

        // Gets the position of the instantiated tile.
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;

        // Checks all neighbouring tiles.
        for (int i = 0; i < adjacentTiles.Count; i++){
            PenroseTile adjacentTile = adjacentTiles[i];

            // If adjacentTile isn't null and isn't instantiated tile.
            if (adjacentTile != null && adjacentTile != tile){ // Not necessary?
                // Gets direction that player is from adjacentTile.
                float xOff = x - adjacentTile.transform.position.x;
                float yOff = y - adjacentTile.transform.position.y;
                float theta = angleWrap(Mathf.Rad2Deg * (Mathf.Atan2(xOff, yOff)) - (float)adjacentTile.transform.rotation.eulerAngles.z);
                Debug.Log("Theta: " + theta);

                // Try to make every connection.
                for(int j = 0; j < 4; j++){
                    int connection = tile.CanConnectWith(adjacentTile, ignore); // The first side on adjacentTile that is also empty on tile.

                    // If a connection can be made.
                    if (connection != 4){
                        float rotationAngle = tile.CalculateRotation(adjacentTile, connection); // The rotation angle that tile needs to be at.

                        // Rotates the tile.
                        tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                        Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection); // Unclear?
                        Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                        tile.transform.position = newPosition;
                        xOff = tile.transform.position.x - adjacentTile.transform.position.x;
                        yOff = tile.transform.position.y - adjacentTile.transform.position.y;
                        float theta2 = angleWrap(Mathf.Rad2Deg * (Mathf.Atan2(xOff, yOff)) - (float)adjacentTile.transform.rotation.eulerAngles.z);
                        //Debug.Log("New offset: " + Normalize(tile.transform.position.x - adjacentTile.transform.position.x) + ", " + Normalize(tile.transform.position.y - adjacentTile.transform.position.y));

                        // If the direction that the tile needs to move is generally the same as the direction it needs to be translated.
                        if(sameRelativeRotation(theta, theta2, adjacentTile.tileType)){
                            j = adjacentTiles.Count; // Not doing anything? Breaks loop but return does that too.
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

    private bool sameRelativeRotation(float a1, float a2, PenroseTile.TileType type){
        if(type == PenroseTile.TileType.ThinRhombus){
            if((a1 <= 72 && a2 <= 72) && (a1 >= -18 && a2 >= -18)){
                return true;
            }
            else if((a1 <= -18 && a2 <= -18) && (a1 >= -108 && a2 >= -108)){
                return true;
            }
            else if((a1 <= 162 && a2 <= 162) && (a1 >= 72 && a2 >= 72)){
                return true;
            }
            else if((a1 >= 162 && a2 >= 162) || (a1 <= -108 && a2 <= -108)){
                return true;
            }
        }
        return false;
    }
    
    private float angleWrap(float r){
        while(r > 180 || r < -180){
            if(r > 180){
                r -= 360;
            }
            else{
                r += 360;
            }
        }
        return r;
    }
    
    // Takes in old offset, normalized new offset in x, old offset, normalized new offset in y.
    // Returns true if offsets are equal, otherwise false.
    private bool Approx(float x1, float x2, float y1, float y2){
        // true if both x offsets and both y offsets are in the same direction. 
        if(x1 == x2 && y1 == y2){
            return true;
        }

        // I don't think this does anything?
        else if (x2 == 0){
            Debug.Log("Is this doing anything?");
            if (y1 == 1 && y1 == x1 && x1 == y2){
                return true;
            }
        }

        return false;
    }

    // Takes in the current tile, the tile it is connecting to and the side on adjacentTile that tile is connecting to.
    // Returns true if the right sides are open.
    private bool IsValidPlacement(PenroseTile tile, PenroseTile adjacentTile, int connection){
        // If the corresponding sides are available.
        if ((connection <= 1 && tile.freeSide[(-1) * (connection - 1)] == null) || (connection >= 1 && tile.freeSide[(connection - 3) * (-1) + 2] == null)){
            Debug.Log("Valid connection");
            return true;
        }
        if (tile.IsTouchingTile()){
            Debug.Log("Overlap detected");
        }
        else {
            Debug.Log("Invalid connection");
        }
        return false;
    }
}
