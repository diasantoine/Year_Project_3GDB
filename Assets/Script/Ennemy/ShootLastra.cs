using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLastra : MonoBehaviour
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string TireTouche = "";

    [FMODUnity.EventRef] 
    public string PlayerHit = "";

    [Header("VarProjectile")]
    [SerializeField] public float vitesse;
    [SerializeField] private float écart;
    [SerializeField] public float portée;
    [SerializeField] public Rigidbody RB;
    
    [Header("DMG")]
    [SerializeField] private float dégat;
    [SerializeField] private int DMGHeat;

    [Header("Particle")]
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;


    private Vector3 moveDirection;

    [HideInInspector] public GameObject LastraWhoFired;


    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }

        Invoke("DestroyMe", this.portée);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<The_Player_Script>().ListOfYourPlayer[collision.transform.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody
                .AddForceAtPosition(transform.forward * (this.dégat + (this.dégat * collision.transform.GetComponent<The_Player_Script>().PercentageArmorHeat / 100)),
                collision.transform.position, ForceMode.Impulse);
            collision.transform.GetComponent<The_Player_Script>().JustHit = true;
            collision.transform.GetComponent<The_Player_Script>().PercentageArmorHeat += this.DMGHeat;
            FMODUnity.RuntimeManager.PlayOneShot(PlayerHit, "", 0, transform.position);
            CameraShake.Instance.Shake(1.5f, 0.2f);

        }

        if (collision.collider.transform.CompareTag("CounterWall"))
        {
            collision.collider.transform.GetComponent<CounterWall>().ProjectileHit(gameObject, this.LastraWhoFired);
        }
        else
        {
            Destroy(gameObject);
            ImpactParticle(collision);
        }

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
