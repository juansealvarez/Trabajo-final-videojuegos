using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffectUI : MonoBehaviour
{
    public Image bloodEffectUI;
    private float r;
    private float g;
    private float b;
    private float a;
    private float saludActual;
    private float saludInicial;
    public PlayerController player;
    void Start()
    {
        r = bloodEffectUI.color.r;
        g = bloodEffectUI.color.g;
        b = bloodEffectUI.color.b;
        a = bloodEffectUI.color.a;
        saludInicial = player.PlayerHealth;
        saludActual = saludInicial;
    }

    
    void Update()
    {
        saludActual = player.PlayerHealth;
        a = (1-saludActual/saludInicial);
        a = Mathf.Clamp(a, 0, 1f);
        ChangeColor();
    }

    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        bloodEffectUI.color = c; 
    }
}
