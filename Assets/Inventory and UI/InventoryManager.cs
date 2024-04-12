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
    }

    public void initializeInventory(int[] presets) {
        // Populates inventory using randomTile.
        try {
            GetActiveTile().gameObject.SetActive(false);
        } catch {}

        for (int i = 0; i < inventorySize; i++) {
            if (i < presets.Length) {
                inventory[i] = setTile(presets[i]);
            } else {
                inventory[i] = randomTile();
            }
        }
        GetActiveTile().gameObject.SetActive(true);
        GetActiveTile().gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void initializeInventory() {
        initializeInventory(new int[0]);
    }

    public SquareTile setTile(int i) {
        SquareTile newTile = Instantiate(tileOptions[i]);
        newTile.gameObject.SetActive(false);
        return newTile;
    }

    public SquareTile randomTile() {
        return setTile(Random.Range(0, tileOptions.Length));
    }

    public void ShuffleInventory(){
        for (int i = 0; i < inventorySize; i++){
            if (inventory[i].sides[0] == -1){
                Destroy(inventory[i].gameObject);
                inventory[i] = randomTile();
            }
        }
        _hotbar.VisualUpdate();
    }

    public void VisualUpdate() {
        _hotbar.VisualUpdate();
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
        SquareTile tile = GetActiveTile();

        tile.rotateSides(dir);
        Quaternion currTileRotation = Quaternion.identity;
        if (tile.rotation % 360 == 90 || tile.rotation % 360 == 270 || tile.rotation == -90 || tile.rotation == -270){
            currTileRotation.eulerAngles = new Vector3(0, 0, (tile.rotation + 180) % 360);
        }
        else{
            currTileRotation.eulerAngles = new Vector3(0, 0, tile.rotation);
        }
        tile.transform.rotation = currTileRotation;

        Quaternion hiveRotation = new Quaternion();
        hiveRotation.eulerAngles = -currTileRotation.eulerAngles;
        tile.transform.GetChild(0).gameObject.transform.localRotation = hiveRotation;
        _hotbar.RotateCurrent(dir, activeIndex, tile);
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
