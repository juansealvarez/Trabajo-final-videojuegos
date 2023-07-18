using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public static BossController Instance { private set; get; }

    public GameObject camara;

    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    [System.NonSerialized]
    public bool dead = false;

    private AudioSource mAudioSource;
    [SerializeField]
    private List<AudioClip> audioList;
    // public GameObject HitboxLeft;
    private CapsuleCollider mCollider;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public PlayerController playerController;
    public EnemySO EnemyType;
    public static float damage;
    private float salud;
    
    private bool endedCinematic = false;
    public Transform finalPosition;

    public float MovingSpeed;
    
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
            salud*=1.5f;
            navMeshAgent.speed*=3.5f;
            damage*=1.2f;
            cooldownSpawnZombies*=0.7f;
            zombiesToSpawn+=5;
        }
        delaySpawnZombies = cooldownSpawnZombies;
    }

    private void Update()
    {

        if (!playerController.IsDead)
        {
            if (endedCinematic)
            {
                //mAudioSource.PlayOneShot(audioList[Random.Range(0,2)]);
                // var collider1 = IsPlayerInAttackArea();
                // if (collider1 != null && !mIsAttacking && !dead)
                // {
                //     mRb.velocity = new Vector3(
                //         0f,
                //         0f,
                //         0f
                //     );
                //     //mAudioSource.PlayOneShot(audioList[Random.Range(2,4)]);
                //     navMeshAgent.isStopped = true;
                //     mAnimator.SetBool("IsWalking", false);
                //     mAnimator.SetTrigger("Attacking");
                //     mAnimator.SetInteger("RandomAttack", Random.Range(0, 3));
                //     return;
                // }
                delaySpawnZombies -= Time.deltaTime;
                if (delaySpawnZombies <= 0 && !mIsAttacking && !dead)
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


                var collider2 = IsPlayerNearby();

                if (collider2 != null && !mIsAttacking && !dead)
                {
                    mAnimator.SetBool("IsWalking", true);
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(collider2.transform.position);
                    //Walk(collider2);
                }
                else
                {
                    // parar
                    mRb.velocity = Vector3.zero;
                    mAnimator.SetBool("IsWalking", false);
                    navMeshAgent.isStopped = true;
                }
            }
        }
        else
        {
            Destroy(gameObject, 5f);
        }

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
        camara.SetActive(false);
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
        //TODO: que el boss sepa qué jugador esta mas cerca para ir a atacarlo
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
        //TODO: que el boss sepa qué jugador esta mas cerca para atacarlo
    }

    public void StartAttack()
    {
        mIsAttacking = true;
    }

    public void EnableHitbox()
    {
        // HitboxLeft.SetActive(true);
    }

    public void StopAttack()
    {
        mIsAttacking = false;
        // HitboxLeft.SetActive(false);
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
        // Reproducir sonido de fin
        gameManager.UItoActivateAndActivate.SetActive(false);
        canvasEnd.SetActive(true);
        yield return new WaitForSeconds(3);
        gameManager.goToEndScene();
    }
}
