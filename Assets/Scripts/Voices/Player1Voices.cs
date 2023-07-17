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
                StartCoroutine(playStartSounds());
                hasPlayedAudio = true;
            }
        }
        
    }
    IEnumerator playStartSounds()
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
    public void AudiosCoopRondas(int Ronda)
    {
        if(Ronda == 2)
        {
            StartCoroutine(playRound2Sounds());
        }else if(Ronda == 5)
        {
            StartCoroutine(playRound5Sounds());
        }else if(Ronda == 7)
        {
            StartCoroutine(playRound7Sounds());
        }else if(Ronda == 9)
        {
            StartCoroutine(playRound9Sounds());
        }
    }

    IEnumerator playRound2Sounds()
    {
        BackgroundSource.PlayOneShot(voicesPlayer2[1], 10f);
        yield return new WaitForSeconds(voicesPlayer2[1].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[2], 10f);
        yield return new WaitForSeconds(voicesPlayer1[2].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[2], 10f);
    }
    IEnumerator playRound5Sounds()
    {
        BackgroundSource.PlayOneShot(voicesPlayer2[3], 10f);
        yield return new WaitForSeconds(voicesPlayer2[3].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[3], 10f);
        yield return new WaitForSeconds(voicesPlayer1[3].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[4], 10f);
    }
    IEnumerator playRound7Sounds()
    {
        BackgroundSource.PlayOneShot(voicesPlayer2[5], 10f);
        yield return new WaitForSeconds(voicesPlayer2[5].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[4], 10f);
        yield return new WaitForSeconds(voicesPlayer1[4].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[6], 10f);
        yield return new WaitForSeconds(voicesPlayer2[6].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[5], 10f);
        yield return new WaitForSeconds(voicesPlayer1[5].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[7], 10f);
    }
    IEnumerator playRound9Sounds()
    {
        BackgroundSource.PlayOneShot(voicesPlayer1[6], 10f);
        yield return new WaitForSeconds(voicesPlayer1[6].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[8], 10f);
        yield return new WaitForSeconds(voicesPlayer2[8].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[7], 10f);
        yield return new WaitForSeconds(voicesPlayer1[7].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer2[9], 10f);
    }
    public void playSoloBossDead()
    {
        BackgroundSource.PlayOneShot(voicesSolo[5], 10f);
    }
    public void playCoopBossDead()
    {
        StartCoroutine(playCoopDeadSounds());
    }
    IEnumerator playCoopDeadSounds()
    {
        BackgroundSource.PlayOneShot(voicesPlayer2[10], 10f);
        yield return new WaitForSeconds(voicesPlayer2[10].length+1f);
        BackgroundSource.PlayOneShot(voicesPlayer1[8], 10f);
    }
    
}
