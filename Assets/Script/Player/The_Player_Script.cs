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
        [SerializeField] private float clamp;
        [SerializeField] private GameObject Avatar;
        [SerializeField] private GameObject Canon;
        [SerializeField] private Animator animAvatar;


        [Header("PlayerBool")]
        public bool Grounded;
        public bool OnDash;
        public bool JustFinishedDash;
        public bool JustHit;
        public bool OnShieldProtection;
        
        [Header("OverHeated_Armor")] 
        [SerializeField] private List<GameObject> ListArmorPart;
        [SerializeField] private float CompteurBeforeDecreaseHeatArmor;
        public int PercentageArmorHeat;
        
        [Header("OverHeated_Weapon")] 
        [SerializeField] private List<GameObject> ListWeaponPart;
        [SerializeField] private float CompteurBeforeDecreaseHeatWeapon;
        public int PercentageWeaponHeat;
        
        [Header("PlayerDash")]
        public Vector3 HitPosition;
        public float DistanceDash;
        public Vector3 PointOrigine;
        public bool Aftershock;

    }
    [SerializeField] private List<YourPlayer> ListOfYourPlayer = new List<YourPlayer>();
    [SerializeField] private int YourPlayerChoosed;

    
    
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    private int xPos = 0, yPos = 0;   
    
    private bool isWalking;
    
    
    private float Compteur = 0;
    private float Compteur1 = 0;
    private float Compteu12 = 0;
    private float Compteur3 = 0;
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CharacterMouvement();
    }

    private void OverHeatedPlayer()
    {
        
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
        if (ListOfYourPlayer[YourPlayerChoosed].JustHit)
        {
            if (!ListOfYourPlayer[YourPlayerChoosed].Grounded)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                if (ListOfYourPlayer[YourPlayerChoosed].OnDash)
                {
                    ListOfYourPlayer[YourPlayerChoosed].JustHit = 
                    ListOfYourPlayer[YourPlayerChoosed].JustHit = false;
                    Compteu12 = 0;
                    Compteur1 = 0;
                }
                else
                {
                    if (Compteu12 < 0.4f)
                    {
                        Compteu12 += Time.deltaTime;
                    }
                    else
                    {
                        JustHit = false;
                        Compteu12 = 0;
                        Compteur1 = 0;
                    }
                }
            }
        }
    }

    private void Player_Deplacement()
    {
        if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && 
            ListOfYourPlayer[YourPlayerChoosed].Grounded && !ListOfYourPlayer[YourPlayerChoosed].OnDash 
            && !ListOfYourPlayer[YourPlayerChoosed].JustHit)
        {
            Vector3 ConteneurCameraPositionForward = Camera.main.transform.forward * Input.GetAxis("Vertical");
            Vector3 ConteneurCameraPositionRight = Camera.main.transform.right * Input.GetAxis("Horizontal");
            Vector3 Vector3_Deplacement_Player =
                Vector3.ClampMagnitude(ConteneurCameraPositionForward + ConteneurCameraPositionRight, 1);
            if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == 1)
            {
                animAvatar.SetBool("Forward", true);
                animAvatar.SetBool("Backward", false);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(transform.forward, 
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized)) == -1)
            {
                animAvatar.SetBool("Backward", true);
                animAvatar.SetBool("Forward", false);
            }
            else
            {
                animAvatar.SetBool("Forward", true);
                animAvatar.SetBool("Backward", false);
            }

            ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                Vector3_Deplacement_Player * 
                (ListOfYourPlayer[YourPlayerChoosed].vitesse / 
                 Mathf.Clamp(detectDead.ressourceInt / clamp, 1, 1.75f));
            animAvatar.speed = (ListOfYourPlayer[YourPlayerChoosed].vitesse / 
                                Mathf.Clamp(detectDead.ressourceInt / clamp, 1, 1.75f)) 
                * 1 / ListOfYourPlayer[YourPlayerChoosed].vitesse;
        }
        else
        {
            animAvatar.SetBool("Forward", false);
            animAvatar.SetBool("Backward", false);

            if (!ListOfYourPlayer[YourPlayerChoosed].JustHit && !ListOfYourPlayer[YourPlayerChoosed].OnDash 
                                                             && ListOfYourPlayer[YourPlayerChoosed].Grounded)
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
            if (ListOfYourPlayer[YourPlayerChoosed].OnDash)
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
            if (!ListOfYourPlayer[YourPlayerChoosed].Grounded && !ListOfYourPlayer[YourPlayerChoosed].OnDash)
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
        if (ListOfYourPlayer[YourPlayerChoosed].OnDash)
        {
            float Distance = Vector3.Distance(Canon.transform.position, 
                ListOfYourPlayer[YourPlayerChoosed].PointOrigine);
            Debug.Log(Distance);
            if (Distance >= ListOfYourPlayer[YourPlayerChoosed].DistanceDash)
            {
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.mass = 1;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity = 
                    ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.velocity.normalized * 
                    ListOfYourPlayer[YourPlayerChoosed].vitesse;
                ListOfYourPlayer[YourPlayerChoosed].JustFinishedDash = true;
                ListOfYourPlayer[YourPlayerChoosed].OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                ListOfYourPlayer[YourPlayerChoosed].Avatar.layer = 9;
                tag = "Player";
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.useGravity = true;
                ListOfYourPlayer[YourPlayerChoosed].ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                if (ListOfYourPlayer[YourPlayerChoosed].Aftershock)
                {
                    //explosion
                }
            }
        }
        else
        {
            if (ListOfYourPlayer[YourPlayerChoosed].JustFinishedDash)
            {
                if (Compteur<=0.6f)
                {
                    Compteur += Time.deltaTime;
                }
                else
                {
                    Compteur = 0;
                    ListOfYourPlayer[YourPlayerChoosed].JustFinishedDash = false;
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
            ListOfYourPlayer[YourPlayerChoosed].JustHit = true;
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