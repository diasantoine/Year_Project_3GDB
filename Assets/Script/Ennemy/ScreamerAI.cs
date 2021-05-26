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

    public State state;
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

    public void SwitchState(State newState)
    {
        OnExitState();
        state = newState;
        OnEnterState();

    }


    void Start()
    {
        state = State.SleepState;
        SpeedConteneur = agent.speed;
        chrono = 0;
        player = GameObject.Find("Player").transform;
        this.TimeBeforeHeMoveContainer = this.TimeBeforeHeMove;
    }

    void OnExitState()
    {
        switch (state)
        {
            case State.SleepState:                
                break;
            case State.TriggerState:
                break;
            case State.dead:
                break;
        }
    }

    void OnEnterState()
    {
        switch (state)
        {
            case State.SleepState:
                break;
            case State.TriggerState:
                agent.speed = 9;
                break;
            case State.dead:
                if(agent != null)
                {
                    agent.speed = 0;
                    GetComponent<Collider>().enabled = false;
                    agent.isStopped = true;
                    agent.enabled = false;
                    RB.isKinematic = true;
                }
                if (!gameObject.GetComponent<ScreamerState>().isPoisoned)
                {
                    AnimatorConteneur.SetTrigger("Death");

                }
                else
                {
                    AnimatorConteneur.SetTrigger("DeathPoisoned");

                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateState();

        if (!agent.enabled && Grounded && !JustHit)
        {
            agent.enabled = true;
        }
        Ground(hit);
        if (JustHit)
        {
            if (RB.velocity.magnitude < 8f && state != State.dead)
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
                    SwitchState(State.TriggerState);
                }
            }
        }

        if (Grounded && !JustHit)
        {
            VisionCone(player);
        }

        if (state == State.SleepState)
        {
            if (SeeThePlayer)
            {
                SwitchState(State.TriggerState);
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

        if (transform.position.y <= -10)
        {
            this.GetComponent<ScreamerState>().Damage(Mathf.Infinity);
        }
    }

    private void UpdateState()
    {
        switch (state)
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
                            Debug.DrawLine(transform.position, this.ContainerNewPos, Color.red, 200f);
                            Debug.Log(this.ContainerNewPos);
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
                    if (agent.speed != 0)
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
                    if (!GetComponent<ScreamerState>().isPoisoned)
                    {
                        SwitchState(State.dead);

                    }
                }
                else
                {
                    if (!AnimatorConteneur.GetBool("TriggerState"))
                    {
                        AnimatorConteneur.SetBool("TriggerState", true);
                    }
                    agent.speed = 9;
                    transform.LookAt(player.transform.position);
                    agent.SetDestination(player.transform.position);
                }
                break;
            case State.dead:                
                break;
            default:
                break;
        }
    }

    public void DeathDebug()
    {
        SwitchState(State.dead);
    }

    public void BoomDeath()
    {
        if (AnimatorConteneur.gameObject.GetComponent<Data_SD_Screamer>())
        {
            AnimatorConteneur.gameObject.GetComponent<Data_SD_Screamer>().BoomBoomDeath();

        }
    }

    public void ImpulsionTahLesfous()
    {
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        CameraShake.Instance.Shake(5, 1.5f);
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
                if (hit[i].GetComponent<BasicAI>() != null)
                {
                    hit[i].GetComponent<BasicAI>().ExplosionImpact(hitPoint, radiusExploBase +  transform.localScale.x, ForceExplosion*10);
                    hit[i].GetComponent<BasicState>().Damage(DMG);
                }
                else if(hit[i].GetComponent<ScreamerState>() != null)
                {
                    hit[i].GetComponent<ScreamerState>().Damage(Mathf.Infinity);
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
                Debug.Log(this.FindValidPosition());
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
        bool isValid = NavMesh.SamplePosition (position + Vector3.up, out hit, 5,  1 << NavMesh.GetAreaFromName("Walkable"));
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
            BoomDeath();
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
