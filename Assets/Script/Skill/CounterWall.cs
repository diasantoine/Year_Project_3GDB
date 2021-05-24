using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterWall : MonoBehaviour
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string Counter_Impact = "";

    [HideInInspector] public bool UpgradeRuant;

    [HideInInspector] public bool UpgradeScreamer;

    [SerializeField] private GameObject Projectile;
    [SerializeField] private Transform ProjectileContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProjectileHit(GameObject Projectile, GameObject Lastra)
    {
        GameObject Stock = Instantiate(this.Projectile, Projectile.transform.position, Quaternion.identity);
        //Stock.GetComponent<DeadProjo>().RB.velocity = -Projectile.GetComponent<ShootLastra>().RB.velocity;
        Stock.GetComponent<DeadProjo>().vitesse *= 2;
        Stock.GetComponent<DeadProjo>().Shoot( Lastra.transform.position - Stock.transform.position);
        FMODUnity.RuntimeManager.PlayOneShot(Counter_Impact, "", 0, transform.position);
        Destroy(Projectile.gameObject);
    }
}
