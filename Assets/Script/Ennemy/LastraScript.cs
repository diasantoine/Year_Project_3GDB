using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private float DMG;

    [SerializeField] private int NumberOfProjectile;
    [SerializeField] private float TimeBeforeShoot;
    [SerializeField] private Transform ProjectileContainer;

    [Header("LastraParameter")] 
    [SerializeField] private GameObject Canon;

    [SerializeField] private GameObject Projectile;

    [SerializeField] private StateLastra LastraState;
    
    [Header("Debug")]
    [SerializeField] private float radius=3f;

    private bool DoOnce;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.LastraState)
        {
            case StateLastra.Idle:
                break;
            case StateLastra.Moving:
                
                if (DistanceWhereTheLastraStartToRunBackward > Vector3.Distance(transform.position, this.player.position))
                {
                    agent.stoppingDistance = 0;
                    Vector3 DirToPlayer = transform.position - this.player.position;
                    Vector3 newPos = transform.position + DirToPlayer;
                    this.agent.SetDestination(newPos);
                }
                else
                {
                    agent.SetDestination(this.player.position);
                    agent.stoppingDistance = this.DistanceMaxNearPlayer;
                }

                if (this.DistanceMaxNearPlayer >= Vector3.Distance(transform.position, this.player.position))
                {
                    this.LastraState = StateLastra.Charging;
                }
                break;
            case StateLastra.Charging:
                if (!this.DoOnce)
                {
                    DoOnce = true;
                    StartCoroutine(this.FunctionTimeBeforeShoot());
                }
                break;
            case StateLastra.Shoot:
                for (int i = 0; i < this.NumberOfProjectile; i++)
                {
                    GameObject ContainerProjectile = Instantiate(this.Projectile, this.Canon.transform.position + new Vector3(i*3,0,0), Quaternion.identity);//projectilecontainer pour le parent
                    ContainerProjectile.GetComponent<Rigidbody>().velocity = (this.player.transform.position - transform.position).normalized * 20;
                    Destroy(ContainerProjectile, 4f);
                }

                this.LastraState = StateLastra.Reloading;
                break;
            case StateLastra.Reloading:
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
}
