using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTile : MonoBehaviour
{
    public int[] sides = new int[4]; // A length-4 array of ints that represent which type of side it is. Possible connections 0 <-> 1, 2 <-> 3, 4 <-> 5
    public bool corners = false; // If the tile has square corners, this is true.
    public SquareTile[] neigh = new SquareTile[4]; // A length-4 array of SquareTiles that represent the neighbors in the cardinal diretions. This is useful for eventually coding the garden filling.
    public float rotation = 0; // A float representing the rotation of the object in degrees.
    public bool connectable = true;
    public bool isHive = false;

    // Rotates the values of sides and updates rotation.
    public void rotateSides(int dir){ // dir true = right
        if(dir > 0){
            int temp = sides[0];
            int temp2 = sides[1];
            sides[0] = sides[3];
            sides[1] = temp;
            temp = sides[2];
            sides[2] = temp2;
            sides[3] = temp;
        }
        else{
            int temp = sides[3];
            int temp2 = sides[2];
            sides[3] = sides[0];
            sides[2] = temp;
            temp = sides[1];
            sides[1] = temp2;
            sides[0] = temp;
        }
        // I'm sure I could think up a better way to do this. But this works.
        rotation = (rotation + (dir * 90)) % 360;
    }

    public void makeHive() {
        transform.GetChild(0).gameObject.SetActive(true);
        isHive = true;
    }
}
