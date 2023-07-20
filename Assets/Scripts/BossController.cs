using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public static BossController Instance { private set; get; }
    public CinematicController CineController;

    private bool PlayingQuarterLifeAnim = false;

    private bool PlayingHalfLifeAnim = false;

    public GameObject camara;

    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    [System.NonSerialized]
    public bool dead = false;
    [System.NonSerialized]
    public AudioSource mAudioSource;
    public GameObject HitboxLeft;
    public GameObject HitboxRight;
    private CapsuleCollider mCollider;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public PlayerController playerController;
    public EnemySO EnemyType;
    public static float damage;
    private float salud;
    
    private bool endedCinematic = false;
    public Transform finalPosition;

    private float MovingSpeed = 3f;
    
    private float moving;
    public GameManager gameManager;
    public CinematicController cinematicController;
    public float cooldownSpawnZombies;
    private float delaySpawnZombies;
    public int zombiesToSpawn;
    [SerializeField]
    private List<GameObject> EnemiesToInstantiate;

    public float SpawnRadius;
    public Player1Voices player1voices;
    public GameObject canvasEnd;
    public AudioClip soundEnd;
    public GameObject canvasInstruction;
    private float randomRoar;
    private float speedFinal;
    private bool isRunning = false;
    public List<AudioClip> Audios;
    private bool audioPlaying;
    private bool aSource = true;
    public AudioSource audioBossFight;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();

        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        mCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        damage = EnemyType.Damage;
        salud = EnemyType.Health;
        moving = MovingSpeed * Time.deltaTime;
        navMeshAgent.speed = EnemyType.Speed;
        if(StateNameController.isHardcoreMode)
        {
            salud*=1.1f;
            navMeshAgent.speed*=3.5f;
            damage*=1.2f;
            cooldownSpawnZombies*=0.7f;
            zombiesToSpawn+=5;
        }
        delaySpawnZombies = cooldownSpawnZombies;
        randomRoar = UnityEngine.Random.Range(10f, 20f);
        speedFinal = navMeshAgent.speed;
        mAudioSource.PlayOneShot(Audios[0]);
    }

    private void Update()
    {

        if (!playerController.IsDead)
        {
            if (endedCinematic)
            {
                if(aSource)
                {
                    mAudioSource.Play();
                    aSource = false;
                }
                var collider1 = IsPlayerInAttackArea();
                if (collider1 != null && !mIsAttacking && !dead)
                {
                    mRb.velocity = new Vector3(
                        0f,
                        0f,
                        0f
                    );
                    navMeshAgent.isStopped = true;
                    mAnimator.SetBool("IsWalking", false);
                    mAnimator.SetTrigger("Attacking");
                    mAnimator.SetInteger("RandomAttack", UnityEngine.Random.Range(0, 2));
                    return;
                }
                Debug.Log(audioPlaying);
                delaySpawnZombies -= Time.deltaTime;
                if (delaySpawnZombies <= 0 && !mIsAttacking && !dead && !isRunning && randomRoar!=delaySpawnZombies)
                {
                    mRb.velocity = new Vector3(
                        0f,
                        0f,
                        0f
                    );
                    mIsAttacking = true;
                    navMeshAgent.isStopped = true;
                    mAnimator.SetBool("IsWalking", false);
                    //Spawnear Zombies
                    mAnimator.SetTrigger("SummonEnemies");
                    if (!audioPlaying)
                    {
                        audioPlaying = true;
                        mAudioSource.PlayOneShot(Audios[4]);
                        StartCoroutine(stopPlaying(4));
                    }
                    gameManager.SpawnEnemiesFromBoss();
                    return;
                }
                if (salud <= EnemyType.Health/2f && !PlayingHalfLifeAnim)
                {
                    PlayingHalfLifeAnim = true;
                    mRb.velocity = new Vector3(
                        0f,
                        0f,
                        0f
                    );
                    mIsAttacking = true;
                    navMeshAgent.isStopped = true;
                    mAnimator.SetTrigger("HasHalfLife");
                    if (!audioPlaying)
                        {
                            audioPlaying = true;
                            mAudioSource.PlayOneShot(Audios[2]);
                            StartCoroutine(stopPlaying(2));
                        }
                }
                if (salud <= EnemyType.Health/4f && !PlayingQuarterLifeAnim)
                {
                    PlayingQuarterLifeAnim = true;
                    mRb.velocity = new Vector3(
                        0f,
                        0f,
                        0f
                    );
                    mIsAttacking = true;
                    navMeshAgent.isStopped = true;
                    mAnimator.SetTrigger("HasHalfLife");
                    if (!audioPlaying)
                        {
                            audioPlaying = true;
                            mAudioSource.PlayOneShot(Audios[2]);
                            StartCoroutine(stopPlaying(2));
                        }
                }

                if (isRunning)
                {
                    navMeshAgent.speed*=2;
                }else
                {
                    navMeshAgent.speed = speedFinal;
                }
                var collider2 = IsPlayerNearby();

                if (collider2 != null && !mIsAttacking && !dead)
                {
                    mAnimator.SetBool("IsWalking", true);
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(collider2.transform.position);
                }
                else
                {
                    // parar
                    mRb.velocity = Vector3.zero;
                    mAnimator.SetBool("IsWalking", false);
                    navMeshAgent.isStopped = true;
                }
                if (randomRoar > 0  && !isRunning)
                {
                    randomRoar-=Time.deltaTime;
                }else if (randomRoar <= 0 && !isRunning)
                {
                    if (!mIsAttacking && !dead)
                    {
                        mRb.velocity = Vector3.zero;
                        navMeshAgent.isStopped = true;
                        mAnimator.SetTrigger("Roaring");
                        if (!audioPlaying)
                        {
                            audioPlaying = true;
                            mAudioSource.PlayOneShot(Audios[1]);
                            StartCoroutine(stopPlaying(1));
                        }
                        
                    }
                }
                Debug.Log(isRunning);
            }
        }
        else
        {
            Destroy(gameObject, 5f);
        }

    }
    IEnumerator stopPlaying(int numero)
    {
        yield return new WaitForSeconds(Audios[1].length);
        audioPlaying = false;
    }
    private void FallJumping()
    {
        mAudioSource.PlayOneShot(Audios[3]);
    }
    private void StartTimerToWalkAgain()
    {
        navMeshAgent.isStopped = false;
        isRunning = true;
        randomRoar = UnityEngine.Random.Range(10f, 20f);
        StartCoroutine(stopRunning());
    }
    IEnumerator stopRunning()
    {
        yield return new WaitForSeconds(7f);
        mAnimator.SetTrigger("StopRunning");
        isRunning = false;
    }
    private void EndSummoning()
    {
        mAnimator.SetBool("IsWalking", true);
        mIsAttacking = false;
        delaySpawnZombies = cooldownSpawnZombies;
    }

    public void DisableCamera()
    {
        PlayerController.mPlayerInput.enabled = true;
        playerController.mAudioSource.enabled = true;
        playerController.pAudioSource.enabled = true;
        playerController.BackgroundSource.enabled = true;
        gameManager.UItoActivateAndActivate.SetActive(true);
        endedCinematic = true;
        canvasInstruction.SetActive(true);
        camara.SetActive(false);
    }
    public void DeactivateInstructions()
    {
        canvasInstruction.SetActive(false);
    }

    public void MoveBoss()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition.position, moving);
    }


    private Collider IsPlayerNearby()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            EnemyType.AwakeRadio,
            LayerMask.GetMask("Player")
        );
        if (colliders.Length == 1)
        {
            return colliders[0];
        }else if (colliders.Length == 2)
        {
            Vector3 player1pos = colliders[0].gameObject.transform.position;
            Vector3 player2pos = colliders[1].gameObject.transform.position;
            var distance1 = Vector3.Distance(transform.position, player1pos);
            var distance2 = Vector3.Distance(transform.position, player2pos);
            if (distance1 < distance2)
            {
                return colliders[0];
            }else
            {
                return colliders[1];
            }
        }else
        {
            return null;
        }
    }

    private Collider IsPlayerInAttackArea()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            EnemyType.AttackRadio,
            LayerMask.GetMask("Player")
        );
        var colliders2 = Physics.OverlapSphere(
            transform.position,
            EnemyType.AttackRadio,
            LayerMask.GetMask("Player2")
        );
        if (colliders.Length == 1)
        {
            return colliders[0];
        }else if (colliders.Length == 2)
        {
            Vector3 player1pos = colliders[0].gameObject.transform.position;
            Vector3 player2pos = colliders[1].gameObject.transform.position;
            var distance1 = Vector3.Distance(transform.position, player1pos);
            var distance2 = Vector3.Distance(transform.position, player2pos);
            if (distance1 < distance2)
            {
                return colliders[0];
            }else
            {
                return colliders[1];
            }
        }else
        {
            return null;
        }
    }

    public void StartAttack()
    {
        mIsAttacking = true;
    }

    public void EnableHitboxLeft()
    {
        HitboxLeft.SetActive(true);
    }
    public void EnableHitboxRight()
    {
        HitboxRight.SetActive(true);
    }

    public void StopAttack()
    {
        mIsAttacking = false;
        HitboxLeft.SetActive(false);
        HitboxRight.SetActive(false);
    }

    public void TakeDamage(float Damage)
    {
        salud -= Damage;
        if (salud <= 0f)
        {

            mAnimator.SetTrigger("Die");
            mCollider.enabled = false;
            dead = true;
            StateNameController.isBossDead = true;
            cinematicController.HacerDia();
            StartCoroutine(MenuEndgame());
            Destroy(gameObject, 20f);
            mAudioSource.PlayOneShot(Audios[5]);
            HitboxLeft.SetActive(false);
            HitboxRight.SetActive(false);
        }
    }

    IEnumerator MenuEndgame()
    {
        CineController.AudioBoss.SetActive(false);
        if(gameManager.isSoloGame)
        {
            player1voices.playSoloBossDead();
        }else
        {
            player1voices.playCoopBossDead();
        }
        yield return new WaitForSeconds(7);
        StartCoroutine(FadeOut(audioBossFight, 1f));
        playerController.BackgroundSource.PlayOneShot(soundEnd, 20f);
        gameManager.UItoActivateAndActivate.SetActive(false);
        canvasEnd.SetActive(true);
        yield return new WaitForSeconds(3);
        gameManager.goToEndScene();
    }

    IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
