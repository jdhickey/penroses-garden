using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PenroseTile[] tileOptions;
    private PenroseTile[] inventory;
    public PenroseTile emptyTile;
    private RuntimeInventoryUI _hotbar;

    private int activeIndex = 1;
    private int inventorySize = 5;

    void Awake()
    {
        // Generate inventory and UI.
        inventory = new PenroseTile[inventorySize];
        _hotbar = GameObject.FindGameObjectWithTag("UI").GetComponent<RuntimeInventoryUI>();
        initializeInventory();
    }

    public void initializeInventory() {
        // Populates inventory using randomTile.
        for (int i = 0; i < inventorySize; i++) {
            inventory[i] = randomTile();
        }
    }
 
    PenroseTile randomTile() 
    {
        // Picks a random tile in tileOptions.
        int index = Random.Range(0, tileOptions.Length);
        PenroseTile prefab = tileOptions[index];
        return prefab;
    }

    public PenroseTile GetActiveTile()
    {
        return inventory[activeIndex - 1]; 
    }

    public int GetInventorySize()
    {
        return inventorySize; 
    }
    
    public PenroseTile[] GetInventory(){
        return inventory; 
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
        // Wrap.
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
        inventory[activeIndex - 1] = emptyTile;
        _hotbar.VisualUpdate();
    }

    public void SetInventorySize(int n)
    {
        inventorySize = n;
    }

    public void SetInventory(PenroseTile[] newInventory){
        inventory = newInventory;
    }
}
