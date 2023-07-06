using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller;

    public void StartAttack()
    {
        controller.StartAttack();
    }

    public void StopAttack()
    {
        controller.StopAttack();
    }

    public void EnableHitbox()
    {
        controller.EnableHitbox();
    }
}
