using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerController PlayerController;
    public GameObject Bullet;

    public List<GameObject> EnemiesToInstantiate;
    public int CantidadZombiesPorHorda = 10;
    public float SpawnRadius = 5f;

    public static GameManager Instance { private set; get; }
    public GameObject UIReload;
    public GameObject UILowAmmo;
    public GameObject UINoAmmo;

    private float timer;
    private float timerFijo = 10f;
    private int Ronda = 0;
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
    private float zombies;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        zombies = CantidadZombiesPorHorda;
        isSoloGame = StateNameController.isSoloMode;
        timer = timerFijo;
        BackgroundSource = transform
            .GetComponent<AudioSource>();
        if(!isSoloGame)
        {
            UISolo.SetActive(false);
            UICoop.SetActive(true);
            Player1.transform.Find("Main Camera").GetComponent<Camera>().rect =  new Rect (0, 0.5f, 1, 1);
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
        /*timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Ronda++;
            RondaUI.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
            RondaUI2.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
            RondaUI1GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
            if (CopyrigthSong)
            {
                BackgroundSource.PlayOneShot(BackgroundAudio[0]);
                //songPlayed = true;
            }
            //Sonido Cambio Ronda
            SpawnEnemies();
            if(timerFijo > 10f)
            {
                timerFijo -= 5f;
                timer = timerFijo;
            }else
            {
                timer = timerFijo;
            }
        }*/ //Sistema de rondas (por tiempo), cambiarlo a por kills de zombies
    }

    private void SpawnEnemies()
    {
        for (int i = 0 ; i < CantidadZombiesPorHorda; i++)
        {
            var instantiatePosition = new Vector3(
                UnityEngine.Random.Range(Player.position.x + SpawnRadius, Player.position.x - SpawnRadius),
                0f,
                UnityEngine.Random.Range(Player.position.z + SpawnRadius, Player.position.z - SpawnRadius)
            );
            // Cambiar lugar de spawn a lugares predefinidos
            int random = UnityEngine.Random.Range(0,2);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
            enemy.GetComponent<EnemyController>().Bullet = GameManager.Instance.Bullet;
        }
        
    }
}
