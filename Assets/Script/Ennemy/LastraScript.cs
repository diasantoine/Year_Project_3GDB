﻿using System.Collections;
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
    [SerializeField] private float DivisionSpeed = 1;
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
    private float SpeedContainer;
    private float TimeShootContainer;
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
        this.SpeedContainer = this.agent.speed;
        this.TimeShootContainer = this.TimeBeforeShoot;
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
                
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position) && this.ContainerNewPos == Vector3.zero && !this.IsRunning)
                {
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
                    }
                    // Vector3 DirToPlayer = transform.position - this.player.position;
                    // Vector3 newPos = transform.position + DirToPlayer;
                    // this.agent.SetDestination(newPos);
                    // this.ContainerNewPos = newPos;
                }
                else if (this.ContainerNewPos != Vector3.zero && this.IsRunning)
                {
                    if (Vector3.Distance(this.ContainerNewPos, transform.position) < 1)
                    {
                        this.ContainerNewPos = Vector3.zero;
                        Debug.Log("arrivé");
                        this.agent.speed = this.SpeedContainer;
                        //this.agent.isStopped = true;
                        this.CatchHisBreath = true;
                        this.IsRunning = false;
                    }
                }

                if (DistanceWhereTheLastraStartToRunBackward < Vector3.Distance(transform.position, this.player.position) && this.ContainerNewPos == Vector3.zero && !this.IsRunning)
                {
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
                    }

                    // if(DistanceWhereTheLastraStartToRunBackward <= Vector3.Distance(transform.position, this.player.position)  && !this.IsRunning)
                    // {
                    //     agent.SetDestination(this.player.position);
                    //     agent.stoppingDistance = this.DistanceMaxNearPlayer;
                    //  
                    // }
                    //
                    // if (this.DistanceMaxNearPlayer >= Vector3.Distance(transform.position, this.player.position) && !this.IsRunning )
                    // {
                    //     this.LastraState = StateLastra.Charging;
                    // }
                }

                break;
            case StateLastra.Charging:
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {  
                    this.LastraState = StateLastra.Moving;
                    this.TimeBeforeShoot = this.TimeShootContainer;
                }

                if (this.TimeBeforeShoot <= 0)
                {
                    this.LastraState = StateLastra.Shoot;
                    this.TimeBeforeShoot = this.TimeShootContainer;
                }
                else
                {
                    this.TimeBeforeShoot -= Time.deltaTime;
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
        var isValid = NavMesh.SamplePosition (position + Vector3.up, out hit, 5,  NavMesh.AllAreas);
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
}
