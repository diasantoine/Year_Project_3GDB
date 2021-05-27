using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class The_Player_Script : MonoBehaviour
{
    [System.Serializable] 
    public struct YourPlayer
    {
        [Header("PlayerBasic")]
        public float vitesse;
        public Rigidbody ConteneurRigibody;
        public Transform SpawnPositionPlayer;
        public GameObject Avatar;
        public GameObject Canon;
        public Animator animAvatar;

        [Header("OverHeated_Armor")] 
        public List<GameObject> ListArmorPart;
        public float CompteurBeforeDecreaseHeatArmor;
        public float FrequenceDecreaseArmorHeat;
        public int NumberOfDecreaseByFrequence_Armor;
        
        [Header("OverHeated_Weapon")] 
        public List<GameObject> ListWeaponPart;
        public float CompteurBeforeDecreaseHeatWeapon;
        public float FrequenceDecreaseWeaponHeat;
        public int NumberOfDecreaseByFrequence_Weapon;
        public int MaxWeaponHeat;
        public int FrequenceDecreasedWhenOverHeated;

    }
    [Header("ChooseYourPlayer")]
    public List<YourPlayer> ListOfYourPlayer = new List<YourPlayer>();
    public int YourPlayerChoosed;

    [Header("Son")]
    [FMODUnity.EventRef]
    public string Jump_Atterissage = "";

    [FMODUnity.EventRef]
    public string Player_GetHit = "";

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    private int xPos = 0, yPos = 0;   
    
    private bool isWalking;
    private RaycastHit hit;
    
    private float slow;
    private float zAiguille;

    private float Compteur = 0;
    private float Compteur1 = 0;
    private float Compteu12 = 0;
    private float CompteurForSlow = 0;
    private float TimePlaque = 0;
    private float TimeColdPlaque = 0;
    private float Compteur3 = 0;
    private float CompteurForArmorHeat = 0;
    private float CompteurForWeaponHeat = 0;
    private float lerpTime;
    
    [Header("PlayerStatArmorHeat")]
    public float PercentageArmorHeat;
    [SerializeField] private float ResiCold;
    [SerializeField] private GameObject FeedbackHitGround;
    
    [Header("PlayerStatWeaponHeat")]
    public float PercentageWeaponHeat;
    public float RColor;
    public float RColorBeforeMax;
    public float GColor;
    public float GColorBeforMax;

    [Header("PlayerLife")]
    [SerializeField] private int maxLife;
    [HideInInspector] public int playerLife;
    [SerializeField] private List<GameObject> lampLife;


    [Header("PlayerBool")]
    public bool Grounded;
    public bool OnDash;
    public bool OnJump;
    public bool OnJumpJustFinished;
    public bool JustFinishedDash;
    public bool JustHit;
    public bool OnShieldProtection;
    public bool ArmorHeated;
    public bool IsNotUsingNormalWeapon = true;
    public bool WeaponOverHeated;
    private bool isOnPlaque;
    private bool Once;
    
    [Header("PlayerDash")]
    public float DistanceDash;
    public Vector3 PointOrigineDash;
    public bool Aftershock;

    [Header("PlayerJump")] 
    public float DistanceJump;
    public Vector3 PointOrigineJump;
    private float HigherPosition = 0;
    public Vector3 SpeedJump;
    public float radiusExploBase;
    public float ForceExplosion;
    public float DMG;
    private bool HitAWall;
    [SerializeField] private GameObject ParticuleAtterissageGameobject;

    [Header("Counter")] 
    public bool OnCounter;

    private float RegulationForce;
    
    

    [Header("Other")] 
    [SerializeField] private Camera cam;
    [SerializeField] private float disToGround;
    [SerializeField] private float TimerBeforeGrounded;
    [SerializeField] public GameObject particleJump;
    [SerializeField] private GameObject aiguilleHeat;
    [SerializeField] private Image barHeat;
    [SerializeField] private Animator UIHeat;
    [SerializeField] private GameSystem GS;

    public float floatTypeOfFootStep;

    private float ContainerTimeBeforeGrounded;

    private GameObject ContainerFeedbackGround;

    
    void Start()
    {
        playerLife = maxLife;
        //Cursor.lockState = CursorLockMode.Locked;
        slow = 1;
        this.ContainerTimeBeforeGrounded = this.TimerBeforeGrounded;
    }

    void Update()
    {
        LifeUI();

        PercentageArmorHeat = Mathf.Clamp(PercentageArmorHeat, 0, 100);

        lerpTime = 3f * Time.deltaTime;

        if(PercentageArmorHeat > 55)
        {
            UIHeat.SetBool("Chaud", true);

        }
        else
        {
            UIHeat.SetBool("Chaud", false);

        }

        if (aiguilleHeat != null)
        {
            updateAiguilleHeat();

        }

        if (barHeat != null)
        {
            updateBarHeat();

        }

        CharacterMouvement();
        HeatPlayer();
        CheckPlaque(hit);

        if (CompteurForSlow >= 0.5f)
        {
            SlowMov(false);

        }
        else
        {
            CompteurForSlow += Time.deltaTime;
        }

    }

    private void updateBarHeat()
    {

        if (PercentageWeaponHeat > 50)
        {
            barHeat.transform.parent.gameObject.SetActive(true);

        }
        else
        {
            barHeat.transform.parent.gameObject.SetActive(false);

        }

        barHeat.fillAmount = Mathf.Lerp(barHeat.fillAmount, PercentageWeaponHeat / 100, lerpTime);

    }

    private void HeatPlayer()
    {
       HeatArmor();
       HeatWeapon();
    }

    private void LifeUI()
    {
        switch (playerLife)
        {
            case 3:
                lampLife[0].SetActive(true);
                break;                   
            case 2:
                lampLife[0].SetActive(false);
                lampLife[1].SetActive(true);
                break;
            case 1:
                lampLife[1].SetActive(false);
                lampLife[2].SetActive(true);
                break;
            case 0:
                lampLife[2].SetActive(false);
                break;

        }
    }

    private void HeatArmor()
    {
        if (!JustHit && PercentageArmorHeat > 0)
        {
            if (CompteurForArmorHeat >= ListOfYourPlayer[YourPlayerChoosed].CompteurBeforeDecreaseHeatArmor)
            {
                //CompteurForArmorHeat = 0;
                if (!isOnPlaque)
                {
                    //StartCoroutine(DecreaseArmorHeat());
                    PercentageArmorHeat -= Time.deltaTime * ListOfYourPlayer[YourPlayerChoosed].NumberOfDecreaseByFrequence_Armor;
                    foreach (GameObject ArmorPart in  ListOfYourPlayer[YourPlayerChoosed].ListArmorPart)
                    {
                        ArmorPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                            new Color(1,1 - PercentageArmorHeat/100f, 1 -PercentageArmorHeat/100f));
                    }
                }
            }
            else
            {
                CompteurForArmorHeat += Time.deltaTime;
            }
        }
        else
        {
            if (CompteurForArmorHeat != 0)
            {
                CompteurForArmorHeat = 0;
            }
        }
    }
    
    
    private void HeatWeapon()
    {
        if (this.PercentageWeaponHeat >= this.ListOfYourPlayer[this.YourPlayerChoosed].MaxWeaponHeat && !this.WeaponOverHeated)
        {
            this.WeaponOverHeated = true;
        }

        if (this.WeaponOverHeated)
        {
            if (PercentageWeaponHeat <=0)
            {
                WeaponOverHeated = false;
                foreach (GameObject WeaponPart in  ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart)
                {
                    WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                        new Color( this.RColor + this.RColorBeforeMax * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,
                            this.GColor - this.GColorBeforMax  * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f , 0));
                }
            }
            else
            {
                PercentageWeaponHeat -= Time.deltaTime * this.ListOfYourPlayer[this.YourPlayerChoosed].FrequenceDecreasedWhenOverHeated;
                this.PercentageWeaponHeat = Mathf.Clamp(this.PercentageWeaponHeat, 0, Mathf.Infinity);
                // Debug.Log(this.PercentageWeaponHeat);
                // foreach (GameObject WeaponPart in  ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart)
                // {
                //     WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                //         new Color( this.RColor + this.RColorBeforeMax * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,
                //             this.GColor - this.GColorBeforMax  * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f , 0));
                // }
            }
        }
        else
        {
            if (IsNotUsingNormalWeapon && PercentageWeaponHeat > 0)
            {
                PercentageWeaponHeat -= Time.deltaTime * this.ListOfYourPlayer[this.YourPlayerChoosed].FrequenceDecreaseWeaponHeat;
                this.PercentageWeaponHeat = Mathf.Clamp(this.PercentageWeaponHeat, 0, Mathf.Infinity);
                Debug.Log(this.PercentageWeaponHeat);
                foreach (GameObject WeaponPart in  ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart)
                {
                    // WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                    //     new Color(GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,0 , 0));
                    WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                        new Color( this.RColor + this.RColorBeforeMax * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,
                            this.GColor - this.GColorBeforMax  * GetComponent<The_Player_Script>().PercentageWeaponHeat/100f , 0));
                }
                // if (CompteurForWeaponHeat >= ListOfYourPlayer[YourPlayerChoosed].CompteurBeforeDecreaseHeatWeapon)
                // {
                //     CompteurForWeaponHeat = 0;
                //     StartCoroutine(DecreaseWeaponHeat());
                // }
                // else
                // {
                //     CompteurForWeaponHeat += Time.deltaTime;
                // }
            }
            else
            {
                if (CompteurForWeaponHeat != 0)
                {
                    CompteurForWeaponHeat = 0;
                }
            }
        }
    }

    IEnumerator DecreaseArmorHeat()
    {
        //Debug.Log("the fuck?");
        yield return new WaitForSeconds(ListOfYourPlayer[YourPlayerChoosed].FrequenceDecreaseArmorHeat);
        PercentageArmorHeat -= ListOfYourPlayer[YourPlayerChoosed].NumberOfDecreaseByFrequence_Armor;
        foreach (GameObject ArmorPart in  ListOfYourPlayer[YourPlayerChoosed].ListArmorPart)
        {
            ArmorPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                new Color(1,1 - PercentageArmorHeat/100f, 1 -PercentageArmorHeat/100f));
        }
    }
    
    IEnumerator DecreaseWeaponHeat()
    {
        yield return new WaitForSeconds(ListOfYourPlayer[YourPlayerChoosed].FrequenceDecreaseWeaponHeat);
        PercentageWeaponHeat -= ListOfYourPlayer[YourPlayerChoosed].NumberOfDecreaseByFrequence_Weapon;
        foreach (GameObject WeaponPart in  ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart)
        {
            WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                new Color(GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,0 , 0));
        }
        if (WeaponOverHeated && PercentageWeaponHeat <=0)
        {
            WeaponOverHeated = false;
        }
    }

    private void CharacterMouvement()
    {
        //Ground();// Teste s'il est ground

        Player_On_Jump();// Take care of the y of the player and set the player to normal state when jump is over
        
        DashFinishCheck(); // Check if the dash is finished, if it is set var at the normal state
       
        Player_Reaction_After_Hit(); // This function take care of the recovery of the player after a hit

        PlayerFall(); // Used when the player fall out of the arena

        Player_Deplacement(); // Player deplacement 

    }

    private void Player_Reaction_After_Hit()
    {
        if (JustHit)
        {
            if (!Grounded)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                if (OnDash)
                {
                    JustHit = false;
                    Compteu12 = 0;
                    Compteur1 = 0;
                }
                else
                {
                    if (!ArmorHeated)
                    {
                        ContainerFeedbackGround = Instantiate(this.FeedbackHitGround, transform.position, Quaternion.Euler(-90,0,0));
                        ContainerFeedbackGround.transform.parent = transform;
                        FMODUnity.RuntimeManager.PlayOneShot(Player_GetHit, "", 0, transform.position);
                        foreach (GameObject ArmorPart in  ListOfYourPlayer[YourPlayerChoosed].ListArmorPart)
                        {
                            ArmorPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                                    new Color(1,1 - PercentageArmorHeat/100f, 1 -PercentageArmorHeat/100f));
                        }
                        ArmorHeated = true;
                    }
                    if (Compteu12 < 0.4f)
                    {
                        Compteu12 += Time.deltaTime;
                    }
                    else
                    {
                        Destroy(ContainerFeedbackGround);
                        JustHit = false;
                        ArmorHeated = false;
                        Compteu12 = 0;
                        Compteur1 = 0;
                    }
                }
            }
        }
    }

    private void Player_Deplacement()
    {
        if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && Grounded && !OnDash && !JustHit && !this.OnJump && !this.OnCounter)
        {
            Vector3 ConteneurCameraPositionForward = this.cam.transform.forward * Input.GetAxis("Vertical");
            Vector3 ConteneurCameraPositionRight = this.cam.transform.right * Input.GetAxis("Horizontal");
            Vector3 Vector3_Deplacement_Player =
                Vector3.ClampMagnitude(ConteneurCameraPositionForward + ConteneurCameraPositionRight, 1);
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("IsMoving", true);
            // Debug.Log(Mathf.RoundToInt(Vector3.Dot(transform.forward, 
            //     ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)));
            if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == 1)
            {
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", true);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Left", false);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Right", false);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("Mouvement", 1);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == -1)
            {
                //Debug.Log(Vector3.Angle(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized, transform.right));
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", false);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", true);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Left", false);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Right", false);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("Mouvement", -1);
            }
            else
            {
                if (transform.InverseTransformDirection(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity).x > 0)
                {
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", false);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Left", true);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Right", false);
                    ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("Mouvement", 0.5f);
                }
                else
                {
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", false);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Left", false);
                    // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Right", true);
                    ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("Mouvement", -0.5f);
                }
                //Debug.Log (ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", true);
                // ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
            }
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity =
                Vector3_Deplacement_Player * ListOfYourPlayer[YourPlayerChoosed].vitesse * slow;
            //ListOfYourPlayer[YourPlayerChoosed].animAvatar.speed = ListOfYourPlayer[YourPlayerChoosed].vitesse * slow * 0.25f;
//            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("SpeedWalk",ListOfYourPlayer[YourPlayerChoosed].vitesse * slow * 0.25f);
        }
        else
        {
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("IsMoving", false);
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetFloat("Mouvement", 0);
            if (!JustHit && !OnDash && Grounded && !this.OnJump)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                    new Vector3(0, ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.y, 0);
            }
        }
    }

    private void PlayerFall()
    {
        if (transform.position.y <= -6 || Input.GetKeyDown(KeyCode.F3))
        {
            transform.position = ListOfYourPlayer[YourPlayerChoosed].SpawnPositionPlayer.position;
            //SetCursorPos(xPos,yPos);//Call this when you want to set the mouse position
            if (OnDash)
            {
                OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                ListOfYourPlayer[YourPlayerChoosed].Avatar.layer = 9;
                transform.tag = "Player";
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.useGravity = true;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }

            if (this.OnJump)
            {
                this.OnJump = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                ListOfYourPlayer[YourPlayerChoosed].Avatar.layer = 9;
                transform.tag = "Player";
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.useGravity = true;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
            PercentageArmorHeat = 0;
            CompteurForArmorHeat = 0;
            PercentageWeaponHeat = 0;
            this.CompteurForWeaponHeat = 0;
            slow = 1;
            Debug.Log("MAISWTF?");
            foreach (GameObject ArmorPart in  ListOfYourPlayer[YourPlayerChoosed].ListArmorPart)
            {
                ArmorPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                    new Color(1,1 - PercentageArmorHeat/100f, 1 -PercentageArmorHeat/100f));
            }
            foreach (GameObject WeaponPart in  ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart)
            {
                WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                    new Color(GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,0 ,0));
            }
            if (WeaponOverHeated)
            {
                WeaponOverHeated = false;
            }
            if(playerLife > 0)
            {
                playerLife--;
                Debug.Log(playerLife);

                if(playerLife == 0)
                {
                    LifeUI();
                    GS.GameOver();
                    gameObject.SetActive(false);
                }

            }
        }
        else
        {
            if (!Grounded && !OnDash && !this.OnJump)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints 
                    = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
            else if( !this.OnJump && ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints != 
                    (RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation))
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = 
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void DashFinishCheck()
    {
        if (OnDash)
        {
            float Distance = Vector3.Distance(ListOfYourPlayer[YourPlayerChoosed].Canon.transform.position, this.PointOrigineDash);
            if (Distance >= DistanceDash)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.mass = 1;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized * 
                    ListOfYourPlayer[YourPlayerChoosed].vitesse;
                JustFinishedDash = true;
                OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                ListOfYourPlayer[YourPlayerChoosed].Avatar.layer = 9;
                tag = "Player";
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.useGravity = true;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                if (Aftershock)
                {
                    //explosion
                    
                }
            }
        }
        else
        {
            if (JustFinishedDash)
            {
                if (Compteur<=0.6f)
                {
                    Compteur += Time.deltaTime;
                }
                else
                {
                    Compteur = 0;
                    JustFinishedDash = false;
                }
            }
        }
    }

    private void Player_On_Jump()
    {
        if (this.OnJump)
        {
            particleJump.SetActive(true);
            float Distance = Vector3.Distance(new Vector3(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.transform.position.x, 0, 
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.transform.position.z)  , new Vector3(this.PointOrigineJump.x, 0, this.PointOrigineJump.z));
            //Debug.Log(Distance + " " + this.DistanceJump);
            //Debug.Log(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity);
            if (Distance >= this.DistanceJump)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.drag = 1;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.angularDrag = 0.5f;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.mass = 1;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized * 
                    ListOfYourPlayer[YourPlayerChoosed].vitesse;
                this.OnJumpJustFinished = true;
                this.OnJump = false;              
                GetComponent<CapsuleCollider>().enabled = !enabled;
                ListOfYourPlayer[YourPlayerChoosed].Avatar.layer = 9;
                tag = "Player";
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.useGravity = true;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                this.HigherPosition = 0;
                this.Once = false;
                this.ImpulsionTahLesfous();
                CameraShake.Instance.Shake(3, 0.5f);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Jump", false);
                FMODUnity.RuntimeManager.PlayOneShot(Jump_Atterissage, "", 0, transform.position);
                GameObject ContainerParticule = Instantiate(this.ParticuleAtterissageGameobject, transform.position, Quaternion.Euler(-90,0,0));
                ContainerParticule.transform.parent = transform;
                Destroy(ContainerParticule,ContainerParticule.GetComponent<ParticleSystem>().main.duration);
            }
            else
            {
                if (ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity != this.SpeedJump)
                {
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = this.SpeedJump;
                }
                if (!this.Once)
                {
                    this.Once = true;
                    this.JumpPlayer();
                }
            }
        }
        else
        {
            if (this.OnJumpJustFinished)
            {
                if (Compteur<=0.6f)
                {
                    Compteur += Time.deltaTime;
                }
                else
                {
                    Compteur = 0;
                    this.OnJumpJustFinished = false;
                }
            }
        }
    }
    
 private void CheckPlaque(RaycastHit hit)
    {
        if (Grounded)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hit, 3f, LayerMask.GetMask("Sol", "Wall")))
            {
                if (hit.collider.CompareTag("sol"))
                {
                    GameObject plaque = hit.collider.gameObject;

                    if (plaque.GetComponent<plaqueScript>())
                    {
                        plaqueScript pS = plaque.GetComponent<plaqueScript>();

                        switch (pS.type)
                        {

                            case plaqueScript.Type.NORMAL:
                                // footStepPlayer.setParameterByName("TypeOfFootstep", 0);
                                floatTypeOfFootStep = 0;
                                isOnPlaque = false;
                                TimeColdPlaque = 0;
                                break;
                            case plaqueScript.Type.HOT:
                                // footStepPlayer.setParameterByName("TypeOfFootstep", 1);
                                if (pS.activ)
                                {
                                    floatTypeOfFootStep = 1;
                                    //Debug.Log("oui");
                                    ArmorHeatPlaque(1);
                                }
                                else
                                {
                                    floatTypeOfFootStep = 0;
                                    isOnPlaque = false;
                                    TimePlaque = 0;
                                }
                                break;
                            case plaqueScript.Type.COLD:
                                //floatTypeOfFootStep = 2;
                                if (pS.activ)
                                {
                                    floatTypeOfFootStep = 2;
                                    TimeColdPlaque += Time.deltaTime;
                                    CompteurForSlow = 0;

                                    ArmorHeatPlaque(-1);

                                    if(TimeColdPlaque >= ResiCold)
                                    {
                                        SlowMov(true);
                                    }
                                    
                                }
                                else
                                {
                                    isOnPlaque = false;
                                    TimePlaque = 0;
                                    TimeColdPlaque = 0;
                                }
                                break;
                            case plaqueScript.Type.TOXIC:
                                //floatTypeOfFootStep = 3;
                                
                                if (pS.ressourceGot >= 10 && !pS.regenUP)
                                {
                                    floatTypeOfFootStep = 3;

                                    if (TimePlaque >= 1f)
                                    {
                                        if(detectDead.ressourceFloat < 100)
                                        {
                                            detectDead.ressourceFloat += 10;
                                            pS.ressourceGot -= 10;
                                            TimePlaque = 0;
                                        }
                                    }
                                    else
                                    {
                                        TimePlaque += Time.deltaTime;
                                    }
                                }
                                else
                                {
                                    TimePlaque = 0;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
 
 private void ArmorHeatPlaque(int One)
 {
     isOnPlaque = true;
     if (TimePlaque >= 0.1f)
     {
         if (PercentageArmorHeat <= 100 && PercentageArmorHeat >= 0)
         {
             PercentageArmorHeat += One;

         }
         foreach (GameObject ArmorPart in ListOfYourPlayer[YourPlayerChoosed].ListArmorPart)
         {
             ArmorPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                 new Color(1, 1 - PercentageArmorHeat / 100f, 1 - PercentageArmorHeat / 100f));
         }

         TimePlaque = 0;

     }
     else
     {
         TimePlaque += Time.deltaTime;
     }
 }

 private void SlowMov(bool Decr)
 {
     if (Decr)
     {
         if(slow >= 0.5f)
         {
             slow -= 0.1f * Time.deltaTime;

         }
     }
     else
     {
         if(slow <= 1)
         {
             slow += 0.75f * Time.deltaTime;
         }

     }
 }

    private void updateAiguilleHeat()
    {
        zAiguille = Mathf.Lerp(zAiguille, PercentageArmorHeat * -1.47f , lerpTime);
        aiguilleHeat.transform.eulerAngles = new Vector3(0, 0, zAiguille);

    }

 private void JumpPlayer()
 {
     this.ListOfYourPlayer[this.YourPlayerChoosed].animAvatar.SetBool("Jump", true);
 }
 
 void ImpulsionTahLesfous()
    {
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        for (int i = 0; i < hit.Length; i++)
        {
           if (hit[i].gameObject.CompareTag("Ennemy"))
           {
                if (hit[i].GetComponent<BasicAI>() != null)
                {
                    hit[i].GetComponent<BasicAI>().ExplosionImpact(hitPoint, radiusExploBase +  transform.localScale.x, ForceExplosion);
                    hit[i].GetComponent<BasicState>().Damage(DMG);
                }
                else if(hit[i].GetComponent<ScreamerState>() != null)
                {
                    hit[i].GetComponent<ScreamerState>().Damage(Mathf.Infinity);
                }
                else
                {
                    //rien
                }
           }
        }
    }
 
 public void Ground()
 {
     //Debug.DrawRay(transform.position, -Vector3.up,Color.yellow, 5000f);
     if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, disToGround, LayerMask.GetMask("Sol", "Wall")))
     {
         Debug.Log(hit.collider.name);
         Debug.DrawRay(transform.position, -Vector3.up, Color.magenta);
         if (hit.collider.transform.CompareTag("sol") || hit.collider.transform.CompareTag("Ennemy")  || hit.collider.transform.CompareTag("Mur") && !Grounded)
         {
            if (this.TimerBeforeGrounded <= 0)
            {
                this.Grounded = true;
                particleJump.SetActive(false);
                this.TimerBeforeGrounded = this.ContainerTimeBeforeGrounded;
            }
            else
            {
                this.TimerBeforeGrounded -= Time.deltaTime;
            }
         }
     }
     else
     {
         Grounded = false;
         this.TimerBeforeGrounded = this.ContainerTimeBeforeGrounded;
     }
 }

 private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 && other.GetComponent<RuantAI>() != null &&
            other.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            JustHit = true;
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.freezeRotation = false;
            Vector3 dir = transform.position;
            dir = (dir - other.transform.position).normalized;
            CameraShake.Instance.Shake(10, 1f);
            //dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
            dir.y = 0;
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
            this.RegulationForce = other.GetComponent<RuantAI>().ForcePercussion;
            Debug.Log(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity);
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.AddForceAtPosition(dir * (other.GetComponent<Rigidbody>().velocity.magnitude 
                    * RegulationForce + (other.GetComponent<Rigidbody>().velocity.magnitude * RegulationForce * PercentageArmorHeat/100)), 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.ClosestPointOnBounds(other.transform.position)); 
            PercentageArmorHeat += other.GetComponent<RuantAI>().DmgArmorHeat;
        }
        if (this.OnJump && other.CompareTag("Mur"))
        {
            this.HitAWall = true;
        }

        /*if (other.gameObject.CompareTag("Plaque"))
        {
            switch (other.gameObject.GetComponent<plaqueScript>().type)
            {
                case plaqueScript.Type.NORMAL:
                    Debug.Log("Ok");
                    break;
                case plaqueScript.Type.HOT:
                    Debug.Log("Chaud");
                    break;
                case plaqueScript.Type.COLD:
                    Debug.Log("Froid");
                    break;
                case plaqueScript.Type.TOXIC:
                    Debug.Log("POISON");
                    break;
                case plaqueScript.Type.PISTON:
                    Debug.Log("Yahou");
                    break;
                default:
                    break;
            }
        }*/
    }
}