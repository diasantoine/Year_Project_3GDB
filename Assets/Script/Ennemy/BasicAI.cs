using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : Ennemy
{

    [Header("Impact")]
    [SerializeField] private float ImpactTirNormal = 1;
    [SerializeField] private float ImpactHit;

    private bool hitPlayer;

    [Header("Time")]
    [SerializeField] private float hitCD;
    private float chronoHit;
    [SerializeField] private float TimeGetUp;
    private float chronoGetUp;


    private RaycastHit hit;
    private float debugChronoStart;
    private bool startNav;
    private GameObject skill;
    private bool Pansement = false;

    public enum State
    {
        IDLE,
        CHASE,
        DUMB,
        DEATH
    }

    [Header("State")]
    public State state;

    public void SwitchState(State newState)
    {
        OnExitState();
        state = newState;
        OnEnterState();

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;

        skill = GameObject.Find("Skill");
        //startNav = false;
        debugChronoStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ground(hit);
        VisionCone(player);
        OnUpdateState();

    }

    private void OnEnterState()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.CHASE:
                break;
            case State.DUMB:
                break;
            case State.DEATH:
                break;
        }
    }

    private void OnUpdateState()
    {
        switch (state)
        {
            case State.IDLE:
                if (Grounded)
                {
                    //DebugStart();
                    SwitchState(State.CHASE);
                   // Debug.Log("Start");

                }
                break;
            case State.CHASE:

                MoveAtPlayer();

                if (!hitPlayer)
                {
                    PatateDansLeJoueur();

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

                    if (chronoHit >= hitCD)
                    {
                        hitPlayer = false;
                        chronoHit = 0;
                    }
                    else
                    {
                        chronoHit += Time.deltaTime;
                    }
                }
                break;
            case State.DUMB:
                if (!JustHit)
                {
                    SwitchState(State.CHASE);
                }
                else
                {
                    if (chronoGetUp >= TimeGetUp)
                    {
                        if (RB.velocity.magnitude < 3f)
                        {
                            chronoGetUp = 0;
                            
                            if (Pansement)
                            {
                                Pansement = false;
                            }
                            else
                            {
                                JustHit = false;
                                agent.enabled = true;
                                RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                            }
                        }
                    }
                    else
                    {
                        chronoGetUp += Time.deltaTime;
                    }
                }
                break;
            case State.DEATH:
                break;
        }
    }

    private void OnExitState()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.CHASE:
                break;
            case State.DUMB:
                break;
            case State.DEATH:
                break;
        }
    }

    public void ExplosionImpact(Vector3 position, float radius, float explosionForce)
    {

        JustHit = true;
        agent.enabled = false;

        RB.freezeRotation = false;
        RB.AddForce(explosionForce * (transform.position - position).normalized);
        SwitchState(State.DUMB);
        //ConteneurRigibody.AddExplosionForce(explosionForce, position, radius, 5f, ForceMode.Impulse);
    }

    private void DebugStart()
    {
        if (!agent.enabled && !startNav)
        {
            if (debugChronoStart >= 0.1f)
            {
                agent.enabled = true;
                startNav = true;
                SwitchState(State.CHASE);
            }
            else
            {
                debugChronoStart += Time.deltaTime;
            }
        }
    }

    public void PatateDansLeJoueur()
    {
        if (SeeThePlayer)
        {
            float dis = Vector3.Distance(player.position, new Vector3(transform.position.x, 1, transform.position.z));
            if (dis < 5f)
            {
                if (AnimatorConteneur != null)
                {
                    AnimatorConteneur.SetBool("Taper", true);                   
                }

                player.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * (ImpactHit + (ImpactHit * player.GetComponent<The_Player_Script>().PercentageArmorHeat / 100)),
                    player.position, ForceMode.Impulse);
                hitPlayer = true;
                player.GetComponent<The_Player_Script>().JustHit = true;
                player.GetComponent<The_Player_Script>().PercentageArmorHeat += DmgArmorHeat;
            }
        }

    }
    private void MoveAtPlayer()
    {
        if (agent.isOnNavMesh && Grounded)
        {
            float dis = Vector3.Distance(player.position, transform.position);
            if (dis > 5f)
            {
                if (!agent.pathPending)
                {
                    agent.destination = player.position;
                    RB.velocity = agent.velocity;
                    //Debug.Log("Marche");

                    if (AnimatorConteneur != null)
                    {
                        if (!AnimatorConteneur.GetBool("Taper"))
                        {
                            AnimatorConteneur.SetBool("Hit", false);
                            AnimatorConteneur.SetBool("Marche", true);

                        }
                    }

                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.layer == 9)
        // {
        //     Debug.Log(other.name);
        //     if (other.transform.GetComponent<The_Player_Script>().OnDash && !JustHit)
        //     {
        //         //Debug.Log(transform.position - collision.transform.position);
        //
        //         JustHit = true;
        //         agent.enabled = false;
        //         RB.freezeRotation = false;
        //         Vector3 dir = transform.position;
        //         dir = (dir - other.transform.position).normalized;
        //         //dir = (dir + collision.GetComponent<Rigidbody>().velocity) / 2;
        //         dir.y = 0;
        //         float RegulationForce = 3;
        //         Transform ConteneurDashScript = null;
        //         foreach (Transform Child in skill.transform)
        //         {
        //             if (Child.name == "DashCharge")
        //             {
        //                 ConteneurDashScript = Child;
        //             }
        //         }
        //         //ConteneurRigibody.constraints = RigidbodyConstraints.None;
        //         RB.AddForceAtPosition(dir * ConteneurDashScript.GetComponent<ChargedDash>().DashSpeed * other.GetComponent<Rigidbody>().velocity.magnitude
        //                                                      * RegulationForce,
        //             RB.ClosestPointOnBounds(other.transform.position));
        //         Pansement = true;
        //
        //         SwitchState(State.DUMB);
        //     }
        // }
        // else 
        if (other.gameObject.layer == 13 && other.GetComponent<RuantAI>() != null && other.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            JustHit = true;
            agent.enabled = false;
            RB.freezeRotation = false;
            Vector3 dir = transform.position;
            dir = (dir - other.transform.position).normalized;
            dir.y = 0;
            float RegulationForce = 100;
            RB.AddForceAtPosition(dir * other.GetComponent<Rigidbody>().velocity.magnitude
                                                 * RegulationForce,
                RB.ClosestPointOnBounds(other.transform.position));
            Pansement = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            if (collision.transform.GetComponent<DeadProjo>().Empoisonnement)
            {
                JustHit = true;
                agent.enabled = false;
                RB.velocity *= ImpactTirNormal;
                SwitchState(State.DUMB);

            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                RB.velocity *= ImpactTirNormal;
                SwitchState(State.DUMB);


            }
            if (AnimatorConteneur != null)
            {
                AnimatorConteneur.SetBool("Hit", true);
            }

        }

        if (collision.transform.CompareTag("Ennemy") && collision.gameObject.GetComponent<ennemyAI>())
        {
            if (collision.gameObject.GetComponent<ennemyAI>().JustHit)
            {
                JustHit = true;
                agent.enabled = false;
                if (Pansement)
                {
                    GetComponent<Rigidbody>().velocity += collision.transform.GetComponent<Rigidbody>().velocity;
                }
            }
        }
    }
}
