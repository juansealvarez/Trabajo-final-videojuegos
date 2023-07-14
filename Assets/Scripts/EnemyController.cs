using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    private bool dead = false;

    private AudioSource mAudioSource;
    [SerializeField]
    private List<AudioClip> audioList;
    public GameObject HitboxLeft;
    private CapsuleCollider mCollider;
    private NavMeshAgent navMeshAgent;

    public PlayerController playerController;
    public EnemySO EnemyType;
    public static float damage;
    private float salud;
    public GameObject Bullet;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        mAnimator = transform
            .GetComponentInChildren<Animator>(false);
        mAudioSource = GetComponent<AudioSource>();
        mCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        damage = EnemyType.Damage;
        salud = EnemyType.Health;
    }

    private void Update()
    {
        
        if (!playerController.IsDead)
        {
            //mAudioSource.PlayOneShot(audioList[Random.Range(0,2)]);

            var collider1 = IsPlayerInAttackArea();
            if (collider1 != null && !mIsAttacking && !dead)
            {
                mRb.velocity = new Vector3(
                    0f,
                    0f,
                    0f
                );
                //mAudioSource.PlayOneShot(audioList[Random.Range(2,4)]);
                navMeshAgent.isStopped = true;
                mAnimator.SetBool("IsWalking", false);
                mAnimator.SetTrigger("Attacking");
                mAnimator.SetInteger("RandomAttack", Random.Range(0,3));
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
        }else
        {
            Destroy(gameObject, 5f);
        }
        
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
        HitboxLeft.SetActive(true);
    }

    public void StopAttack()
    {
        mIsAttacking = false;
        HitboxLeft.SetActive(false);
    }

    public void TakeDamage(float Damage)
    {
        salud -= Damage;
        if (salud  == 0f)
        {
            var instantiatePosition = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );
            var bullet = Instantiate(Bullet, instantiatePosition, Quaternion.identity);
            mAnimator.SetTrigger("Die");
            mCollider.enabled = false;
            dead = true;
            Destroy(gameObject, 20f);
            HitboxLeft.SetActive(false);
        }
    }
}
