using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public GameObject smokeParticles;
    public GameObject chispasParticles;
    public GameObject bigRock;
    public GameObject[] smallRocks; // Ahora es un array
    public GameObject character;
    public Animator characterAnimator;

    void Start()
    {
        StartCoroutine(PlayScene());
    }

    IEnumerator PlayScene()
    {
        // 1. Aparecen partículas de humo
        chispasParticles.SetActive(true);

        yield return new WaitForSeconds(5);
        chispasParticles.SetActive(false);
        smokeParticles.SetActive(true);
        // 2. La roca desaparece del mapa (se oculta)
        bigRock.SetActive(false);

        // 3. Las rocas pequeñas que están ocultas dentro del modelo de la roca se mueven como si saltaran.
        ActivateSmallRocks();

        yield return new WaitForSeconds(3);

        // REALIZADO
        // 4. El modelo del personaje (que está hundido en el mapa) subirá a la superficie y aplicará una animación.
        character.SetActive(true);
        characterAnimator.Play("YourAnimationName");

        yield return new WaitForSeconds(5);

        // ANOTADOOOOOOOOOOOOOOOOO
        // 5. La pantalla se oscurecerá poco a poco como transición a la siguiente escena
        // Aquí necesitarás un objeto que oscurezca la pantalla, como un panel UI que cubra toda la pantalla.
        // Supongamos que tienes un componente Image en un objeto llamado "fadePanel"
        UnityEngine.UI.Image fadePanel = GameObject.Find("fadePanel").GetComponent<UnityEngine.UI.Image>();
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // Cambia el color del panel gradualmente a negro
            fadePanel.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // REGRESO A LA ESCENA MAIN -------------------------
        // Aquí puedes cargar la siguiente escena
        // UnityEngine.SceneManagement.SceneManager.LoadScene("YourNextSceneName");
    }

    void ActivateSmallRocks()
    {
        // Activa las rocas pequeñas y aplica una fuerza a cada una
        foreach (GameObject smallRock in smallRocks)
        {
            smallRock.SetActive(true);
            Rigidbody rb = smallRock.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)) * 100;
            rb.AddForce(force);
        }
    }
}
