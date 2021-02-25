using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuantAI : Ennemy
{

    [SerializeField] private GameObject preExplo;

    [SerializeField] private float distForRush;
    [SerializeField] private float waitRush;
    [SerializeField] private float stunTime;
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
        SPAWN,
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
        state = State.SPAWN;
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
                    RB.isKinematic = true;

                }
                break;
            case State.SPAWN:
                break;
            case State.WAIT:
                break;
            case State.CHASE:
                agent.enabled = true;
                agent.isStopped = false;
                break;
            case State.RUSH:
                speedRush = speedRushIni;
                RB.isKinematic = false;
                break;
            case State.STUN:
                SeeThePlayer = false;
                chrono = 0;
                break;
            case State.DEATH:
                chrono = 0;
                agent.enabled = false;
                RB.constraints = RigidbodyConstraints.None;
                RB.isKinematic = true;
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

                if(chrono >= 0.5f)
                {
                    SwitchState(State.CHASE);
                    chrono = 0;
                }
                else
                {
                    chrono += Time.deltaTime;
                }

                break;

            case State.SPAWN:

                if (Grounded)
                {
                    GameObject newExplo = Instantiate(preExplo, transform.position, Quaternion.identity);
                    Destroy(newExplo, 0.2f);
                    SwitchState(State.IDLE);
                    
                }

                break;

            case State.WAIT:
                if (chrono >= waitRush)
                {
                    SwitchState(State.RUSH);

                }
                else
                {
                    chrono += Time.deltaTime;
                    transform.LookAt(player);
                }
                break;

            case State.CHASE:
                Vector3 dist = transform.position - player.position;
                agent.SetDestination(player.position);

                if (SeeThePlayer)
                {
                    SwitchState(State.WAIT);

                }
                break;

            case State.RUSH:
                if (Grounded)
                {
                    if (isRushing)
                    {
                        Vector3 place = rushPlace - transform.position;
                        RB.AddForce(place.normalized * speedRush, ForceMode.Impulse);

                        if (place.magnitude < 0.5f)
                        {
                            isRushing = false;
                        }

                    }
                    else
                    {
                        RB.drag = deceleration;

                        if (RB.velocity.magnitude < 1)
                        {
                            SwitchState(State.IDLE);

                        }
                    }
                }
                break;

            case State.STUN:
                if(chrono >= stunTime)
                {                   
                    SwitchState(State.IDLE);
                    chrono = 0;
                }
                else
                {
                    chrono += Time.deltaTime;

                    if(chrono < 0.5f)
                    {
                        transform.position += -transform.forward * 0.75f * Time.deltaTime;
                    }
                }
                break;

            case State.DEATH:
                transform.Rotate(-35f * Time.deltaTime, 0, 0);

                if(chrono >= 2.5f)
                {
                    GetComponent<RuantState>().Die();
                    GameObject exploFee = Instantiate(preExplo, transform.position, Quaternion.identity);
                    Destroy(exploFee, 0.25f);
                }
                else
                {
                    chrono += Time.deltaTime;
                    Debug.Log(chrono);
                }
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

            case State.SPAWN:
                break;

            case State.WAIT:
                rushPlace = player.position;
                chrono = 0;
                isRushing = true;
                break;
            case State.CHASE:
                agent.isStopped = true;
                agent.enabled = false;
                break;
            case State.RUSH:
                RB.drag = 0;
                break;
            case State.STUN:
                break;
            case State.DEATH:
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Mur"))
        {
            if(state == State.RUSH)
            {
                RB.velocity = Vector3.zero;
                SwitchState(State.STUN);
                RB.isKinematic = true;

            }
        }
    }
}