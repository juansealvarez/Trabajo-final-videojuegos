using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEnd : MonoBehaviour
{
    public Image endUI;
    private float r;
    private float g;
    private float b;
    private float a;
    void Start()
    {
        r = endUI.color.r;
        g = endUI.color.g;
        b = endUI.color.b;
        a = endUI.color.a;
    }

    
    void Update()
    {
        float gradient = 0.01f;
        a = (a+gradient);
        a = Mathf.Clamp(a, 0, 1f);
        ChangeColor();
    }

    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        endUI.color = c; 
    }
}
