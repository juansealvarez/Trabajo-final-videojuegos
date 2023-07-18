using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerController PlayerController;
    public GameObject Bullet;

    public List<GameObject> EnemiesToInstantiate;
    public int CantidadZombiesPorHorda = 10;
    public int CantidadZombiesPorHordaCoop = 15;

    public static GameManager Instance { private set; get; }
    public GameObject UIReload;
    public GameObject UILowAmmo;
    public GameObject UINoAmmo;
    [System.NonSerialized]
    public int Ronda = 0;
    public GameObject RondaUI;
    public GameObject RondaUI1;
    public GameObject RondaUI2;
    public bool CopyrigthSong = false;
    public List<AudioClip> BackgroundAudio;
    private AudioSource BackgroundSource;
    [System.NonSerialized]
    public bool isSoloGame;
    public GameObject UISolo;
    public GameObject UICoop;
    public AimShotgun escopeta;
    public AimShotgun pistola;
    public TextMeshProUGUI arma;
    public TextMeshProUGUI balas;
    private int zombies;
    [System.NonSerialized]
    public GameObject UItoActivateAndActivate;

    [SerializeField]
    private List<GameObject> Spawnpoints;
    [System.NonSerialized]
    public int zombiesActuales = 0;
    [SerializeField]
    private Player1Voices player1voices;
    public CinematicController cinecontrol;
    private bool bossIniciado = false;
    [SerializeField]
    private int rondaMaxima;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        isSoloGame = StateNameController.isSoloMode;
        if(isSoloGame)
        {
            zombies = CantidadZombiesPorHorda;
            UItoActivateAndActivate = UISolo;
        }else
        {
            zombies = CantidadZombiesPorHordaCoop;
            UItoActivateAndActivate = UICoop;
        }
        BackgroundSource = transform
            .GetComponent<AudioSource>();
        if (!isSoloGame)
        {
            UISolo.SetActive(false);
            UICoop.SetActive(true);
            Player1.transform.Find("Main Camera").GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 1);
            Player2.SetActive(true);
            escopeta.arma = arma;
            escopeta.balas = balas;
            pistola.arma = arma;
            pistola.balas = balas;
            PlayerController.UIReload = UIReload;
            PlayerController.UILowAmmo = UILowAmmo;
            PlayerController.UINoAmmo = UINoAmmo;
        }
    }
    private void Update()
    {
        if (Ronda <= (rondaMaxima-1))
        {
            if (zombiesActuales == 0)
            {
                Ronda++;
                RondaUI.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                RondaUI2.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                RondaUI1.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                StartCoroutine(newRound());
            }
        }
        else
        {
            if (zombiesActuales == 0)
            {

                if (!bossIniciado)
                {
                    UItoActivateAndActivate.SetActive(false);
                    cinecontrol.IniciarCinematica();
                    bossIniciado = true;
                }

            }
        }

    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < zombies; i++)
        {
            var LugarRandom = UnityEngine.Random.Range(0, Spawnpoints.Count);
            var instantiatePosition = Spawnpoints[LugarRandom].transform.position;
            int random = UnityEngine.Random.Range(0, 2);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
            enemy.GetComponent<EnemyController>().Bullet = GameManager.Instance.Bullet;
        }

    }

    IEnumerator newRound()
    {
        zombiesActuales = zombies;
        if (CopyrigthSong)
        {
            BackgroundSource.PlayOneShot(BackgroundAudio[0], 0.5f);
        }
        yield return new WaitForSeconds(BackgroundAudio[0].length - 2f);
        if (isSoloGame)
        {
            player1voices.AudiosSoloRondas(Ronda);
        }
        else
        {
            player1voices.AudiosCoopRondas(Ronda);
        }
        SpawnEnemies();
    }
}
