using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCadavre : MonoBehaviour
{

    public bool gotcha;

    [HideInInspector] public bool isMunitions;
    [HideInInspector] public bool charge;


    public Transform player;
    public Transform pierre;


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
            if (pierre != null)
            {
                if (!pierre.GetComponent<TirCharge>().tipar)
                {
                    Vector3 direction = pierre.position - transform.position;

                    direction = direction.normalized;

                    transform.position += direction * vitesse * Time.deltaTime;

                    if(pierre.GetComponent<TirCharge>().nCharge >= pierre.GetComponent<TirCharge>().nChargeMax)
                    {
                        gotcha = true;
                        charge = false;
                        gameObject.layer = 8;
                    }
                }
                else
                {
                    pierre = null;
                }
;
            }
            else
            {
                gotcha = true;
                charge = false;
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
                pierre.GetComponent<TirCharge>().nCharge++;
                pierre.transform.localScale = pierre.transform.localScale + new Vector3(radiusGave, radiusGave, radiusGave);
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
}
