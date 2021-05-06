using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadProjo : MonoBehaviour
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string TireTouche = "";
    [FMODUnity.EventRef]
    public string Ruant_Touche_N = "";
    [FMODUnity.EventRef] 
    public string PlayerHit = "";

    [Header("VarProjectile")]
    [SerializeField] public float vitesse;
    [SerializeField] private float écart;
    [SerializeField] private float portée;

    [Header("LastraTir")]
    [SerializeField] private bool LastraTir;
    [SerializeField] private int DMGHeat;
    
    [Header("DMG")]
    [SerializeField] private float dégat;

    [Header("Shake")]
    [SerializeField] private float intensityShake;
    [SerializeField] private float timeShake;

    [Header("Particle")]
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;

    private Vector3 moveDirection;

    [SerializeField] public Rigidbody RB;

    public bool Empoisonnement = false;
    public bool Rocket = false;

    

    // Start is called before the first frame update
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }

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
        if (this.LastraTir)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.GetComponent<The_Player_Script>().ListOfYourPlayer[collision.transform.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody
                    .AddForceAtPosition(transform.forward * (this.dégat + (this.dégat * collision.transform.GetComponent<The_Player_Script>().PercentageArmorHeat / 100)),
                    collision.transform.position, ForceMode.Impulse);
                collision.transform.GetComponent<The_Player_Script>().JustHit = true;
                collision.transform.GetComponent<The_Player_Script>().PercentageArmorHeat += this.DMGHeat;
            }
        }
        else
        {
             
            if (collision.gameObject.CompareTag("Ennemy"))
            {
                /*if (Empoisonnement)
                {
                    collision.gameObject.GetComponent<BasicState>().isPoisoned = true;
                }
                if (Rocket)
                {
                    dégat *= 1.5f;
                    // faire une mini explosion qui peut toucher d'autre ennemie
                }*/
          
                FMODUnity.RuntimeManager.PlayOneShot(TireTouche, "", 0, transform.position);               
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
                            FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_N, "", 0, transform.position);
                        }
                    }
                }
            }
        }

        CameraShake.Instance.Shake(intensityShake, timeShake);
        ImpactParticle(collision);
        Destroy(gameObject);
    }

    private void ImpactParticle(Collision col)
    {
        GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, col.collider.bounds.center)) as GameObject; // Spawns impact effect

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                             //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
        {
            ParticleSystem trail = trails[i];

            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null); // Detaches the trail from the projectile
                Destroy(trail.gameObject, 2f); // Removes the trail after seconds
            }
        }

        Destroy(projectileParticle, 3f); // Removes particle effect after delay
        Destroy(impactP, 3.5f); // Removes impact effect after delay
    }



}
