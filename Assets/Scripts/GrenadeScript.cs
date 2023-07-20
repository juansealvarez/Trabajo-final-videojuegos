using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;

    private float countdown;
    private bool hasExploded = false;

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Show explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Get all nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Apply force to nearby objects
            // Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            // }

            // // Damage nearby enemies
            // Zombie zombie = nearbyObject.GetComponent<Zombie>();
            // if (zombie != null)
            // {
            //     zombie.TakeDamage(50); // Assume the TakeDamage function exists in your Zombie script
            // }
        }

        // Destroy the grenade
        Destroy(gameObject);
    }
}
