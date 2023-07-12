using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    public static AmmoCrate Instance { private set; get; }
    public void Destroy()
    {
        this.Destroy();
    }
}
