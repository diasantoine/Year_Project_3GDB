using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreamerScript : Ennemy
{
    public enum State
    {
        SleepState,
        TriggerState,
        dead
    }

    public State ScreamerState;
    private float SpeedConteneur;
    public bool Poisoned;
    public SpawnSysteme spawn;
    
    [SerializeField] private float ForceExplosion;
    [SerializeField] private float radiusExploBase;
    [SerializeField] private int DMG;
    [SerializeField] private float compteur = 2;
    [SerializeField] private float freqTick;
    [SerializeField] private int numberCadav;
    [SerializeField] private bool Fall = false;
    [SerializeField] private GameObject preDead;
    [SerializeField] private float timeBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarSec;
    [SerializeField] private float hpMax;
    
    private float dpsTick;
    private float chrono;
    private float chronoTick;
    private bool touched;
    private float EmpoisonnementTick = 0;
    private float HpNow = 0;

    private bool HitPlayer = false;

    [SerializeField] private float ImpactTirNormal = 1;

    private float Cooldown = 2;

    private GameObject Skill;


    private bool Pansement = false;
    private bool Explosion;
    private bool startNav = false;

    private RaycastHit hit;
    
    void Start()
    {
        
        ScreamerState = State.SleepState;
        SpeedConteneur = agent.speed;
        chrono = 0;
        HpNow = hpMax;

        healthBar.maxValue = hpMax;
        healthBar.value = healthBar.maxValue;

        healthBarSec.maxValue = healthBar.maxValue;
        healthBarSec.value = healthBar.maxValue;

        //numberCadav = Random.Range(1, 4);

        EmpoisonnementTick = 0;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Grounded);
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
            }
        }

        switch (ScreamerState)
        {
            case State.SleepState:
                if (agent.speed !=0)
                {
                    agent.speed = 0;
                }

                if (!AnimatorConteneur.GetBool("SleepState"))
                {
                    AnimatorConteneur.SetBool("SleepState", true);
                }
                break;
            case State.TriggerState:
                if (Vector3.Distance(transform.position, player.transform.position) <= 6)
                {
                    if (Poisoned)
                    {
                        agent.speed = 0;
                        RB.isKinematic = true;
                        agent.enabled = false;
                        agent.isStopped = true;
                        ScreamerState = State.dead;
                        
                    }
                    else
                    {
                        agent.speed = 0;
                        agent.enabled = false;
                        RB.isKinematic = true;
                        agent.isStopped = true;
                        ScreamerState = State.dead;
                    }
                }
                else
                {
                    if (!AnimatorConteneur.GetBool("TriggerState"))
                    {
                        AnimatorConteneur.SetBool("TriggerState", true);
                    }
                    Debug.Log("a");
                    agent.speed = SpeedConteneur;
                    transform.LookAt(player.transform.position);
                    agent.SetDestination(player.transform.position);
                }
                break;
            case State.dead:
                if (Poisoned)
                {
                    if (!AnimatorConteneur.GetBool("PoisonedDeathState"))
                    {
                        AnimatorConteneur.SetBool("PoisonedDeathState", true);
                    }
                    if (AnimatorConteneur.GetBool("Dead"))
                    {
                        HpNow = 0;
                    }
                    else
                    {
                        Debug.Log("WaitDeath");
                    }
                    // if (compteur >= 0)
                    // {
                    //     compteur -= Time.deltaTime;
                    // }
                    // else
                    // {
                    //     ImpulsionTahLesfous();
                    //     compteur = 1;
                    // }
                }
                else
                {
                    if (!AnimatorConteneur.GetBool("DeathState"))
                    {
                        AnimatorConteneur.SetBool("DeathState", true);
                    }
                    if (AnimatorConteneur.GetBool("Dead"))
                    {
                        HpNow = 0;
                    }
                    else
                    {
                        Debug.Log("WaitDeath");
                    }
                    // if (compteur >= 0)
                    // {
                    //     compteur -= Time.deltaTime;
                    // }
                    // else
                    // {
                    //     ImpulsionTahLesfous();
                    //     compteur = 1;
                    // }
                }
                break;
            default:
                break;
        }
        
        if (Poisoned)
        {
            if (EmpoisonnementTick >= freqTick)
            {

                damage(dpsTick);
                //Poisoned = false;
                EmpoisonnementTick = 0;
            }
            else
            {
                EmpoisonnementTick += Time.deltaTime;
            }
        }
        HealthbarDecrease();

        if (transform.position.y <= -10)
        {
           HpNow = 0;
           Fall = true;
        }
        if (HpNow <= 0 && !Explosion)
        {
            Explosion = true;
            float écart = -numberCadav / 2;

            if (spawn.ListEnnemy.Contains(this.gameObject))
            {
                spawn.ListEnnemy.Remove(this.gameObject);
            }

            compteur = -1;

            for (int i = 1; i <= numberCadav; i++)
            {
                if (Fall)
                {
                    Instantiate(preDead, player.position,Quaternion.identity, GameObject.Find("CadavreParent").transform);
                    Debug.Log(detectDead.ressourceInt);
                }
                else
                {
                    Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f), 
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                écart++;
            }
            ImpulsionTahLesfous();
        }
    }

    void ImpulsionTahLesfous()
    {
        Destroy(gameObject);
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].gameObject.CompareTag("Player"))
            { 
                hit[i].transform.parent.GetComponent<The_Player_Script>().JustHit = true;
                hit[i].transform.parent.GetComponent<The_Player_Script>().ListOfYourPlayer[
                    hit[i].GetComponent<The_Player_Script>().YourPlayerChoosed].
                    ConteneurRigibody.velocity = Vector3.zero;
                hit[i].transform.parent.GetComponent<The_Player_Script>().ListOfYourPlayer[
                        hit[i].GetComponent<The_Player_Script>().YourPlayerChoosed].
                    ConteneurRigibody.
                    AddForce(ForceExplosion*(hit[i].gameObject.transform.position - transform.position).normalized);
                /*.AddExplosionForce(ForceExplosion,hitPoint, 
               radiusExploBase + transform.localScale.x, 5f);*/
            }
            else if (hit[i].gameObject.CompareTag("Ennemy"))
            {
                if (hit[i].GetComponent<ennemyAI>() != null)
                {
                    hit[i].GetComponent<ennemyAI>().ExplosionImpact(hitPoint, radiusExploBase +  transform.localScale.x, ForceExplosion/4);
                    hit[i].GetComponent<ennemyState>().damage(DMG);
                }
                else if(hit[i].GetComponent<ScreamerScript>() != null)
                {
                    hit[i].GetComponent<ScreamerScript>().HpNow = 0;
                }
                else
                {
                    //rien
                }
                //hit[i].GetComponent<ScreamerScript>().ExplosionImpact(hitPoint, radiusExploBase + transform.localScale.x, ForceExplosion, DMG);
                //if(poisonned) { hit[i].GetComponent<ScreamerScript>().Poisonned = true;
            }
        }
    }
    public void damage(float hit)
    {
        HpNow -= hit;
        healthBar.value = HpNow;
        touched = true;
        chrono = 0;
    }
    
    public void ExplosionImpact(Vector3 position, float radius, float explosionForce, int DMG)
    {

        JustHit = true;
        agent.enabled = false;
        damage(DMG);

        RB.freezeRotation = false;
        RB.AddExplosionForce(explosionForce, position, radius, 5f, ForceMode.Impulse);
    }
    
    void HealthbarDecrease()
    {
        if (touched)
        {
            if (chrono >= timeBar)
            {
                healthBarSec.value -= 1.5f * Time.deltaTime;
    
                if (healthBarSec.value <= healthBar.value)
                {
                    chrono = 0;
                    touched = false;
                }
            }
            else
            {
                chrono += Time.deltaTime;
            }
        }
    
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
            HpNow = 0;
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
            if(AnimatorConteneur != null)
            {
                AnimatorConteneur.SetBool("Hit", true);
                AnimatorConteneur.SetBool("Marche", false);
            }
        }
        if (collision.transform.CompareTag("Ennemy") && collision.gameObject.GetComponent<ScreamerScript>().JustHit)
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
