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
    [System.NonSerialized]
    public bool dead = false;

    private AudioSource mAudioSource;
    public GameObject HitboxLeft;
    private CapsuleCollider mCollider;
    private NavMeshAgent navMeshAgent;

    public PlayerController playerController;
    public EnemySO EnemyType;
    public static float damage;
    private float salud;
    public GameObject Bullet;

    private bool HizoDeadAnimation = false;
    public PlayerController player1Controller;
    public PlayerController player2Controller;

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
        navMeshAgent.speed = EnemyType.Speed;
        if(StateNameController.isHardcoreMode)
        {
            salud*=1.5f;
            navMeshAgent.speed*=1.5f;
            damage*=1.2f;
        }
    }

    private void Update()
    {
        
        if (!playerController.IsDead)
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
                mAnimator.SetInteger("RandomAttack", Random.Range(0,3));
                return;
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
            if(StateNameController.isBossDead && !HizoDeadAnimation)
            {
                mAnimator.SetTrigger("Die");
                mCollider.enabled = false;
                dead = true;
                mAudioSource.Stop();
                Destroy(gameObject, 20f);
                HizoDeadAnimation = true;
                HitboxLeft.SetActive(false);
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
        if (salud  <= 0f)
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
            mAudioSource.Stop();
            GameManager.Instance.zombiesActuales -= 1;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("KnifeAttack"))
        {
            TakeDamage(1f);
            if (col.GetComponent<KnifeScript>().isFromPlayer1)
            {
                if(salud == 0f)
                {
                    player1Controller.puntaje+=100;
                }else
                {
                    player1Controller.puntaje+=10;
                }   
            }else
            {
                if(salud == 0f)
                {
                    player2Controller.puntaje+=100;
                }else
                {
                    player2Controller.puntaje+=10;
                } 
            }
        }
    }
}
