using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdate : MonoBehaviour
{
    TextMeshProUGUI textBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void textSet(string str) {
        textBox = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textBox.SetText(str);
        textBox.ForceMeshUpdate(true);
    }
}
