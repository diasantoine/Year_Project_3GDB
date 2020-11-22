using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyAI : MonoBehaviour
{

    public NavMeshAgent agent;

    [HideInInspector] public Transform player;

    private bool Grounded = false;
    [SerializeField] private bool JustHit = false;
    private bool HitPlayer = false;

    public int DMG_Percentage = 1;

    private Rigidbody ConteneurRigibody;

    private float Cooldown = 2;


    // Start is called before the first frame update
    void Start()
    {
        
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
                        if (Physics.Raycast(transform.position, transform.forward,
                            out RaycastHit hit, 20, LayerMask.GetMask("Default")))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                Debug.Log("JeTape");
                                int Explosion = 2000;
                                //hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                hit.transform.GetComponent<Rigidbody>()
                                    .AddForceAtPosition(transform.forward * Explosion, hit.point);
                                this.HitPlayer = true;
                            }
                        }
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

                agent.SetDestination(player.position);

            }
            else
            {
                if (ConteneurRigibody.velocity.magnitude < 0.2f)
                {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
        if (collision.transform.CompareTag("Projectile"))
        {
            JustHit = true;
            int Explosion = DMG_Percentage * 3;
            agent.enabled = false;
            DMG_Percentage = Explosion;
            ConteneurRigibody.constraints = RigidbodyConstraints.None;
            ConteneurRigibody.AddForceAtPosition(transform.forward * Explosion, collision.GetContact(0).point);
        }

        if (collision.transform.CompareTag("Ennemy")
            && collision.gameObject.GetComponent<ennemyAI>().ConteneurRigibody.constraints == RigidbodyConstraints.None)
        {
            JustHit = true;
            agent.enabled = false;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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
