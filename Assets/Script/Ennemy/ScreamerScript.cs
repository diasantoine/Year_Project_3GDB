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
    private float Compteur = 0;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled && Grounded && !JustHit)
        {
            agent.enabled = true;
        }
        if (!Grounded && !JustHit)
        {
            Ground(hit);
            
        }else if(JustHit)
        {
            if (RB.velocity.magnitude < 3f )
            {
                if (Pansement)
                {
                    Pansement = false;
                }
                else
                {
                    JustHit = false;
                    agent.enabled = true;
                    RB.constraints = RigidbodyConstraints.FreezePositionY;
                    RB.freezeRotation = true;
                    transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
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
                break;
            case State.TriggerState:
                if (Vector3.Distance(transform.position, player.transform.position) <= 6)
                {
                    if (Poisoned)
                    {
                        agent.speed = 0;
                        agent.isStopped = true;
                        ScreamerState = State.dead;
                    }
                    else
                    {
                        agent.speed = 0;
                        //agent.enabled = false;
                        agent.isStopped = true;
                        ScreamerState = State.dead;
                        Debug.Log("?");
                    }
                }
                else
                {
                    Debug.Log("a");
                    agent.speed = SpeedConteneur;
                    transform.LookAt(player.transform.position);
                    agent.SetDestination(player.transform.position);
                }
                break;
            case State.dead:
                if (Poisoned)
                {
                    if (compteur >= 0)
                    {
                        compteur -= Time.deltaTime;
                    }
                    else
                    {
                        ImpulsionTahLesfous();
                        compteur = 1;
                    }
                }
                else
                {
                    if (compteur >= 0)
                    {
                        compteur -= Time.deltaTime;
                    }
                    else
                    {
                        ImpulsionTahLesfous();
                        compteur = 1;
                    }
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
        if (HpNow <= 0)
        {
            float écart = -numberCadav / 2;
            ScreamerState = State.dead;

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
        }
    }

    void ImpulsionTahLesfous()
    {
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].gameObject.CompareTag("Player"))
            { 
                hit[i].gameObject.transform.parent.GetComponent<CharacterMovement>().JustHit = true;
                hit[i].gameObject.transform.parent.GetComponent<CharacterMovement>().
                    ConteneurRigibody.AddForce(ForceExplosion*(hit[i].gameObject.transform.position - transform.position).normalized);
                /*.AddExplosionForce(ForceExplosion,hitPoint, 
               radiusExploBase + transform.localScale.x, 5f);*/
            }
            else if (hit[i].gameObject.CompareTag("Ennemy"))
            {
                hit[i].GetComponent<ScreamerScript>().ExplosionImpact(hitPoint, radiusExploBase + transform.localScale.x, ForceExplosion, DMG);
                //if(poisonned) { hit[i].GetComponent<ScreamerScript>().Poisonned = true;
            }
        }
        Destroy(gameObject);
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
            if (collision.transform.GetComponent<CharacterMovement>().OnDash && !JustHit)
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
                RB.velocity *= ImpactTirNormal;
            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                RB.velocity *= ImpactTirNormal;

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
