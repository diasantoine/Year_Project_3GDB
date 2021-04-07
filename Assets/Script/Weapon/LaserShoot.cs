using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{

    [HideInInspector] public bool goGoGo;
    [HideInInspector] public int charged;
    [HideInInspector] public bool IsCharging;

    [SerializeField] private int hitDmg;
    [SerializeField] private List<Collider> ListCollider = new List<Collider>();

    private int ChargedOneByOne = 0;
    private bool Finish;
    private bool FinishCompteurStart;
    private float IfNoWallHit = 0.2f;

    private Vector3 StartScale;

    private bool Wall;
    private bool WallLaserSmooth;
    private float DistancePlayerWall;
    void Start()
    {
        goGoGo = false;
        charged = 0;
        this.StartScale = transform.parent.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsCharging && !this.Wall && !this.WallLaserSmooth)
        {
            this.GetComponent<Renderer>().enabled = true;
            this.ListCollider[0].enabled = true;
            Vector3 localZ = transform.parent.localScale;
            localZ.z = charged * this.StartScale.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
        }
        
        if (goGoGo)
        {
            this.ListCollider[0].enabled = false;
            this.ListCollider[1].enabled = true;
            this.GetComponent<Renderer>().enabled = true;
            Destroy(transform.parent.gameObject, 0.1f);
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
        if (other.gameObject.CompareTag("Ennemy") && !this.IsCharging)
        {
            if (other.gameObject.GetComponent<ennemyState>() != null)
            {
                other.gameObject.GetComponent<ennemyState>().damage(hitDmg);

            }
            else if (other.gameObject.GetComponent<damageTuto>() != null)
            {
                other.gameObject.GetComponent<damageTuto>().damage(hitDmg);
            }
            else if (other.gameObject.GetComponent<RuantState>() != null)
            {
                other.gameObject.GetComponent<RuantState>().takeDmg(hitDmg);
            }

            if (other.gameObject.GetComponent<ScreamerScript>() != null)
            {
                other.gameObject.GetComponent<ScreamerScript>().damage(hitDmg);
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
            Debug.Log(Vector3.Distance(transform.parent.localPosition, other.transform.position) +" " + this.DistancePlayerWall * 1.1f);
            if (Vector3.Distance(transform.parent.localPosition, other.transform.position) >= this.DistancePlayerWall*1.1f)
            {
                Debug.Log(Vector3.Distance(transform.parent.localPosition, other.transform.position) +" " + this.DistancePlayerWall * 1.1f);
                Vector3 localZ = transform.parent.localScale;
                localZ.z = 0.9f * localZ.z;
                transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
                this.DistancePlayerWall = Vector3.Distance(transform.parent.localPosition, other.transform.position);
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
                this.WallLaserSmooth = false;
            }
            else
            {
                Debug.Log("thefuck?");
                this.Wall = false;
                this.WallLaserSmooth = true;
                Vector3 localZ = transform.parent.localScale;
                localZ.z = 1.1f * localZ.z;
                transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
                this.DistancePlayerWall = Vector3.Distance(transform.parent.localPosition, other.transform.position);
            }
        }
    }
}
