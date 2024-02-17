using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    private ButtonScript[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons = FindObjectsOfType<ButtonScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelect() {
        foreach (ButtonScript button in buttons) {
            button.AttemptActivate();
        }
    }
}
