using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimShotgun : MonoBehaviour
{
    public float AimingSpeed = 2f;
    public Transform targetAim;
    public Transform targetNoAim;

    public Transform LookCamera;

    private Animator mAnimator;
    private Animator gAnimator;
    public WeaponsSO Weapon;
    public bool WeaponActive;
    public GameObject WeaponToActivate;
    public ParticleSystem shootPS;
    public TextMeshProUGUI arma;
    public TextMeshProUGUI balas;

    private void Start()
    {
        mAnimator = LookCamera.GetComponent<Animator>();
        gAnimator = GetComponent<Animator>();
        if (WeaponActive)
        {
            gameObject.SetActive(true);
        }else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        arma.text = Weapon.GunName;
        balas.text = Weapon.balasCargador.ToString() + "/" + Weapon.balas.ToString();
        float aiming = AimingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetAim.position, aiming);
        }else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetNoAim.position, aiming);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            WeaponActive = false;
            WeaponToActivate.GetComponent<AimShotgun>().WeaponActive = true;
            gameObject.SetActive(false);
            WeaponToActivate.gameObject.SetActive(true);
        }
    }
}
