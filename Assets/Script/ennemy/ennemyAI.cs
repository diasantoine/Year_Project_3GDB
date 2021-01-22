using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyAI : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    [SerializeField] private bool Grounded = false;
    [SerializeField] private bool JustHit = false;
    private bool HitPlayer = false;

    [SerializeField] private float ImpactTirNormal = 1;

    private Rigidbody ConteneurRigibody;

    private float Cooldown = 2;

    private GameObject Player;
    private GameObject Skill;

    [SerializeField] private int FieldOfView = 90;

    private bool Pansement = false;
    private float Compteur = 0;
    private bool ActivationCompteur = false;

    [SerializeField] private Animator AnimatorConteneur;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Skill = GameObject.Find("Skill");
        ConteneurRigibody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Grounded)
        {
            if (!JustHit)
            {
                if (!HitPlayer)
                {
                    VisionCone();
                    /*Debug.DrawRay(new Vector3(transform.position.x,1, transform.position.z)
                        , player.position-transform.position, Color.blue);
                        if (Physics.Raycast(new Vector3(transform.position.x,1, transform.position.z), 
                            player.position-transform.position,
                            out RaycastHit hit, 4, LayerMask.GetMask("Player")))
                        {
                            Debug.Log(hit.transform.name);
                            if (hit.transform.GetChild(1).CompareTag("Player"))
                            {
                                Debug.Log("JeTape");
                                float Explosion = 200*GetComponent<ennemyState>().RandomMultiplicatorSize;
                                //hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                hit.transform.GetComponent<Rigidbody>()
                                    .AddForceAtPosition(transform.forward * Explosion, hit.point);
                                this.HitPlayer = true;
                            }
                        }*/
                }
                else
                {


                    if (AnimatorConteneur != null)
                    {
                        if (AnimatorConteneur.GetBool("Taper"))
                        {
                            AnimatorConteneur.SetBool("Taper", false);

                        }
                    }

                    if (Cooldown <= 0)
                    {
                        HitPlayer = false;
                        Cooldown = 2;
                    }
                    else
                    {
                        Cooldown -= Time.deltaTime;
                    }



                }
                if (agent.isOnNavMesh && Vector3.Distance(player.position, transform.position)>5)
                {
                    agent.SetDestination(player.position);
                    ConteneurRigibody.velocity = agent.velocity;// attention

                    if (AnimatorConteneur != null)
                    {
                        if (!AnimatorConteneur.GetBool("Taper"))
                        {
                            AnimatorConteneur.SetBool("Marche", true);

                        }
                    }
                    
                    
                }
                //agent.SetDestination(player.position);

            }
            else
            {
                if(AnimatorConteneur != null)
                {
                    AnimatorConteneur.SetBool("Marche", false);

                }

                if (ConteneurRigibody.velocity.magnitude < 2f )
                {
                    if (Pansement)
                    {
                        Pansement = false;
                    }
                    else
                    {
                        JustHit = false;
                        agent.enabled = true;
                        //transform.rotation = Quaternion.identity;
                        transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
                        // if (ConteneurRigibody.constraints == (RigidbodyConstraints.FreezeRotationX & RigidbodyConstraints.FreezeRotationZ))
                        // {
                        //     ConteneurRigibody.constraints = RigidbodyConstraints.FreezeRotation;
                        //     
                        //     //ConteneurRigibody.velocity = agent.velocity;
                        // }
                    }
                }
            }

        }
        else
        {
            if (Compteur == 0)
            {
                Compteur = 1;
            }
            else
            {
                Compteur += Time.deltaTime;
            }

            if (Compteur >= 4)
            {
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
            }
        }
               
    }
    
    public void ExplosionImpact(Vector3 position, float radius, float explosionForce)
    {
        JustHit = true;
        agent.enabled = false;

        ConteneurRigibody.constraints = RigidbodyConstraints.None;
        ConteneurRigibody.AddExplosionForce(explosionForce, position, radius, 5f, ForceMode.Impulse);
    }
        
    private void VisionCone()
    {
        var rayDirection = this.player.transform.position - transform.position;
        if (Vector3.Angle(rayDirection, transform.forward) < this.FieldOfView && 
            Vector3.Distance(transform.position, player.transform.position) < 30f && !player.GetComponent<CharacterMovement>().OnDash
            &&!player.GetComponent<CharacterMovement>().JustFinishedDash && !player.GetComponent<CharacterMovement>().OnShieldProtection
            && player.GetComponent<CharacterMovement>().Grounded)
        {

            Debug.Log("prout");
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity,LayerMask.GetMask("Player")))
            { 
                Debug.DrawRay(new Vector3(transform.position.x,1, transform.position.z)
                    , player.position-transform.position, Color.blue);
                if(hit.transform.name == "Player" && Vector3.Distance(hit.transform.position, new Vector3(transform.position.x, 1, transform.position.z)) < 5f)
                {
                    Debug.Log("JeTape");
                    if(AnimatorConteneur != null)
                    {
                        AnimatorConteneur.SetBool("Taper", true);
                        AnimatorConteneur.SetBool("Marcher", false);
                    }
                    float Explosion = 10*GetComponent<ennemyState>().DMG_Percentage;
                   // hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.transform.GetComponent<Rigidbody>()
                        .AddForceAtPosition(transform.forward * Explosion, hit.point, ForceMode.Impulse);
                    this.HitPlayer = true;
                    player.GetComponent<CharacterMovement>().JustHit = true;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.transform.GetComponent<CharacterMovement>().OnDash && !JustHit)
            {
                Debug.Log(transform.position - collision.transform.position);
            
                JustHit = true;
                agent.enabled = false;
                Vector3 dir = transform.position;
                dir = (dir - collision.transform.position).normalized;
                //dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
                dir.y = 0;
                float RegulationForce = 3;
                Transform ConteneurDashScript = null;
                foreach (Transform Child in Skill.transform)
                {
                    if (Child.name == "DashCharge")
                    {
                        ConteneurDashScript = Child;
                    }
                }
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
                ConteneurRigibody.AddForceAtPosition(dir * ConteneurDashScript.GetComponent<ChargedDash>().DashSpeed  * collision.GetComponent<Rigidbody>().velocity.magnitude
                                                             * RegulationForce, 
                    ConteneurRigibody.ClosestPointOnBounds(collision.transform.position));
                Pansement = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
        if (collision.transform.CompareTag("Projectile"))
        {
            if (collision.transform.GetComponent<DeadProjo>().Empoisonnement)
            {
                JustHit = true;
                agent.enabled = false;
                ConteneurRigibody.velocity *= ImpactTirNormal;
            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                ConteneurRigibody.velocity *= ImpactTirNormal;
            }
            //ConteneurRigibody.AddForceAtPosition(collision.transform.forward.normalized * ForceTirNormal, collision.GetContact(0).point);
        }
        if (collision.transform.CompareTag("Ennemy")
            && collision.gameObject.GetComponent<ennemyAI>().ConteneurRigibody.constraints == RigidbodyConstraints.None 
                && collision.gameObject.GetComponent<ennemyState>().Size >= GetComponent<ennemyState>().Size)
        {
            if (collision.gameObject.GetComponent<ennemyState>().Size > 2*GetComponent<ennemyState>().Size)
            {
                JustHit = true;
                agent.enabled = false;
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<ennemyState>().damage(99999);
            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; 
            }
            /*transform.GetComponent<Rigidbody>()
                .AddForceAtPosition(transform.forward * collision.gameObject.GetComponent<ennemyState>().DMG_Percentage
                    , collision.GetContact(0).point);*/
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && Grounded)
        {
            Grounded = false;
        }
    }


}
