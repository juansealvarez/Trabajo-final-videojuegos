using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipLoader : MonoBehaviour
{
    public List<string> tipsACargar;
    public TextMeshProUGUI Tip;

    void Start()
    {
        int random = Random.Range(0, tipsACargar.Count);
        Tip.text = "Tip: " + tipsACargar[random];
    }
}
