using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    private Slider slider;
    [SerializeField]
    private Cargando cargando;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        slider.value = cargando.porcentaje;
    }
}
