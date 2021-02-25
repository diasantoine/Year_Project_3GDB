using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool Explosion;
    [SerializeField] private float ForceExplosion;
    [SerializeField] private float radiusExploBase;
    [SerializeField] private float DMG;
    [SerializeField] private float compteur = 2;


    // Start is called before the first frame update
    void Start()
    {
        ScreamerState = State.SleepState;
        SpeedConteneur = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        VisionCone(player);

        if (ScreamerState != State.dead)
        {
            if (SeeThePlayer)
            {
                ScreamerState = State.TriggerState;
            }
            else 
            {
                ScreamerState = State.SleepState;
            }
        }
       

        switch (ScreamerState)
        {
            case State.SleepState:
                //agent.SetDestination(transform.position);
                agent.speed = 0;
                break;
            case State.TriggerState:
                if (Vector3.Distance(transform.position, player.transform.position) <= 4)
                {
                    agent.speed = 0;
                    if (Poisoned)
                    {
                        //coup
                    }
                    else
                    {
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
                    //dead
                }
                else
                {
                    if (compteur >= 0)
                    {
                        compteur -= Time.deltaTime;
                    }
                    else
                    {
                        Explosion = true;
                        ImpulsionTahLesfous();
                        compteur = 1;
                    }
                }
                break;
            default:
                break;
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
}
