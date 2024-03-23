using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialManager : MonoBehaviour
{
    public TextUpdate moveBox;
    public TextUpdate placeBox;
    public TextUpdate selectBox;
    public TextUpdate rotateBox;
    public TextUpdate shuffleBox;
    
    private ReadOnlyArray<InputBinding> bindings;
    private string move;
    private string place;
    private string select;
    private string scroll;
    private string rotate;
    private string shuffle;

    private List<bool> stateFlags = new List<bool>();
    private List<GameObject> states = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // The below code iterates through all of the different key bindings, assigning them to the "action"
        // that the player takes using that binding.

        // It then formats the bindings into a string which can be used in the tutorial to explain how to take an action.

        bindings = GameObject.FindObjectOfType<PlayerInput>().actions.FindActionMap("Player").bindings;
        List<string> moveList = new List<string>();
        List<string> placeList = new List<string>();
        List<string> selectList = new List<string>();
        List<string> scrollList = new List<string>();
        List<string> rotateList = new List<string>();
        List<string> shuffleList = new List<string>();

        for (int i = 0; i < bindings.Count; i++) {
            string displayString = bindings[i].ToDisplayString();
            displayString.Replace("/", " ");
            if (string.IsNullOrEmpty(displayString)) {
                continue;
            }

            if (bindings[i].action == "Move") {
                moveList.Add(displayString);
            } else if (bindings[i].action == "Place") {
                placeList.Add(displayString);
            } else if (bindings[i].action == "Inventory Select") {
                selectList.Add(displayString);
            } else if (bindings[i].action == "Inventory Scroll") {
                scrollList.Add(displayString);
            } else if (bindings[i].action == "Inventory Rotate") {
                rotateList.Add(displayString);
            } else if (bindings[i].action == "Shuffle") {
                shuffleList.Add(displayString);
            } 
        }

        // This sets the display strings for each action.
        move = BindingsToString(moveList);
        place = BindingsToString(placeList);
        select = BindingsToString(selectList);
        scroll = BindingsToString(scrollList);
        rotate = BindingsToString(rotateList);
        shuffle = BindingsToString(shuffleList);

        Instantiate(moveBox);
        Instantiate(placeBox);
        Instantiate(selectBox);
        Instantiate(rotateBox);
        Instantiate(shuffleBox);
    }

    // Update is called once per frame
    void Update()
    {
        moveBox.textSet(move);
        placeBox.textSet(place);
        selectBox.textSet(select);
        rotateBox.textSet(rotate);
        shuffleBox.textSet(shuffle);
    }

    private static string BindingsToString(List<string> arr) {
        // This joins together the multiple strings representing the control bindings
        // into one string which is suitable for display in the tutorial.

        // It places single characters on the same line, but multichar bindings on individual lines.
        // It replaces control path '/' with a space, assuming that will be human readable
        // It converts the final string to lowercase and removes trailing whitespace.
        string display = arr[0];

        for (int i = 1; i < arr.Count; i++) {
            string sep;

            if (arr[i].Length > 1) {
                sep = "\n";
            } else {
                sep = ", ";
            }

            display = display + sep + arr[i];
        }

        return display.ToLower().Trim().Replace("/", " ");
    }
}
