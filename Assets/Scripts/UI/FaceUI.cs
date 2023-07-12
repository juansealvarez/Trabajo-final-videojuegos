using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceUI : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private List<Sprite> imagesStates;

    private float saludActual;
    private float saludInicial;
    public PlayerController player;
    private void Start()
    {
        saludInicial = player.PlayerHealth;
        saludActual = saludInicial;
    }

    private void Update()
    {
        saludActual = player.PlayerHealth;
        if(saludActual == saludInicial)
        {
            image.sprite = imagesStates[0];
        }else if (saludActual > saludInicial*0.75)
        {
            image.sprite = imagesStates[1];
        }else if (saludActual > saludInicial*0.5)
        {
            image.sprite = imagesStates[2];
        }else
        {
            image.sprite = imagesStates[3];
        }
    }
}
