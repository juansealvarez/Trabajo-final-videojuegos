using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEndgame : MonoBehaviour
{
    public void ChangeScene(string escena)
    {
        //PlayerController.mPlayerInput.SwitchCurrentActionMap("Player");
        SceneManager.LoadScene(escena);
    }
}
