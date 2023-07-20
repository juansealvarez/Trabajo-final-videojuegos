using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    private AudioSource mAudioSource;
    public AudioClip sonidoFinal;
    public GameObject buttons;
    public TextMeshProUGUI text;
    private float r;
    private float g;
    private float b;
    private float a;
    
    private void Start()
    {
        r = text.color.r;
        g = text.color.g;
        b = text.color.b;
        a = text.color.a;
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.PlayOneShot(sonidoFinal, 1f);
        StartCoroutine(EnableButtons());
    }

    private void Update()
    {
        float gradient = 0.01f;
        a = (a+gradient);
        a = Mathf.Clamp(a, 0, 1f);
        ChangeColor();
    }

    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        text.color = c; 
    }

    IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(sonidoFinal.length/2f);
        buttons.SetActive(true);
    }
}
