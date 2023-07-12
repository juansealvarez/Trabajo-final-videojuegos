using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    private float saludActual;
    private float saludInicial;
    private float lifePercentage;
    public PlayerController player;
    private void Start()
    {
        saludInicial = player.PlayerHealth;
        saludActual = saludInicial;
        lifePercentage = 100f;
    }

    private void Update()
    {
        saludActual = player.PlayerHealth;
        lifePercentage = (saludActual/saludInicial)*100;
        GetComponent<TextMeshProUGUI>().text = lifePercentage.ToString()+"%";
    }
}
