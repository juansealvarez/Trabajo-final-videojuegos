using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Voices : MonoBehaviour
{
    public static Player1Voices Instance { private set; get; }
    [SerializeField]
    private List<AudioClip> voicesSolo;

    private AudioSource BackgroundSource;
    private float Timer = 3f;
    private bool hasPlayedAudio = false;

    private void Start()
    {
        BackgroundSource = transform
            .Find("Main Camera").GetComponent<AudioSource>();
    }
    private void Update()
    {
        
        Timer -= Time.deltaTime;
        if (Timer < 0f)
        {
            if (StateNameController.isSoloMode && !hasPlayedAudio)
            {
                StartCoroutine(playEngineSound());
                hasPlayedAudio = true;
            }
        }
        
    }
    IEnumerator playEngineSound()
        {
        BackgroundSource.PlayOneShot(voicesSolo[0], 10f);
        yield return new WaitForSeconds(voicesSolo[0].length+1f);
        BackgroundSource.PlayOneShot(voicesSolo[1], 10f);
    }

    
}
