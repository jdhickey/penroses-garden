using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public SquareTile[] tileOptions;
    private SquareTile[] inventory;
    public SquareTile emptyTile;
    private RuntimeInventoryUI _hotbar;

    private int activeIndex = 1;
    private int inventorySize = 5;

    void Awake()
    {
        // Generate inventory and UI.
        inventory = new SquareTile[inventorySize];
        _hotbar = GameObject.FindGameObjectWithTag("UI").GetComponent<RuntimeInventoryUI>();
        initializeInventory();
        GetActiveTile().gameObject.SetActive(true);
        GetActiveTile().gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void initializeInventory() {
        // Populates inventory using randomTile.
        for (int i = 0; i < inventorySize; i++) {
            inventory[i] = randomTile();
        }
    }
 
    public SquareTile randomTile() 
    {
        // Picks a random tile in tileOptions.
        int index = Random.Range(0, tileOptions.Length);
        SquareTile newTile = Instantiate(tileOptions[index]);
        newTile.gameObject.SetActive(false);
        return newTile;
    }

    public void ShuffleInventory(){
        for (int i = 0; i < inventorySize; i++){
            if (inventory[i].sides[0] == -1){
                inventory[i] = randomTile();
            }
        }
    }

    public SquareTile GetActiveTile()
    {
        return inventory[activeIndex - 1]; 
    }

    public int GetInventorySize()
    {
        return inventorySize; 
    }
    
    public SquareTile[] GetInventory(){
        return inventory; 
    }

    public int GetActiveIndex(){
        return activeIndex;
    }

    public void PlayerShuffle()
    {
        foreach (SquareTile tile in inventory){
            Destroy(tile.gameObject);
        }
        initializeInventory();
        _hotbar.VisualUpdate();
    }

    public void PlayerSelect(int value)
    {
        GetActiveTile().gameObject.SetActive(false);
        activeIndex = value;
        _hotbar.Select(activeIndex);
        if (GetActiveTile().sides[0] != -1){
            GetActiveTile().gameObject.SetActive(true);
        }
        GetActiveTile().gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void PlayerScroll(int value)
    {
        GetActiveTile().gameObject.SetActive(false);
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
        if (GetActiveTile().sides[0] != -1){
            GetActiveTile().gameObject.SetActive(true);
        }
        GetActiveTile().gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void RotateCurrent(int dir){
        GetActiveTile().rotateSides(dir);
        Quaternion currTileRotation = Quaternion.identity;
        if (GetActiveTile().rotation % 360 == 90 || GetActiveTile().rotation % 360 == 270 || GetActiveTile().rotation == -90 || GetActiveTile().rotation == -270){
            currTileRotation.eulerAngles = new Vector3(0, 0, (GetActiveTile().rotation + 180) % 360);
        }
        else{
            currTileRotation.eulerAngles = new Vector3(0, 0, GetActiveTile().rotation);
        }
        GetActiveTile().transform.rotation = currTileRotation;
        _hotbar.RotateCurrent(dir, activeIndex, GetActiveTile());
    }

    public void ActiveDestroy()
    {
        SquareTile newEmptyTile = Instantiate(emptyTile);
        newEmptyTile.gameObject.SetActive(false);
        inventory[activeIndex - 1] = newEmptyTile;
        _hotbar.VisualUpdate();
    }

    public void SetInventorySize(int n)
    {
        inventorySize = n;
    }

    public void SetInventory(SquareTile[] newInventory){
        inventory = newInventory;
    }
}
