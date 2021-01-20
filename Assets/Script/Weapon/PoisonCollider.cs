using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{

    [SerializeField] private float FreqTick;

    [SerializeField] private float freqRessource;
    public float timer;

    [SerializeField] private float dps;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(detectDead.ressourceInt >= 0)
        {
            if (timer >= freqRessource)
            {
                detectDead.ressourceInt--;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
           
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        if(detectDead.ressourceInt > 0)
        {
            if (other.gameObject.CompareTag("Ennemy"))
            {
                other.gameObject.GetComponent<ennemyState>().Empoisonne = true;
                other.gameObject.GetComponent<ennemyState>().dpsTick = dps;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            other.gameObject.GetComponent<ennemyState>().Empoisonne = false;
            other.gameObject.GetComponent<ennemyState>().dpsTick = 0;


        }
    }

}
