using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cargando : MonoBehaviour
{
    [System.NonSerialized]
    public float porcentaje = 0f;
    public void Start()
    {
        string NivelACargar = CargaNivel.SiguienteNivel;
        StartCoroutine(IniciarCarga(NivelACargar));
    }

    IEnumerator IniciarCarga(string nivel)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nivel);
        operacion.allowSceneActivation = false;
        
        while (!operacion.isDone)
        {
            porcentaje = operacion.progress*100;
            if (operacion.progress >= 0.9f)
            {
                operacion.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
