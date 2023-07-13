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
    [System.NonSerialized]
    public bool isReloading = false;
    private AimShotgun scriptGun;
    [SerializeField]
    private GameObject UIReload;
    [SerializeField]
    private GameObject UILowAmmo;
    [SerializeField]
    private GameObject UINoAmmo;
    public ParticleSystem shootGunPSModel;
    public ParticleSystem shootPistolPSModel;
    [SerializeField]
    private bool grounded;
    [SerializeField]
    private float groundDrag = 5f;
    [SerializeField]
    private float playerHeight = 0.4f;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float airMultiplier = 1f;
    private float turnSpeed2;
    private Animator legAnimator;
    [SerializeField]
    private GameObject swat;
    [SerializeField]
    private GameObject legs;
    [System.NonSerialized]
    public bool isInspecting = false;
    private float maxPlayerHealth;
    private bool isBeingDamaged = false;
    [System.NonSerialized]
    public bool apuntando;
    public PlayerController OtherPlayer;
    private bool corriendo;

    //TODO: poner sonidos de recarga y de caminar y sonidos de daño
    //TODO: que el jugador recoja balas del suelo dropeadas por los zombies
    //TODO: voice acting
    private void Awake()
    {
        Instance = this;
        maxPlayerHealth = PlayerHealth;
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
        legAnimator = transform.Find("Legs")
            .GetComponent<Animator>();

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
        turnSpeed2 = turnSpeed;
    }

    private void Update()
    {
        if(!isBeingDamaged)
        {
            if (PlayerHealth < maxPlayerHealth)
            {
                PlayerHealth += 0.05f;
            }else if (PlayerHealth >= maxPlayerHealth)
            {
                PlayerHealth = maxPlayerHealth;
            }
        }
        

        if (aimShotgun.WeaponActive)
        {
            scriptGun = aimShotgun;
        }else
        {
            scriptGun = aimPistol;
        }
        if (!IsDead && !OtherPlayer.IsDead)
        {
            if (!MenuPausa.isPaused)
            {
                Vector3 PosicionSalto = transform.position + new Vector3(0f, playerHeight, 0f);
                grounded = Physics.Raycast(PosicionSalto, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
                SpeedControl();
                if (grounded) {
                    mRb.drag = groundDrag;
                }else 
                {
                    mRb.drag = 0f;
                }
                if (corriendo)
                {
                    speed = WalkingSpeed * RunnigMultiplier;
                }else
                {
                    speed = WalkingSpeed;
                }
                
                /*mRb.velocity = mDirection.y * speed * transform.forward
                    + mDirection.x * speed * transform.right;*/
                MovePlayer();

                var control = GetComponent<PlayerInput>().currentControlScheme;
                //Debug.Log(control);
                if (control == "Gamepad" || control == "Player1Controller" || control == "Player2Controller")
                {
                    turnSpeed2 = turnSpeed*50;
                }else
                {
                    turnSpeed2 = turnSpeed;
                }
                transform.Rotate(
                    Vector3.up,
                    turnSpeed2 * Time.deltaTime * mDeltaLook.x
                );
                cameraMain.GetComponent<CameraMovement>().RotateUpDown(
                    -turnSpeed2 * Time.deltaTime * mDeltaLook.y
                );

                if (mDirection != Vector2.zero && grounded)
                {
                    modelAnimator.SetFloat("Horizontal", mDirection.x);
                    modelAnimator.SetFloat("Vertical", mDirection.y);
                    modelAnimator.SetBool("IsWalking", true);
                    legAnimator.SetBool("IsWalking", true);
                    legAnimator.SetFloat("Horizontal", mDirection.x);
                    legAnimator.SetFloat("Vertical", mDirection.y);
                }
                else
                {
                    modelAnimator.SetBool("IsWalking", false);
                    legAnimator.SetBool("IsWalking", false);
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
                if (aimPistol.isAiming)
                {
                    modelAnimator.SetBool("IsAimingPistol", true);
                }else if (!aimShotgun.isAiming)
                {
                    modelAnimator.SetBool("IsAimingPistol", false);
                }
                if (!isReloading && scriptGun.balasActuales==0 && scriptGun.balasTotales>0)
                {
                    //Recarga
                    reloading();
                }
                if(scriptGun.balasActuales <= scriptGun.balasCargador*0.2 && scriptGun.balasTotales!=0)
                {
                    UIReload.SetActive(true);
                    UILowAmmo.SetActive(false);
                    UINoAmmo.SetActive(false);
                }else if (scriptGun.balasActuales <= scriptGun.balasCargador*0.2 && scriptGun.balasActuales!=0 && scriptGun.balasTotales==0)
                {
                    UIReload.SetActive(false);
                    UILowAmmo.SetActive(true);
                    UINoAmmo.SetActive(false);
                }else if (scriptGun.balasActuales==0 && scriptGun.balasTotales==0)
                {
                    UIReload.SetActive(false);
                    UILowAmmo.SetActive(false);
                    UINoAmmo.SetActive(true);
                }else
                {
                    UIReload.SetActive(false);
                    UILowAmmo.SetActive(false);
                    UINoAmmo.SetActive(false);
                }
                if(grounded){
                    modelAnimator.SetBool("IsJumping", false);
                    legAnimator.SetBool("IsJumping", false);
                }else
                {
                    modelAnimator.SetBool("IsJumping", true);
                    legAnimator.SetBool("IsJumping", true);
                }
            }
        }else
        {
            Cursor.lockState = CursorLockMode.None;
            mPlayerInput.SwitchCurrentActionMap("MenuAndEndgame");
            PlayerCapsulle.SetActive(false);
            primary.SetActive(false);
            secondary.SetActive(false);
            UI.gameObject.SetActive(false);
            DeadScreen.gameObject.SetActive(true);
            swat.SetActive(false);
            legs.SetActive(false);
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

    private void MovePlayer()
    {
        Vector3 moveDirection = mDirection.y * transform.forward + mDirection.x * transform.right;
        if(grounded){
            mRb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }else{
            mRb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(mRb.velocity.x, 0f, mRb.velocity.z);
        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            mRb.velocity = new Vector3(limitedVel.x, mRb.velocity.y, limitedVel.z);
        }
    }

    private void reloading()
    {
        
        isReloading = true;
        if (aimShotgun.WeaponActive)
        {
            mAudioSource.PlayOneShot(aimPistol.Weapon.audioList[2]);
            modelAnimator.SetTrigger("IsReloading");
            mAnimator.SetTrigger("IsReloading");
        }else
        {
            pAudioSource.PlayOneShot(aimPistol.Weapon.audioList[2]);
            modelAnimator.SetTrigger("IsReloading");
            pAnimator.SetTrigger("IsReloading");
        }
        
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
                    if (!isInspecting && !isReloading && (scriptGun.balasActuales>0 && scriptGun.balasActuales<scriptGun.balasCargador && scriptGun.balasTotales!=0))
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
                        if(!isInspecting && !isReloading && (scriptGun.balasTotales > 0 || scriptGun.balasActuales > 0))
                        {
                            mAudioSource.PlayOneShot(aimShotgun.Weapon.audioList[0]);
                            mAnimator.SetTrigger("GunShooting");
                            modelAnimator.SetTrigger("IsShooting");
                            shootGunPSModel.Play();
                            Shoot(scriptGun);
                        }else if(!isInspecting && !isReloading && (scriptGun.balasTotales == 0 && scriptGun.balasActuales == 0))
                        {
                            mAudioSource.PlayOneShot(aimShotgun.Weapon.audioList[1]);
                        }
                    }else
                    {
                        if(!isInspecting && !isReloading && (scriptGun.balasTotales > 0 || scriptGun.balasActuales > 0))
                        {
                            pAudioSource.PlayOneShot(aimPistol.Weapon.audioList[0]);
                            pAnimator.SetTrigger("GunShooting");
                            modelAnimator.SetTrigger("IsShooting");
                            shootPistolPSModel.Play();
                            Shoot(scriptGun);
                        }else if (!isInspecting && !isReloading && (scriptGun.balasTotales == 0 && scriptGun.balasActuales == 0))
                        {
                            pAudioSource.PlayOneShot(aimPistol.Weapon.audioList[1]);
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
            if (OtherPlayer.CameraAnimator != null)
            {
                OtherPlayer.CameraAnimator.enabled = true;
                OtherPlayer.CameraAnimator.SetBool("IsDead", true);
            }
            IsDead = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyAttack"))
        {
            isBeingDamaged = true;
            var damage = col.gameObject.GetComponentInParent<EnemyController>().EnemyType.Damage;
            TakeDamage(damage);
        }else
        {
            isBeingDamaged = false;
        }

        if(col.CompareTag("AmmoCrate"))
        {
            Destroy(col.gameObject);
            scriptGun.balasTotales+=1;
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
                Debug.Log("se presiono play");
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
                    if (grounded)
                    {
                        Saltar();
                    }
                }
            }
        }
    }

    private void Saltar()
    {
        mRb.velocity = new Vector3(mRb.velocity.x, 0f, mRb.velocity.z);

        mRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void OnSwitchWeapon(InputValue value)
    {
        if(!IsDead)
        {
            if(!MenuPausa.isPaused && !isInspecting)
            {
                if(value.isPressed)
                {
                    scriptGun.SwitchWeapon();
                }
            }
        }
    }
    private void OnInspectWeapon(InputValue value)
    {
        if(!IsDead)
        {
            if(!MenuPausa.isPaused)
            {
                if(value.isPressed && !isInspecting && !isReloading)
                {
                    isInspecting = true;
                    if(aimShotgun.WeaponActive)
                    {
                        mAnimator.SetTrigger("IsInspecting");
                    }else
                    {
                        pAnimator.SetTrigger("IsInspecting");
                    }
                }
            }
        }
    }
    public void StopInspecting()
    {
        isInspecting = false;
    }
    
    private void OnStartAiming(InputValue value)
    {
        if(!IsDead)
        {
            if(!MenuPausa.isPaused)
            {
                if (value.isPressed)
                {
                    apuntando = true;
                }
            }
        }
        
    }
    private void OnStopAiming(InputValue value)
    {
        if (apuntando)
        {
            scriptGun.StopAiming();
            apuntando = false;
        }
    }
    private void OnStartSprinting(InputValue value)
    {
        if(!IsDead)
        {
            if(!MenuPausa.isPaused)
            {
                if (value.isPressed)
                {
                    corriendo = true;
                }
            }
        }
        
    }
    private void OnStopSprinting(InputValue value)
    {
        if (apuntando)
        {
            corriendo = false;
        }
    }
}