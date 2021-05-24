using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Data_SD : MonoBehaviour
{

    public The_Player_Script TPS;

    [FMODUnity.EventRef]
    public string Ambiance = "";

    [FMODUnity.EventRef]
    public string footStep = "";
    FMOD.Studio.EventInstance footStepPlayer;

    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ambiance, "", 0, transform.position);
        if (this.footStep != String.Empty)
        {
            footStepPlayer = FMODUnity.RuntimeManager.CreateInstance(footStep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(TPS.floatTypeOfFootStep);
        if (this.footStepPlayer.isValid())
        {
            footStepPlayer.setParameterByName("TypeOfFootstep", TPS.floatTypeOfFootStep);
        }
    }


    public void PlayFootStep()
    {
        // footStepPlayer.start();
        FMODUnity.RuntimeManager.PlayOneShot(footStep, "TypeOfFootstep", TPS.floatTypeOfFootStep, transform.position);
        // FMODUnity.RuntimeManager.PlayOneShot(footStep, transform.position);
        /// Debug.Log("test son footstep");
    }
}
