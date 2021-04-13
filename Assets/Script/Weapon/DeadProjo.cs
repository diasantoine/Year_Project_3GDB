using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadProjo : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string TireTouche = "";

    [FMODUnity.EventRef]
    public string Ruant_Touche_N = "";
    // FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_N, transform.position); // son no-degat

    //public Transform cible;

    [SerializeField] public float vitesse;
    [SerializeField] private float écart;

    [SerializeField] private float dégat;

    private Vector3 moveDirection;

    [SerializeField] private Rigidbody RB;

    public bool Empoisonnement = false;
    public bool Rocket = false;

    [SerializeField] private float portée;


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
                collision.gameObject.GetComponent<BasicState>().Empoisonne = true;
            }
            if (Rocket)
            {
                dégat *= 1.5f;
                // faire une mini explosion qui peut toucher d'autre ennemie
            }
          
            FMODUnity.RuntimeManager.PlayOneShot(TireTouche, transform.position);
            if (collision.gameObject.GetComponent<BasicState>() != null)
            {
                collision.gameObject.GetComponent<BasicState>().damage(dégat);

            }
            else if(collision.gameObject.GetComponent<damageTuto>() != null)
            {
                collision.gameObject.GetComponent<damageTuto>().damage(dégat);
            }
            else if (collision.gameObject.GetComponent<RuantState>() != null)
            {
                if (collision.gameObject.GetComponent<RuantState>().isWeak)
                {
                    collision.gameObject.GetComponent<RuantState>().takeDmg(dégat);
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_N, transform.position); // son no-degat
                }
            }

            if (collision.gameObject.GetComponent<ScreamerScript>() != null)
            {
                collision.gameObject.GetComponent<ScreamerScript>().damage(dégat);
            }

            
        }

        Destroy(gameObject);

    }



}
