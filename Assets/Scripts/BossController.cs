using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public static BossController Instance { private set; get; }
    private bool PlayingQuarterLifeAnim = false;

    private bool PlayingHalfLifeAnim = false;

    public GameObject camara;

    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    [System.NonSerialized]
    public bool dead = false;

    private AudioSource mAudioSource;
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
    }

    private void Update()
    {

        if (!playerController.IsDead)
        {
            if (endedCinematic)
            {
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
        var colliders2 = Physics.OverlapSphere(
            transform.position,
            EnemyType.AttackRadio,
            LayerMask.GetMask("Player2")
        );
        if (colliders.Length == 1) return colliders[0];
        else if (colliders2.Length == 1) return colliders2[0];
        else return null;
        //TODO: que el boss sepa qué jugador esta mas cerca para ir a atacarlo (Comparar distancias)
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
        if (colliders.Length == 1) return colliders[0];
        else if (colliders2.Length == 1) return colliders2[0];
        else return null;
        //TODO: que el boss sepa qué jugador esta mas cerca para atacarlo (Comparar distancias)
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
            // HitboxLeft.SetActive(false);
        }
    }

    IEnumerator MenuEndgame()
    {
        if(gameManager.isSoloGame)
        {
            player1voices.playSoloBossDead();
        }else
        {
            player1voices.playCoopBossDead();
        }
        yield return new WaitForSeconds(7);
        playerController.BackgroundSource.PlayOneShot(soundEnd, 20f);
        gameManager.UItoActivateAndActivate.SetActive(false);
        canvasEnd.SetActive(true);
        yield return new WaitForSeconds(3);
        gameManager.goToEndScene();
    }
}
