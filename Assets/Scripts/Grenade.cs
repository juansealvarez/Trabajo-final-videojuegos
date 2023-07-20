using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 10f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffectFire;
    public GameObject explosionEffectSmoke;
    public GameObject explosionEffectChispas;

    private Rigidbody mRb;
    // private float throwForce = 0f;

    float countdown = 5f;
    bool hasExploded = false;

    public float ExplodeRadio = 20f;



    private void Start()
    {

    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Debug.Log("Explode update called");
            Explode();
            hasExploded = true;
        }
    }

    public void Awake()
    {
        mRb = GetComponent<Rigidbody>();
        if (mRb == null)
        {
            Debug.Log("Rigidbody is null in start");
        }
        else
        {
            Debug.Log("Rigidbody is not null in start");
        }
    }
    public void Throw(Vector3 dir, float throwForce)
    {
        // Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        // mRb.AddForce(randomDirection.normalized * force / 2);

        Debug.Log("Throwing grenade in grende script");

        mRb.AddForce(dir * throwForce);
        mRb.AddForce(transform.up * force / 2);
    }

    private void IsZombieNearby()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            ExplodeRadio,
            LayerMask.GetMask("Enemies")
        );
        if (colliders.Length >= 1)
        {
            foreach (var collider in colliders)
            {
                var enemy = collider.gameObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    Debug.Log("Enemigo dañado por granada");
                    enemy.TakeDamage(100f);
                }
                else
                {
                    Debug.Log("NO HAY ENEMIGOS EN EL RANGO");
                }
            }

        }
        else
        {
            Debug.Log("NO HAY ENEMIGOS EN EL RANGO");
        }

    }

    public void Explode()
    {
        // Show explosion effects
        GameObject fire = Instantiate(explosionEffectFire, transform.position, transform.rotation);
        GameObject smoke = Instantiate(explosionEffectSmoke, transform.position, transform.rotation);
        GameObject chispas = Instantiate(explosionEffectChispas, transform.position, transform.rotation);

        // Start particle systems
        fire.GetComponent<ParticleSystem>().Play();
        smoke.GetComponent<ParticleSystem>().Play();
        chispas.GetComponent<ParticleSystem>().Play();

        // Apply force to nearby objects
        IsZombieNearby();

        // Destroy grenade and explosion effects after a delay
        Destroy(fire, 1f);
        Destroy(smoke, 1f);
        Destroy(chispas, 1f);
        Destroy(gameObject);
    }
}