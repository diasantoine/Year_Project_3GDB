﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill : MonoBehaviour
{
    public detectDead ressource;
    public bool chargedSkill;

    [SerializeField] protected private float freqCharge;
    protected private float chrono;

    [HideInInspector] public GameObject conteneur;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UsingSkill()
    {

    }
    
    public virtual void ChargingSkill()
    {
        if (ressource.deadList.Count > 0)
        {
            if (chrono >= freqCharge)
            {
                takeCadavre TC = ressource.deadList[0].GetComponent<takeCadavre>();

                if (TC.isMunitions)
                {
                    TC.isMunitions = false;
                    TC.charge = true;
                    TC.bombe = conteneur.transform;

                    ressource.deadList.Remove(ressource.deadList[0]);
                    chrono = 0;
                }

            }
            else
            {
                chrono += Time.deltaTime;
            }
        }
    }

    public virtual void EndUsing(Ray rayon)
    {

    }
}
