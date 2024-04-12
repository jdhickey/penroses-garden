using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager2 : TutorialManagerParent
{
    private static InventoryManager inv;

    void Awake() {
        inv = FindObjectOfType<InventoryManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GoalCanvas.SetActive(false);
        inv.initializeInventory(new int[] {0, 0, 3, 4, 5});
        inv.VisualUpdate();
        GenerateTutorialBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        StateCheck();
    }
}
