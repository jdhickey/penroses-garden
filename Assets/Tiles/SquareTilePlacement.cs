using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTilePlacement : MonoBehaviour
{
    public bool initialTile = false;

    // This should get a 3x3 neighborhood of colliders around gridPos. It must sort the 3x3 properly to represent relative positioning somehow.
    // Check placement rules which are their own functions. Edge rules and corner rules.
    // If valid, place at gridPos and update the values of the tiles in the neighborhood and currTile. Else return false.
    public bool PlaceTile(Vector3 gridPos, SquareTile currTile){
        // Variables initialized.
        bool valid = true;
        SquareTile[,] currNeighborhood = new SquareTile[3, 3];

        // If there is at least one tile on the board.
        if (initialTile){
            // Get colliders.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(gridPos, 1.0f);

            // If there are colliders.
            if (colliders.Length != 0){
                currNeighborhood = OrganizeNeighborhood(colliders, gridPos); // See OrganizeNeighborhood
                if (currNeighborhood[1, 1] == null){ // Makes sure tiles can't be placed over top of each other.

                    // Check corners rules.
                    valid = CheckCornerRules(currNeighborhood, currTile);
                    if (valid){
                        //Check edge rules.
                        valid = CheckEdgeRules(currNeighborhood, currTile);
                        if (valid){
                            // Update the newly placed tile's internal neighborhood and that of those in currNeighborhood.
                            FinishNeighbourhood(currNeighborhood, currTile);
                        }
                    }
                }
                else{
                    valid = false;
                }
            }
            // If there are no colliders. Do not place.
            else{
                valid = false;
            }
        }

        if (valid){
            // Creating rotation angle. Probably a faster way to do it but I'm just trying to get it finished.
            Quaternion currTileRotation = Quaternion.identity;
            // Unity's scene view rotates counter clockwise but everything else does not. That's why this is here. Boooo Unity!
            if (currTile.rotation % 360 == 90 || currTile.rotation % 360 == 270 || currTile.rotation == -90 || currTile.rotation == -270){
                currTileRotation.eulerAngles = new Vector3(0, 0, (currTile.rotation + 180) % 360);
            }
            else{
                currTileRotation.eulerAngles = new Vector3(0, 0, currTile.rotation);
            }

            // The tile was already initialized when it was drawn in InventoryManager. We're just moving, rotating and enabling it.
            currTile.transform.position = gridPos;
            currTile.transform.rotation = currTileRotation;
            currTile.gameObject.SetActive(true);
            
            // If a tile had not been placed. One has been placed now.
            if (!initialTile){
                initialTile = true;
            }
        }

        return valid;
    }

    // This function will organize the colliders properly. Then it returns the organized neighbourhood.
    public SquareTile[,] OrganizeNeighborhood(Collider2D[] colliders, Vector3 gridPos){
        // Variable initialization.
        SquareTile[,] currNeighborhood = new SquareTile[3, 3];

        // Loop through colliders.
        foreach (Collider2D collider in colliders) {
            // Variable initialization.
            SquareTile tile = collider.gameObject.GetComponent<SquareTile>();
            Vector3 tilePos = collider.gameObject.transform.position;

            // Gets relative position.
            Vector3 diff = tilePos - gridPos;

            // Place it where it should go in the neighborhood. I'm certain there is a formula but I wanted to get this done fast.
            if (diff.x == -1){
                if (diff.y == 1){ // Ex. -1, 1 -> 0, 0
                    currNeighborhood[0, 0] = tile;
                }
                else if (diff.y == 0){ // Ex. -1, 0 -> 1, 0
                    currNeighborhood[1, 0] = tile;
                }
                else{ // Ex. -1, -1 -> 2, 0
                    currNeighborhood[2, 0] = tile;
                }
            }
            else if (diff.x == 0){
                if (diff.y == 1){ // Ex. 0, 1 -> 0, 1
                    currNeighborhood[0, 1] = tile;
                }
                else if (diff.y == 0){
                    currNeighborhood[1, 1] = tile;
                }
                else { // Ex. 0, -1 -> 2, 1
                    currNeighborhood[2, 1] = tile;
                }
            }
            else if (diff.x == 1){
                if (diff.y == 1){ // Ex. 1, 1 -> 0, 2
                    currNeighborhood[0, 2] = tile;
                }
                else if (diff.y == 0){ // Ex. 1, 0 -> 1, 2
                    currNeighborhood[1, 2] = tile;
                }
                else{ // Ex. 1, -1 -> 2, 2
                    currNeighborhood[2, 2] = tile;
                }
            }
        }

        return currNeighborhood;
    }

    // Return true if the edge rules determine this is a valid placement. Else false.
    public bool CheckEdgeRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;
        bool lurd = false; // Tracks if at least one of the cardinal directions is full.

        // If any of the following return false, valid should permanently become false in this case?
        if (currNeighborhood[1, 0] != null){
            valid = valid && CompareSides(currNeighborhood[1, 0].sides[0], currTile.sides[2]); // Left.
            if (currNeighborhood[1, 0].connectable){
                lurd = true;
            }
        }
        if (currNeighborhood[0, 1] != null){
            valid = valid && CompareSides(currNeighborhood[0, 1].sides[1], currTile.sides[3]); // Up.
            if (currNeighborhood[0, 1].connectable){
                lurd = true;
            }
        }
        if (currNeighborhood[1, 2] != null){
            valid = valid && CompareSides(currNeighborhood[1, 2].sides[2], currTile.sides[0]); // Right.
            if (currNeighborhood[1, 2].connectable){
                lurd = true;
            }
        }
        if (currNeighborhood[2, 1] != null){
            valid = valid && CompareSides(currNeighborhood[2, 1].sides[3], currTile.sides[1]); // Down.
            if (currNeighborhood[2, 1].connectable){
                lurd = true;
            }
        }

        // False if there are no tiles left, right, up, or down.
        if (!lurd){
            valid = false;
        }

        if (!valid){
            Debug.Log("Violates edge rules.");
        }
        else{
            Debug.Log("Follows edge rules.");
        }

        return valid;
    }

    // Possible connections 0 <-> 1, 2 <-> 3, 4 <-> 5. Returns true if valid, else false.
    public bool CompareSides(int side1, int side2){
        Debug.Log("Comparing Sides:" + side1 + " " + side2 + ".");
        if ((side1 == 0 && side2 == 1) || (side1 == 1 && side2 == 0)){ // 0 <-> 1
            Debug.Log("Side check is valid!");
            return true;
        }
        if ((side1 == 2 && side2 == 3) || (side1 == 3 && side2 == 2)){ // 2 <-> 3
            Debug.Log("Side check is valid!");
            return true;
        }
        if ((side1 == 4 && side2 == 5) || (side1 == 5 && side2 == 4)){ // 4 <-> 5
            Debug.Log("Side check is valid!");
            return true;
        }
        //Debug.Log("Invalid!");
        return false;
    }

    // Return true if the corner rules determine this is a valid placement. Else false.
    public bool CheckCornerRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;

        // Case 1. currTile has corners.
        // In this case, no other tile in its neighborhood can have corners.
        if (currTile.corners){
            foreach (SquareTile tile in currNeighborhood){
                if (tile != null){
                    if (tile.corners){
                        valid = false;
                    }
                }
            }
        }

        // Case 2. currTile does not have corners.
        // In this case, we have to check the 3 tiles in each corner. If a corner is full and none are corners yet, this one must be a corner.
        else{
            if (currNeighborhood[0, 0] != null && currNeighborhood[1, 0] != null && currNeighborhood[0, 1] != null){
                if (!currNeighborhood[0, 0].corners && !currNeighborhood[1, 0].corners && !currNeighborhood[0, 1].corners){
                    //Debug.Log("Violates corner #1!");
                    valid = false;
                }
            }
            if (currNeighborhood[2, 0] != null && currNeighborhood[1, 0] != null && currNeighborhood[2, 1] != null){
                if (!currNeighborhood[2, 0].corners && !currNeighborhood[1, 0].corners && !currNeighborhood[2, 1].corners){
                    //Debug.Log("Violates corner #2!");
                    valid = false;
                }
            }
            if (currNeighborhood[2, 2] != null && currNeighborhood[1, 2] != null && currNeighborhood[2, 1] != null){
                if (!currNeighborhood[2, 2].corners && !currNeighborhood[1, 2].corners && !currNeighborhood[2, 1].corners){
                    //Debug.Log("Violates corner #3!");
                    valid = false;
                }
            }
            if (currNeighborhood[0, 2] != null && currNeighborhood[1, 2] != null && currNeighborhood[0, 1] != null){
                if (!currNeighborhood[0, 2].corners && !currNeighborhood[1, 2].corners && !currNeighborhood[0, 1].corners){
                    //Debug.Log("Violates corner #4!");
                    valid = false;
                }
            }
        }

        // if (!valid){
        //     Debug.Log("Violates corner rules.");
        // }
        // else{
        //     Debug.Log("Follows corner rules.");
        // }

        return valid;
    } 

    // Updates the neighbors of the tile it is called upon and inserts it into the neighborhood of its adjacent tiles.
    public void FinishNeighbourhood(SquareTile[,] currNeighborhood, SquareTile currTile){
        if (currNeighborhood[1, 2] != null){
            currTile.neigh[0] = currNeighborhood[1, 2];
            currNeighborhood[1, 2].neigh[2] = currTile;
        }
        if (currNeighborhood[2, 1] != null){
            currTile.neigh[1] = currNeighborhood[2, 1];
            currNeighborhood[2, 1].neigh[3] = currTile; 
        }
        if (currNeighborhood[1, 0] != null){
            currTile.neigh[2] = currNeighborhood[1, 0];
            currNeighborhood[1, 0].neigh[0] = currTile;
        }
        if (currNeighborhood[0, 1] != null){
            currTile.neigh[3] = currNeighborhood[0, 1];
            currNeighborhood[0, 1].neigh[1] = currTile;
        }
    }
}
