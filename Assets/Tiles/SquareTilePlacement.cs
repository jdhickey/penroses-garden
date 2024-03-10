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
        bool valid = true;
        SquareTile[,] currNeighborhood = new SquareTile[3, 3];

        if (initialTile){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(gridPos, 1.0f);

            if (colliders.Length != 0){
                currNeighborhood = OrganizeNeighborhood(colliders, gridPos);

                valid = CheckCornerRules(currNeighborhood, currTile);
                if (valid){
                    valid = CheckEdgeRules(currNeighborhood, currTile);
                }
            }
            else{
                valid = false;
            }
        }

        if (valid){
            Quaternion currTileRotation = Quaternion.identity;
            // Unity's scene view rotates counter clockwise but everything else does not. That's why this is here. Boooo Unity!
            if (currTile.rotation == 90 || currTile.rotation == 270){
                currTileRotation.eulerAngles = new Vector3(0, 0, (currTile.rotation + 180) % 360);
            }
            else{
                currTileRotation.eulerAngles = new Vector3(0, 0, currTile.rotation);
            }

            currTile.transform.position = gridPos;
            currTile.transform.rotation = currTileRotation;
            currTile.gameObject.SetActive(true);
            currTile.FinishNeighbourhood(currNeighborhood);

            if (!initialTile){
                initialTile = true;
            }
        }

        return valid;
    }

    // This function will organize the colliders properly.
    public SquareTile[,] OrganizeNeighborhood(Collider2D[] colliders, Vector3 gridPos){
        SquareTile[,] currNeighborhood = new SquareTile[3, 3];

        foreach (Collider2D collider in colliders) {
            if (collider != null){
                Vector3 tilePos = collider.gameObject.transform.position;
                Vector3 diff = tilePos - gridPos;
                SquareTile tile = collider.gameObject.GetComponent<SquareTile>();
                if (diff.x == -1){
                    if (diff.y == 1){
                        currNeighborhood[0, 0] = tile;
                    }
                    else if (diff.y == 0){
                        currNeighborhood[1, 0] = tile;
                    }
                    else{
                        currNeighborhood[2, 0] = tile;
                    }
                }
                else if (diff.x == 0){
                    if (diff.y == 1){
                        currNeighborhood[0, 1] = tile;
                    }
                    else if (diff.y == -1){
                        currNeighborhood[2, 1] = tile;
                    }
                }
                else if (diff.x == 1){
                    if (diff.y == 1){
                        currNeighborhood[0, 2] = tile;
                    }
                    else if (diff.y == 0){
                        currNeighborhood[1, 2] = tile;
                    }
                    else{
                        currNeighborhood[2, 2] = tile;
                    }
                }
            }
        }

        return currNeighborhood;
    }

    // Return true if the edge rules determine this is a valid placement. Else false.
    public bool CheckEdgeRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;

        // If any of the following return false, valid should permanently become false in this case?
        if (currNeighborhood[1, 0] != null){
            valid = valid && CompareSides(currNeighborhood[1, 0].sides[0], currTile.sides[2]);
        }
        if (currNeighborhood[0, 1] != null){
            valid = valid && CompareSides(currNeighborhood[0, 1].sides[1], currTile.sides[3]);
        }
        if (currNeighborhood[1, 2] != null){
            valid = valid && CompareSides(currNeighborhood[1, 2].sides[2], currTile.sides[0]);
        }
        if (currNeighborhood[2, 1] != null){
            valid = valid && CompareSides(currNeighborhood[2, 1].sides[3], currTile.sides[1]);
        }

        if (currNeighborhood[1, 0] == null && currNeighborhood[0, 1] == null && currNeighborhood[1, 2] == null && currNeighborhood[2, 1] == null){
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

    // Possible connections 0 <-> 1, 2 <-> 3, 4 <-> 5
    public bool CompareSides(int side1, int side2){
        Debug.Log("Comparing Sides:" + side1 + " " + side2 + ".");
        if ((side1 == 0 && side2 == 1) || (side1 == 1 && side2 == 0)){
            Debug.Log("Side check is valid!");
            return true;
        }
        if ((side1 == 2 && side2 == 3) || (side1 == 3 && side2 == 2)){
            Debug.Log("Side check is valid!");
            return true;
        }
        if ((side1 == 4 && side2 == 5) || (side1 == 5 && side2 == 4)){
            Debug.Log("Side check is valid!");
            return true;
        }
        Debug.Log("Invalid!");
        return false;
    }

    // Return true if the corner rules determine this is a valid placement. Else false.
    public bool CheckCornerRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;

        // Case 1. currTile has corners.
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
                    Debug.Log("Violates #1!");
                    valid = false;
                }
            }
            if (currNeighborhood[2, 0] != null && currNeighborhood[1, 0] != null && currNeighborhood[2, 1] != null){
                if (!currNeighborhood[2, 0].corners && !currNeighborhood[1, 0].corners && !currNeighborhood[2, 1].corners){
                    Debug.Log("Violates #2!");
                    valid = false;
                }
            }
            if (currNeighborhood[2, 2] != null && currNeighborhood[1, 2] != null && currNeighborhood[2, 1] != null){
                if (!currNeighborhood[2, 2].corners && !currNeighborhood[1, 2].corners && !currNeighborhood[2, 1].corners){
                    Debug.Log("Violates #3!");
                    valid = false;
                }
            }
            if (currNeighborhood[0, 2] != null && currNeighborhood[1, 2] != null && currNeighborhood[0, 1] != null){
                if (!currNeighborhood[0, 2].corners && !currNeighborhood[1, 2].corners && !currNeighborhood[0, 1].corners){
                    Debug.Log("Violates #4!");
                    valid = false;
                }
            }
        }

        if (!valid){
            Debug.Log("Violates corner rules.");
        }
        else{
            Debug.Log("Follows corner rules.");
        }

        return valid;
    } 
}
