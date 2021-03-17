﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShield : skill
{

    [SerializeField] private float freqRessource;
    private float timer;

    [SerializeField] private float freqPoisonCloud;
    private float timerCloud;

    private bool isActive;

    [FMODUnity.EventRef]
    public string SonPoisonShield = "";
    FMOD.Studio.EventInstance sonPoisonShield;

    [SerializeField] private GameObject poisonCloud;
    [SerializeField] private Transform cuve;

    // Start is called before the first frame update
    void Start()
    {
        sonPoisonShield = FMODUnity.RuntimeManager.CreateInstance(SonPoisonShield);
        timer = freqRessource;

    }
    
    void Update()
    {
        if (isActive)
        {
            if (detectDead.ressourceInt > 0)
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

                if(timerCloud >= freqPoisonCloud)
                {
                    Instantiate(poisonCloud, cuve.position, Quaternion.identity);
                    timerCloud = 0;
                }
                else
                {
                    timerCloud += Time.deltaTime;

                }

            }
            else
            {
                sonPoisonShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isActive = false;
            }
        }
    }

     public override void UsingSkill()
     {
        if(detectDead.ressourceInt > 0)
        {
            if (isActive)
            {
                isActive = false;
                sonPoisonShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                timer = freqRessource;

            }
            else
            {
                isActive = true;
                sonPoisonShield.start();
            }
        }
         
    }
     

    // public override void ChargingSkill(int WhichWeapon)
    // {
    // }
    //
    // public override void EndUsing(Ray rayon)
    // {
    //     if (isCharging)
    //     {
    //         
    //     }
    // }
}
