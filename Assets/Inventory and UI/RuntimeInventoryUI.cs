using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class RuntimeInventoryUI : MonoBehaviour
{

    public GameObject inventoryObj;
    public Sprite[] select_sprites;

    private UIDocument uiDocument;
    private VisualElement root;
    private int itemCount;
    private PenroseTile[] inventory;

    void Start() {
        
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {

        inventoryObj.GetComponent<InventoryManager>().initializeInventory();
        itemCount = inventoryObj.GetComponent<InventoryManager>().inventorySize;
        inventory = inventoryObj.GetComponent<InventoryManager>().inventory;

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        for (int i = 0; i < itemCount; i++) {
            Sprite tileSprite = inventory[i].GetComponent<SpriteRenderer>().sprite;
            uiDocument.rootVisualElement.Q((i+1).ToString()).style.backgroundImage = new StyleBackground(tileSprite);
        }

        uiDocument = GetComponent<UIDocument>();
        // The UXML is already instantiated by the UIDocument component
        Camera mainCamera = Camera.main;
        uiDocument.transform.parent = mainCamera.transform;
        uiDocument.transform.localPosition = new Vector3(mainCamera.transform.position.x, 0, 1);

        //_button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    // index is from 0-4
    public void Select(int index) {
        Sprite unselected = select_sprites[0];
        Sprite selected = select_sprites[1];

        for (int i = 0; i < itemCount; i++) {
            // Gets the highlight visual elements of each inventory item and deselects it
            uiDocument.rootVisualElement.Q((i+1).ToString()).Q("highlight").style.backgroundImage = new StyleBackground(unselected);
        }

        // Sets the chosen inventory item to selected
        uiDocument.rootVisualElement.Q((index).ToString()).Q("highlight").style.backgroundImage = new StyleBackground(selected);
    }

    private void OnDisable()
    {
        //_button.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        //++_clickCount;

        //Debug.Log($"{"button"} was clicked!" +
        //        (_toggle.value ? " Count: " + _clickCount : ""));
    }
}
