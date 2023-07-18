using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    private float r;
    private float g;
    private float b;
    private float a;
    private bool fadingOut = false;
    private float gradient = 0f;
    public BossController bossController;

    void Start()
    {
        r = text.color.r;
        g = text.color.g;
        b = text.color.b;
        a = text.color.a;
    }

    
    void Update()
    {
        if(fadingOut)
        {
            gradient = -0.1f;
        }else
        {
            gradient = 0.1f;
        }
        
        a = (a+gradient);
        a = Mathf.Clamp(a, 0, 1f);
        ChangeColor();
        StartCoroutine(FadeOut());
    }

    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        text.color = c; 
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(10f);
        fadingOut = true;
        yield return new WaitForSeconds(10f);
        bossController.DeactivateInstructions();
    }
}
