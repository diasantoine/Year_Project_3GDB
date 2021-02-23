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
    
    // Start is called before the first frame update
    void Start()
    {
        ScreamerState = State.SleepState;
    }

    // Update is called once per frame
    void Update()
    {
        VisionCone(player);
        
        if (SeeThePlayer)
        {
            ScreamerState = State.TriggerState;
        }
        else
        {
            ScreamerState = State.SleepState;
        }

        switch (ScreamerState)
        {
            case State.SleepState:
                agent.SetDestination(transform.position);
                SpeedConteneur = agent.speed;
                agent.speed = 0;
                break;
            case State.TriggerState:
                agent.speed = SpeedConteneur;
                agent.SetDestination(player.transform.position);
                if (Vector3.Distance(transform.position, player.transform.position) <= 2)
                {
                    if (Poisoned)
                    {
                        //coup
                    }
                    else
                    {
                        //anim puis explosion
                    }
                }
                break;
            case State.dead:
                if (Poisoned)
                {
                    //dead
                }
                else
                {
                    //dead+explosion
                }
                break;
            default:
                break;
        }
    }
}
