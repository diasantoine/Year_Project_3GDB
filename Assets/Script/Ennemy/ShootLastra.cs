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


    private Vector3 moveDirection;

    [HideInInspector] public GameObject LastraWhoFired;


    void Start()
    {
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
        }
    }
}
