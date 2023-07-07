using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { private set; get; }
    [SerializeField]
    private float WalkingSpeed;

    private float RunnigMultiplier;

    private float speed = 0f;
    [SerializeField]
    public float turnSpeed;
    public float PlayerHealth = 20f;
    [SerializeField]
    private float JumpSpeed = 10f;

    private Rigidbody mRb;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;
    private Transform cameraMain;
    private GameObject debugImpactSphere;
    private GameObject bloodObjectParticles;
    private GameObject otherObjectParticles;

    private Animator mAnimator;

    private Animator CameraAnimator;

    private AudioSource mAudioSource;
    //[SerializeField]
    //private List<AudioClip> audioList;
    [System.NonSerialized]
    public bool IsDead = false;

    public GameObject PlayerCapsulle;
    public GameObject primary;
    public GameObject secondary;

    public GameObject DeadScreen;
    public GameObject UI;

    public List<AudioClip> BackgroundAudio;
    private AudioSource BackgroundSource;
    private bool songPlayed = false;
    private AimShotgun aimShotgun;
    private AimShotgun aimPistol;
    private AudioSource pAudioSource;
    private Animator pAnimator;
    [SerializeField]
    private GameManager gameManager;
    [System.NonSerialized]
    public static PlayerInput mPlayerInput;
    [SerializeField]
    private Animator modelAnimator;
    [SerializeField]
    private GameObject modelShotgun;
    [SerializeField]
    private GameObject modelPistol;

    [SerializeField] 
    private float jumpForce = 5.0f;
    [SerializeField] 
    private bool isReadyToJump = true;
    [System.NonSerialized]
    public bool isReloading = false;
    private AimShotgun scriptGun;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticles = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticles = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");
        aimShotgun = transform.Find("Main Camera").Find("SM_Army_Shotgun").GetComponent<AimShotgun>();
        aimPistol = transform.Find("Main Camera").Find("SM_Army_Pistol").GetComponent<AimShotgun>();

        mAnimator = transform.Find("Main Camera")
            .Find("SM_Army_Shotgun")
            .GetComponent<Animator>();

        pAnimator = transform.Find("Main Camera")
            .Find("SM_Army_Pistol")
            .GetComponent<Animator>();

        CameraAnimator = transform.Find("Main Camera").GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;

        mAudioSource = transform
            .Find("Main Camera")
            .Find("SM_Army_Shotgun").GetComponent<AudioSource>();
        BackgroundSource = transform
            .Find("Main Camera").GetComponent<AudioSource>();
        pAudioSource = transform
            .Find("Main Camera")
            .Find("SM_Army_Pistol").GetComponent<AudioSource>();

        RunnigMultiplier = 1.5f;
        CameraAnimator.enabled = false;
        modelAnimator.SetBool("IsShotgun", true);
        scriptGun = aimShotgun;
    }

    private void Update()
    {
        
        if (aimShotgun.WeaponActive)
        {
            scriptGun = aimShotgun;
        }else
        {
            scriptGun = aimPistol;
        }
        if (!IsDead)
        {
            if (!MenuPausa.isPaused)
            {
                if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
                {
                    speed = WalkingSpeed * RunnigMultiplier;
                }else
                {
                    speed = WalkingSpeed;
                }
                
                mRb.velocity = mDirection.y * speed * transform.forward
                    + mDirection.x * speed * transform.right;

                //var control = GetComponent<PlayerInput>().currentControlScheme;
                transform.Rotate(
                    Vector3.up,
                    turnSpeed * Time.deltaTime * mDeltaLook.x
                );
                cameraMain.GetComponent<CameraMovement>().RotateUpDown(
                    -turnSpeed * Time.deltaTime * mDeltaLook.y
                );

                if (mDirection != Vector2.zero)
                {
                    modelAnimator.SetFloat("Horizontal", mDirection.x);
                    modelAnimator.SetFloat("Vertical", mDirection.y);
                    modelAnimator.SetBool("IsWalking", true);
                }
                else
                {
                    modelAnimator.SetBool("IsWalking", false);
                }

                if(!aimShotgun.WeaponActive)
                {
                    modelAnimator.SetBool("IsShotgun", false);
                    modelPistol.SetActive(true);
                    modelShotgun.SetActive(false);
                }else
                {
                    modelAnimator.SetBool("IsShotgun", true);
                    modelPistol.SetActive(false);
                    modelShotgun.SetActive(true);
                }

                if (aimShotgun.isAiming)
                {
                    modelAnimator.SetBool("IsAiming", true);
                }else if (!aimShotgun.isAiming)
                {
                    modelAnimator.SetBool("IsAiming", false);
                }
                if (!isReloading && scriptGun.balasActuales==0 && scriptGun.balasTotales>0)
                {
                    //Recarga
                    reloading();
                }
            }
            
        }else
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerCapsulle.SetActive(false);
            primary.SetActive(false);
            secondary.SetActive(false);
            UI.gameObject.SetActive(false);
            DeadScreen.gameObject.SetActive(true);
            gameManager.enabled = false;
            if(gameManager.CopyrigthSong && !songPlayed)
            {
                BackgroundSource.PlayOneShot(BackgroundAudio[0]);
                songPlayed = true;
            }else if (!gameManager.CopyrigthSong && !songPlayed)
            {
                BackgroundSource.PlayOneShot(BackgroundAudio[1]);
                songPlayed = true;
            }
        }
    }

    private void reloading()
    {
        isReloading = true;
        mAnimator.SetTrigger("IsReloading");
    }

    public void stopReloading()
    {
        var balasARecargar = 0f;
        balasARecargar = scriptGun.balasCargador-scriptGun.balasActuales;
        if (scriptGun.balasTotales>=balasARecargar)
        {
            scriptGun.balasTotales-=balasARecargar;
            scriptGun.balasActuales+=balasARecargar;
        }else
        {
            scriptGun.balasActuales+=scriptGun.balasTotales;
            scriptGun.balasTotales-=scriptGun.balasTotales;
        }
        isReloading = false;
    }
    private void OnReload(InputValue value)
    {
        if(!IsDead)
        {
            if (value.isPressed)
            {
                if(!MenuPausa.isPaused)
                {
                    if (!isReloading && (scriptGun.balasActuales>0 && scriptGun.balasActuales<scriptGun.balasCargador))
                    {
                        reloading();
                    }
                }
            }
        }
    }
    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        if(!IsDead)
        {
            if (value.isPressed)
            {
                if(!MenuPausa.isPaused)
                {
                    if(aimShotgun.WeaponActive)
                    {
                        if(!isReloading && (scriptGun.balasTotales > 0 || scriptGun.balasActuales > 0))
                        {
                            //mAudioSource.PlayOneShot(aimShotgun.Weapon.audioList[0]);
                            mAnimator.SetTrigger("GunShooting");
                            Shoot(scriptGun);
                        }
                    }else
                    {
                        if(!isReloading && (scriptGun.balasCargador > 0 || scriptGun.balasActuales > 0))
                        {
                            //pAudioSource.PlayOneShot(aimPistol.Weapon.audioList[0]);
                            pAnimator.SetTrigger("GunShooting");
                            Shoot(scriptGun);
                        }
                    }
                }
                
            }
        }
        
    }

    private void Shoot(AimShotgun scriptGun)
    {
        scriptGun.balasActuales-=1;
        scriptGun.shootPS.Play();

        RaycastHit hit;
        if (Physics.Raycast(
            cameraMain.position,
            cameraMain.forward,
            out hit,
            scriptGun.Weapon.shootDistance
        ))
        {
            //var debugSphere = Instantiate(debugImpactSphere, hit.point, Quaternion.identity);
            //Destroy(debugSphere, 3f);
            if(hit.collider.CompareTag("Enemigos"))
            {
                var bloodPS = Instantiate(bloodObjectParticles, hit.point, Quaternion.identity);
                Destroy(bloodPS, 3f);
                var enemyController = hit.collider.GetComponent<EnemyController>();
                enemyController.TakeDamage(scriptGun.Weapon.GunDamage);
            }else
            {
                var otherPS = Instantiate(otherObjectParticles, hit.point, Quaternion.identity);
                otherPS.GetComponent<ParticleSystem>().Play();
                Destroy(otherPS, 3f);
            }
        }
    }

    public void TakeDamage(float Damage)
    {
        PlayerHealth -= Damage;
        if (PlayerHealth <= 0)
        {
            mPlayerInput.SwitchCurrentActionMap("PauseMenu");
            CameraAnimator.enabled = true;
            CameraAnimator.SetBool("IsDead", true);
            IsDead = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyAttack"))
        {
            TakeDamage(EnemyController.damage);
        }
    }
    private void OnPause(InputValue value)
    {
        if(!IsDead)
        {
            if(value.isPressed)
            {
                Cursor.lockState = CursorLockMode.None;
                MenuPausa.Instance.PausarJuego();
            }
        }
        
    }
    private void OnPlay(InputValue value)
    {
        if(!IsDead)
        {
            if(value.isPressed)
            {
                 Cursor.lockState = CursorLockMode.Locked;
                MenuPausa.Instance.ReanudarJuego();
            }
        }
        
    }

    private void OnJump(InputValue value)
    {
        if(!IsDead)
        {
            if(!MenuPausa.isPaused)
            {
                if(value.isPressed)
                {
                    if (isReadyToJump)
                    {
                        Debug.Log("va a saltar");
                        isReadyToJump = false;
                        Saltar();
                        Invoke("ReiniciarSalto", 1f);
                    }
                    //Debug.Log("saltando...");
                    // Saltar
                    /*mRb.velocity = new Vector3(
                        mRb.velocity.x,
                        JumpSpeed,
                        mRb.velocity.z
                        
                    );
                    Debug.Log(mRb.velocity.y);*/
                    //mRb.velocity = new Vector3(mRb.velocity.x, 0f, mRb.velocity.z);

                    //mRb.AddForce(transform.up * JumpSpeed, ForceMode.Impulse);
                    //mRb.AddForce(new Vector3(0f, JumpSpeed, 0f), ForceMode.Impulse);
                }
            }
        }
    }

    private void Saltar()
    {
        mRb.velocity = new Vector3(mRb.velocity.x, 0f, mRb.velocity.z);

        mRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ReiniciarSalto()
    {
        isReadyToJump = true;
    }
}