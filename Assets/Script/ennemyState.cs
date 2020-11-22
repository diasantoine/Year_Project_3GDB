using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ennemyState : MonoBehaviour
{

    private Transform player;

    public bool moving;
    [HideInInspector] public spawnEnnemyBasique SEB;

    [SerializeField] private GameObject preDead;

    private float hpNow;

    [SerializeField] private float hpMax;
    [SerializeField] private float vitesse;

    [SerializeField] private int numberCadav;

    [SerializeField] private bool Grounded = false;

    private float RandomMultiplicatorSize = 0;

    private Rigidbody ConteneurRigibody;
    
    [SerializeField] public int DMG_Percentage = 1;

    [SerializeField] private bool JustHit = false;

    // Start is called before the first frame update

    private void Awake()
    {
        RandomMultiplicatorSize = Random.Range(0.2f, 2.5f);
        Color ConteneurColor = GetComponent<MeshRenderer>().material.GetColor("_Color");
        ConteneurColor.g /= RandomMultiplicatorSize;
        GetComponent<MeshRenderer>().material.color = ConteneurColor;
        transform.localScale *= RandomMultiplicatorSize;
        ConteneurRigibody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        hpNow = hpMax;

        numberCadav = Random.Range(1, 4);

    }

    // Update is called once per frame
    void Update()
    {
        if (Grounded)
        {
            if (moving && !JustHit)
            {
                Vector3 distance = player.position - transform.position;
                distance = distance.normalized;
                ConteneurRigibody.velocity = (vitesse / RandomMultiplicatorSize) * distance;
                //transform.position += distance * (vitesse/RandomMultiplicatorSize) * Time.deltaTime;
                transform.LookAt(player.position);
            }
            else if(JustHit)
            {
                if (ConteneurRigibody.velocity.magnitude< 0.2f )
                {
                    JustHit = false;
                    if (ConteneurRigibody.constraints == RigidbodyConstraints.None)
                    {
                        ConteneurRigibody.constraints = RigidbodyConstraints.FreezeRotation;
                        transform.rotation = Quaternion.identity;
                    }
                }
            }
        }
       
        if (transform.position.y <= -10)
        {
            hpNow = 0;
        }
        if (hpNow <= 0)
        {
            float écart = -numberCadav / 2;

            Destroy(gameObject);
            for (int i = 1; i <= numberCadav; i++)
            {
                if (transform.position.y<= -10)
                {
                    Instantiate(preDead, player.transform.position + new Vector3(0, 0, écart * 1.25f), 
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                else
                {
                    Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f), 
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                écart++;
            }

            SEB.numberEnnemy--;
        }
       
    }

    public void damage(float hit)
    {
        hpNow -= hit;
    }


   private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
        if (collision.transform.CompareTag("Projectile"))
        {
            JustHit = true;
            int Explosion = DMG_Percentage * 3;
            DMG_Percentage = Explosion;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.GetComponent<Rigidbody>()
                .AddForceAtPosition(transform.forward * Explosion, collision.GetContact(0).point);
        }

        if (collision.transform.CompareTag("Ennemy") 
            && collision.gameObject.GetComponent<ennemyState>().ConteneurRigibody.constraints == RigidbodyConstraints.None)
        {
            JustHit = true;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            /*transform.GetComponent<Rigidbody>()
                .AddForceAtPosition(transform.forward * collision.gameObject.GetComponent<ennemyState>().DMG_Percentage
                    , collision.GetContact(0).point);*/
        }
    }

   private void OnCollisionStay(Collision collision)
   {
       if (collision.transform.CompareTag("sol") && !Grounded)
       {
           Grounded = true;
       }
   }

   private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("sol") &&Grounded)
        {
            Grounded = false;
        }
    }
}
