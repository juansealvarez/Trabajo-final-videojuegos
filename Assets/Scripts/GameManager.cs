using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Interactable2;
    public TextMeshProUGUI UILocator;

    public TextMeshProUGUI Interactable;
    public Transform Player;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerController PlayerController;
    public GameObject Bullet;

    public List<GameObject> EnemiesToInstantiate;
    public int CantidadZombiesPorHorda = 10;
    public int CantidadZombiesPorHordaCoop = 15;

    public static GameManager Instance { private set; get; }
    public AudioSource BossSound;
    public List<AudioClip> StartRoundAudio;

    public List<AudioClip> EndRoundAudio;

    public GameObject UIRockText;

    public GameObject UIReload;
    public GameObject UILowAmmo;
    public GameObject UINoAmmo;
    public TextMeshProUGUI UIScore;
    [System.NonSerialized]
    public int Ronda = 0;
    public GameObject RondaUI;
    public GameObject RondaUI1;
    public GameObject RondaUI2;
    public bool CopyrigthSong = false;
    public List<AudioClip> BackgroundAudio;
    [System.NonSerialized]
    public AudioSource BackgroundSource;
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
    public BossController bossController;
    public Transform boss;
    [System.NonSerialized]
    public int objetosInteractuados;
    public int cantidadObjsParaEEMusical;
    private bool reproduciendoCancionEE;
    public AudioClip EEMusical;
    public GameObject bossSound;
    public AudioClip interactSound;
    public PlayerController player1Controller;
    public PlayerController player2Controller;
    [System.NonSerialized]
    public bool executedKillCommand;
    public bool commandKillDone;
    [System.NonSerialized]
    public bool executedGodModeCommand;
    public bool commandGodModeDone;

    private void Awake()
    {
        Instance = this;
        StateNameController.isNewGame = true;
    }
    private void Start()
    {
        objetosInteractuados = 0;
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
        if(StateNameController.isHardcoreMode)
        {
            zombies+=5;
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
            PlayerController.RockText = UIRockText;
            PlayerController.UIScore = UIScore;
            PlayerController.Interactable = Interactable;
            PlayerController.Interactable2 = Interactable2;
            PlayerController.UILocator = UILocator;
        }else
        {
            UISolo.SetActive(true);
        }
    }
    private void Update()
    {
        executedKillCommand = false;
        if(commandKillDone)
        {
            executedKillCommand = true;
        }
        executedGodModeCommand = false;
        if(commandGodModeDone)
        {
            executedGodModeCommand = true;
        }
        if (Ronda <= (rondaMaxima-1))
        {
            if (zombiesActuales == 0)
            {
                if(Ronda==0)
                {
                    Ronda++;
                    RondaUI.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                    RondaUI2.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                    RondaUI1.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
                    StartCoroutine(startGame());
                }else
                {
                    StartCoroutine(newRound());
                }
                
            }
        }
        else
        {
            if (zombiesActuales == 0)
            {
                RondaUI.GetComponent<TextMeshProUGUI>().text = "∞";
                RondaUI2.GetComponent<TextMeshProUGUI>().text = "∞";
                RondaUI1.GetComponent<TextMeshProUGUI>().text = "∞";
                if (!bossIniciado)
                {
                    cinecontrol.IniciarCinematica();
                    bossIniciado = true;
                }

            }
        }
        if(objetosInteractuados == cantidadObjsParaEEMusical && !reproduciendoCancionEE)
        {
            //sonido musica;
            Debug.Log("se pone cancion");
            StartCoroutine(PlayEESong());
            reproduciendoCancionEE = true;
        }
        if(!MenuPausa.isPaused)
        {
            BossSound.UnPause();
        }else
        {
            BossSound.Pause();
        }
    }

    IEnumerator PlayEESong()
    {
        bossSound.GetComponent<AudioSource>().enabled = false;
        BackgroundSource.PlayOneShot(EEMusical);
        yield return new WaitForSeconds(EEMusical.length);
        bossSound.GetComponent<AudioSource>().enabled = true;
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < zombies; i++)
        {
            var LugarRandom = UnityEngine.Random.Range(0, Spawnpoints.Count);
            var instantiatePosition = Spawnpoints[LugarRandom].transform.position;
            int random = UnityEngine.Random.Range(0, EnemiesToInstantiate.Count);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
            enemy.GetComponent<EnemyController>().Bullet = GameManager.Instance.Bullet;   
            enemy.GetComponent<EnemyController>().player1Controller = GameManager.Instance.player1Controller;
            enemy.GetComponent<EnemyController>().player2Controller = GameManager.Instance.player2Controller;
        }

    }

    public void SpawnEnemiesFromBoss()
    {
        for (int i = 0; i < bossController.zombiesToSpawn; i++)
        {
            var instantiatePosition = new Vector3(
                UnityEngine.Random.Range(boss.position.x + bossController.SpawnRadius, boss.position.x - bossController.SpawnRadius),
                0f,
                UnityEngine.Random.Range(boss.position.z + bossController.SpawnRadius, boss.position.z - bossController.SpawnRadius)
            );
            int random = UnityEngine.Random.Range(0,EnemiesToInstantiate.Count);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
            enemy.GetComponent<EnemyController>().Bullet = GameManager.Instance.Bullet;
            enemy.GetComponent<EnemyController>().player1Controller = GameManager.Instance.player1Controller;
            enemy.GetComponent<EnemyController>().player2Controller = GameManager.Instance.player2Controller;
        }

    }

    IEnumerator newRound()
    {
        zombiesActuales = zombies;
        int random = UnityEngine.Random.Range(0, EndRoundAudio.Count);
        BackgroundSource.PlayOneShot(EndRoundAudio[random], 0.5f);
        yield return new WaitForSeconds(EndRoundAudio[random].length);
        Ronda++;
        RondaUI.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
        RondaUI2.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
        RondaUI1.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
        
        int random2 = UnityEngine.Random.Range(0, StartRoundAudio.Count);
        BackgroundSource.PlayOneShot(StartRoundAudio[random2], 0.5f);
        yield return new WaitForSeconds(StartRoundAudio[random2].length );
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

    IEnumerator startGame()
    {
        zombiesActuales = zombies;

        BackgroundSource.PlayOneShot(BackgroundAudio[0], 0.5f);

        yield return new WaitForSeconds(BackgroundAudio[0].length);
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

    public void goToEndScene()
    {
        PlayerController.mPlayerInput.SwitchCurrentActionMap("MenuAndEndgame");
        SceneManager.LoadScene("Victory");
    }
}
