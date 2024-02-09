using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PenroseTile[] tileOptions;
    private PenroseTile[] inventory;
    private System.Random random_tile_type = new System.Random();

    public int activeIndex = 1;
    public int inventorySize = 5;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new PenroseTile[inventorySize];

        PlayerShuffle();
    }

    public void PlayerShuffle()
    {
        for (int i = 0; i <= inventorySize - 1; i++)
        {
            inventory[i] = randomTile();
        }
        print("This is working!");
    }

    public void PlayerSelect(float value)
    {
        activeIndex = (int)value;
    }

    public void PlayerScroll(float value)
    {
        if (value < 0)
        {
            activeIndex--;
        }
        else if (value > 0)
        {
            activeIndex++;
        }
    }

    public void PlayerDelete()
    {
        inventory[activeIndex] = null; // Replace with empty tile.
    }

    // This returns a random tile type to be put into the inventory.
    // The tile types it selects from are contained in the attribute tileOptions
    PenroseTile randomTile()
    {
        int index = random_tile_type.Next(tileOptions.Length);
        PenroseTile prefab = tileOptions[index];
        return prefab;
    }
}
