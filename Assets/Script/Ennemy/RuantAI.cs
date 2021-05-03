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
    [SerializeField] private float stunEcart;
    [SerializeField] private float deceleration;
    [SerializeField] private GameObject RuantCollider;
    
    private float chrono;
    private float speedRushIni;

    private bool isRushing;
    private RaycastHit hit;

    private Vector3 rushPlace;

    [FMODUnity.EventRef]
    public string Ruant_Collision = "";

    [FMODUnity.EventRef]
    public string Ruant_Cris_Mort = "";

    [FMODUnity.EventRef]
    public string Ruant_Cris_Dash = "";

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
        player = GameObject.Find("Player").transform;
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
                AnimatorConteneur.SetBool("isFreining", false);
                AnimatorConteneur.SetBool("isRushing", false);
                if (Grounded)
                {  
                    RB.isKinematic = true;

                }
                break;
            case State.SPAWN:
                break;
            case State.WAIT:
                AnimatorConteneur.SetTrigger("StartRush");
                FMODUnity.RuntimeManager.PlayOneShot(Ruant_Cris_Dash, "", 0, transform.position);
                break;
            case State.CHASE:
                AnimatorConteneur.SetBool("isWalking", true);
                AnimatorConteneur.SetBool("isRushing", false);
                agent.enabled = true;
                agent.isStopped = false;
                break;
            case State.RUSH:
                AnimatorConteneur.SetBool("isWalking", false);
                AnimatorConteneur.SetBool("isRushing", true);
                //FMODUnity.RuntimeManager.PlayOneShot(Ruant_Cris_Dash, transform.position); // son cris de dash du ruant
                speedRush = speedRushIni;
                agent.enabled = false;
                RB.isKinematic = false;
                break;
            case State.STUN:
                AnimatorConteneur.SetTrigger("HitWall");
                SeeThePlayer = false;
                chrono = 0;
                break;
            case State.DEATH:
                AnimatorConteneur.SetTrigger("Death");
                FMODUnity.RuntimeManager.PlayOneShot(Ruant_Cris_Mort, "", 0, transform.position); // son cris de mort du ruant
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

                if(chrono >= 1f)
                {
                    chrono = 0;
                    SwitchState(State.CHASE);
                }
                else
                {
                    chrono += Time.deltaTime;
                }

                break;

            case State.SPAWN:

                if (Grounded)
                {
                    Debug.Log("GROUND");
                    //FMODUnity.RuntimeManager.PlayOneShot(Ruant_Collision, transform.position); // son de collision lorsqu'il attérit apres le spawn
                    GameObject newExplo = Instantiate(preExplo, transform.position, Quaternion.identity);
                    Destroy(newExplo, 0.2f);
                    CameraShake.Instance.Shake(5, 0.5f);
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
                    var distance = player.position - transform.position;
                    distance.y = 0;

                    Quaternion newRotation = Quaternion.LookRotation(distance);
                    RB.MoveRotation(newRotation);

                    chrono += Time.deltaTime;
                    //transform.LookAt(player);
                }
                break;

            case State.CHASE:
                Vector3 dist = transform.position - player.position;
                agent.SetDestination(player.position);

                if (SeeThePlayer && dist.magnitude <= distForRush)
                {
                    SwitchState(State.WAIT);

                }
                break;

            case State.RUSH:
                if (Grounded)
                {
                    if (isRushing)
                    {
                        DashRuant();
                    }
                    else
                    {
                        RB.drag = deceleration;
                        Debug.Log(RB.velocity.magnitude);
                        AnimatorConteneur.SetFloat("Velocity", Mathf.Clamp(RB.velocity.magnitude, 6, 12) / 10);

                        if(RB.velocity.magnitude < 3)
                        {
                            AnimatorConteneur.SetBool("isFreining", false);
                            SwitchState(State.IDLE);
                        }
                    }
                    // if (isRushing)
                    // {
                    //     Vector3 place = rushPlace - transform.position;
                    //     RB.AddForce(place.normalized * speedRush, ForceMode.Impulse);
                    //
                    //     if (place.magnitude < 0.5f)
                    //     {
                    //         isRushing = false;
                    //     }
                    //
                    // }
                    // else
                    // {
                    //     RB.drag = deceleration;
                    //
                    //     if (RB.velocity.magnitude < 1)
                    //     {
                    //         SwitchState(State.IDLE);
                    //
                    //     }
                    // }
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
                        transform.position += -transform.forward * stunEcart * Time.deltaTime;
                    }
                }
                break;

            case State.DEATH:
                //transform.Rotate(-35f * Time.deltaTime, 0, 0);
                if(chrono >= 2.3f)
                {
                    GetComponent<RuantState>().Die();
                    FMODUnity.RuntimeManager.PlayOneShot(Ruant_Collision, "", 0, transform.position); // son de collision lorsque le ruant tombe
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
                DashFini();
                RB.drag = 0;
                AnimatorConteneur.SetFloat("Velocity", 1f);
                break;
            case State.STUN:
                AnimatorConteneur.SetBool("isRushing", false);
                break;
            case State.DEATH:
                break;
            default:
                break;
        }
    }

    private void DashFini()
    {
        RuantCollider.layer = 13;
        GetComponent<CapsuleCollider>().enabled = !enabled;
        tag = "Ennemy";
        RB.useGravity = true;
        RB.mass = 100;
        AnimatorConteneur.SetBool("isFreining", true);
        AnimatorConteneur.SetBool("isRushing", false);
    }

    private void DashRuant()
    {
        Vector3 place = rushPlace - transform.position;
        //Vector3 Direction = (player.transform.position - transform.position).normalized;
        RuantCollider.layer = 12;
        GetComponent<CapsuleCollider>().enabled = enabled;
        tag = "Dash";
        RB.useGravity = false;
        RB.mass = 250;

        if (place.magnitude < 0.5f)
        {
            isRushing = false;
            DashFini();

        }
        RB.velocity = place.normalized * speedRush;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Mur"))
        {
            if(state == State.RUSH)
            {
                FMODUnity.RuntimeManager.PlayOneShot(Ruant_Collision, "", 0, transform.position); // son de collision lorsque le ruant tape un mur
                RB.velocity = Vector3.zero;
                SwitchState(State.STUN);
                RB.isKinematic = true;
                CameraShake.Instance.Shake(5, 0.3f);

            }
        }

        if (collision.collider.CompareTag("Ennemy"))
        {
            if(collision.collider.name == "Ruant(Clone)")
            {
                FMODUnity.RuntimeManager.PlayOneShot(Ruant_Collision, "", 0, transform.position);
                RB.velocity = Vector3.zero;
                SwitchState(State.STUN);
                RB.isKinematic = true;
                CameraShake.Instance.Shake(5, 0.3f);


            }
        }


    }
}