using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    private AudioSource mAudioSource;
    public AudioClip sonidoFinal;
    public GameObject buttons;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.PlayOneShot(sonidoFinal, 1f);
        StartCoroutine(EnableButtons());
    }
    IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(sonidoFinal.length-5f);
        buttons.SetActive(true);
    }
}
