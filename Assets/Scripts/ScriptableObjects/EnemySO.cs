using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/enemy")]
public class EnemySO : ScriptableObject
{
    
    public string Tipo;
    public float Speed;

    public float AwakeRadio;
    public float AttackRadio;
    public float Health;

    public float Damage;
}

