using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;


public class The_Player_Script : MonoBehaviour
{

    [System.Serializable] 
    public struct YourPlayer
    {
        [Header("PlayerBasic")]
        public float vitesse;
        public Rigidbody ConteneurRigibody;
        public Transform SpawnPositionPlayer;
        public float clamp;
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
        public float FrequenceDecreaseWeaponHeat;
        public int NumberOfDecreaseByFrequence_Weapon;
        public float CompteurBeforeDecreaseHeatWeapon;
        
      

    }
    [Header("ChooseYourPlayer")]
    public List<YourPlayer> ListOfYourPlayer = new List<YourPlayer>();
    public int YourPlayerChoosed;

    
    
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    private int xPos = 0, yPos = 0;   
    
    private bool isWalking;
    
    
    private float Compteur = 0;
    private float Compteur1 = 0;
    private float Compteu12 = 0;
    private float Compteur3 = 0;
    private float CompteurForArmorHeat = 0;
    private float CompteurForWeaponHeat = 0;
    
    [Header("PlayerStatArmorHeat")]
    public int PercentageArmorHeat;
    
    [Header("PlayerStatWeaponHeat")]
    public int PercentageWeaponHeat;

    [Header("PlayerBool")]
    public bool Grounded;
    public bool OnDash;
    public bool JustFinishedDash;
    public bool JustHit;
    public bool OnShieldProtection;
    public bool ArmorHeated;
    public bool IsNotUsingNormalWeapon = true;
    public bool WeaponOverHeated;
    
    [Header("PlayerDash")]
    public float DistanceDash;
    public Vector3 PointOrigine;
    public bool Aftershock;
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CharacterMouvement();
        HeatPlayer();
        
    }

    private void HeatPlayer()
    {
       HeatArmor();
       HeatWeapon();
    }

    private void HeatArmor()
    {
        if (!JustHit && PercentageArmorHeat >0)
        {
            if (CompteurForArmorHeat >= ListOfYourPlayer[YourPlayerChoosed].CompteurBeforeDecreaseHeatArmor)
            {
                CompteurForArmorHeat = 0;
                DecreaseArmorHeat();
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
        if ((IsNotUsingNormalWeapon || WeaponOverHeated) && PercentageWeaponHeat >0)
        {
            if (CompteurForWeaponHeat >= ListOfYourPlayer[YourPlayerChoosed].CompteurBeforeDecreaseHeatWeapon)
            {
                CompteurForWeaponHeat = 0;
                DecreaseWeaponHeat();
            }
            else
            {
                CompteurForWeaponHeat += Time.deltaTime;
            }
        }
        else
        {
            if (CompteurForWeaponHeat != 0)
            {
                CompteurForWeaponHeat = 0;
            }
        }
    }

    IEnumerator DecreaseArmorHeat()
    {
        yield return new WaitForSeconds(ListOfYourPlayer[YourPlayerChoosed].FrequenceDecreaseArmorHeat);
        PercentageArmorHeat -= ListOfYourPlayer[YourPlayerChoosed].NumberOfDecreaseByFrequence_Armor;
        ListOfYourPlayer[YourPlayerChoosed].ListArmorPart[0].
            GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor",
                new Color(1,PercentageArmorHeat/100, PercentageArmorHeat/100));

    }
    
    IEnumerator DecreaseWeaponHeat()
    {
        yield return new WaitForSeconds(ListOfYourPlayer[YourPlayerChoosed].FrequenceDecreaseWeaponHeat);
        PercentageWeaponHeat -= ListOfYourPlayer[YourPlayerChoosed].NumberOfDecreaseByFrequence_Weapon;
        ListOfYourPlayer[YourPlayerChoosed].ListWeaponPart[0].
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",
                new Color(1,PercentageWeaponHeat/100, PercentageWeaponHeat/100));
        if (WeaponOverHeated && PercentageWeaponHeat <=0)
        {
            WeaponOverHeated = false;
        }

    }

    private void CharacterMouvement()
    {
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
                    JustHit = 
                    JustHit = false;
                    Compteu12 = 0;
                    Compteur1 = 0;
                }
                else
                {
                    if (!ArmorHeated)
                    {
                        ListOfYourPlayer[YourPlayerChoosed].ListArmorPart[0].
                            GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor",
                                new Color(1,PercentageArmorHeat/100, PercentageArmorHeat/100));
                        ArmorHeated = true;
                    }
                    if (Compteu12 < 0.4f)
                    {
                        Compteu12 += Time.deltaTime;
                    }
                    else
                    {
                        
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
        if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && Grounded && !OnDash && !JustHit)
        {
            Vector3 ConteneurCameraPositionForward = Camera.main.transform.forward * Input.GetAxis("Vertical");
            Vector3 ConteneurCameraPositionRight = Camera.main.transform.right * Input.GetAxis("Horizontal");
            Vector3 Vector3_Deplacement_Player =
                Vector3.ClampMagnitude(ConteneurCameraPositionForward + ConteneurCameraPositionRight, 1);
            if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == 1)
            {
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", true);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == -1)
            {
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", true);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", false);
            }
            else
            {
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", true);
                ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);
            }

            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                Vector3_Deplacement_Player * 
                (ListOfYourPlayer[YourPlayerChoosed].vitesse / 
                 Mathf.Clamp(detectDead.ressourceInt / 
                             ListOfYourPlayer[YourPlayerChoosed].clamp, 1, 1.75f));
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.speed = 
                (ListOfYourPlayer[YourPlayerChoosed].vitesse / Mathf.Clamp(
                    detectDead.ressourceInt / ListOfYourPlayer[YourPlayerChoosed].clamp, 1, 1.75f)) 
                * 1 / ListOfYourPlayer[YourPlayerChoosed].vitesse;
        }
        else
        {
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Forward", false);
            ListOfYourPlayer[YourPlayerChoosed].animAvatar.SetBool("Backward", false);

            if (!JustHit && !OnDash && Grounded)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                    new Vector3(0, ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.y, 0);
            }
        }
    }

    private void PlayerFall()
    {
        if (transform.position.y <= -6)
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

            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
        }
        else
        {
            if (!Grounded && !OnDash)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints 
                    = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
            else if(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints != 
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
            float Distance = Vector3.Distance(ListOfYourPlayer[YourPlayerChoosed].Canon.transform.position, PointOrigine);
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            transform.position = ListOfYourPlayer[YourPlayerChoosed].SpawnPositionPlayer.position;
        }

        if (other.gameObject.layer == 13 && other.GetComponent<RuantAI>() != null &&
            other.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            JustHit = true;
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.freezeRotation = false;
            Vector3 dir = transform.position;
            dir = (dir - other.transform.position).normalized;
            //dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
            dir.y = 0;
            float RegulationForce = 150;
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
            Debug.Log(ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity);
            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.
                AddForceAtPosition(dir * other.GetComponent<Rigidbody>().velocity.magnitude 
                                       * RegulationForce,
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.ClosestPointOnBounds(other.transform.position));
        }
    }
}