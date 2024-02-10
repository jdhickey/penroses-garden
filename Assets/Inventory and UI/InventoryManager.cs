using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PenroseTile[] tileOptions;
    public PenroseTile[] inventory;

    public int activeIndex = 1;
    public int inventorySize = 5;

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
        print("Tiles have been shuffled!");
    }

    public void PlayerSelect(int value)
    {
        activeIndex = value;
        print("Tile #" + activeIndex + " is the active tile!");
    }

    public void PlayerScroll(int value)
    {
        activeIndex += value;
        if (activeIndex > inventorySize)
        {
            activeIndex -= inventorySize;
        }
        else if (activeIndex < 1)
        {
            activeIndex += inventorySize;
        }
        print("Tile #" + activeIndex + " is the active tile!");
    }

    public void ActiveDestroy()
    {
        inventory[activeIndex] = null; // Replace with empty tile.
    }

    public PenroseTile ActiveTile()
    {
        return inventory[activeIndex];
    }

    // This returns a random tile type to be put into the inventory.
    // The tile types it selects from are contained in the attribute tileOptions    
    PenroseTile randomTile() 
    {
        int index = Random.Range(0, tileOptions.Length);
        PenroseTile prefab = tileOptions[index];
        return prefab;
    }

    // Nothing is initialized ever because the world hates me
    public void initializeInventory() {
        for (int i = 0; i < inventorySize; i++) {
            inventory[i] = randomTile();
        }
    }
}