using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatRandom : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    public void stopModelKnifing()
    {
        playerController.stopModelKnifing();
    }
}
