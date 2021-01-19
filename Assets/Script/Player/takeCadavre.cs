using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCadavre : MonoBehaviour
{

    public bool gotcha;

    [HideInInspector] public bool isMunitions;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool dash;


    public Transform player;
    public Transform Dash;
    public Transform bombe;


    [SerializeField] private float threshold;
    [SerializeField] private float vitesse;
    [SerializeField] private float radiusGave;
    [SerializeField] private int DmgShield = 1;



    public detectDead deadD;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gotcha)
        {
            if(player != null)
            {
                Vector3 direction = player.position - transform.position;

                if (direction.magnitude < threshold)
                {
                    isMunitions = true;
                    gotcha = false;
                    gameObject.transform.parent = player.transform;
                    gameObject.layer = 10;

                }

                direction = direction.normalized;

                transform.position += direction * vitesse * Time.deltaTime;
            }
        }

        if (player != null)
        {
            if (player.parent.GetComponent<CharacterMovement>().OnDash)
            {
                GetComponent<BoxCollider>().enabled = !enabled;
            }
            else if(GetComponent<BoxCollider>().enabled == !enabled)
            {
                GetComponent<BoxCollider>().enabled = enabled;
            }
        }
        if (isMunitions)
        {
            if (!deadD.deadList.Contains(this.gameObject))
            {
                deadD.deadList.Add(this.gameObject);
            }

            Vector3 Position = 2 * Vector3.Normalize(transform.position - player.position) + player.position;
            transform.position = Position;
            gameObject.transform.RotateAround(player.position, Vector3.up, 280f * Time.deltaTime);
            gameObject.transform.LookAt(player);
        }

        if (charge)
        {
            if (bombe != null)
            {
                if (!bombe.GetComponent<TirCharge>().tipar)
                {
                    Vector3 direction = bombe.position - transform.position;

                    direction = direction.normalized;

                    transform.position += direction * vitesse * Time.deltaTime;

                    if(bombe.GetComponent<TirCharge>().nCharge >= bombe.GetComponent<TirCharge>().nChargeMax)
                    {
                        gotcha = true;
                        charge = false;
                        gameObject.layer = 8;
                    }
                }
                else
                {
                    bombe = null;
                }
;
            }
            else
            {
                gotcha = true;
                charge = false;
                gameObject.layer = 8;

            } 
        }else if (dash)
        {
            if (Dash != null)
            {
                if (Dash.GetComponent<ChargedDash>().isCharging)
                {
                    Vector3 direction = Dash.position - transform.position;

                    direction = direction.normalized;

                    transform.position += direction * vitesse * Time.deltaTime;

                    if(Dash.GetComponent<ChargedDash>().Charge >= Dash.GetComponent<ChargedDash>().ChargeMax)
                    {
                        gotcha = true;
                        dash = false;
                        gameObject.layer = 8;
                    }
                }
                else
                {
                    Dash = null;
                }
                ;
            }
            else
            {
                gotcha = true;
                dash = false;
                gameObject.layer = 8;
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (charge)
        {
            if (other.gameObject.CompareTag("chargeTrigger"))
            {
                Destroy(gameObject);
                deadD.deadList.Remove(gameObject);
                bombe.GetComponent<TirCharge>().nCharge++;
                bombe.transform.localScale = bombe.transform.localScale + new Vector3(radiusGave, radiusGave, radiusGave);
            }
        }else if (dash)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                deadD.deadList.Remove(gameObject);
                Destroy(gameObject);
                Dash.GetComponent<ChargedDash>().Charge++;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ennemy") && isMunitions)
        {
            other.transform.GetComponent<ennemyState>().damage(DmgShield);
            deadD.deadList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void ExplosionShield()
    {
        foreach (var Ressource in deadD.deadList)
        {
            deadD.deadList.Remove(Ressource);
            int Explosion = 20;
            Vector3 VectorDirection = Ressource.transform.position - player.transform.position;
            VectorDirection = VectorDirection.normalized;
            //Instantiate(Projectile, Ressource.transform.position, Ressource.transform.rotation);
            Destroy(Ressource);
            //FMODUnity.RuntimeManager.PlayOneShot(TireSon, transform.position);
            //Projectile.GetComponent<Rigidbody>().AddForce(VectorDirection * Explosion, ForceMode.Impulse);
        }
    }
}
