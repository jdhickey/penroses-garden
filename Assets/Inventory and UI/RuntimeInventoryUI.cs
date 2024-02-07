using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeInventoryUI : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;

    private int _clickCount;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        Camera mainCamera = Camera.main;
        uiDocument.transform.parent = mainCamera.transform;
        uiDocument.transform.localPosition = new Vector3(0, 0, 1);


        _button = uiDocument.rootVisualElement.Q("button") as Button;
        _toggle = uiDocument.rootVisualElement.Q("toggle") as Toggle;

        _button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        ++_clickCount;

        Debug.Log($"{"button"} was clicked!" +
                (_toggle.value ? " Count: " + _clickCount : ""));
    }
}
