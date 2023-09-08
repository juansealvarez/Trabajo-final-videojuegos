using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeArms : MonoBehaviour
{
    public PlayerController playerController;

    public void stopKnifing()
    {
        playerController.stopKnifing();
    }
}
