using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialManager : MonoBehaviour
{
    private ReadOnlyArray<InputBinding> bindings;

    // Start is called before the first frame update
    void Start()
    {
        bindings = GameObject.FindObjectOfType<PlayerInput>().actions.FindActionMap("Player").bindings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
