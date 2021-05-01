﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadProjo : MonoBehaviour
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string TireTouche = "";

    [FMODUnity.EventRef]
    public string Ruant_Touche_N = "";

    [Header("VarProjectile")]
    [SerializeField] public float vitesse;
    [SerializeField] private float écart;
    [SerializeField] private float portée;
    
    [Header("DMG")]
    [SerializeField] private float dégat;

    private Vector3 moveDirection;

    [SerializeField] private Rigidbody RB;

    public bool Empoisonnement = false;
    public bool Rocket = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, portée);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position += moveDirection * vitesse * Time.deltaTime;


        //Cible a tête chercheuse
        /*if(cible != null)
        {
            Vector3 direction = cible.position - transform.position;

            if (direction.magnitude < écart)
            {
                Destroy(gameObject);
                cible.gameObject.GetComponent<ciblage>().damage(dégat);
            }

            direction = direction.normalized;

            transform.position += direction * vitesse * Time.deltaTime;
            transform.LookAt(cible);
        }
        else
        {
            Destroy(gameObject);
        }*/

    }

    public void Shoot(Vector3 dir)
    {
        moveDirection = dir;
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;
        RB.AddForce(moveDirection * vitesse, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            if (Empoisonnement)
            {
                collision.gameObject.GetComponent<BasicState>().isPoisoned = true;
            }
            if (Rocket)
            {
                dégat *= 1.5f;
                // faire une mini explosion qui peut toucher d'autre ennemie
            }
      
            FMODUnity.RuntimeManager.PlayOneShot(TireTouche, transform.position);               
            if(collision.gameObject.GetComponent<damageTuto>() != null)
            {
                collision.gameObject.GetComponent<damageTuto>().damage(dégat);
            }
            
            if (collision.gameObject.GetComponent<State>())
            {
                if (collision.gameObject.GetComponent<State>().isWeak)
                {
                    collision.gameObject.GetComponent<State>().Damage(dégat);
                }
                else
                {
                    if (collision.gameObject.GetComponent<RuantState>())
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_N, transform.position);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
