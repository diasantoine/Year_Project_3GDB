using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyAI : MonoBehaviour
{

    public NavMeshAgent agent;

    [HideInInspector] public Transform player;

    [SerializeField] private bool Grounded = false;
    [SerializeField] private bool JustHit = false;
    private bool HitPlayer = false;

    public int DMG_Percentage = 1;

    private Rigidbody ConteneurRigibody;

    private float Cooldown = 2;

    private GameObject Player;

    [SerializeField] private int FieldOfView = 90;

    private bool Pansement = false;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
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
                }
                //agent.SetDestination(player.position);

            }
            else
            {
                if (ConteneurRigibody.velocity.magnitude < 0.2f)
                {
                    if (Pansement)
                    {
                        Pansement = false;
                    }
                    else
                    {
                        Debug.Log("aaa");
                        JustHit = false;
                        agent.enabled = true;
                        if (ConteneurRigibody.constraints == RigidbodyConstraints.None)
                        {
                            ConteneurRigibody.constraints = RigidbodyConstraints.FreezeRotation;
                            transform.rotation = Quaternion.identity;
                        }
                    }
                }
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
            &&!player.GetComponent<CharacterMovement>().JustFinishedDash)
        {
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity,LayerMask.GetMask("Player")))
            { 
                Debug.DrawRay(new Vector3(transform.position.x,1, transform.position.z)
                    , player.position-transform.position, Color.blue);
                if(hit.transform.GetChild(1).GetChild(0).CompareTag("Player") && Vector3.Distance(hit.transform.position, 
                    new Vector3(transform.position.x, 1, transform.position.z)) < 5f)
                {
                    Debug.Log("JeTape");
                    float Explosion = 10*GetComponent<ennemyState>().RandomMultiplicatorSize;
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
            if (collision.transform.GetComponent<CharacterMovement>().OnDash)
            {
                Debug.Log("aie");
            
                JustHit = true;
                agent.enabled = false;
                Vector3 dir = transform.position;
                dir = (dir - collision.transform.position).normalized;
                dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
                dir.y = 10;
                float RegulationForce = 2;
                Debug.Log("before" + ConteneurRigibody.velocity.magnitude);
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
                ConteneurRigibody.AddForceAtPosition(dir * collision.GetComponent<Rigidbody>().velocity.magnitude
                                                         * RegulationForce, 
                    ConteneurRigibody.ClosestPointOnBounds(collision.transform.position));
                Debug.Log(ConteneurRigibody.velocity.magnitude);
                Pansement = true;

                /*Debug.Log(collision.gameObject.name);
                int DashExplosion = 5;
                ConteneurRigibody.constraints = RigidbodyConstraints.None;
                ConteneurRigibody.AddExplosionForce(DashExplosion*200,transform.position,200,10,ForceMode.Force);
                JustHit = true;
                agent.enabled = false;
                //ConteneurRigibody.velocity = transform.up * DashExplosion;
                Vector3 dir = (collision.transform.position - transform.position).normalized;
                //ConteneurRigibody.AddForceAtPosition(dir * DashExplosion,
                //   ConteneurRigibody.ClosestPointOnBounds(collision.transform.position));
                //ConteneurRigibody.AddForce(transform.up*DashExplosion,ForceMode.Impulse);*/
            }
        }
    }

   // private void OnTriggerStay(Collider collision)
   //  {
   //      if (collision.gameObject.layer == 9 && !JustHit)
   //      {
   //          if (collision.transform.GetComponent<CharacterMovement>().OnDash)
   //          {
   //              Debug.Log("aie*2");
   //              JustHit = true;
   //              agent.enabled = false;
   //              Vector3 dir = (transform.position - collision.transform.position).normalized;
   //              int DashExplosion = 25;
   //              ConteneurRigibody.constraints = RigidbodyConstraints.None;
   //              ConteneurRigibody.AddForceAtPosition(dir * DashExplosion,
   //                  ConteneurRigibody.ClosestPointOnBounds(collision.transform.position), ForceMode.Impulse);
   //          }
   //      }
   //  }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
        if (collision.transform.CompareTag("Projectile"))
        {
            Debug.Log("?");
            JustHit = true;
            int Explosion = DMG_Percentage * 3;
            agent.enabled = false;
            DMG_Percentage = Explosion;
            ConteneurRigibody.constraints = RigidbodyConstraints.None;
            ConteneurRigibody.AddForceAtPosition(collision.transform.forward * Explosion, collision.GetContact(0).point);
        }

        if (collision.transform.CompareTag("Ennemy")
            && collision.gameObject.GetComponent<ennemyAI>().ConteneurRigibody.constraints == RigidbodyConstraints.None 
                && collision.gameObject.GetComponent<ennemyState>().RandomMultiplicatorSize >= GetComponent<ennemyState>().RandomMultiplicatorSize)
        {
            if (collision.gameObject.GetComponent<ennemyState>().RandomMultiplicatorSize > 2*GetComponent<ennemyState>().RandomMultiplicatorSize )
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
