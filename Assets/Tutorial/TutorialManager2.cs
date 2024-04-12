using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager2 : TutorialManagerParent
{
    // Start is called before the first frame update
    void Start()
    {
        GoalCanvas.SetActive(false);
        GenerateTutorialBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        StateCheck();
    }

    
}
