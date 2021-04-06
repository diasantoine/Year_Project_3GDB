using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{

    [HideInInspector] public bool goGoGo;
    [HideInInspector] public int charged;

    [SerializeField] private int hitDmg;
    void Start()
    {
        goGoGo = false;
        charged = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (goGoGo)
        {
            Vector3 localZ = transform.parent.localScale;
            localZ.z = charged * localZ.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
            Destroy(transform.parent.gameObject, 0.1f);
            goGoGo = false;
        }
       // this.GetComponent<Renderer>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mur"))
        {
            this.goGoGo = true;
            this.charged--;
        }
        else
        {
            if (other.gameObject.CompareTag("Ennemy"))
            {
                if (other.gameObject.GetComponent<ennemyState>() != null)
                {
                    other.gameObject.GetComponent<ennemyState>().damage(hitDmg);

                }
                else if (other.gameObject.GetComponent<damageTuto>() != null)
                {
                    other.gameObject.GetComponent<damageTuto>().damage(hitDmg);
                }
                else if (other.gameObject.GetComponent<RuantState>() != null)
                {
                    other.gameObject.GetComponent<RuantState>().takeDmg(hitDmg);
                }

                if (other.gameObject.GetComponent<ScreamerScript>() != null)
                {
                    other.gameObject.GetComponent<ScreamerScript>().damage(hitDmg);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mur"))
        {
            this.goGoGo = true;
            this.charged--;
        } else
        {
            if (other.gameObject.CompareTag("Ennemy"))
            {
                if (other.gameObject.GetComponent<ennemyState>() != null)
                {
                    other.gameObject.GetComponent<ennemyState>().damage(hitDmg);

                }
                else if (other.gameObject.GetComponent<damageTuto>() != null)
                {
                    other.gameObject.GetComponent<damageTuto>().damage(hitDmg);
                }
                else if (other.gameObject.GetComponent<RuantState>() != null)
                {
                    other.gameObject.GetComponent<RuantState>().takeDmg(hitDmg);
                }

                if (other.gameObject.GetComponent<ScreamerScript>() != null)
                {
                    other.gameObject.GetComponent<ScreamerScript>().damage(hitDmg);
                }
            }
        }
    }
}
