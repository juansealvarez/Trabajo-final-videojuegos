using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBox : MonoBehaviour
{
    public int precio;
    // Start is called before the first frame update
    void Start()
    {
        if(StateNameController.isHardcoreMode)
        {
            precio *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
