using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuantAI : Ennemy
{
    [Header("ParamenterRuant")]
    [SerializeField] private GameObject preExplo;
    [SerializeField] private GameObject stunParticle;
    [SerializeField] private GameObject RuantCollider;

    [Header("VarRuant")]
    [SerializeField] private float distForRush;
    [SerializeField] private float waitRush;
    [SerializeField] private float stunTime;
    [SerializeField] private float speedRush;
    [SerializeField] private float stunEcart;
    [SerializeField] private float deceleration;
    [SerializeField] private float PorteMinimale;
    [SerializeField] private int ForceExplosion;
    [SerializeField] private int radiusExploBase;
    
    private float chrono;
    private float speedRushIni;

    private bool DistanceRemainOk;
    private Vector3 LastPosition;

    private float TimeBeforeHitGround;

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
        HitGround,
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
        this.LastPosition = Vector3.zero;
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
                stunParticle.SetActive(true);
                SeeThePlayer = false;
                chrono = 0;
                break;
            case State.HitGround:
                this.RB.isKinematic = true;
                AnimatorConteneur.SetBool("isWalking", false);
                AnimatorConteneur.SetBool("isRushing", false);
                AnimatorConteneur.SetBool("IsHittingGround", true);
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
                    if(chrono < waitRush * 0.625f)
                    {
                        var distance = player.position - transform.position;
                        distance.y = 0;

                        Quaternion newRotation = Quaternion.LookRotation(distance);
                        RB.MoveRotation(newRotation);

                        rushPlace = player.position;
                    };

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
                        if (this.LastPosition == Vector3.zero)
                        {
                            this.LastPosition = transform.position;
                        }
                        DashRuant();
                    }
                    else
                    {
                        RB.drag = deceleration;
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

            case State.HitGround:
                if (this.AnimatorConteneur.GetBool("IsHittingGround"))
                {
                    if (this.TimeBeforeHitGround >= this.AnimatorConteneur.GetCurrentAnimatorStateInfo(0).length)
                    {
                        this.TimeBeforeHitGround = 0;
                        this.ImpulsionTahLesfous();
                    }
                    else
                    {
                        this.TimeBeforeHitGround += Time.deltaTime;
                    }
                }
                else
                {
                    this.AnimatorConteneur.SetBool("IsHittingGround",true);
                }
                break;
            case State.DEATH:
                //transform.Rotate(-35f * Time.deltaTime, 0, 0);
                if(chrono >= 1.7f)
                {
                    GetComponent<RuantState>().Die();
                    FMODUnity.RuntimeManager.PlayOneShot(Ruant_Collision, "", 0, transform.position); // son de collision lorsque le ruant tombe
                    GameObject exploFee = Instantiate(preExplo, transform.position, Quaternion.identity);
                    CameraShake.Instance.Shake(5, 0.5f);
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
                stunParticle.SetActive(false);
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
        //Vector3 place = rushPlace - transform.position;
        Vector3 Direction = this.rushPlace - this.LastPosition;
        //Vector3 Direction = (player.transform.position - transform.position).normalized;
        RuantCollider.layer = 12;
        GetComponent<CapsuleCollider>().enabled = enabled;
        tag = "Dash";
        RB.useGravity = false;
        RB.mass = 250;
        float Magnitude = Mathf.Clamp(Direction.magnitude, this.PorteMinimale, Direction.magnitude);
        if (Vector3.Distance(this.LastPosition, new Vector3(transform.position.x, this.LastPosition.y, transform.position.z)) >= Magnitude)
        {
            this.DistanceRemainOk = true;
        }
        RB.velocity = Direction.normalized * speedRush;
        if (this.DistanceRemainOk)
        {
            Debug.Log("arrivé");
            isRushing = false;
            Debug.Log(this.RB.velocity.magnitude);
            DashFini();
            this.LastPosition = Vector3.zero;
            this.DistanceRemainOk = false;
        }
    }
    
     public void ImpulsionTahLesfous()
    {
        Vector3 hitPoint = transform.position;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].gameObject.CompareTag("Player"))
            {
                float ForceExplosionWithArmorHeat = ForceExplosion + ForceExplosion * player.GetComponent<The_Player_Script>().PercentageArmorHeat / 100;
                
                player.GetComponent<The_Player_Script>().JustHit = true; 
                player.GetComponent<The_Player_Script>().ListOfYourPlayer[player.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody.velocity = Vector3.zero;
                
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
                }
                else if(hit[i].GetComponent<ScreamerState>() != null)
                {
                    hit[i].GetComponent<ScreamerState>().Damage(Mathf.Infinity);
                }
               
            }
        }
        this.state = State.IDLE;
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