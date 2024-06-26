using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    private InventoryManager inventoryManagementScript;
    private SquareTilePlacement squareTilePlacementScript;
    private float lastScroll = 0f;
    [Range(0.0f, 1.0f)]
    public float scrollInterval = 1f;
    private PlayerInput input;
    public float shuffleRadius = 1.5f;
    
    public GameObject PauseMenu;
    public GameObject winCondition;
    public GameObject loseCondition;
    public GameObject inventoryUI;
    public GameObject hatPrefab;
    
    private AudioSource audio;
    [SerializeField]
    private AudioClip placeSound;
    [SerializeField]
    private AudioClip failSound;
    [SerializeField]
    private AudioClip shuffleSound;
    private ScoreUpdater scoreUpdaterScript;
    private Sprite[] hats;

    private float startTime; 
    private bool startTimeSet = false;
    private int hatsSpawned = 0;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
        inventoryUI = FindObjectOfType<RuntimeInventoryUI>().gameObject;
        squareTilePlacementScript = this.gameObject.GetComponent<SquareTilePlacement>();
        PauseMenu.SetActive(false);
        audio = GetComponent<AudioSource>();
        try{
            scoreUpdaterScript = GameObject.FindGameObjectWithTag("Scoring").GetComponent<ScoreUpdater>();
        }
        catch (Exception){
            Debug.Log("The score updater thingy isn't present.");
        }

        hats = Resources.LoadAll<Sprite>("Hats");
    }

    void Update() {
        if (!startTimeSet) {
            startTime = Time.realtimeSinceStartup;
            startTimeSet = true;
        }
    }

    private void OnShuffle()
    {
        Collider2D[] near = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), shuffleRadius);

        bool found = false;

        foreach (Collider2D obj in near) {
            SquareTile tile = obj.gameObject.GetComponent<SquareTile>();

            if ((tile != null && tile.isHive) || SceneManager.GetActiveScene().name == "tutorial") {
                if (SceneManager.GetActiveScene().name.Contains("tutorial")){
                    BroadcastMessage("ShuffleSuccess");
                    inventoryManagementScript.PlayerShuffle();
                }
                else{
                    inventoryManagementScript.ShuffleInventory();
                }
                audio.PlayOneShot(shuffleSound);
                found = true;
                break;
            }
        }

        if (!found){
            audio.PlayOneShot(failSound);
        }
        SquareTile activeTile = inventoryManagementScript.GetActiveTile();
        if (activeTile.sides[0] != -1){
            activeTile.gameObject.SetActive(true);
            activeTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void OnInventorySelect(InputValue value)
    {
        if (value.isPressed)
        {
            int intVal = (int) value.Get<float>();
            inventoryManagementScript.PlayerSelect(intVal);
        }
    }

    private void OnInventoryScroll(InputValue value)
    {
        // The time check manages scrolling going too fast.
        int intVal = (int) value.Get<float>();
        if (intVal != 0 && Time.time - lastScroll > scrollInterval){
            if (intVal != 1 || intVal != -1){
                lastScroll = Time.time;
                intVal /= Mathf.Abs(intVal);
                inventoryManagementScript.PlayerScroll(intVal);
            }
        } 
    }

    public void OnExit()
    {
        if (PauseMenu.activeSelf || (winCondition != null && winCondition.activeSelf) || (loseCondition != null &&loseCondition.activeSelf)){
            inventoryUI.SetActive(true);
            PauseMenu.SetActive(false);
            winCondition.SetActive(false);
            if (loseCondition != null){
                loseCondition.SetActive(false);
            }
            input.actions.FindAction("Move").Enable();
            input.actions.FindAction("Place").Enable();
            input.actions.FindAction("Inventory Select").Enable();
            input.actions.FindAction("Inventory Scroll").Enable();
            input.actions.FindAction("Inventory Rotate").Enable();
            input.actions.FindAction("Shuffle").Enable();
        }
        else {
            PauseMenu.SetActive(true);
            input.actions.FindAction("Move").Disable();
            input.actions.FindAction("Place").Disable();
            input.actions.FindAction("Inventory Select").Disable();
            input.actions.FindAction("Inventory Scroll").Disable();
            input.actions.FindAction("Inventory Rotate").Disable();
            input.actions.FindAction("Shuffle").Disable();
        }
    }

    private void OnInventoryRotate(InputValue value){
        int intVal = (int) value.Get<float>();
        if (intVal != 0){
            if (intVal != 1 || intVal != -1){
                intVal /= Mathf.Abs(intVal);
            }
        }
        inventoryManagementScript.RotateCurrent(intVal, inventoryManagementScript.GetActiveTile());
    }

    public Vector3 GetGridPos(){
        Vector3 currPos = transform.position - new Vector3(0f, 0.35f, 0f);
        Vector3 gridPos = new Vector3(Mathf.Round(currPos.x), Mathf.Round(currPos.y), 0);
        return gridPos;
    }

    private void OnPlace()
    {
        SquareTile tilePlayed = inventoryManagementScript.GetActiveTile();
        if (tilePlayed.sides[0] != -1) // Replace with reference to the empty tile.
        {
            Vector3 gridPos = GetGridPos();
            bool result = squareTilePlacementScript.PlaceTile(gridPos, tilePlayed);

            if (result)
            {
                int newPoints = 0;
                audio.PlayOneShot(placeSound);
                if (LevelManager.pointPerTile){
                    newPoints++;
                }
                if (LevelManager.pointPerConnection){
                    foreach (SquareTile side in tilePlayed.neigh){
                        if (side != null){
                            newPoints++;
                        }
                    }
                }
                LevelManager.tilePlaced = true;
                if (scoreUpdaterScript != null && newPoints != 0){
                    scoreUpdaterScript.UpdateScore(newPoints);
                }
                inventoryManagementScript.ActiveDestroy();

                if (SceneManager.GetActiveScene().name.Contains("tutorial")){
                    BroadcastMessage("PlaceSuccess");
                }
            }
            else{
                audio.PlayOneShot(failSound);
                tilePlayed.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.5f);
                Debug.Log(tilePlayed.gameObject.GetComponent<SpriteRenderer>().color);
                Invoke("ReturnToNormal", 0.5f);
                Debug.Log(tilePlayed.gameObject.GetComponent<SpriteRenderer>().color);
            }
        }

        CheckHat();
    }

    void ReturnToNormal(){
        inventoryManagementScript.GetActiveTile().gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    void FixedUpdate(){
        inventoryManagementScript.GetActiveTile().transform.position = GetGridPos();
        float dt = Time.realtimeSinceStartup - startTime;

        // MULTIPLY CHECKVAL BUT A LARGE NUMBER TO CHANGE HAT FREQUENCY

        float checkVal = hatCalc(dt);

        if (UnityEngine.Random.value < checkVal * Time.fixedDeltaTime) {
            SpawnHat();
        }
    }

    void SpawnHat() {
        Vector3 randVec = UnityEngine.Random.insideUnitCircle * 5 * gameObject.GetComponent<SpriteRenderer>().size.x;
        GameObject hat = Instantiate(hatPrefab, gameObject.transform.position + randVec, Quaternion.identity);
        hat.GetComponent<SpriteRenderer>().sprite = hats[UnityEngine.Random.Range(0, hats.Length)];
        hatsSpawned += 1;
        startTimeSet = false;
    }

    void CheckHat() {
        Collider2D[] potentialHats = Physics2D.OverlapCircleAll(transform.position, gameObject.GetComponent<SpriteRenderer>().size.x, 128);
        
        if (potentialHats.Length > 0) {
            GameObject chosenHat = potentialHats[0].gameObject;

            // If there is no hat, assigns the found hat as the child. Otherwise just updates the old hat sprite.
            try {
                GameObject currentHat = gameObject.transform.Find("Hat").gameObject;
                currentHat.GetComponent<SpriteRenderer>().sprite = chosenHat.GetComponent<SpriteRenderer>().sprite;
                Destroy(chosenHat);
            } catch {
                chosenHat.gameObject.name = "Hat";
                Destroy(chosenHat.GetComponent<Collider2D>());
                chosenHat.transform.SetParent(gameObject.transform);
                chosenHat.transform.localPosition = new Vector3(0.1f, -0.2f, 0);
                chosenHat.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Cover");
            }
        }
    }

    // A sigmoidal function which slowly increases the change of a hat to spawn per second up to 100%
    float hatCalc(float dt) {
        float val = 1 + Mathf.Exp(-(dt/100 - 1.5f));
        return 1 / ((20 + 10 * hatsSpawned) * val);
    }
}
