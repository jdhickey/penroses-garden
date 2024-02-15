using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PenroseTile[] tileOptions;
    public PenroseTile[] inventory;
    public PenroseTile emptyTile;
    private RuntimeInventoryUI _hotbar;

    private int activeIndex = 1;
    private int inventorySize = 5;

    void Awake()
    {
        inventory = new PenroseTile[inventorySize];
        _hotbar = GameObject.FindGameObjectWithTag("UI").GetComponent<RuntimeInventoryUI>();
        initializeInventory();
    }

    public void PlayerShuffle()
    {
        initializeInventory();
        _hotbar.VisualUpdate();
    }

    public void PlayerSelect(int value)
    {
        activeIndex = value;
        _hotbar.Select(activeIndex);
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

        _hotbar.Select(activeIndex);
    }

    public void ActiveDestroy()
    {
        inventory[activeIndex - 1] = emptyTile; // Replace with empty tile.
        _hotbar.VisualUpdate();
    }

    public PenroseTile ActiveTile()
    {
        // There was an off by one error before. The -1 fixes this.
        return inventory[activeIndex - 1];
    }

    public void SetInventorySize(int n)
    {
        inventorySize = n;
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }

    // This returns a random tile type to be put into the inventory.
    // The tile types it selects from are contained in the attribute tileOptions    
    PenroseTile randomTile() 
    {
        int index = Random.Range(0, tileOptions.Length);
        PenroseTile prefab = tileOptions[index];
        return prefab;
    }

    public void initializeInventory() {
        for (int i = 0; i < inventorySize; i++) {
            inventory[i] = randomTile();
        }
    }
}
