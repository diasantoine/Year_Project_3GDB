using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class LastraAI : Ennemy
{
    enum StateLastra
    {
        Idle,
        Moving,
        Charging,
        Shoot,
        Reloading,
        Hit,
        Dead
    }

    [Header("LastraVar")] 
    [SerializeField] private float DistanceMaxNearPlayer;
    [SerializeField] private float DistanceWhereTheLastraStartToRunBackward;
    [SerializeField] private float TimeBeforeHeCatchHisBreath;
    [SerializeField] private float DivisionSpeed = 1;
    [SerializeField] private float RadiusMaxForHisEscape;
    
    [Header("Impact")]
    [SerializeField] private float ImpactTirNormal = 1;
    [SerializeField] private float TimeStayHit;

    [SerializeField] private int NumberOfProjectile;
    [SerializeField] private float TimeBeforeShoot;
    [SerializeField] private float TimeBetweenEachShoot;
    [SerializeField] private float ReloadingTime;
    [SerializeField] private Transform ProjectileContainer;

    [Header("LastraParameter")] 
    [SerializeField] private GameObject Canon;

    [SerializeField] private GameObject Projectile;

    [SerializeField] private StateLastra LastraState;

    [SerializeField] private List<GameObject> ListPartIntensityDuringShoot;

    [SerializeField] private float GestionPowerIntensityChargingShoot;
    
    [Header("Debug")]
    [SerializeField] private float radius=3f;

    private bool DoOnce;
    private float TimeBetweenShootContainer;
    private float ReloadingTimeContainer;
    private float TimeBeforeHeCatchHisBreathContainer;
    private float SpeedContainer;
    private float TimeShootContainer;
    private float TimeStayHitContainer;
    private Color Intensity = Color.clear;
    private int NumberOfProjectileFired;
    private Vector3 ContainerNewPos;
    private int BreakWhile;
    private bool NewPosFind;
    private bool CatchHisBreath;
    [SerializeField] private bool IsRunning;

    private StateLastra ContainerLastState;
    
    [HideInInspector] public SpawnSysteme spawn;

    // Start is called before the first frame update
    void Start()
    {
        this.TimeBetweenShootContainer = this.TimeBetweenEachShoot;
        this.ReloadingTimeContainer = this.ReloadingTime;
        this.TimeBeforeHeCatchHisBreathContainer = this.TimeBeforeHeCatchHisBreath;
        this.SpeedContainer = this.agent.speed;
        this.TimeShootContainer = this.TimeBeforeShoot;
        this.TimeStayHitContainer = this.TimeStayHit; 
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, this.ContainerNewPos, Color.blue);
        switch (this.LastraState)
        {
            case StateLastra.Idle:
                break;
            case StateLastra.Moving:
               // Debug.Log(this.IsRunning + " " + this.ContainerNewPos + " " + Vector3.Distance(transform.position, this.player.position));
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position) && this.ContainerNewPos == Vector3.zero)
                {
                    if ( !this.AnimatorConteneur.GetBool("IsRunning"))
                    {
                        this.AnimatorConteneur.SetBool("IsRunning", true);
                    }
                    if (this.AnimatorConteneur.GetBool("IsMoving"))
                    {
                        this.AnimatorConteneur.SetBool("IsMoving", false);
                    }
                    if (this.agent.speed == 0)
                    {
                        this.agent.speed = this.SpeedContainer;
                    }
                    this.IsRunning = true;
                    this.TimeBeforeHeCatchHisBreath = this.TimeBeforeHeCatchHisBreathContainer;
                    this.CatchHisBreath = false;
                    agent.stoppingDistance = 0.5f;
                    this.agent.speed *= 3/ this.DivisionSpeed;
                    this.DivisionSpeed *= 2;
                    if (this.FindGoal())
                    {
                        this.agent.SetDestination(this.ContainerNewPos);
                        agent.stoppingDistance = 0;
                        this.NewPosFind = false;
                        this.agent.isStopped = false;
                    }
                    else
                    {
                        this.agent.isStopped = true;
                    }
                }
                else if (this.ContainerNewPos != Vector3.zero && this.IsRunning)
                {
                    if (Vector3.Distance(this.ContainerNewPos, transform.position) < 1)
                    {
                        this.ContainerNewPos = Vector3.zero;
                        Debug.Log("arrivé");
                        this.agent.speed = this.SpeedContainer;
                        this.agent.isStopped = true;
                        this.CatchHisBreath = true;
                        this.IsRunning = false;
                        this.AnimatorConteneur.SetBool("IsRunning", false);
                        this.AnimatorConteneur.SetBool("IsMoving", true);
                    }
                    else
                    {
                        this.agent.SetDestination(this.ContainerNewPos);
                        agent.stoppingDistance = 0;
                    }
                }

                if (DistanceWhereTheLastraStartToRunBackward < Vector3.Distance(transform.position, this.player.position) && this.ContainerNewPos == Vector3.zero && !this.IsRunning)
                {
                    this.agent.isStopped = false;
                    if (!this.AnimatorConteneur.GetBool("IsMoving"))
                    {
                        this.AnimatorConteneur.SetBool("IsMoving",true);
                    }
                    if (this.AnimatorConteneur.GetBool("IsRunning"))
                    {
                        this.AnimatorConteneur.SetBool("IsRunning", false);
                    }
                    if (this.CatchHisBreath)
                    {
                        if (this.TimeBeforeHeCatchHisBreath <= 0)
                        {
                            this.CatchHisBreath = false;
                            this.TimeBeforeHeCatchHisBreath = this.TimeBeforeHeCatchHisBreathContainer;
                            this.DivisionSpeed = 1;
                        }
                        else
                        {
                            this.TimeBeforeHeCatchHisBreath -= Time.deltaTime;
                        }
                    }
                    if (this.DistanceMaxNearPlayer + 1 < Vector3.Distance(transform.position, this.player.position) && !this.IsRunning)
                    {
                        this.agent.speed = this.SpeedContainer;
                        agent.SetDestination(this.player.position);
                        agent.stoppingDistance = this.DistanceMaxNearPlayer;
                    }
                    else if (this.DistanceMaxNearPlayer + 1 >= Vector3.Distance(transform.position, this.player.position) && !this.IsRunning)
                    {
                        this.LastraState = StateLastra.Charging;
                        this.AnimatorConteneur.SetBool("IsMoving",false);
                        this.AnimatorConteneur.SetBool("IsCharging",true);
                        this.agent.isStopped = true;
                    }
                }

                break;
            case StateLastra.Charging:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.LastraState = StateLastra.Moving;
                    this.TimeBeforeShoot = this.TimeShootContainer;
                    this.AnimatorConteneur.SetBool("IsRunning",true);
                    this.AnimatorConteneur.SetBool("IsCharging",false);
                    this.Intensity = Color.clear;
                    foreach (GameObject PartIntensityDuringShoot in this.ListPartIntensityDuringShoot)
                    {
                        PartIntensityDuringShoot.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.Intensity);
                    }
                }
                else if(this.DistanceMaxNearPlayer *1.4f < Vector3.Distance(transform.position, this.player.position))
                {
                    this.LastraState = StateLastra.Moving;
                    this.TimeBeforeShoot = this.TimeShootContainer;
                    this.AnimatorConteneur.SetBool("IsMoving",true);
                    this.AnimatorConteneur.SetBool("IsCharging",false);
                    this.Intensity = Color.clear;
                    foreach (GameObject PartIntensityDuringShoot in this.ListPartIntensityDuringShoot)
                    {
                        PartIntensityDuringShoot.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.Intensity);
                    }
                }

                if (this.TimeBeforeShoot <= 0)
                {
                    this.LastraState = StateLastra.Shoot;
                    this.TimeBeforeShoot = this.TimeShootContainer;
                    this.AnimatorConteneur.SetBool("IsCharging",false);
                    this.AnimatorConteneur.SetBool("Fire",true);
                }
                else
                {
                    this.TimeBeforeShoot -= Time.deltaTime;
                    foreach (GameObject PartIntensityDuringShoot in this.ListPartIntensityDuringShoot)
                    {
                        Color ContainerColor = PartIntensityDuringShoot.GetComponent<Renderer>().material.color;
                        this.Intensity += ContainerColor * Time.deltaTime/this.GestionPowerIntensityChargingShoot;
                        PartIntensityDuringShoot.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.Intensity);
                    }
                }
                break;
            case StateLastra.Shoot:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.NumberOfProjectileFired = 0;
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.LastraState = StateLastra.Moving;
                    this.AnimatorConteneur.SetBool("IsRunning",true);
                    this.AnimatorConteneur.SetBool("IsCharging",false);
                    this.Intensity = Color.clear;
                    foreach (GameObject PartIntensityDuringShoot in this.ListPartIntensityDuringShoot)
                    {
                        PartIntensityDuringShoot.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.Intensity);
                    }
                }else if(this.DistanceMaxNearPlayer *1.4f < Vector3.Distance(transform.position, this.player.position))
                {
                    this.LastraState = StateLastra.Moving;
                    this.NumberOfProjectileFired = 0;
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.AnimatorConteneur.SetBool("IsMoving",true);
                    this.AnimatorConteneur.SetBool("Fire",false);
                     this.Intensity = Color.clear;
                    foreach (GameObject PartIntensityDuringShoot in this.ListPartIntensityDuringShoot)
                    {
                        PartIntensityDuringShoot.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.Intensity);
                    }
                }
                if (this.TimeBetweenEachShoot >= 0)
                {
                    this.TimeBetweenEachShoot -= Time.deltaTime;
                }
                else if(this.NumberOfProjectileFired <= this.NumberOfProjectile)
                {
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.NumberOfProjectileFired++;
                    GameObject ContainerProjectile = Instantiate(this.Projectile, this.Canon.transform.position, Quaternion.identity);//projectilecontainer pour le parent
                   //ContainerProjectile.GetComponent<Rigidbody>().velocity = (this.player.transform.position - transform.position).normalized * 20;
                    ContainerProjectile.GetComponent<DeadProjo>().Shoot(this.player.transform.position - transform.position);
                }
                if (this.NumberOfProjectileFired >= this.NumberOfProjectile)
                {
                    this.NumberOfProjectileFired = 0;
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.LastraState = StateLastra.Reloading;
                    this.AnimatorConteneur.SetBool("IsReloading",true);
                    this.AnimatorConteneur.SetBool("Fire",false);
                    this.Intensity = Color.clear;
                }
                break;
            case StateLastra.Reloading:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.ReloadingTime = this.ReloadingTimeContainer;
                    this.LastraState = StateLastra.Moving;
                    this.AnimatorConteneur.SetBool("IsRunning",true);
                    this.AnimatorConteneur.SetBool("IsReloading",false);
                }
                if (this.ReloadingTime < 0)
                {
                    this.ReloadingTime = this.ReloadingTimeContainer;
                    this.LastraState = StateLastra.Shoot;
                    this.AnimatorConteneur.SetBool("IsReloading",false);
                    this.AnimatorConteneur.SetBool("Fire",true);
                }
                else
                {
                    this.ReloadingTime -= Time.deltaTime;
                }
                break;
            case StateLastra.Hit:
                if (this.JustHit)
                {
                    this.AnimatorConteneur.SetBool("IsRunning",false);
                    this.AnimatorConteneur.SetBool("IsReloading",false);
                    this.AnimatorConteneur.SetBool("IsCharging",false);
                    this.AnimatorConteneur.SetBool("Fire",false);
                    this.AnimatorConteneur.SetBool("IsMoving",false);
                    this.AnimatorConteneur.SetBool("Hit",true);
                    if (this.TimeStayHit <= 0)
                    {
                        this.JustHit = false;
                        this.TimeStayHit = this.TimeStayHitContainer;
                        this.LastraState = this.ContainerLastState;
                        this.agent.enabled = true;
                        if (RB.velocity.magnitude < 8f)
                        {
                            RB.isKinematic = false;
                            JustHit = false;
                            agent.enabled = true;
                            RB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                            RB.freezeRotation = true;
                            switch (ContainerLastState)
                            {
                                case StateLastra.Idle:
                                    break;
                                case StateLastra.Moving:
                                    if (this.IsRunning)
                                    {
                                        this.AnimatorConteneur.SetBool("IsRunning",true);
                                        this.AnimatorConteneur.SetBool("Hit",false);
                                    }
                                    else
                                    {
                                        this.AnimatorConteneur.SetBool("IsMoving",true);
                                        this.AnimatorConteneur.SetBool("Hit",false);
                                    }
                                    break;
                                case StateLastra.Charging:
                                    this.AnimatorConteneur.SetBool("IsCharging",true);
                                    this.AnimatorConteneur.SetBool("Hit",false);
                                    break;
                                case StateLastra.Shoot:
                                    this.AnimatorConteneur.SetBool("Fire",true);
                                    this.AnimatorConteneur.SetBool("Hit",false);
                                    break;
                                case StateLastra.Reloading:
                                    this.AnimatorConteneur.SetBool("IsReloading",true);
                                    this.AnimatorConteneur.SetBool("Hit",false);
                                    break;
                            }
                            this.LastraState = this.ContainerLastState;
                        }
                    }
                    else
                    {
                        this.TimeStayHit -= Time.deltaTime;
                    }
                }
                break;
            case StateLastra.Dead:
                break;
        }
    }
    
    void OnDrawGizmosSelected ()
    {
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radius);

    }
    
    private bool FindGoal ()
    {
        while (!this.NewPosFind && this.BreakWhile < 15)
        {
            this.BreakWhile++;
            if (this.FindValidPosition())
            {
                this.agent.destination = this.ContainerNewPos;
                this.NewPosFind = true;
                this.BreakWhile = 0;
                return true;
            }
        }
        this.BreakWhile = 0;
        return false;
    }
    
    private bool FindValidPosition ()
    {
        Vector2 offset = Random.insideUnitCircle * this.RadiusMaxForHisEscape;
        Vector3 position = this.transform.position + new Vector3 (offset.x, 0, offset.y);
        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition (position + Vector3.up, out hit, 10,  NavMesh.AllAreas);
        Debug.Log(isValid + " " + FrontTest(new Vector3(hit.position.x, transform.position.y, hit.position.z)));
        if (!isValid || Vector3.Distance(new Vector3(hit.position.x, transform.position.y, hit.position.z), this.player.transform.position) < this.DistanceMaxNearPlayer * 1.1f || 
            !FrontTest(new Vector3(hit.position.x, transform.position.y, hit.position.z)))
            return false;
        
        this.ContainerNewPos = new Vector3(hit.position.x, transform.position.y, hit.position.z);
        Debug.Log(this.ContainerNewPos);
        return true;
    }
    
    bool FrontTest(Vector3 PositionHit)
    {
        Vector3 fwd = this.player.forward;
        Vector3 vec = PositionHit - this.player.position;
        vec = vec.normalized;
 
        float ang = Mathf.Acos(Vector3.Dot(fwd, vec)) * Mathf.Rad2Deg;
        if (ang <= 180f && ang >= 0)
            return true;
 
        return false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            if (collision.transform.GetComponent<DeadProjo>().Empoisonnement)
            {
                JustHit = true;
                agent.enabled = false;
                RB.velocity *= ImpactTirNormal;
                this.ContainerLastState = this.LastraState;
                this.RB.isKinematic = true;
                this.LastraState = StateLastra.Hit;

            }
            else
            {
                JustHit = true;
                agent.enabled = false;
                RB.velocity *= ImpactTirNormal;
                this.ContainerLastState = this.LastraState;
                this.LastraState = StateLastra.Hit;
                this.RB.isKinematic = true;
                //SwitchState(State.DUMB);
            }
            if (AnimatorConteneur != null)
            {
                AnimatorConteneur.SetBool("Hit", true);
            }
        }
        if (collision.transform.CompareTag("Mur") && this.JustHit)
        {
            this.GetComponent<LastraState>().Damage(Mathf.Infinity);
        }
    }
}
