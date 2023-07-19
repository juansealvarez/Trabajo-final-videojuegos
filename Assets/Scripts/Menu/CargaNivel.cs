using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CargaNivel
{
    public static string SiguienteNivel;
    public static void NivelCarga(string nombre)
    {
        SiguienteNivel = nombre;
        SceneManager.LoadScene("PantallaCarga");
    }
}
