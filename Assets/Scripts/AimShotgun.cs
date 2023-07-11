using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimShotgun : MonoBehaviour
{
    public AimShotgun Instance { set; get; }
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
    [System.NonSerialized]
    public bool isAiming = false;
    [System.NonSerialized]
    public float balasActuales;
    [System.NonSerialized]
    public float balasCargador;
    [System.NonSerialized]
    public float balasTotales;
    [SerializeField]
    private PlayerController PlayerController;

    private void Awake()
    {
        Instance = this;
        balasActuales = Weapon.balasCargador;
        balasCargador = Weapon.balasCargador;
        balasTotales = Weapon.balas;
    }

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
        balas.text = balasActuales.ToString() + "/" + balasTotales.ToString();
        float aiming = AimingSpeed * Time.deltaTime;
        var animator = GetComponent<Animator>();
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if(!PlayerController.Instance.isReloading && !PlayerController.Instance.isInspecting)
            {
                animator.enabled = false;
                isAiming = true;
                transform.position = Vector3.MoveTowards(transform.position, targetAim.position, aiming);
            }
            
        }else
        {
            animator.enabled = true;
            isAiming = false;
            transform.position = Vector3.MoveTowards(transform.position, targetNoAim.position, aiming);
        }
    }
    private void stopReloading()
    {
        PlayerController.stopReloading();
    }
    public void SwitchWeapon()
    {
        if(!PlayerController.Instance.isReloading)
        {
            WeaponActive = false;
            WeaponToActivate.GetComponent<AimShotgun>().WeaponActive = true;
            gameObject.SetActive(false);
            WeaponToActivate.gameObject.SetActive(true);
        }
    }
    private void stopInspecting()
    {
        PlayerController.StopInspecting();
    }
}
