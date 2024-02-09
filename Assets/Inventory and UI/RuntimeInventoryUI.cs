using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class RuntimeInventoryUI : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;
    private UIDocument uiDocument;
    private VisualTreeAsset itemBox;

    private int _clickCount;

    public void Awake() {
        uiDocument = GetComponent<UIDocument>();
        itemBox = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Inventory and UI/InventoryElement.uxml");
    }

    public void SetUp(int itemCount, PenroseTile[] inventory) {
        VisualElement wrapper = uiDocument.rootVisualElement.Query("InventoryBar");
        for (int i = 0; i < itemCount; i++) {
            Sprite tileSprite = inventory[i].GetComponent<SpriteRenderer>().sprite;

            VisualElement box = itemBox.Instantiate();
            box.Q("icon").style.backgroundImage = new StyleBackground(tileSprite);
            box.name = string.Format("{0}", i);

            uiDocument.rootVisualElement.Add(box);
            Debug.Log("success!");
        }
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        Camera mainCamera = Camera.main;
        uiDocument.transform.parent = mainCamera.transform;
        uiDocument.transform.localPosition = new Vector3(mainCamera.transform.position.x, 0, 1);

        //_button.RegisterCallback<ClickEvent>(PrintClickMessage);
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
