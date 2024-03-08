using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownScript : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void OnEnable(){
        dropdown.value = (int) PlayerPreferences.colourBlindMode;
    }
}
