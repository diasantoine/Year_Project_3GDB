using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterWall : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Projectile") && other.transform.GetComponent<ShootLastra>() != null)
        {
            Debug.Log("?");
            GameObject Stock = Instantiate(this.Projectile, other.transform.position, Quaternion.identity);
            Stock.GetComponent<DeadProjo>().RB.velocity = -transform.GetComponent<ShootLastra>().RB.velocity;
            Destroy(other.gameObject);
        }
    }
}
