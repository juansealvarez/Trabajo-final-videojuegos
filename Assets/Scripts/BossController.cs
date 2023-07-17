using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public GameObject camara;

    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    private bool dead = false;

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

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();

        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        mCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        damage = EnemyType.Damage;
        salud = EnemyType.Health;
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

    public void DisableCamera()
    {
        endedCinematic = true;
        camara.SetActive(false);
    }
    // Start is called before the first frame update


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
        //TODO: que el zombie sepa que jugador esta mas cerca para ir a atacarlo
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
        //TODO: que el zombie sepa que jugador esta mas cerca para atacarlo
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
            // Destroy(gameObject, 20f);
            // HitboxLeft.SetActive(false);
        }
    }

}
