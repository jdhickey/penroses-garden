using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class RuntimeInventoryUI : MonoBehaviour
{
    public Sprite[] select_sprites;

    private InventoryManager inventoryManagementScript;

    private UIDocument uiDocument;
    private VisualElement root;

    private void Awake()
    {
        inventoryManagementScript = GameObject.FindGameObjectWithTag("InventoryManagement").GetComponent<InventoryManager>();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        VisualUpdate();

        // The UXML is already instantiated by the UIDocument component
        Camera mainCamera = Camera.main;
        uiDocument.transform.parent = mainCamera.transform;
        uiDocument.transform.localPosition = new Vector3(mainCamera.transform.position.x, 0, 1);
        uiDocument.panelSettings.scale = 1.25f;
    }

    public void VisualUpdate() {
        // Variable set-up.
        SquareTile[] inventory = inventoryManagementScript.GetInventory();
        int itemCount = inventoryManagementScript.GetInventorySize();

        // Populate UI.
        for (int i = 0; i < itemCount; i++) {
            Sprite tileSprite = inventory[i].GetComponent<SpriteRenderer>().sprite;
            uiDocument.rootVisualElement.Q("Inventory_Slot_"+(i+1).ToString()+"_Border").Q((i + 1).ToString()).style.backgroundImage = new StyleBackground(tileSprite);
            uiDocument.rootVisualElement.Q("Inventory_Slot_"+(i+1).ToString()+"_Border").Q((i + 1).ToString()).style.rotate = new StyleRotate(new Rotate(0));
        }
    }

    // index is from 0 to inventorySize - 1.
    public void Select(int index) {
        // Variable set-up.
        Sprite unselected = select_sprites[0];
        Sprite selected = select_sprites[1];
        int itemCount = inventoryManagementScript.GetInventorySize();
        
        for (int i = 0; i < itemCount; i++) {
            // Gets the highlight visual elements of each inventory item and deselects it
            uiDocument.rootVisualElement.Q("Inventory_Slot_"+(i+1).ToString()+"_Border").style.backgroundImage = new StyleBackground(unselected);
        }

        // Sets the chosen inventory item to selected
        uiDocument.rootVisualElement.Q("Inventory_Slot_"+(index)+"_Border").style.backgroundImage = new StyleBackground(selected);
    }

    public void RotateCurrent(int dir, int index, SquareTile activeTile){
        uiDocument.rootVisualElement.Q("Inventory_Slot_"+(index).ToString()+"_Border").Q(index.ToString()).style.rotate = new StyleRotate(new Rotate(activeTile.rotation));
    }
}
