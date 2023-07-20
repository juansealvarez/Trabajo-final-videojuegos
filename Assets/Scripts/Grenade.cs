using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffectFire;
    public GameObject explosionEffectSmoke;
    public GameObject explosionEffectChispas;

    private Rigidbody mRb;
    private float throwForce = 0f;

    float countdown;
    bool hasExploded = false;


    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    public void Awake()
    {
        mRb = GetComponent<Rigidbody>();
    }
    void Throw()
    {
        // mRb.AddForce(dir * throwForce);
        mRb.AddForce(transform.up * throwForce / 2);
    }

    void Explode()
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            // // Add damage to nearby enemies
            // Zombie zombie = nearbyObject.GetComponent<Zombie>();
            // if (zombie != null)
            // {
            //     // Calculate damage based on distance from the grenade
            //     float damage = 100f * (1f - (transform.position - zombie.transform.position).magnitude / radius);
            //     zombie.TakeDamage(damage);
            // }
        }

        // Destroy grenade and explosion effects after a delay
        Destroy(fire, 1f);
        Destroy(smoke, 1f);
        Destroy(chispas, 1f);
        Destroy(gameObject);
    }
}