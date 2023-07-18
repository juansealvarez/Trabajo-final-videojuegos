using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = false;
    }

    void Update()
    {
        if (toggle.isOn)
        {
            StateNameController.isHardcoreMode = true;
        }else
        {
            StateNameController.isHardcoreMode = false;
        }
    }
}
