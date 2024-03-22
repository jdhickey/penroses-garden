using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialManager : MonoBehaviour
{
    private ReadOnlyArray<InputBinding> bindings;
    private List<string> move;
    private List<string> place;
    private List<string> select;
    private List<string> scroll;
    private List<string> rotate;
    private List<string> shuffle;

    // Start is called before the first frame update
    void Start()
    {
        bindings = GameObject.FindObjectOfType<PlayerInput>().actions.FindActionMap("Player").bindings;
        move = new List<string>();
        place = new List<string>();
        select = new List<string>();
        scroll = new List<string>();
        rotate = new List<string>();
        shuffle = new List<string>();

        for (int i = 0; i < bindings.Count; i++) {
            string displayString = bindings[i].ToDisplayString();

            if (string.IsNullOrEmpty(displayString)) continue;

            if (bindings[i].action == "Move") {
                move.Add(displayString);
            } else if (bindings[i].action == "Place") {
                place.Add(displayString);
            } else if (bindings[i].action == "Inventory Select") {
                select.Add(displayString);
            } else if (bindings[i].action == "Inventory Scroll") {
                scroll.Add(displayString);
            } else if (bindings[i].action == "Inventory Rotate") {
                rotate.Add(displayString);
            } else if (bindings[i].action == "Shuffle") {
                shuffle.Add(displayString);
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
