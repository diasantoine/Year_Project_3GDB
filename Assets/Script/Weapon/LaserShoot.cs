using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{

    [HideInInspector] public bool goGoGo;
    [HideInInspector] public int charged;
    [HideInInspector] public bool IsCharging;
    [SerializeField] private ParticleSystem ps;

    [SerializeField] private int hitDmg;
    [SerializeField] private List<Collider> ListCollider = new List<Collider>();
    [SerializeField] private GameObject ImpactParticle;

    private int ChargedOneByOne = 0;
    private bool Finish;
    private bool FinishCompteurStart;
    private float IfNoWallHit = 0.2f;

    private Vector3 StartScale;

    private bool Wall;
    private bool WallLaserSmooth;
    private float DistancePlayerWall;

    private List<GameObject> touchMob;

    void Start()
    {
        touchMob = new List<GameObject>();
        goGoGo = false;
        charged = 0;
        this.StartScale = transform.parent.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsCharging && !this.Wall && !this.WallLaserSmooth)
        {
            this.ListCollider[0].enabled = true;
            Vector3 localZ = transform.parent.localScale;
            localZ.z = charged * this.StartScale.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        }
        
        if (goGoGo)
        {
            this.ListCollider[0].enabled = false;
            this.ListCollider[1].enabled = true;
            //this.GetComponent<Renderer>().enabled = true;
            ps.gameObject.SetActive(true);
            ps.Play();
            transform.parent.transform.parent = null;
            Destroy(transform.parent.gameObject, 1f);
            goGoGo = false;
        //     this.IfNoWallHit = 0.2f;
        //     this.FinishCompteurStart = false;
        //     //if (this.ChargedOneByOne <= this.charged)
        //     if(transform.parent.localScale.z < transform.parent.localScale.z * this.charged)
        //     {
        //         // Vector3 localZ = transform.parent.localScale;
        //         // localZ.z =  this.LocalScale.z * this.ChargedOneByOne;
        //         // transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        //         // this.ChargedOneByOne++;
        //         Vector3 localZ = transform.parent.localScale;
        //         localZ.z =  this.LocalScale.z +1f ;
        //         Debug.Log("?");
        //         transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        //         //this.ChargedOneByOne++;
        //     }
        //     else
        //     {
        //         this.goGoGo = false;
        //         this.ChargedOneByOne = 0;
        //         this.FinishCompteurStart = true;
        //         //Destroy(transform.parent.gameObject, 0.1f);
        //     }
        //     // Vector3 localZ = transform.parent.localScale;
        //     // localZ.z = charged * localZ.z;
        //     // transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        //     // Destroy(transform.parent.gameObject, 0.1f);
        //     // goGoGo = false;
        // }
        // else
        // {
        //     if (this.Finish)
        //     {
        //         this.GetComponent<Renderer>().enabled = true;
        //         Destroy(transform.parent.gameObject, 0.1f);
        //     }
        //     else
        //     {
        //         if (this.FinishCompteurStart)
        //         {
        //             if (this.IfNoWallHit>= 0)
        //             {
        //                 this.IfNoWallHit -= Time.deltaTime;
        //             }
        //             else
        //             {
        //                 this.GetComponent<Renderer>().enabled = true;
        //                 Destroy(transform.parent.gameObject, 0.1f);
        //             }
        //         }
        //         else
        //         {
        //             this.IfNoWallHit = 0;
        //         }
        //     }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mur") && this.IsCharging && !this.WallLaserSmooth)
        {
            // Debug.Log(this.ChargedOneByOne);
            //  this.goGoGo = true;
            //  this.ChargedOneByOne = this.charged;
            //  --this.ChargedOneByOne;
            //  this.charged = this.ChargedOneByOne;
            //this.goGoGo = true;
            // this.FinishCompteurStart = false;
            // Vector3 localZ = transform.parent.localScale;
            // localZ.z =  this.LocalScale.z -0.1f ;
            // transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
            this.Wall = true;
            Debug.Log(this.Wall);
            Vector3 localZ = transform.parent.localScale;
            localZ.z = 0.9f * localZ.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
            

        }
        if (other.CompareTag("Ennemy") && !this.IsCharging)
        {
            
            if (other.gameObject.GetComponent<damageTuto>() != null)
            {
                other.gameObject.GetComponent<damageTuto>().damage(hitDmg);
            }

            /*if (other.gameObject.GetComponent<ScreamerState>() != null)
            {
                other.gameObject.GetComponent<ScreamerState>().Damage(hitDmg);
            }*/

            if (other.GetComponent<State>() && !touchMob.Contains(other.gameObject))
            {
                Debug.Log(other.name);
                other.GetComponent<State>().Damage(hitDmg);
                touchMob.Add(other.gameObject);
                Impact(other);
            }

            if(other.name == "MeshRuant" && !touchMob.Contains(other.gameObject))
            {
                other.transform.parent.gameObject.GetComponent<State>().Damage(hitDmg);
                touchMob.Add(other.gameObject);
                Impact(other);

            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mur") && this.IsCharging && !this.WallLaserSmooth)
        {
            // Debug.Log(this.ChargedOneByOne);
            // this.goGoGo = true;
            // this.ChargedOneByOne = this.charged;
            // --this.ChargedOneByOne;
            // this.charged = this.ChargedOneByOne;
            // this.FinishCompteurStart = false;
            // Vector3 localZ = transform.parent.localScale;
            // localZ.z =  this.LocalScale.z -0.1f ;
            // transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
            this.Wall = true;
            Vector3 localZ = transform.parent.localScale;
            localZ.z = 0.9f * localZ.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        }else if (this.WallLaserSmooth && other.gameObject.CompareTag("Mur"))
        {
            if (Vector3.Distance(transform.parent.position, other.transform.position) < this.DistancePlayerWall*0.9f)
            {
                Vector3 localZ = transform.parent.localScale;
                localZ.z = 0.9f * localZ.z;
                transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
                this.DistancePlayerWall = Vector3.Distance(transform.parent.position, other.transform.position);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mur") && this.IsCharging)
        {
            // Debug.Log(this.ChargedOneByOne);
            // this.goGoGo = true;
            // this.ChargedOneByOne = this.charged;
            // ++this.ChargedOneByOne;
            // this.charged = this.ChargedOneByOne;
            // this.Finish = true;
            if (this.WallLaserSmooth)
            {
                Debug.Log(this.Wall);
                Debug.Log(this.WallLaserSmooth);
                this.WallLaserSmooth = false;
                this.Wall = false;
            }
            else
            {
                Debug.Log("thefuck?");
                this.Wall = false;
                this.WallLaserSmooth = true;
                Vector3 localZ = transform.parent.localScale;
                localZ.z = 1.2f * localZ.z;
                transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
                this.DistancePlayerWall = Vector3.Distance(transform.parent.position, other.transform.position);
            }
        }
     
    }

    private void Impact(Collider col)
    {
        GameObject impactP = Instantiate(ImpactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, col.bounds.center)) as GameObject; // Spawns impact effect
        Destroy(impactP, 3.5f);
    }
}
