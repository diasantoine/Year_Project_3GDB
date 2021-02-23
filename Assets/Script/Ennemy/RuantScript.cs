using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuantScript : Ennemy
{

    [SerializeField] private float distForRush;
    [SerializeField] private float waitRush;
    [SerializeField] private float speedRush;
    [SerializeField] private float deceleration;

    private float chrono;
    private float speedRushIni;

    private bool isRushing;
    private RaycastHit hit;

    private Vector3 rushPlace;

    public enum State
    {
        IDLE,
        WAIT,
        CHASE,
        RUSH,
        STUN,
        DEATH,

    };

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
        state = State.CHASE;
        speedRushIni = speedRush;
    }

    // Update is called once per frame
    void Update()
    {
        VisionCone(player);
        Ground(hit);
        OnUptadeState();
    }

    void OnEnterState()
    {
        switch (state)
        {
            case State.IDLE:
                if (Grounded)
                {
                    SwitchState(State.CHASE);

                }
                break;
            case State.WAIT:
                break;
            case State.CHASE:
                agent.enabled = true;
                agent.isStopped = false;
                break;
            case State.RUSH:
                speedRush = speedRushIni;
                break;
            case State.STUN:
                break;
            case State.DEATH:
                break;
            default:
                break;
        }
    }

    void OnUptadeState()
    {
        switch (state)
        {
            case State.IDLE:
                break;

            case State.WAIT:
                if (chrono >= waitRush)
                {
                    SwitchState(State.RUSH);
                    chrono = 0;

                }
                else
                {
                    chrono += Time.deltaTime;
                    transform.LookAt(player);
                }
                break;

            case State.CHASE:
                Vector3 dist = transform.position - player.position;
                Debug.Log(dist.magnitude);
                if(distForRush < dist.magnitude)
                {
                    agent.SetDestination(player.position);

                }
                else
                {
                    if (SeeThePlayer)
                    {
                        SwitchState(State.WAIT);

                    }
                }
                break;

            case State.RUSH:
                if (Grounded)
                {
                    if (isRushing)
                    {
                        Vector3 place = rushPlace - transform.position;
                        RB.AddForce(place.normalized * speedRush, ForceMode.Acceleration);

                        if (place.magnitude < 1f)
                        {
                            isRushing = false;
                        }

                    }
                    else
                    {
                        RB.velocity = transform.forward * speedRush;
                        speedRush -= deceleration * Time.deltaTime;

                        if (speedRush <= 0)
                        {
                            SwitchState(State.IDLE);
                        }
                    }
                }
                break;

            case State.STUN:
                break;

            case State.DEATH:
                break;

            default:
                break;
        }
    }

    void OnExitState()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.WAIT:
                rushPlace = player.position;
                isRushing = true;
                break;
            case State.CHASE:
                agent.isStopped = true;
                agent.enabled = false;
                break;
            case State.RUSH:
                break;
            case State.STUN:
                break;
            case State.DEATH:
                break;
            default:
                break;
        }
    }
}