using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{ 

    public int inventorySize = 5;
    public PenroseTile[] tileOptions;
    private PenroseTile[] inventory;
    private System.Random random_tile_type = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        inventory = new PenroseTile[inventorySize];

        for (int i = 0; i < inventorySize; i++) {
            inventory[i] = randomTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This returns a random tile type to be put into the inventory.
    // The tile types it selects from are contained in the attribute tileOptions
    PenroseTile randomTile() {
        int index = random_tile_type.Next(tileOptions.Length);
        PenroseTile prefab = tileOptions[index];
        return prefab;
    }
}
