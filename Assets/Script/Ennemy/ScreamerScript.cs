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
    [SerializeField] private float ForceExplosion;
    [SerializeField] private float radiusExploBase;
    [SerializeField] private float DMG;
    [SerializeField] private float compteur = 2;
    [SerializeField] private float freqTick;
    [SerializeField] private int numberCadav;
    [SerializeField] private bool Fall = false;
    [SerializeField] private GameObject preDead;
    [SerializeField] private float timeBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarSec;
    [SerializeField] private float hpMax;
    private Rigidbody ConteneurRigibody;
    private float dpsTick;
    private float chrono;
    private float chronoTick;
    private bool touched;
    private float EmpoisonnementTick = 0;
    private float HpNow = 0;

    // Start is called before the first frame update
    void Start()
    {
        ScreamerState = State.SleepState;
        SpeedConteneur = agent.speed;
        ConteneurRigibody = GetComponent<Rigidbody>();
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
        VisionCone(player);
        
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
                //agent.SetDestination(transform.position);
                if (agent.speed !=0)
                {
                    agent.speed = 0;
                }
                break;
            case State.TriggerState:
                if (Vector3.Distance(transform.position, player.transform.position) <= 4)
                {
                    if (Poisoned)
                    {
                        //coup
                    }
                    else
                    {
                        agent.speed = 0;
                        ScreamerState = State.dead;
                    }
                }
                else
                {
                    agent.speed = SpeedConteneur;
                    agent.SetDestination(player.transform.position);
                }
                break;
            case State.dead:
                if (Poisoned)
                {
                   Destroy(gameObject);
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

            Destroy(gameObject);
            
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
            else if (hit[i].GetComponent<ennemyAI>() != null)
            {
                hit[i].GetComponent<ennemyAI>().ExplosionImpact(hitPoint, radiusExploBase + transform.localScale.x, ForceExplosion);
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
}
