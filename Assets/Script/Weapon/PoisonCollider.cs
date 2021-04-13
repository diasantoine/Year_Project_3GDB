using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{

    [SerializeField] private ParticleSystem cloud;

    [SerializeField] private float freqDie;
    private float timer;

    [SerializeField] private float dps;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= freqDie)
        {
            cloud.Stop();
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 2.5f);
        }
        else
        {
            timer += Time.deltaTime;
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        if(detectDead.ressourceInt > 0)
        {
            if (other.gameObject.CompareTag("Ennemy"))
            {
                other.gameObject.GetComponent<BasicState>().Empoisonne = true;
                other.gameObject.GetComponent<BasicState>().dpsTick = dps;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            other.gameObject.GetComponent<BasicState>().Empoisonne = false;
            other.gameObject.GetComponent<BasicState>().dpsTick = 0;


        }
    }

}
