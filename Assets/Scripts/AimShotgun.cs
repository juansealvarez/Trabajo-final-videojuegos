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
    private AudioSource audioSource;
    private Animator animator;
    private float aiming;
    [SerializeField]
    private Camera camara;

    private void Awake()
    {
        Instance = this;
        balasActuales = Weapon.balasCargador;
        balasCargador = Weapon.balasCargador;
        balasTotales = Weapon.balas;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        mAnimator = LookCamera.GetComponent<Animator>();
        gAnimator = GetComponent<Animator>();
        if (WeaponActive)
        {
            gameObject.SetActive(true);
        }else
        {
            gameObject.SetActive(false);
        }
        aiming = AimingSpeed * Time.deltaTime;
        if(StateNameController.isNewGame)
        {
            animator.SetBool("newGame", true);
        }else
        {
            animator.SetBool("newGame", false);
        }
    }

    private void Update()
    {
        arma.text = Weapon.GunName;
        balas.text = balasActuales.ToString() + "/" + balasTotales.ToString();
        if (PlayerController.apuntando)
        {
            StartAiming();
        }else
        {
            StopAiming();
        }
    }
    private void stopReloading()
    {
        PlayerController.stopReloading();
    }

    private void stopStartingGun()
    {
        StateNameController.isNewGame=false;
        animator.SetBool("newGame", false);
        PlayerController.stopStartingGun();
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
    private void click()
    {
        audioSource.PlayOneShot(Weapon.audioList[3]);
    }
    public void StartAiming()
    {
        if(!PlayerController.Instance.isReloading && !PlayerController.Instance.isInspecting)
        {
            animator.enabled = false;
            isAiming = true;
            camara.fieldOfView = 40f;
            transform.position = Vector3.MoveTowards(transform.position, targetAim.position, aiming);
        }
    }
    public void StopAiming()
    {
        animator.enabled = true;
        isAiming = false;
        camara.fieldOfView = 60f;
        transform.position = Vector3.MoveTowards(transform.position, targetNoAim.position, aiming);
    }
}
