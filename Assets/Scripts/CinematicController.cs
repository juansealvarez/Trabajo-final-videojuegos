using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CinematicController : MonoBehaviour
{

    public static CinematicController Instance { private set; get; }
    public GameObject smokeParticles;
    public GameObject chispasParticles;
    public GameObject bigRock;
    public GameObject[] smallRocks; // Ahora es un array
    public GameObject character;
    public GameObject camara;
    public Material skyNight;
    public Material skyDay;
    public GameObject ligth;
    public PlayerController playerController;
    private GameManager gameManager;
    private AudioSource mAudioSource;
    public AudioClip explosion;

    public void IniciarCinematica()
    {
        mAudioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayScene());
    }
    public void HacerDia()
    {
        ligth.SetActive(true);
        RenderSettings.skybox = skyDay;
    }

    IEnumerator PlayScene()
    {
        yield return new WaitForSeconds(2);
        PlayerController.mPlayerInput.enabled = false;
        playerController.mAudioSource.enabled = false;
        playerController.pAudioSource.enabled = false;
        playerController.BackgroundSource.enabled = false;
        gameManager.UItoActivateAndActivate.SetActive(false);
        ligth.SetActive(false);
        RenderSettings.skybox = skyNight;
        camara.SetActive(true);
        // 1. Aparecen partículas de humo
        chispasParticles.SetActive(true);

        yield return new WaitForSeconds(5);
        chispasParticles.SetActive(false);
        smokeParticles.SetActive(true);
        // 2. La roca desaparece del mapa (se oculta)
        ActivateSmallRocks();

        character.SetActive(true);
        bigRock.SetActive(false);
        mAudioSource.PlayOneShot(explosion);

        // 3. Las rocas pequeñas que están ocultas dentro del modelo de la roca se mueven como si saltaran.

    }

    void ActivateSmallRocks()
    {
        // Activa las rocas pequeñas y aplica una fuerza a cada una
        foreach (GameObject smallRock in smallRocks)
        {
            smallRock.SetActive(true);
            Rigidbody rb = smallRock.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)) * 500;
            rb.AddForce(force);
        }
    }
}
