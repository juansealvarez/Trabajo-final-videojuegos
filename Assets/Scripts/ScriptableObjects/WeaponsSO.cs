using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class WeaponsSO : ScriptableObject
{
    public string GunName;
    public float GunDamage;
    public List<AudioClip> audioList;
    public float shootDistance;
    public float balasCargador;
    public float balas;
}
