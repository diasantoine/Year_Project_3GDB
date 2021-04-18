using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class LastraScript : Ennemy
{
    enum StateLastra
    {
        Idle,
        Moving,
        Charging,
        Shoot,
        Reloading,
        Dead
    }

    [Header("LastraVar")] 
    [SerializeField] private float DistanceMaxNearPlayer;
    [SerializeField] private float DistanceWhereTheLastraStartToRunBackward;
    [SerializeField] private float TimeBeforeHeCatchHisBreath;
    [SerializeField] private float RadiusMaxForHisEscape;

    [SerializeField] private float DMG;

    [SerializeField] private int NumberOfProjectile;
    [SerializeField] private float TimeBeforeShoot;
    [SerializeField] private float TimeBetweenEachShoot;
    [SerializeField] private float ReloadingTime;
    [SerializeField] private Transform ProjectileContainer;

    [Header("LastraParameter")] 
    [SerializeField] private GameObject Canon;

    [SerializeField] private GameObject Projectile;

    [SerializeField] private StateLastra LastraState;
    
    [Header("Debug")]
    [SerializeField] private float radius=3f;

    private bool DoOnce;
    private float TimeBetweenShootContainer;
    private float ReloadingTimeContainer;
    private float TimeBeforeHeCatchHisBreathContainer;
    private int NumberOfProjectileFired;
    private Vector3 ContainerNewPos;
    private int BreakWhile;
    private bool NewPosFind;
    private bool CatchHisBreath;
    private bool IsRunning;
    
    // Start is called before the first frame update
    void Start()
    {
        this.TimeBetweenShootContainer = this.TimeBetweenEachShoot;
        this.ReloadingTimeContainer = this.ReloadingTime;
        this.TimeBeforeHeCatchHisBreathContainer = this.TimeBeforeHeCatchHisBreath;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.LastraState)
        {
            case StateLastra.Idle:
                break;
            case StateLastra.Moving:
                
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position) && this.ContainerNewPos == Vector3.zero)
                {
                    agent.stoppingDistance = 0;
                    this.IsRunning = true;
                    this.agent.speed *= 6;
                    if (this.FindGoal())
                    {
                        this.agent.SetDestination(this.ContainerNewPos);
                        this.NewPosFind = false;
                    }
                    // Vector3 DirToPlayer = transform.position - this.player.position;
                    // Vector3 newPos = transform.position + DirToPlayer;
                    // this.agent.SetDestination(newPos);
                    // this.ContainerNewPos = newPos;
                }
                else if (this.ContainerNewPos != Vector3.zero)
                {
                    if (Vector3.Distance(this.ContainerNewPos, transform.position) < 0.1f)
                    {
                        this.ContainerNewPos = Vector3.zero;
                        Debug.Log("arrivé");
                        this.agent.speed /= 6;
                        //this.agent.isStopped = true;
                        this.CatchHisBreath = true;
                    }
                }

                if (this.CatchHisBreath)
                {
                    if (this.TimeBeforeHeCatchHisBreath <=0)
                    {
                        this.CatchHisBreath = false;
                        this.IsRunning = false;
                        this.TimeBeforeHeCatchHisBreath = this.TimeBeforeHeCatchHisBreathContainer;
                    }
                    else
                    {
                        this.TimeBeforeHeCatchHisBreath -= Time.deltaTime;
                    }
                }
                else
                {
                    if(DistanceWhereTheLastraStartToRunBackward <= Vector3.Distance(transform.position, this.player.position)  && !this.IsRunning)
                    {
                        agent.SetDestination(this.player.position);
                        agent.stoppingDistance = this.DistanceMaxNearPlayer;
                    }

                    if (this.DistanceMaxNearPlayer >= Vector3.Distance(transform.position, this.player.position) && !this.IsRunning )
                    {
                        Debug.Log(this.DistanceMaxNearPlayer + " " +  Vector3.Distance(transform.position, this.player.position));
                        this.LastraState = StateLastra.Charging;
                    }
                }
                break;
            case StateLastra.Charging:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    StopCoroutine(this.FunctionTimeBeforeShoot());
                    this.LastraState = StateLastra.Moving;
                }
                if (!this.DoOnce)
                {
                    DoOnce = true;
                    StartCoroutine(this.FunctionTimeBeforeShoot());
                }
                break;
            case StateLastra.Shoot:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.NumberOfProjectileFired = 0;
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.LastraState = StateLastra.Moving;
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
                    ContainerProjectile.GetComponent<Rigidbody>().velocity = (this.player.transform.position - transform.position).normalized * 20;
                    Destroy(ContainerProjectile, 4f);
                }
                if (this.NumberOfProjectileFired >= this.NumberOfProjectile)
                {
                    this.NumberOfProjectileFired = 0;
                    this.TimeBetweenEachShoot = this.TimeBetweenShootContainer;
                    this.LastraState = StateLastra.Reloading;
                }
                break;
            case StateLastra.Reloading:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.ReloadingTime = this.ReloadingTimeContainer;
                    this.LastraState = StateLastra.Moving;
                }
                if (this.ReloadingTime < 0)
                {
                    this.ReloadingTime = this.ReloadingTimeContainer;
                    this.LastraState = StateLastra.Shoot;
                }
                else
                {
                    this.ReloadingTime -= Time.deltaTime;
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

    IEnumerator FunctionTimeBeforeShoot()
    {
        yield return new WaitForSeconds(TimeBeforeShoot);
        this.LastraState = StateLastra.Shoot;
        this.DoOnce = false;
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
        return false;
    }
    
    private bool FindValidPosition ()
    {
        var offset = Random.insideUnitCircle * this.RadiusMaxForHisEscape;
        var position = this.transform.position + new Vector3 (offset.x, 0, offset.y);
        NavMeshHit hit;
        var isValid = NavMesh.SamplePosition (position + Vector3.up, out hit, 5, NavMesh.AllAreas);
        if (!isValid && Vector3.Distance(position, this.player.transform.position) > this.DistanceMaxNearPlayer *1.3f) 
            return false;

        this.ContainerNewPos = position;
        Debug.Log(position);
        return true;
    }
}
