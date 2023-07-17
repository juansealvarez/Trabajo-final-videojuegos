using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Voices : MonoBehaviour
{
    public static Player1Voices Instance { private set; get; }
    [SerializeField]
    private List<AudioClip> voicesSolo;
    public List<AudioClip> voicesPlayer1;
    public List<AudioClip> voicesPlayer2;
    [System.NonSerialized]
    public AudioSource BackgroundSource;
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
                BackgroundSource.PlayOneShot(voicesSolo[0], 10f);
                hasPlayedAudio = true;
            }else if(!StateNameController.isSoloMode && !hasPlayedAudio)
            {
                StartCoroutine(playEngineSound());
                hasPlayedAudio = true;
            }
        }
        
    }
    IEnumerator playEngineSound()
    {
        BackgroundSource.PlayOneShot(voicesPlayer1[0], 10f);
        yield return new WaitForSeconds(voicesPlayer1[0].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[0], 10f);
        yield return new WaitForSeconds(voicesPlayer2[0].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[1], 10f);
    }
    public void AudiosSoloRondas(int Ronda)
    {
        if(Ronda == 2)
        {
            BackgroundSource.PlayOneShot(voicesSolo[1], 10f);
        }else if(Ronda == 5)
        {
            BackgroundSource.PlayOneShot(voicesSolo[2], 10f);
        }else if(Ronda == 7)
        {
            BackgroundSource.PlayOneShot(voicesSolo[3], 10f);
        }else if(Ronda == 9)
        {
            BackgroundSource.PlayOneShot(voicesSolo[4], 10f);
        }
    }

    
}
