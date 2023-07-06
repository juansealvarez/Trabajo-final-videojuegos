using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerController.TakeDamage(PlayerController.Instance.PlayerHealth);
        }
    }
}
