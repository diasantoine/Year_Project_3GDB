﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD : MonoBehaviour
{

    public The_Player_Script TPS;

    [FMODUnity.EventRef]
    public string Ambiance = "";

    [FMODUnity.EventRef]
    public string footStep = "";
    FMOD.Studio.EventInstance footStepPlayer;

    [FMODUnity.EventRef]
    public string Ruant_FootStep = "";

    [FMODUnity.EventRef]
    public string Screamer_Explosion = "";

    [FMODUnity.EventRef]
    public string Screamer_FootStep = "";

    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ambiance, "", 0, transform.position);
        footStepPlayer = FMODUnity.RuntimeManager.CreateInstance(footStep);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(TPS.floatTypeOfFootStep);
        footStepPlayer.setParameterByName("TypeOfFootstep", TPS.floatTypeOfFootStep);
    }


    public void PlayFootStep()
    {
        // footStepPlayer.start();
        FMODUnity.RuntimeManager.PlayOneShot(footStep, "TypeOfFootstep", TPS.floatTypeOfFootStep, transform.position);
        // FMODUnity.RuntimeManager.PlayOneShot(footStep, transform.position);
        /// Debug.Log("test son footstep");
    }


    public void PlayFootStepRuant()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_FootStep, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void PlayScreamerExplosion()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_Explosion, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void PlayFootStepScreamer()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_FootStep, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }
}
