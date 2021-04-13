using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BasicState : State
{

    public Transform player;
    public SpawnSysteme spawn;

    [HideInInspector] public spawnEnnemyBasique SEB;

    [SerializeField] private GameObject preDead;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarSec;

    //[SerializeField] private Animator AnimatorConteneur;

    private float hpNow;

    [SerializeField] private float hpMax;
    //[SerializeField] private float vitesse;
    [SerializeField] private int numberCadav;

    private Rigidbody ConteneurRigibody;
    
    public int DMG_Percentage = 1;
    public int Size = 1;

    //private bool JustHit = false;
    //private bool HitPlayer = false;
    //private bool Grounded = false;

    //private float Cooldown = 2;

    [SerializeField] private float timeBar;
    private float chrono;
    private float chronoTick;
    private bool touched;

    public bool Empoisonne = false;
    private float EmpoisonnementTick;

    [SerializeField] private float freqTick;
    [HideInInspector] public float dpsTick;

    private bool Fall = false;


    // Start is called before the first frame update

   /* private void Awake()
    {
        RandomMultiplicatorSize = Random.Range(0.8f, 4.5f);
        Color ConteneurColor = transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.GetColor("_Color");
        ConteneurColor.g /= RandomMultiplicatorSize;
        transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.color = ConteneurColor;
        transform.localScale *= RandomMultiplicatorSize;
        GetComponent<ennemyAI>().agent.speed /= RandomMultiplicatorSize;
        hpMax *= RandomMultiplicatorSize;
        AnimatorConteneur.speed /= (RandomMultiplicatorSize * 0.2f);
    }*/

    void Start()
    {

        player = GameObject.Find("Player").transform;

        ConteneurRigibody = GetComponent<Rigidbody>();
        chrono = 0;
        hpNow = hpMax;

        healthBar.maxValue = hpMax;
        healthBar.value = healthBar.maxValue;

        healthBarSec.maxValue = healthBar.maxValue;
        healthBarSec.value = healthBar.maxValue;

        //numberCadav = Random.Range(1, 4);

        EmpoisonnementTick = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Empoisonne)
        {
            if (EmpoisonnementTick >= freqTick)
            {

                damage(dpsTick);
                Empoisonne = false;
                EmpoisonnementTick = 0;
                

            }
            else
            {
                EmpoisonnementTick += Time.deltaTime;
            }
        }
        HealthbarDecrease();

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
                if (spawn.ListEnnemy.Contains(this.gameObject))
                {
                    spawn.ListEnnemy.Remove(this.gameObject);
                }

                if (Fall)
                {
                    Instantiate(preDead, player.position,Quaternion.identity, GameObject.Find("CadavreParent").transform);
                    Debug.Log(detectDead.ressourceInt);
                }
                else
                {
                    Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f), 
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                écart++;
            }

            if(SEB != null)
            {
                SEB.numberEnnemy--;

            }
        }
       
    }

    public void damage(float hit)
    {
        hpNow -= hit;
        healthBar.value = hpNow;
        touched = true;
        chrono = 0;
    }

    void HealthbarDecrease()
    {
        if (touched)
        {
            if (chrono >= timeBar)
            {
                healthBarSec.value -= 1.5f * Time.deltaTime;

                if (healthBarSec.value <= healthBar.value)
                {
                    chrono = 0;
                    touched = false;
                }
            }
            else
            {
                chrono += Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            hpNow = 0;
            Fall = true;
        }
    }

    /*void MoveSansRigidboy()
    {
        if (Grounded)
        {
            if (!JustHit)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > 4f)
                {
                    Vector3 distance = player.position - transform.position;
                    distance = distance.normalized;
                    ConteneurRigibody.velocity = (vitesse / RandomMultiplicatorSize) * distance;
                    //transform.position += distance * (vitesse/RandomMultiplicatorSize) * Time.deltaTime;
                    transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                }
                else
                {
                    if (!HitPlayer)
                    {
                        if (Physics.Raycast(transform.position, transform.forward,
                            out RaycastHit hit, 20, LayerMask.GetMask("Default")))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                Debug.Log("JeTape");
                                int Explosion = 2000;
                                //hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                hit.transform.GetComponent<Rigidbody>()
                                    .AddForceAtPosition(transform.forward * Explosion, hit.point);
                                this.HitPlayer = true;
                            }
                        }
                    }
                    else
                    {
                        if (Cooldown <= 0)
                        {
                            HitPlayer = false;
                            Cooldown = 2;
                        }
                        else
                        {
                            Cooldown -= Time.deltaTime;
                        }
                    }

                }

            }
            else if (JustHit)
            {
                if (ConteneurRigibody.velocity.magnitude < 0.2f)
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
    }*/
}
