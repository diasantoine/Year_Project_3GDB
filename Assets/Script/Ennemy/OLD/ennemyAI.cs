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

    private GameObject Skill;

    [SerializeField] private int FieldOfView = 90;

    private bool Pansement = false;
    private float Compteur = 0;
    private bool startNav = false;

    [SerializeField] private Animator AnimatorConteneur;

    private RaycastHit hit;

    [SerializeField] private float portée;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;

        agent.enabled = false;
        Skill = GameObject.Find("Skill");
        ConteneurRigibody = GetComponent<Rigidbody>();
        Compteur = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!agent.enabled && !startNav)
        {
            if(Compteur >= 0.1f)
            {
                agent.enabled = true;
                startNav = true;
            }
            else
            {
                Compteur += Time.deltaTime;
            }
        }

        Debug.DrawRay(transform.position, -Vector3.up * portée);

        if(Physics.Raycast(transform.position, -Vector3.up, out hit , portée, LayerMask.GetMask("Sol", "Wall")))
        {
            if (hit.collider.CompareTag("sol") || hit.collider.CompareTag("Mur"))
            {
                
                Grounded = true;              
            }
        }
        else
        {
            Grounded = false;
            agent.enabled = false;
            ConteneurRigibody.constraints = RigidbodyConstraints.None;
            Debug.Log(Grounded);

        }

        if (Grounded)
        {
            if (!JustHit)
            {
                if (!HitPlayer)
                {
                    if(player.name == "Player")
                    {
                        VisionCone();

                    }
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

                    /*if(player.name == "Poteau")
                    {
                        var dir = player.position - gameObject.transform.position;
                        Debug.Log(dir.magnitude);
                        if (dir.magnitude < 5.2)
                        {
                            agent.enabled = false;
                        }
                    }*/
                 

                    if (AnimatorConteneur != null)
                    {
                        if (!AnimatorConteneur.GetBool("Taper"))
                        {
                            AnimatorConteneur.SetBool("Hit", false);
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

                if (ConteneurRigibody.velocity.magnitude < 3f )
                {
                    if (Pansement)
                    {
                        Pansement = false;
                    }
                    else
                    {
                        JustHit = false;
                        agent.enabled = true;
                        ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY;
                        ConteneurRigibody.freezeRotation = true;
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

            /*if(Compteur >= 4)
            {
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
                Compteur = 0;

            }
            else
            {
                Compteur += Time.deltaTime;
            }*/
        }
               
    }
    
    public void ExplosionImpact(Vector3 position, float radius, float explosionForce)
    {

        JustHit = true;
        agent.enabled = false;

        ConteneurRigibody.freezeRotation = false;
        ConteneurRigibody.AddForce(explosionForce*(transform.position - position).normalized);
        //ConteneurRigibody.AddExplosionForce(explosionForce, position, radius, 5f, ForceMode.Impulse);
    }
        
    private void VisionCone()
    {
        var rayDirection = this.player.transform.position - transform.position;
        if (Vector3.Angle(rayDirection, transform.forward) < this.FieldOfView && 
            Vector3.Distance(transform.position, player.transform.position) < 30f && !player.GetComponent<The_Player_Script>().OnDash
            &&!player.GetComponent<The_Player_Script>().JustFinishedDash && !player.GetComponent<The_Player_Script>().OnShieldProtection
            && player.GetComponent<The_Player_Script>().Grounded)
        {
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
                        AnimatorConteneur.SetBool("Marche", false);
                    }
                    float Explosion = 10*GetComponent<ennemyState>().DMG_Percentage;
                   // hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.transform.GetComponent<Rigidbody>()
                        .AddForceAtPosition(transform.forward * 
                                            (Explosion + (Explosion * 
                                                hit.transform.GetComponent<The_Player_Script>().PercentageArmorHeat / 100))
                            , hit.point, ForceMode.Impulse);
                    this.HitPlayer = true;
                    player.GetComponent<The_Player_Script>().JustHit = true;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.transform.GetComponent<The_Player_Script>().OnDash && !JustHit)
            {
                //Debug.Log(transform.position - collision.transform.position);
            
                JustHit = true;
                agent.enabled = false;
                ConteneurRigibody.freezeRotation = false;
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
                //ConteneurRigibody.constraints = RigidbodyConstraints.None;
                ConteneurRigibody.AddForceAtPosition(dir * ConteneurDashScript.GetComponent<ChargedDash>().DashSpeed  * collision.GetComponent<Rigidbody>().velocity.magnitude
                                                             * RegulationForce, 
                    ConteneurRigibody.ClosestPointOnBounds(collision.transform.position));
                Pansement = true;
            }
        }else if (collision.gameObject.layer == 13 && collision.GetComponent<RuantAI>() != null &&
                   collision.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            JustHit = true;
            agent.enabled = false;
            ConteneurRigibody.freezeRotation = false;
            Vector3 dir = transform.position;
            dir = (dir - collision.transform.position).normalized;
            dir.y = 0;
            float RegulationForce = 100;
            ConteneurRigibody.AddForceAtPosition(dir * collision.GetComponent<Rigidbody>().velocity.magnitude
                                                 * RegulationForce, 
                ConteneurRigibody.ClosestPointOnBounds(collision.transform.position));
            Pansement = true;
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
            if(AnimatorConteneur != null)
            {
                AnimatorConteneur.SetBool("Hit", true);
                AnimatorConteneur.SetBool("Marche", false);
            }
            //ConteneurRigibody.AddForceAtPosition(collision.transform.forward.normalized * ForceTirNormal, collision.GetContact(0).point);
        }
        if (collision.transform.CompareTag("Ennemy") && collision.gameObject.GetComponent<ennemyAI>().JustHit)
        {
            JustHit = true;
            agent.enabled = false;
            if (Pansement)
            {
                GetComponent<Rigidbody>().velocity += collision.transform.GetComponent<Rigidbody>().velocity;
            }
            //transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; 
            // if (collision.gameObject.GetComponent<ennemyState>().Size > 2GetComponent<ennemyState>().Size)
            // {
            //     JustHit = true;
            //     agent.enabled = false;
            //     transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //     GetComponent<ennemyState>().damage(99999);
            // }
            // else
            // {
            //     JustHit = true;
            //     agent.enabled = false;
            //     transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; 
            // }
            /*transform.GetComponent<Rigidbody>()
                .AddForceAtPosition(transform.forward * collision.gameObject.GetComponent<ennemyState>().DMG_Percentage
                    , collision.GetContact(0).point);*/
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
    }*/

    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && Grounded)
        {
            Grounded = false;           
        }
    }*/


}
