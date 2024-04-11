using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialManager1 : TutorialManagerParent
{

    public Texture2D rules;
    // The different actions being explained in the tutorial
    public string[] actionStrings = {"Move", "Place", "Inventory Select", "Inventory Scroll", "Inventory Rotate", "Shuffle"};
    // Which texture each action corresponds to
    public int[] actionTextureNumber = {0, 1, 2, 2, 3, 4};
    // The strings being displayed on the text boxes
    private string[] displayStrings;
    
    private ReadOnlyArray<InputBinding> bindings;
    private List<string> bindStrings = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateTutorialBoxes();
        GoalCanvas.SetActive(false);

        // The below code iterates through all of the different key bindings, assigning them to the "action"
        // that the player takes using that binding.

        // It then formats the bindings into a string which can be used in the tutorial to explain how to take an action.

        bindings = GameObject.FindObjectOfType<PlayerInput>().actions.FindActionMap("Player").bindings;
        List<List<string>> bindingList = new List<List<string>>();

        // Makes an empty list for each action
        for (int i = 0; i < actionStrings.Length; i++) {
            bindingList.Add(new List<string>());
        }

        // Iterates over the different bindings, getting their display string
        // then it assigns it to the list corresponding to the action it is part of
        for (int i = 0; i < bindings.Count; i++) {
            string displayString = bindings[i].ToDisplayString();
            if (string.IsNullOrEmpty(displayString)) {
                continue;
            }

            for (int j = 0; j < actionStrings.Length; j++) {
                if (bindings[i].action == actionStrings[j]) {
                    bindingList[j].Add(displayString);
                    break;
                }
            }
        }

        // This sets the display strings for each action.
        for (int i = 0; i < bindingList.Count; i++) {
            bindStrings.Add(BindingsToString(bindingList[i]));
        }

        // Block to set the location and form of the persistent rules block
        {
        GameObject ruleBox = Instantiate(tutorialPrefab, new Vector3(0, 0, 0), Quaternion.identity, Camera.main.transform);
        ruleBox.GetComponent<TextUpdate>().textSet("");
        SpriteRenderer _rend = ruleBox.GetComponent<SpriteRenderer>();
        Vector3 boxSize = _rend.sprite.bounds.size;

        Sprite newSprite = Sprite.Create(rules, new Rect(0, 0, textures[1].width, textures[1].height), new Vector2(0.5f, 0.5f), 64);
        _rend.sprite = newSprite;
        float vert = Camera.main.orthographicSize;
        float hoz = vert * Camera.main.aspect;
        ruleBox.transform.position = new Vector3(hoz - boxSize.x / 1.75f, vert - boxSize.y / 1.75f, 0);
        }


        // Merges action strings into one string to be displayed if they are assigned the same tutorial box.
        displayStrings = new string[textures.Length];
        for (int i = 0; i < actionTextureNumber.Length; i++) {
            int idx = actionTextureNumber[i];

            if (string.IsNullOrEmpty(displayStrings[idx])) {
                displayStrings[idx] = bindStrings[i];
            } else {
                displayStrings[idx] = displayStrings[idx] + "\n\n" + bindStrings[i];
            }
        }

        // Updates the text for each tutorial box
        for (int i = 0; i < tutorialBoxes.Length; i++) {
            tutorialBoxes[i].GetComponent<TextUpdate>().textSet(displayStrings[i]);
        }

    }

    // Update is called once per frame
    void Update() {
        StateCheck();
    } 

    public void OnMove() {
        if (stateFlag == 0) {
            LevelManager.playerScore += 1;
            stateFlag++;
        }
    }

    void PlaceSuccess() {
        if (stateFlag == 3) {
            LevelManager.playerScore += 1;
            stateFlag++;
        }
    }

    void OnInventorySelect() {
        if (stateFlag == 1) {
            LevelManager.playerScore += 1;
            stateFlag++;
        }
    }

    void OnInventoryScroll() {
        OnInventorySelect();
    }

    void OnInventoryRotate() {
        if (stateFlag == 2) {
            LevelManager.playerScore += 1;
            stateFlag++;
        }
    }

    void ShuffleSuccess() {
        if (stateFlag == 4) {
            LevelManager.playerScore += 1;
            stateFlag++;
        }
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
