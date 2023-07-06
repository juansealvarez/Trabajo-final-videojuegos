using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public PlayerController PlayerController;

    public List<GameObject> EnemiesToInstantiate;
    public int CantidadZombiesPorHorda = 10;
    public float SpawnRadius = 5f;

    public static GameManager Instance { private set; get; }
    private float timer;
    private float timerFijo = 10f;
    private int Ronda = 0;
    public GameObject RondaUI;
    public bool CopyrigthSong = false;
    public List<AudioClip> BackgroundAudio;
    private AudioSource BackgroundSource;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        timer = timerFijo;
        BackgroundSource = transform
            .GetComponent<AudioSource>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Ronda++;
            RondaUI.GetComponent<TextMeshProUGUI>().text = Ronda.ToString();
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
        }
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
            int random = UnityEngine.Random.Range(0,2);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().Player = GameManager.Instance.Player;
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
        }
        
    }
}
