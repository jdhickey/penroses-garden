using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplashText : MonoBehaviour
{
    public TextAsset splash;
    private string[] splashLines;
    private int length;
    private bool initialized;
    private string thisString;

    // Start is called before the first frame update
    void Start()
    {
        if (!initialized) {
            splashLines = splash.text.Split('\n');
            length = splashLines.Length;

            initialized = true;
        }

        thisString = splashLines[Random.Range(0, length)];
        GetComponent<TextMeshProUGUI>().text = thisString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
