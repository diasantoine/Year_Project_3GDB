using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : Ennemy
{

    [Header("Impact")]
    [SerializeField] private float ImpactTirNormal = 1;
    [SerializeField] private float ImpactHit;
    [SerializeField] private float DistanceForBasicForPunch;

    private bool hitPlayer;

    [Header("Time")]
    [SerializeField] private float hitCD;
    private float chronoHit;
    [SerializeField] private float TimeGetUp;
    private float chronoGetUp;
    private float chronoDie;



    private RaycastHit hit;
    private float debugChronoStart;
    private bool startNav;
    private GameObject skill;
    private bool Pansement = false;
    [HideInInspector] public bool InPunch;

    public enum State
    {
        IDLE,
        CHASE,
        DUMB,
        TAPER,
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
        chronoDie = 0;
        skill = GameObject.Find("Skill");
        //startNav = false;
        debugChronoStart = 0;
        InPunch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null && state == State.CHASE)
        {
            player = GameObject.Find("Player").transform;

        }

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
                agent.enabled = true;
                break;
            case State.DUMB:
                AnimatorConteneur.SetBool("Taper", false);
                break;
            case State.TAPER:
                AnimatorConteneur.SetBool("Taper", true);
                break;
            case State.DEATH:
                GetComponent<Collider>().enabled = false;
                AnimatorConteneur.SetTrigger("Death");
                agent.enabled = false;
                RB.constraints = RigidbodyConstraints.None;
                RB.isKinematic = true;
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
                if (Grounded)
                {
                    MoveAtPlayer();

                }

                if (!hitPlayer)
                {
                    this.PunchInRange();
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
                                transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);

                            }
                        }
                    }
                    else
                    {
                        chronoGetUp += Time.deltaTime;
                    }
                }
                break;
            case State.TAPER:
                break;
            case State.DEATH:
                if(chronoDie >= 2f)
                {
                    GetComponent<BasicState>().Die();
                }
                else
                {
                    chronoDie += Time.deltaTime;
                }
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
            case State.TAPER:
                AnimatorConteneur.SetBool("Taper", false);
                break;
            case State.DEATH:
                break;
        }
    }

    public void ExplosionImpact(Vector3 position, float radius, float explosionForce)
    {
        JustHit = true;
        agent.enabled = false;
        Vector3 dis = transform.position - position;
        dis = dis.normalized;
        RB.freezeRotation = false;
        RB.AddForce(explosionForce * dis, ForceMode.Impulse);
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

    private void PunchInRange()
    {
        if (SeeThePlayer)
        {
            float dis = Vector3.Distance(player.position, new Vector3(transform.position.x, 1, transform.position.z));
            if (dis < DistanceForBasicForPunch)//5
            {
                if (AnimatorConteneur != null)
                {
                    SwitchState(State.TAPER);
                    this.InPunch = true;
                    this.agent.isStopped = true;
                }
            }
        }
    }

    public void PatateDansLeJoueur()
    {
        player.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * (ImpactHit + (ImpactHit * player.GetComponent<The_Player_Script>().PercentageArmorHeat / 100)),
            player.position, ForceMode.Impulse);
        hitPlayer = true;
        player.GetComponent<The_Player_Script>().JustHit = true;
        player.GetComponent<The_Player_Script>().PercentageArmorHeat += DmgArmorHeat;
        this.InPunch = false;
        AnimatorConteneur.SetBool("Taper", false);

    }

    private void MoveAtPlayer()
    {
        if (Grounded)
        {
            float dis = Vector3.Distance(player.position, transform.position);
            if (dis > 2.5f)
            {
                if (!agent.pathPending)
                {
                    agent.destination = player.position;
                    RB.velocity = agent.velocity;
                    
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
        if (other.gameObject.layer == 13 && other.GetComponent<RuantAI>() != null && other.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            SwitchState(State.DUMB);
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

        if (collision.transform.CompareTag("Ennemy") && collision.gameObject.GetComponent<BasicAI>())
        {
            if (collision.gameObject.GetComponent<BasicAI>().JustHit)
            {
                JustHit = true;
                agent.enabled = false;
                SwitchState(State.DUMB);
                if (Pansement)
                {
                    GetComponent<Rigidbody>().velocity += collision.transform.GetComponent<Rigidbody>().velocity;
                }
            }
        }
    }
}
