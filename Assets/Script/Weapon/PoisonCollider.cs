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
        if(detectDead.ressourceFloat > 0)
        {
            if (other.gameObject.CompareTag("Ennemy"))
            {
                if (other.gameObject.GetComponent<BasicState>())
                {
                    other.gameObject.GetComponent<BasicState>().isPoisoned = true;
                    other.gameObject.GetComponent<BasicState>().dpsTick = dps;
                }
                else if (other.gameObject.GetComponent<ScreamerState>())
                {
                    other.gameObject.GetComponent<ScreamerState>().isPoisoned = true;
                    other.gameObject.GetComponent<ScreamerState>().dpsTick = dps;
                }
            }
        }
    }

}
