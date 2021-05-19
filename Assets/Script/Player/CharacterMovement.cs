using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;

public class CharacterMovement : MonoBehaviour
{
    
    //[SerializeField] private Animator anim;
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    int xPos = 0, yPos = 0;   
    
    private bool isWalking;

    public float vitesse = 6f;

    public Rigidbody ConteneurRigibody;

    [SerializeField] public bool Grounded;
    public bool OnDash = false;
    public bool JustFinishedDash = false;
    public bool JustHit = false;
    public bool OnShieldProtection = false;
    private float Compteur = 0;
    private float Compteur1 = 0;
    private float Compteu12 = 0;
    private float Compteur3 = 0;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private float clamp;

    public Vector3 HitPosition = new Vector3();
    public Transform SpawnPositionPlayer;

    [SerializeField] private Animator animAvatar;
    [SerializeField] private GameObject Canon;

    public float DistanceDash = 0;
    public Vector3 PointOrigine = new Vector3();


    public bool Aftershock;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Grounded);
        DashFinishCheck();
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

        if (JustHit)
        {
            if (!Grounded)
            {
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
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
                    if (Compteu12< 0.4f)
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
                // if (Compteur1<=0.5f)
                // {
                //     Compteur1 += Time.deltaTime;
                // }
                // else
                // {
                //     ConteneurRigibody.velocity *=0.3f;
                //     if (Compteur1 != 0)
                //     {
                //         Compteur1 = 0;
                //     }
                //     if (ConteneurRigibody.velocity.magnitude<=4)
                //     {
                //         Debug.Log("bjr");
                //         JustHit = false;
                //     }
                // }
            }
        }
        if (transform.position.y<=-6)
        {
            transform.position = SpawnPositionPlayer.position;
            //SetCursorPos(xPos,yPos);//Call this when you want to set the mouse position
            if (OnDash)
            {
                OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                Avatar.layer = 9;
                transform.tag = "Player";
                ConteneurRigibody.useGravity = true;
                ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
            ConteneurRigibody.velocity = Vector3.zero;
        }

        //Debug.DrawRay(transform.position, transform.forward*20, Color.blue);
        if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && Grounded && !OnDash && !JustHit)
        {
            Vector3 ConteneurCameraPositionForward = Camera.main.transform.forward * Input.GetAxis("Vertical");
            Vector3 ConteneurCameraPositionRight = Camera.main.transform.right * Input.GetAxis("Horizontal");
            //Vector3 Vector3_Deplacement_Player =  new Vector3(-Input.GetAxis("Vertical") , 0, Input.GetAxis("Horizontal"));
            Vector3 Vector3_Deplacement_Player = Vector3.ClampMagnitude(ConteneurCameraPositionForward + ConteneurCameraPositionRight, 1);
            if (Mathf.RoundToInt(Vector3.Dot(transform.forward, ConteneurRigibody.velocity.normalized)) == 1)
            {
                animAvatar.SetBool("Forward", true);
                animAvatar.SetBool("Backward", false);

            }
            else if (Mathf.RoundToInt(Vector3.Dot(transform.forward, ConteneurRigibody.velocity.normalized)) == -1)
            {
                animAvatar.SetBool("Backward", true);
                animAvatar.SetBool("Forward", false);

            }
            else
            {
                animAvatar.SetBool("Forward", true);         
                animAvatar.SetBool("Backward", false);

            }
            ConteneurRigibody.velocity = Vector3_Deplacement_Player * (vitesse / Mathf.Clamp(detectDead.ressourceFloat / clamp, 1, 1.75f));
            animAvatar.speed = (vitesse / Mathf.Clamp(detectDead.ressourceFloat / clamp, 1, 1.75f)) * 1/vitesse;
            // Debug.DrawRay(transform.position, ConteneurRigibody.velocity.normalized*20, Color.red);
            //RigibodyAvatar.AddForce(Vector3_Deplacement_Player * Speed_Player);
        }
        else
        {
            animAvatar.SetBool("Forward", false);
            animAvatar.SetBool("Backward", false);

            if (!JustHit && !OnDash && Grounded)
            {
                ConteneurRigibody.velocity = new Vector3(0, ConteneurRigibody.velocity.y, 0);

            }
        }

        if (!Grounded && !OnDash)
        {
            ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        }
        else if(ConteneurRigibody.constraints != (RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation))
        {
            ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        /*float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            isWalking = true;
            CC.Move(moveDir.normalized * vitesse * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }*/
    }

    private void DashFinishCheck()
    {
        if (OnDash)
        {
            float Distance = Vector3.Distance(Canon.transform.position, PointOrigine);
            Debug.Log(Distance);
            if (Distance>=DistanceDash)
            {
                ConteneurRigibody.mass = 1;
                ConteneurRigibody.velocity = ConteneurRigibody.velocity.normalized * vitesse;
                //ConteneurRigibody.constraints = RigidbodyConstraints.FreezeAll;
                JustFinishedDash = true;
                OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                Avatar.layer = 9;
                tag = "Player";
                ConteneurRigibody.useGravity = true;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                if (Aftershock)
                {
                    //explosion
                }
            }
            // Debug.Log(Vector3.Distance(HitPosition, new Vector3(transform.position.x, Canon.transform.position.y,transform.position.z)));
            // if (Vector3.Distance(HitPosition, new Vector3(transform.position.x, Canon.transform.position.y,transform.position.z)) 
            //     < 0.5f && HitPosition != new Vector3())
            // {
            //     ConteneurRigibody.mass = 1;
            //     ConteneurRigibody.velocity = ConteneurRigibody.velocity.normalized * vitesse;
            //     //ConteneurRigibody.constraints = RigidbodyConstraints.FreezeAll;
            //     JustFinishedDash = true;
            //     OnDash = false;
            //     GetComponent<CapsuleCollider>().enabled = !enabled;
            //     Avatar.layer = 9;
            //     tag = "Player";
            //     ConteneurRigibody.useGravity = true;
            //     ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //     if (Aftershock)
            //     {
            //         //explosion
            //     }
            // }
            // else if (GetComponent<Rigidbody>().velocity.magnitude < 3)
            // {
            //     ConteneurRigibody.mass = 1;
            //     ConteneurRigibody.velocity = ConteneurRigibody.velocity.normalized * vitesse;
            //     //ConteneurRigibody.constraints = RigidbodyConstraints.FreezeAll;
            //     JustFinishedDash = true;
            //     OnDash = false;
            //     GetComponent<CapsuleCollider>().enabled = !enabled;
            //     Avatar.layer = 9;
            //     tag = "Player";
            //     ConteneurRigibody.useGravity = true;
            //     ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //     if (Aftershock)
            //     {
            //         //explosion
            //     }
            // }
            //GetComponent<LineRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            transform.position = SpawnPositionPlayer.position;
        }

        if (other.gameObject.layer == 13 && other.GetComponent<RuantAI>() != null &&
            other.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            JustHit = true;
            ConteneurRigibody.freezeRotation = false;
            Vector3 dir = transform.position;
            dir = (dir - other.transform.position).normalized;
            //dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
            dir.y = 0;
            float RegulationForce = 150;
            ConteneurRigibody.velocity = Vector3.zero;
            Debug.Log(ConteneurRigibody.velocity);
            ConteneurRigibody.AddForceAtPosition(dir * other.GetComponent<Rigidbody>().velocity.magnitude
                                                 * RegulationForce,
                ConteneurRigibody.ClosestPointOnBounds(other.transform.position));
        }
    }
}
