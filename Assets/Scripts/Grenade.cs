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
    private LineRenderer lineRenderer;
    private Queue<Vector3> positions = new Queue<Vector3>();
    private int maxPositions = 50;

    private Rigidbody mRb;
    // private float throwForce = 0f;

    public float countdown = 5f;
    bool hasExploded = false;

    public float ExplodeRadio = 20f;
    public PlayerController playerController;

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
        else
        {
            // Agrega la posición actual de la granada a la cola
            positions.Enqueue(transform.position);

            // Si la cola se ha vuelto demasiado grande, elimina la posición más antigua
            while (positions.Count > maxPositions)
            {
                positions.Dequeue();
            }

            // Comprueba si el LineRenderer aún existe antes de intentar acceder a él
            if (lineRenderer != null)
            {
                // Actualiza el LineRenderer con las posiciones de la cola
                lineRenderer.positionCount = positions.Count;
                lineRenderer.SetPositions(positions.ToArray());
            }
        }
    }

    public void Awake()
    {
        // LineRenderer to show the grenade trajectory __________________
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.positionCount = 0;

        lineRenderer.startWidth = 0.03f; // Ajusta el grosor al inicio de la línea
        lineRenderer.endWidth = 0.01f; // Ajusta el grosor al final de la línea
        // End of LineRenderer __________________________________________

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
        Debug.Log("Throwing grenade in grende script");

        // Adjust the direction of the throw
        dir.x = dir.x * 4;
        dir.y = dir.y * 0.5f; // reduce the vertical component of the direction
        dir = dir.normalized; // re-normalize the direction vector

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
                    enemy.TakeDamage(100f);
                    playerController.puntaje+=50;
                }
            }

        }

        var colliders2 = Physics.OverlapSphere(
            transform.position,
            ExplodeRadio,
            LayerMask.GetMask("Player")
        );
        if (colliders2.Length >= 1 && StateNameController.isHardcoreMode)
        {
            //Fuego amigo granada;
            foreach (var collider in colliders2)
            {
                var player = collider.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(7f);
                }
            }

        }

        var colliders3 = Physics.OverlapSphere(
            transform.position,
            ExplodeRadio,
            LayerMask.GetMask("Boss")
        );
        if (colliders3.Length >= 1)
        {
            foreach (var collider in colliders3)
            {
                var enemy = collider.gameObject.GetComponent<BossController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(20f);
                }
            }

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

        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        // Get the length of the audio clip
        float audioLength = audioSource.clip.length;

        // Apply force to nearby objects
        IsZombieNearby();

        // Destroy the LineRenderer
        Destroy(lineRenderer);

        // Make the grenade object invisible
        GetComponent<Renderer>().enabled = false;

        // Destroy grenade and explosion effects after a delay
        // Destroy(fire, 1f);
        // Destroy(smoke, 1f);
        // Destroy(chispas, 1f);
        // Destroy(gameObject);

        // Destroy grenade and explosion effects after a delay
        Destroy(fire, audioLength);
        Destroy(smoke, audioLength);
        Destroy(chispas, audioLength);
        Destroy(gameObject, audioLength);
    }
}