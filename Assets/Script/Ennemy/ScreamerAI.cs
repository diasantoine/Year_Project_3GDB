using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class ScreamerAI : Ennemy
{
    [Header("SoundScreamer")]
    [FMODUnity.EventRef]
    public string Screamer_Mort = "";

    [FMODUnity.EventRef]
    public string Screamer_Explosion = "";

    [FMODUnity.EventRef]
    public string Screamer_Touche = "";

    public enum State
    {
        SleepState,
        TriggerState,
        dead
    }

    public State ScreamerState;
    private float SpeedConteneur;
    public SpawnSysteme spawn;
    
    [Header("VarScreamer")]
    [SerializeField] private float ForceExplosion;
    [SerializeField] private float radiusExploBase;
    [SerializeField] private int DMG;
    [SerializeField] private float TimeBeforeHeMove;

    [Header("MoveScreamer")] 
    [SerializeField] private int RadiusMaxForHisMove;

    private float chrono;
    private float chronoTick;
    private bool touched;
    private bool HitPlayer = false;
    private float Cooldown = 2;
    private GameObject Skill;

    private float TimeBeforeHeMoveContainer;
    private int BreakWhile;
    private bool NewPosFind;
    private Vector3 ContainerNewPos;

    private bool Pansement = false;
    private bool Explosion;
    private bool startNav = false;

    private RaycastHit hit;
    
    void Start()
    {
        
        ScreamerState = State.SleepState;
        SpeedConteneur = agent.speed;
        chrono = 0;
        player = GameObject.Find("Player").transform;
        this.TimeBeforeHeMoveContainer = this.TimeBeforeHeMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled && Grounded && !JustHit)
        {
            agent.enabled = true;
        }
        Ground(hit);
        if(JustHit)
        {
            if (RB.velocity.magnitude < 8f  && ScreamerState != State.dead)
            {
                if (Pansement)
                {
                    Pansement = false;
                }
                else
                {
                    RB.isKinematic = false;
                    JustHit = false;
                    agent.enabled = true;
                    RB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                    RB.freezeRotation = true;
                    transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
                    ScreamerState = State.TriggerState;
                }
            }
        }

        if (Grounded && !JustHit)
        {
            VisionCone(player);
        }
        
        if (ScreamerState == State.SleepState)
        {
            if (SeeThePlayer)
            {
                ScreamerState = State.TriggerState;
                if (AnimatorConteneur.GetBool("Wander"))
                {
                    AnimatorConteneur.SetBool("Wander", false);
                }
                if (AnimatorConteneur.GetBool("SleepState"))
                {
                    AnimatorConteneur.SetBool("SleepState", false);
                }
            }
        }

        switch (ScreamerState)
        {
            case State.SleepState:
                if (this.TimeBeforeHeMove <= 0)
                {
                    if (this.NewPosFind)
                    {
                        if (Vector3.Distance(transform.position, this.ContainerNewPos) < 1f)
                        {
                            this.TimeBeforeHeMove = this.TimeBeforeHeMoveContainer;
                            this.NewPosFind = false;
                            this.agent.speed = 0;
                            if (AnimatorConteneur.GetBool("Wander"))
                            {
                                AnimatorConteneur.SetBool("Wander", false);
                            }
                            if (!AnimatorConteneur.GetBool("SleepState"))
                            {
                                AnimatorConteneur.SetBool("SleepState", true);
                            }
                        }
                    }
                    else
                    {
                        if (this.FindGoal())
                        {
                            Debug.Log("allo");
                            this.agent.SetDestination(this.ContainerNewPos);
                            this.agent.speed = this.SpeedConteneur;
                            if (!AnimatorConteneur.GetBool("Wander"))
                            {
                                AnimatorConteneur.SetBool("Wander", true);
                            }
                        }
                    }
                }
                else
                {
                    if (agent.speed !=0)
                    {
                        agent.speed = 0;
                    }

                    this.TimeBeforeHeMove -= Time.deltaTime;
                }

                if (!AnimatorConteneur.GetBool("SleepState"))
                {
                    AnimatorConteneur.SetBool("SleepState", true);
                }
                break;
            case State.TriggerState:
                if (Vector3.Distance(transform.position, player.transform.position) <= 6)
                {
                    if (this.GetComponent<ScreamerState>().isPoisoned)
                    {
                        agent.speed = 0;
                        RB.isKinematic = true;
                        agent.isStopped = true;
                        agent.enabled = false;
                        ScreamerState = State.dead;
                        
                    }
                    else
                    {
                        agent.speed = 0;
                        agent.isStopped = true;
                        agent.enabled = false;
                        RB.isKinematic = true;
                        ScreamerState = State.dead;
                    }
                }
                else
                {
                    if (!AnimatorConteneur.GetBool("TriggerState"))
                    {
                        AnimatorConteneur.SetBool("TriggerState", true);
                    }
                    agent.speed = SpeedConteneur;
                    transform.LookAt(player.transform.position);
                    agent.SetDestination(player.transform.position);
                }
                break;
            case State.dead:
                if (this.GetComponent<ScreamerState>().isPoisoned)
                {
                    if (!AnimatorConteneur.GetBool("PoisonedDeathState"))
                    {
                        AnimatorConteneur.SetBool("PoisonedDeathState", true);
                    }
                    if (AnimatorConteneur.GetBool("Dead"))
                    {
                        this.GetComponent<ScreamerState>().Damage(Mathf.Infinity);
                    }
                    else
                    {
                        Debug.Log("WaitDeath");
                    }
                }
                else
                {
                    if (!AnimatorConteneur.GetBool("DeathState"))
                    {
                        AnimatorConteneur.SetBool("DeathState", true);
                        // FMODUnity.RuntimeManager.PlayOneShot(Screamer_Explosion, transform.position);
                    }
                    if (AnimatorConteneur.GetBool("Dead"))
                    {
                        this.GetComponent<ScreamerState>().Damage(Mathf.Infinity);
                    }
                    else
                    {
                    }
                }
                break;
            default:
                break;
        }
        
        if (transform.position.y <= -10)
        {
            this.GetComponent<ScreamerState>().Damage(Mathf.Infinity);
        }
    }

    public void ImpulsionTahLesfous()
    {
        Destroy(gameObject);
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].gameObject.CompareTag("Player"))
            {
                float ForceExplosionWithArmorHeat = ForceExplosion + ForceExplosion * player.GetComponent<The_Player_Script>().PercentageArmorHeat / 100;
                
                player.GetComponent<The_Player_Script>().JustHit = true; player.GetComponent<The_Player_Script>().ListOfYourPlayer[
                    player.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
                
                player.GetComponent<The_Player_Script>().ListOfYourPlayer[player.GetComponent<The_Player_Script>().YourPlayerChoosed].
                    ConteneurRigibody.angularVelocity = Vector3.zero;
                
                player.GetComponent<The_Player_Script>().ListOfYourPlayer[player.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody.
                    AddForce(ForceExplosionWithArmorHeat*(player.transform.position - transform.position).normalized,ForceMode.Impulse);

                player.GetComponent<The_Player_Script>().PercentageArmorHeat += DmgArmorHeat;
            }
            else if (hit[i].gameObject.CompareTag("Ennemy"))
            {
                if (hit[i].GetComponent<ennemyAI>() != null)
                {
                    hit[i].GetComponent<ennemyAI>().ExplosionImpact(hitPoint, radiusExploBase +  transform.localScale.x, ForceExplosion*10);
                    hit[i].GetComponent<BasicState>().Damage(DMG);
                }
                else if(hit[i].GetComponent<ScreamerState>() != null)
                {
                    hit[i].GetComponent<ScreamerState>().Damage(Mathf.Infinity);
                }
                else
                {
                }
            }
        }
    }
    
    private bool FindGoal ()
    {
        while (!this.NewPosFind && this.BreakWhile < 15)
        {
            this.BreakWhile++;
            if (this.FindValidPosition())
            {
                this.agent.destination = this.ContainerNewPos;
                this.NewPosFind = true;
                this.BreakWhile = 0;
                return true;
            }
        }
        return false;
    }
    
    private bool FindValidPosition ()
    {
        Vector2 offset = Random.insideUnitCircle * this.RadiusMaxForHisMove;
        Vector3 position = this.transform.position + new Vector3 (offset.x, 0, offset.y);
        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition (position + Vector3.up, out hit, 5,  NavMesh.AllAreas);
        if (!isValid)
            return false;
        
        this.ContainerNewPos = new Vector3(hit.position.x, transform.position.y, hit.position.z);
        return true;
    }
  
    
     private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.transform.GetComponent<The_Player_Script>().OnDash && !JustHit)
            {
                JustHit = true;
                agent.enabled = false;
                RB.freezeRotation = false;
                Vector3 dir = transform.position;
                dir = (dir - collision.transform.position).normalized;
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
                RB.AddForceAtPosition(dir * ConteneurDashScript.GetComponent<ChargedDash>().DashSpeed  * collision.GetComponent<Rigidbody>().velocity.magnitude
                                                             * RegulationForce, 
                    RB.ClosestPointOnBounds(collision.transform.position));
                Pansement = true;
            }
        }

        if (collision.gameObject.layer == 13 && collision.GetComponent<RuantAI>() != null &&
            collision.GetComponent<RuantAI>().state == RuantAI.State.RUSH)
        {
            GetComponent<ScreamerState>().Damage(Mathf.Infinity);
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
                RB.isKinematic = true;
                //RB.velocity *= ImpactTirNormal;
            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                RB.isKinematic = true;
                // RB.velocity *= ImpactTirNormal;

            }
            // if(AnimatorConteneur != null)
            // {
            //     AnimatorConteneur.SetBool("Hit", true);
            //     AnimatorConteneur.SetBool("Marche", false);
            // }
        }
        if (collision.transform.CompareTag("Ennemy"))// && collision.gameObject.GetComponent<ScreamerScript>().JustHit)
        {
            if (collision.transform.GetComponent<ScreamerAI>()!=null)
            {
                if (collision.transform.GetComponent<ScreamerAI>().JustHit)
                {
                    JustHit = true;
                    agent.enabled = false;
                    if (Pansement)
                    {
                        GetComponent<Rigidbody>().velocity += collision.transform.GetComponent<Rigidbody>().velocity;
                    }
                }
            }
            else if(collision.transform.GetComponent<ennemyAI>()!=null)
            {
                if (collision.transform.GetComponent<ennemyAI>().JustHit)
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
}
